using System;
using System.IO;
using System.Windows.Forms;

namespace HttpProxy.GUI
{
    public partial class 主窗口 : Form
    {
        private static 日志记录器 log = new 日志记录器("Proxy");

        public static string 服务器地址;
        public static int 监听端口;
        public static string 配置文件 = "ProxyConfig.txt";
        public static string[] 参数;

        public 主窗口(string[] args)
        {
            InitializeComponent();
            参数 = args;
        }

        private static void 加载配置()
        {
            if (!File.Exists(配置文件))
            {
                try
                {
                    FileStream fs = File.Create(配置文件);
                    fs.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"创建配置文件时发生错误: {ex.Message}");
                }
            }

            配置解析器 配置 = new 配置解析器(配置文件);
            配置.解析();

            服务器地址 = 配置.获取值("服务器地址");
            if (服务器地址 == null)
            {
                配置.写入值("服务器地址", "http://127.0.0.1:520");
                服务器地址 = "http://127.0.0.1:520";
            }
            string 监听端口str = 配置.获取值("监听端口");
            if (监听端口str == null)
            {
                配置.写入值("监听端口", "8848");
                监听端口 = 8848;
            }
            else
            {
                监听端口 = int.Parse(监听端口str);
            }
        }

        private static void 保存配置()
        {
            配置解析器 配置 = new 配置解析器(配置文件);
            配置.解析();

            try
            {
                配置.写入值("监听端口", 监听端口.ToString());
                配置.写入值("服务器地址", 服务器地址);
            } catch (Exception ex)
            {
                MessageBox.Show($"写入配置文件时发生错误: {ex.Message}", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 主窗口_Load(object sender, EventArgs e)
        {

            if (参数.Length > 0)
            {
                if (参数[0] == "/DestroyCert")
                {
                    bool DestroyCerted = ProxyHelper.DestroyCertificate();
                    if (!DestroyCerted)
                    {
                        MessageBox.Show("未能卸载根证书。", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    Application.Exit();
                }
            }

            代理开关.Text = "开启代理";
            加载配置();
            log.Debug("服务器地址：" + 服务器地址);
            log.Debug("监听端口：" + 监听端口.ToString());
            监听端口输入框.Text = 监听端口.ToString();
            服务器地址输入框.Text = 服务器地址;
        }

        private void 监听端口输入框_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void 代理开关_Click(object sender, EventArgs e)
        {
            try
            {
                // 正在运行则关闭
                if (ProxyHelper.IsRunning)
                {
                    ProxyHelper.StopProxy();
                    代理开关.Text = "开启代理";
                    监听端口输入框.Enabled = true;
                    服务器地址输入框.Enabled = true;
                }
                else
                {
                    if (int.Parse(监听端口输入框.Text) > 65535 || int.Parse(监听端口输入框.Text) < 1)
                    {
                        MessageBox.Show("监听端口不能大于 65535 或小于 1 。", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (记录配置勾选.Checked)
                    {
                        服务器地址 = 服务器地址输入框.Text;
                        监听端口 = int.Parse(监听端口输入框.Text);
                        保存配置();
                    }

                    // 创建根证书并检查信任
                    if (!ProxyHelper.CheckAndCreateCertificate())
                    {
                        MessageBox.Show("未能安装根证书。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 启动代理
                    ProxyHelper.StartProxy(服务器地址);
                    代理开关.Text = "停止代理";
                    监听端口输入框.Enabled = false;
                    服务器地址输入框.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(Name, "启动代理失败：", ex.ToString());
                MessageBox.Show("启动代理失败：\n" + ex.Message, "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void 卸载证书_Click(object sender, EventArgs e)
        {
            bool DestroyCerted = ProxyHelper.DestroyCertificate();
            if (DestroyCerted)
            {
                MessageBox.Show("已卸载根证书。", "信息：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("未能卸载根证书。", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;
            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_CLOSE)
            {
                if (ProxyHelper.IsRunning)
                {
                    ProxyHelper.StopProxy();
                }

                Application.Exit();
            }
            base.WndProc(ref m);
        }
    }
}
