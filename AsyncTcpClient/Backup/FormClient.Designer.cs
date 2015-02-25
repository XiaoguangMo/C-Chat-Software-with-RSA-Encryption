namespace AsyncTcpClient
{
    partial class FormClient
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txt_UserName = new System.Windows.Forms.TextBox();
            this.btn_Login = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lst_OnlineUser = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtf_MessageInfo = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_SendeMessage = new System.Windows.Forms.Button();
            this.rtf_SendMessage = new System.Windows.Forms.RichTextBox();
            this.rtf_StatusInfo = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // txt_UserName
            // 
            this.txt_UserName.Location = new System.Drawing.Point(68, 12);
            this.txt_UserName.Name = "txt_UserName";
            this.txt_UserName.Size = new System.Drawing.Size(171, 21);
            this.txt_UserName.TabIndex = 1;
            // 
            // btn_Login
            // 
            this.btn_Login.Location = new System.Drawing.Point(11, 39);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(228, 23);
            this.btn_Login.TabIndex = 2;
            this.btn_Login.Text = "登 陆";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lst_OnlineUser);
            this.groupBox1.Location = new System.Drawing.Point(11, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(228, 173);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "当前在线";
            // 
            // lst_OnlineUser
            // 
            this.lst_OnlineUser.FormattingEnabled = true;
            this.lst_OnlineUser.ItemHeight = 12;
            this.lst_OnlineUser.Location = new System.Drawing.Point(6, 20);
            this.lst_OnlineUser.Name = "lst_OnlineUser";
            this.lst_OnlineUser.Size = new System.Drawing.Size(216, 148);
            this.lst_OnlineUser.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtf_MessageInfo);
            this.groupBox2.Location = new System.Drawing.Point(255, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(385, 140);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "对话信息";
            // 
            // rtf_MessageInfo
            // 
            this.rtf_MessageInfo.Location = new System.Drawing.Point(15, 20);
            this.rtf_MessageInfo.Name = "rtf_MessageInfo";
            this.rtf_MessageInfo.Size = new System.Drawing.Size(351, 105);
            this.rtf_MessageInfo.TabIndex = 0;
            this.rtf_MessageInfo.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_SendeMessage);
            this.groupBox3.Controls.Add(this.rtf_SendMessage);
            this.groupBox3.Location = new System.Drawing.Point(255, 159);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(385, 100);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "发送信息";
            // 
            // btn_SendeMessage
            // 
            this.btn_SendeMessage.Location = new System.Drawing.Point(323, 41);
            this.btn_SendeMessage.Name = "btn_SendeMessage";
            this.btn_SendeMessage.Size = new System.Drawing.Size(56, 23);
            this.btn_SendeMessage.TabIndex = 3;
            this.btn_SendeMessage.Text = "发 送";
            this.btn_SendeMessage.UseVisualStyleBackColor = true;
            this.btn_SendeMessage.Click += new System.EventHandler(this.btn_SendeMessage_Click);
            // 
            // rtf_SendMessage
            // 
            this.rtf_SendMessage.Location = new System.Drawing.Point(15, 20);
            this.rtf_SendMessage.Name = "rtf_SendMessage";
            this.rtf_SendMessage.Size = new System.Drawing.Size(302, 71);
            this.rtf_SendMessage.TabIndex = 1;
            this.rtf_SendMessage.Text = "";
            // 
            // rtf_StatusInfo
            // 
            this.rtf_StatusInfo.Location = new System.Drawing.Point(11, 265);
            this.rtf_StatusInfo.Name = "rtf_StatusInfo";
            this.rtf_StatusInfo.Size = new System.Drawing.Size(629, 95);
            this.rtf_StatusInfo.TabIndex = 6;
            this.rtf_StatusInfo.Text = "";
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 370);
            this.Controls.Add(this.rtf_StatusInfo);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_Login);
            this.Controls.Add(this.txt_UserName);
            this.Controls.Add(this.label1);
            this.Name = "FormClient";
            this.Text = "客户端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClient_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_UserName;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox rtf_MessageInfo;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox rtf_SendMessage;
        private System.Windows.Forms.Button btn_SendeMessage;
        private System.Windows.Forms.RichTextBox rtf_StatusInfo;
        private System.Windows.Forms.ListBox lst_OnlineUser;
    }
}

