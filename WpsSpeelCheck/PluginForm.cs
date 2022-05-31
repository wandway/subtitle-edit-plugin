using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word;

namespace Nikse.SubtitleEdit.PluginLogic
{
    internal sealed partial class PluginForm : Form
    {
        private readonly Subtitle _subtitle;
        public string FixedSubtitle { get; private set; }
        private Word.Application app;
        private Paragraph currentParagraph;
        private bool auth = false;
        // count 记数方式，适用于语法检查
        // split 分隔符方式，速度快，准确性高，但会影响拼写语法的结果
        // mark 标记方式，断句更准确，不影响语法检查，但是效率低
        private string BreakMode = "split"; 

        public PluginForm(Subtitle sub, string name, string description)
        {
            InitializeComponent();
            _subtitle = sub;
            FillSubtitleListView();
            labelStatus.Text = "字幕加载完成。";
        }

        /// <summary>
        /// 插件启动后初始化授权查询、授权后启动word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PluginForm_Load(object sender, EventArgs e)
        {
            comboBoxBreak.SelectedIndex = 1;
            CheckAuth();
            if (auth)
            {
                app = new Word.Application();
                app.Documents.Add();
                app.Options.CheckGrammarWithSpelling = true;
                app.Options.SuggestSpellingCorrections = true;
                app.Visible = true;
            }
        }

        /// <summary>
        /// 检查授权
        /// </summary>
        private void CheckAuth()
        {
            labelInfo.Text = "正在检查插件授权...";
            string result = HttpRequest.SendGet("https://www.435205.com/bbs/plugin.wps_spell_check/auth", "username=yangying&password=123456");
            var match = Regex.Match(result, "\"status\":(\\d),\"msg\":\"(.+?)\"");
            if (!match.Success)
            {
                labelInfo.Text = "服务器错误";
                return;
            }
            labelInfo.Text = match.Groups[2].Value;
            if (match.Groups[1].Value == "1")
            {
                auth = true;
            }
        }

        /// <summary>
        /// 填充字幕列表项
        /// </summary>
        private void FillSubtitleListView()
        {
            listViewSubtitle.BeginUpdate();
            int i = 1;
            foreach(var p in _subtitle.Paragraphs)
            {
                p.Text = p.Text.Replace(Environment.NewLine, " ");
                AddSubtitleToSubtitleListView(p, i.ToString());
                ++i;
            }

            if (listViewSubtitle.Items.Count > 0)
            {
                listViewSubtitle.Items[0].Selected = true;
            }

            listViewSubtitle.EndUpdate();
        }

        /// <summary>
        /// 初始化字幕列表
        /// </summary>
        /// <param name="p"></param>
        /// <param name="index"></param>
        private void AddSubtitleToSubtitleListView(Paragraph p, string index)
        {
            var item = new ListViewItem(index) { Tag = p};

            var startTime = p.StartTime.IsMaxTime ? "-" : p.StartTime.ToShortString();
            var subItem = new ListViewItem.ListViewSubItem(item, startTime);
            item.SubItems.Add(subItem);

            var endTime = p.EndTime.IsMaxTime ? "-" : p.EndTime.ToShortString();
            subItem = new ListViewItem.ListViewSubItem(item, endTime);
            item.SubItems.Add(subItem);
            var text = p.Text;
            subItem = new ListViewItem.ListViewSubItem(item, text);
            item.SubItems.Add(subItem);

            listViewSubtitle.Items.Add(item);
        }

        /// <summary>
        /// 切换当前字幕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewSubtitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSubtitle.Items.Count < 1)
            {
                return;
            }
            if (listViewSubtitle.SelectedItems.Count < 1)
            {
                return;
            }

            var p = listViewSubtitle.SelectedItems[0].Tag as Paragraph;
            currentParagraph = p;
            richTextBoxParagraph.Text = p.Text;
        }

        /// <summary>
        /// 单句拼写检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCheckParagraph_Click(object sender, EventArgs e)
        {
            if (!auth)
            {
                labelStatus.Text = "未能获得授权";
                return;
            }
            var doc = app.ActiveDocument;
            doc.Content.Text = richTextBoxParagraph.Text;
            doc.CheckSpelling();
            doc.CheckGrammar();
            var text = doc.Range().Text;
            richTextBoxParagraph.Text = text;
            currentParagraph.Text = text;
            listViewSubtitle.SelectedItems[0].SubItems[3].Text = text;
        }

        ~PluginForm()
        {
            if (auth)
            {                
                //app.ActiveDocument.Close(false);
                //app.Quit(false);
            }
        }

        /// <summary>
        /// 点击全文检查按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFullTextCheck_Click(object sender, EventArgs e)
        {

            if (!auth)
            {
                labelStatus.Text = "未能获得授权";
                return;
            }

            if (BreakMode == "mark")
            {
                CheckWithComments();
                return;
            }

            if (BreakMode == "split")
            {
                CheckWithSplit();
                return;
            }

            CheckWithBlankCount();
        }

        /// <summary>
        /// 使用空格计数进行拼写检查
        /// </summary>
        private void CheckWithBlankCount()
        {
            var doc = app.ActiveDocument;
            doc.Range().Text = string.Join(" ", _subtitle.Paragraphs.Select(e => e.Text).ToList<String>());

            MessageBox.Show("Word已经运行，请切换到Word完成拼写检查");
            doc.CheckSpelling();
            doc.CheckGrammar();
            BlankCountSync();
        }

        /// <summary>
        /// 使用批注进行语法、拼写检查
        /// </summary>
        private void CheckWithComments()
        {
            MessageBox.Show("当前使用批注断句模式，该模式运行较为缓慢，在弹出【请开始检查】的提示框前，请不要操作，现在请点击确定", "开始在word中标记断句");
            var doc = app.ActiveDocument;
            doc.Range().Text = string.Join(" ", _subtitle.Paragraphs.Select(e => e.Text).ToList<String>());
            var start = 0;
            int end;
            Range range;
            foreach (var p in _subtitle.Paragraphs)
            {
                end = start + p.Text.Length + 1;
                start = end - 1;
                range = doc.Range(start, end);
                range.Comments.Add(range, string.Format("{0}", p.Number));
                start = end + 1;
                labelStatus.Text = string.Format("正在标记第{0}/{1}句唱词，请耐心等待", p.Number, _subtitle.Paragraphs.Count);
            }
            labelStatus.Text = "标记完成，请不要破坏断句批注！！！";
            MessageBox.Show("Word已经运行，请在Word内完成拼写检查，注意不要破坏批注范围", "请开始检查");
            doc.CheckSpelling();
            doc.CheckGrammar();
            CommentsSync();
        }

        /// <summary>
        /// 使用分割符进行语法、拼写检查
        /// </summary>
        private void CheckWithSplit()
        {
            var doc = app.ActiveDocument;
            var c = "\0";
            doc.Range().Text = string.Join(c, _subtitle.Paragraphs.Select(e => e.Text).ToList<String>());
            doc.CheckSpelling();
            doc.CheckGrammar();
            SplitSync();
        }

        /// <summary>
        /// 点击全文同步按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSyncDoc_Click(object sender, EventArgs e)
        {
            if (!auth)
            {
                labelStatus.Text = "未能获得授权";
                return;
            }
            if (BreakMode == "mark")
            {
                CommentsSync();
                return;
            }
            if (BreakMode == "split")
            {
                SplitSync();
                return;
            }
            BlankCountSync();
        }

        /// <summary>
        /// 空格计数模式同步
        /// </summary>
        private void BlankCountSync()
        {
            labelStatus.Text = "正在同步中……标记模式同步会需要较长时间！！！";
            var pos = 0;
            var offsets = new List<int>();
            foreach (var p in _subtitle.Paragraphs)
            {
                offsets.Add(pos);
                pos += p.Text.Split(' ').Count();
            }
            var doc = app.ActiveDocument;
            var words = doc.Range().Text.Split(' ');

            for (int i = 0; i < _subtitle.Paragraphs.Count; i++)
            {
                labelStatus.Text = string.Format("正在同步第{0}/{1}句字幕...", i + 1, _subtitle.Paragraphs.Count);
                var p = _subtitle.Paragraphs[i];
                var wordCount = p.Text.Split(' ').Count();
                int end = wordCount + offsets[i];
                if (end > words.Length)
                {
                    labelStatus.Text = "同步失败!!!";
                    MessageBox.Show("当前处于空格计数模式，本模式不支持增删单词，无法完成唱词同步");
                    return;
                }
                var line = "";
                for (int offset = offsets[i]; offset < end; offset++)
                {
                    line += words[offset] + " ";
                }
                line = line.Trim();
                listViewSubtitle.Items[i].SubItems[3].Text = line;
                p.Text = line;
                if (i == listViewSubtitle.SelectedIndices[0])
                {
                    richTextBoxParagraph.Text = line;
                }
            }
            labelStatus.Text = "同步完成";
            MessageBox.Show("同步完成");
        }

        /// <summary>
        /// 通过批注识别唱词行数
        /// </summary>
        private void CommentsSync()
        {
            var doc = app.ActiveDocument;
            if (doc.Comments.Count != _subtitle.Paragraphs.Count)
            {
                MessageBox.Show("word唱词行数不一致，请核对当前word文档内容是否为对应字幕内容");
                return;
            }
            var start = 0;
            int end;
            Range range;
            for (int i = 0; i < _subtitle.Paragraphs.Count; i++)
            {
                end = doc.Comments[i + 1].Reference.Start;
                range = doc.Range(start, end);
                _subtitle.Paragraphs[i].Text = range.Text;
                listViewSubtitle.Items[i].SubItems[3].Text = range.Text;
                if (i == listViewSubtitle.SelectedIndices[0])
                {
                    richTextBoxParagraph.Text = range.Text;
                }
                start = end;
            }
            MessageBox.Show("同步完成");
        }

        /// <summary>
        /// 通过分割符识别唱词行数
        /// </summary>
        private void SplitSync()
        {
            var doc = app.ActiveDocument;
            var c = '\0';
            var words = doc.Range().Text.Split(c);
            if (words.Length != _subtitle.Paragraphs.Count)
            {
                MessageBox.Show("word唱词行数不一致，请核对当前word文档内容是否为对应字幕内容");
                return;
            }
            
            for (int i = 0; i < words.Length; i++)
            {
                _subtitle.Paragraphs[i].Text = words[i];
                listViewSubtitle.Items[i].SubItems[3].Text = words[i];
                if (i == listViewSubtitle.SelectedIndices[0])
                {
                    richTextBoxParagraph.Text = words[i];
                }
            }
            MessageBox.Show("同步完成");
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            FixedSubtitle = _subtitle.ToText(new SubRip());
            if (auth)
            {
                app.Quit(false);
            }
            DialogResult = DialogResult.OK;
        }

        private void comboBoxBreak_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBoxBreak.SelectedIndex)
            {
                case 0:
                    labelInfo.Text = "完整的语法检查，速度快，但是不支持在word中增删单词";
                    BreakMode = "count";
                    break;
                case 1:
                    labelInfo.Text = "支持增删单词，速度快，断句点语法检查不完整";
                    BreakMode = "split";
                    break;
                default:
                    labelInfo.Text = "【不推荐】支持在word中增删单词，速度慢,不完整的语法检查";
                    BreakMode = "mark";
                    break;
            }
        }
    }
}
