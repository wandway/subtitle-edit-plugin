
namespace Nikse.SubtitleEdit.PluginLogic
{
    partial class PluginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.listViewSubtitle = new System.Windows.Forms.ListView();
            this.columnIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnStart = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.richTextBoxParagraph = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonCheckParagraph = new System.Windows.Forms.Button();
            this.buttonFullTextCheck = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonSyncDoc = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.comboBoxBreak = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Location = new System.Drawing.Point(717, 431);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonSubmit.TabIndex = 0;
            this.buttonSubmit.Text = "提交字幕";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // listViewSubtitle
            // 
            this.listViewSubtitle.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnIndex,
            this.columnStart,
            this.columnEnd,
            this.columnText});
            this.listViewSubtitle.FullRowSelect = true;
            this.listViewSubtitle.HideSelection = false;
            this.listViewSubtitle.Location = new System.Drawing.Point(17, 104);
            this.listViewSubtitle.Margin = new System.Windows.Forms.Padding(2);
            this.listViewSubtitle.Name = "listViewSubtitle";
            this.listViewSubtitle.Size = new System.Drawing.Size(777, 324);
            this.listViewSubtitle.TabIndex = 1;
            this.listViewSubtitle.UseCompatibleStateImageBehavior = false;
            this.listViewSubtitle.View = System.Windows.Forms.View.Details;
            this.listViewSubtitle.SelectedIndexChanged += new System.EventHandler(this.listViewSubtitle_SelectedIndexChanged);
            // 
            // columnIndex
            // 
            this.columnIndex.Text = "#";
            // 
            // columnStart
            // 
            this.columnStart.Text = "开始时间";
            this.columnStart.Width = 102;
            // 
            // columnEnd
            // 
            this.columnEnd.Text = "结束时间";
            this.columnEnd.Width = 100;
            // 
            // columnText
            // 
            this.columnText.Text = "字幕内容";
            this.columnText.Width = 512;
            // 
            // richTextBoxParagraph
            // 
            this.richTextBoxParagraph.Location = new System.Drawing.Point(17, 8);
            this.richTextBoxParagraph.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxParagraph.Name = "richTextBoxParagraph";
            this.richTextBoxParagraph.Size = new System.Drawing.Size(341, 93);
            this.richTextBoxParagraph.TabIndex = 2;
            this.richTextBoxParagraph.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(361, 8);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(77, 22);
            this.button2.TabIndex = 3;
            this.button2.Text = "修改原文";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // buttonCheckParagraph
            // 
            this.buttonCheckParagraph.Location = new System.Drawing.Point(361, 34);
            this.buttonCheckParagraph.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCheckParagraph.Name = "buttonCheckParagraph";
            this.buttonCheckParagraph.Size = new System.Drawing.Size(77, 23);
            this.buttonCheckParagraph.TabIndex = 4;
            this.buttonCheckParagraph.Text = "单句检查";
            this.buttonCheckParagraph.UseVisualStyleBackColor = true;
            this.buttonCheckParagraph.Click += new System.EventHandler(this.buttonCheckParagraph_Click);
            // 
            // buttonFullTextCheck
            // 
            this.buttonFullTextCheck.Location = new System.Drawing.Point(635, 431);
            this.buttonFullTextCheck.Margin = new System.Windows.Forms.Padding(2);
            this.buttonFullTextCheck.Name = "buttonFullTextCheck";
            this.buttonFullTextCheck.Size = new System.Drawing.Size(77, 23);
            this.buttonFullTextCheck.TabIndex = 5;
            this.buttonFullTextCheck.Text = "全文检查";
            this.buttonFullTextCheck.UseVisualStyleBackColor = true;
            this.buttonFullTextCheck.Click += new System.EventHandler(this.buttonFullTextCheck_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(15, 436);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(143, 12);
            this.labelStatus.TabIndex = 6;
            this.labelStatus.Text = "成功开启wps拼写检查插件";
            // 
            // buttonSyncDoc
            // 
            this.buttonSyncDoc.Location = new System.Drawing.Point(554, 431);
            this.buttonSyncDoc.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSyncDoc.Name = "buttonSyncDoc";
            this.buttonSyncDoc.Size = new System.Drawing.Size(77, 23);
            this.buttonSyncDoc.TabIndex = 7;
            this.buttonSyncDoc.Text = "全文同步";
            this.buttonSyncDoc.UseVisualStyleBackColor = true;
            this.buttonSyncDoc.Click += new System.EventHandler(this.buttonSyncDoc_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(462, 13);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(167, 12);
            this.labelInfo.TabIndex = 8;
            this.labelInfo.Text = "欢迎使用wps spell check插件";
            // 
            // comboBoxBreak
            // 
            this.comboBoxBreak.FormattingEnabled = true;
            this.comboBoxBreak.Items.AddRange(new object[] {
            "空格计数",
            "断句标记",
            "批注标记"});
            this.comboBoxBreak.Location = new System.Drawing.Point(364, 63);
            this.comboBoxBreak.Name = "comboBoxBreak";
            this.comboBoxBreak.Size = new System.Drawing.Size(121, 20);
            this.comboBoxBreak.TabIndex = 9;
            this.comboBoxBreak.SelectedIndexChanged += new System.EventHandler(this.comboBoxBreak_SelectedIndexChanged);
            // 
            // PluginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 461);
            this.Controls.Add(this.comboBoxBreak);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.buttonSyncDoc);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonFullTextCheck);
            this.Controls.Add(this.buttonCheckParagraph);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.richTextBoxParagraph);
            this.Controls.Add(this.listViewSubtitle);
            this.Controls.Add(this.buttonSubmit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PluginForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WPS拼写检查";
            this.Load += new System.EventHandler(this.PluginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.ListView listViewSubtitle;
        private System.Windows.Forms.RichTextBox richTextBoxParagraph;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonCheckParagraph;
        private System.Windows.Forms.Button buttonFullTextCheck;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ColumnHeader columnStart;
        private System.Windows.Forms.ColumnHeader columnEnd;
        private System.Windows.Forms.ColumnHeader columnText;
        private System.Windows.Forms.ColumnHeader columnIndex;
        private System.Windows.Forms.Button buttonSyncDoc;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.ComboBox comboBoxBreak;
    }
}