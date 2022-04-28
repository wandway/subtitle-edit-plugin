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
        private string BreakMode = "count"; // count 计数方式，适用于语法检查 mark 标记方式，断句更准确

        public PluginForm(Subtitle sub, string name, string description)
        {
            InitializeComponent();
            _subtitle = sub;
            FillSubtitleListView();
            labelStatus.Text = "字幕加载完成。";
        }

        private void PluginForm_Load(object sender, EventArgs e)
        {
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

        private void FillSubtitleListView()
        {
            listViewSubtitle.BeginUpdate();
            int i = 1;
            foreach(var p in _subtitle.Paragraphs)
            {
                AddSubtitleToSubtitleListView(p, i.ToString());
                ++i;
            }

            if (listViewSubtitle.Items.Count > 0)
            {
                listViewSubtitle.Items[0].Selected = true;
            }

            listViewSubtitle.EndUpdate();
        }

        private void AddSubtitleToSubtitleListView(Paragraph p, string index)
        {
            var item = new ListViewItem(index) { Tag = p};

            var startTime = p.StartTime.IsMaxTime ? "-" : p.StartTime.ToShortString();
            var subItem = new ListViewItem.ListViewSubItem(item, startTime);
            item.SubItems.Add(subItem);

            var endTime = p.EndTime.IsMaxTime ? "-" : p.EndTime.ToShortString();
            subItem = new ListViewItem.ListViewSubItem(item, endTime);
            item.SubItems.Add(subItem);

            var text = p.Text.Replace(Environment.NewLine, Configuration.ListViewLineSeparatorString);
            subItem = new ListViewItem.ListViewSubItem(item, text);
            item.SubItems.Add(subItem);

            listViewSubtitle.Items.Add(item);
        }

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
                app.ActiveDocument.Close(false);
                app.Quit(false);
            }
        }

        private void buttonFullTextCheck_Click(object sender, EventArgs e)
        {
            if (!auth)
            {
                labelStatus.Text = "未能获得授权";
                return;
            }
            var doc = app.ActiveDocument;
            var lines = new List<String>();
            var pos = 0;
            var offsets = new List<int>();
            foreach (var p in _subtitle.Paragraphs)
            {
                offsets.Add(pos);
                lines.Add(p.Text);
                pos += p.Text.Split(' ').Count();
            }
            doc.Content.Text = string.Join(" ", lines);

            MessageBox.Show("Word已经运行，请切换到Word完成拼写检查");
            doc.CheckSpelling();
            doc.CheckGrammar();
            var words = doc.Range().Text.Split(' ');

            for (int i = 0; i < _subtitle.Paragraphs.Count; i++)
            {
                var p = _subtitle.Paragraphs[i];
                var wordCount = p.Text.Split(' ').Count();
                int end = wordCount + offsets[i];
                var line = "";
                for (int offset = offsets[i]; offset < end; offset ++)
                {
                    line += words[offset] + " ";
                }
                listViewSubtitle.Items[i].SubItems[3].Text = line.Trim();
                if (i == listViewSubtitle.SelectedIndices[0])
                {
                    richTextBoxParagraph.Text = line;
                }
            }
        }

        private void buttonSyncDoc_Click(object sender, EventArgs e)
        {
            if (!auth)
            {
                labelStatus.Text = "未能获得授权";
                return;
            }
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
                var p = _subtitle.Paragraphs[i];
                var wordCount = p.Text.Split(' ').Count();
                int end = wordCount + offsets[i];
                var line = "";
                for (int offset = offsets[i]; offset < end; offset++)
                {
                    line += words[offset] + " ";
                }
                listViewSubtitle.Items[i].SubItems[3].Text = line.Trim();
                if (i == listViewSubtitle.SelectedIndices[0])
                {
                    richTextBoxParagraph.Text = line;
                }
            }
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
    }
}
