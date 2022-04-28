using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Nikse.SubtitleEdit.PluginLogic
{
    public class WordSpellCheck : IPlugin // dll file name must "<classname>.dll" - e.g. "SyncViaOtherSubtitle.dll"
    {

        string IPlugin.Name => "Word拼写检查";

        string IPlugin.Text => "Word拼写检查";

        decimal IPlugin.Version => 1.7M;

        string IPlugin.Description => "本功能调用Word对字幕进行拼写检查，可以避免字幕文件转存文件，然后在word中打卡检查的步骤，提高生产效率";

        string IPlugin.ActionType => "spellcheck"; // Can be one of these: file, tool, sync, translate, spellcheck

        string IPlugin.Shortcut => string.Empty;

        string IPlugin.DoAction(Form parentForm, string subtitle, double frameRate, string listViewLineSeparatorString, string subtitleFileName, string videoFileName, string rawText)
        {
            subtitle = subtitle.Trim();
            if (string.IsNullOrWhiteSpace(subtitle))
            {
                //MessageBox.Show("No subtitle loaded", parentForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show("字幕为空", parentForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return string.Empty;
            }
            if (!IsOfficeInstalled())
            {
                //MessageBox.Show(@"Microsoft Office (Word) is not installed in this system.", "Office is not installed!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(@"本功能需要您的电脑上安装了Microsoft Office Word软件才能运行", "请安装Microsoft Office Word!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

            // set frame rate
            Configuration.CurrentFrameRate = frameRate;

            // set newline visualizer for listviews
            if (!string.IsNullOrEmpty(listViewLineSeparatorString))
            {
                Configuration.ListViewLineSeparatorString = listViewLineSeparatorString;
            }

            // load subtitle text into object
            var list = new List<string>();
            foreach (string line in subtitle.Replace(Environment.NewLine, "\n").Split('\n'))
            {
                list.Add(line);
            }

            var sub = new Subtitle();
            var srt = new SubRip();
            srt.LoadSubtitle(sub, list, subtitleFileName);
            var form = new PluginForm(sub, (this as IPlugin).Name, (this as IPlugin).Description);
            if (form.ShowDialog(parentForm) == DialogResult.OK)
            {
                return form.FixedSubtitle;
            }
            return string.Empty;
        }

        private bool IsOfficeInstalled()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\Winword.exe");
            key?.Close();
            return key != null;
        }
    }
}
