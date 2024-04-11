namespace HttpProxy.GUI
{
    partial class 主窗口
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.服务器地址输入框 = new System.Windows.Forms.TextBox();
            this.服务器地址标签 = new System.Windows.Forms.Label();
            this.监听端口输入框 = new System.Windows.Forms.TextBox();
            this.监听端口标签 = new System.Windows.Forms.Label();
            this.代理开关 = new System.Windows.Forms.Button();
            this.卸载证书 = new System.Windows.Forms.Button();
            this.信息框 = new System.Windows.Forms.TextBox();
            this.记录配置勾选 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // 服务器地址输入框
            // 
            this.服务器地址输入框.Location = new System.Drawing.Point(167, 11);
            this.服务器地址输入框.Name = "服务器地址输入框";
            this.服务器地址输入框.Size = new System.Drawing.Size(167, 21);
            this.服务器地址输入框.TabIndex = 0;
            // 
            // 服务器地址标签
            // 
            this.服务器地址标签.AutoSize = true;
            this.服务器地址标签.Location = new System.Drawing.Point(12, 14);
            this.服务器地址标签.Name = "服务器地址标签";
            this.服务器地址标签.Size = new System.Drawing.Size(131, 12);
            this.服务器地址标签.TabIndex = 1;
            this.服务器地址标签.Text = "目标 http 服务器地址:";
            // 
            // 监听端口输入框
            // 
            this.监听端口输入框.Location = new System.Drawing.Point(167, 38);
            this.监听端口输入框.MaxLength = 5;
            this.监听端口输入框.Name = "监听端口输入框";
            this.监听端口输入框.Size = new System.Drawing.Size(167, 21);
            this.监听端口输入框.TabIndex = 2;
            this.监听端口输入框.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.监听端口输入框_KeyPress);
            // 
            // 监听端口标签
            // 
            this.监听端口标签.AutoSize = true;
            this.监听端口标签.Location = new System.Drawing.Point(78, 43);
            this.监听端口标签.Name = "监听端口标签";
            this.监听端口标签.Size = new System.Drawing.Size(83, 12);
            this.监听端口标签.TabIndex = 3;
            this.监听端口标签.Text = "代理监听端口:";
            // 
            // 代理开关
            // 
            this.代理开关.Location = new System.Drawing.Point(351, 12);
            this.代理开关.Name = "代理开关";
            this.代理开关.Size = new System.Drawing.Size(105, 23);
            this.代理开关.TabIndex = 4;
            this.代理开关.Text = "代理开关";
            this.代理开关.UseVisualStyleBackColor = true;
            this.代理开关.Click += new System.EventHandler(this.代理开关_Click);
            // 
            // 卸载证书
            // 
            this.卸载证书.Location = new System.Drawing.Point(359, 173);
            this.卸载证书.Name = "卸载证书";
            this.卸载证书.Size = new System.Drawing.Size(105, 23);
            this.卸载证书.TabIndex = 6;
            this.卸载证书.Text = "卸载证书";
            this.卸载证书.UseVisualStyleBackColor = true;
            this.卸载证书.Click += new System.EventHandler(this.卸载证书_Click);
            // 
            // 信息框
            // 
            this.信息框.Location = new System.Drawing.Point(11, 70);
            this.信息框.Multiline = true;
            this.信息框.Name = "信息框";
            this.信息框.ReadOnly = true;
            this.信息框.Size = new System.Drawing.Size(453, 97);
            this.信息框.TabIndex = 7;
            this.信息框.Text = "    启动代理需要信任本程序的临时根证书，该证书仅用于代理动漫游戏相关请求，你可以随时点击右下角卸载证书。\r\n\r\n    如果不能启动代理，请尝试更改 \"代理监" +
    "听端口\"\r\n\r\n    程序退出时会自动关闭代理，请放心使用 :)";
            // 
            // 记录配置勾选
            // 
            this.记录配置勾选.AutoSize = true;
            this.记录配置勾选.Checked = true;
            this.记录配置勾选.CheckState = System.Windows.Forms.CheckState.Checked;
            this.记录配置勾选.Location = new System.Drawing.Point(351, 43);
            this.记录配置勾选.Name = "记录配置勾选";
            this.记录配置勾选.Size = new System.Drawing.Size(72, 16);
            this.记录配置勾选.TabIndex = 8;
            this.记录配置勾选.Text = "记录配置";
            this.记录配置勾选.UseVisualStyleBackColor = true;
            // 
            // 主窗口
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 205);
            this.Controls.Add(this.记录配置勾选);
            this.Controls.Add(this.信息框);
            this.Controls.Add(this.卸载证书);
            this.Controls.Add(this.代理开关);
            this.Controls.Add(this.监听端口标签);
            this.Controls.Add(this.监听端口输入框);
            this.Controls.Add(this.服务器地址标签);
            this.Controls.Add(this.服务器地址输入框);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "主窗口";
            this.Text = "HttpProxy GUI";
            this.Load += new System.EventHandler(this.主窗口_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox 服务器地址输入框;
        private System.Windows.Forms.Label 服务器地址标签;
        private System.Windows.Forms.TextBox 监听端口输入框;
        public System.Windows.Forms.Label 监听端口标签;
        private System.Windows.Forms.Button 代理开关;
        private System.Windows.Forms.Button 卸载证书;
        private System.Windows.Forms.TextBox 信息框;
        private System.Windows.Forms.CheckBox 记录配置勾选;
    }
}

