using Nikse.SubtitleEdit.PluginLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nikse.SubtitleEdit.PluginLogic
{
    class WpsSpellCheck : IPlugin
    {
        string IPlugin.Name => "WPS拼写检查";
        string IPlugin.Text => "WPS拼写检查 v0.4";
        decimal IPlugin.Version => 0.4M;
        string IPlugin.Description => "调用WPS进行拼写检查，避免来回切换软件，提升工作效率。";
        string IPlugin.ActionType => "spellcheck";
        string IPlugin.Shortcut => String.Empty;
        
        string IPlugin.DoAction(System.Windows.Forms.Form parentForm, string subtitle, double frameRate, string listViewLineSeparatorString, string subtitleFileName, string videoFileName, string rawText)
        {
            subtitle = subtitle.Trim();
            if (string.IsNullOrEmpty(subtitle))
            {
                MessageBox.Show("没有加载字幕", parentForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return string.Empty;
            }
            Configuration.CurrentFrameRate = frameRate;
            if (!string.IsNullOrEmpty(listViewLineSeparatorString))
            {
                Configuration.ListViewLineSeparatorString = listViewLineSeparatorString;
            }
            var list = new List<string>();
            foreach (string line in subtitle.Replace(Environment.NewLine, "\n").Split('\n')) {
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
    }
}
