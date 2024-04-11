using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace HttpProxy
{
    internal class Program
    {
        public static string 服务器地址;
        public static int 监听端口;
        private static readonly 日志记录器 c = new 日志记录器("Proxy", ConsoleColor.Green);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "/DestroyCert")
                {
                    卸载证书();
                    return;
                }
            }

            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnProcessExit);
            Console.Title = "Http Proxy  （请按下 Ctrl + C 退出程序，不要按右上角的 ╳ 关闭按钮！！！  否则会无法关闭代理，导致无法上网！！！）";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("请按下 Ctrl + C 退出程序，不要按右上角的 ╳  关闭按钮！！！  否则会无法关闭代理，导致无法上网！！！");
            Console.WriteLine("请按下 Ctrl + C 退出程序，不要按右上角的 ╳  关闭按钮！！！  否则会无法关闭代理，导致无法上网！！！");
            Console.WriteLine("请按下 Ctrl + C 退出程序，不要按右上角的 ╳  关闭按钮！！！  否则会无法关闭代理，导致无法上网！！！\n");
            Console.ResetColor();
            c.Log("QQ群：819551320");
            加载配置();
            代理开关();
        }

        private static void OnProcessExit(object sender, EventArgs args)
        {
            Console.WriteLine("退出程序");
            代理开关();
            卸载证书();
            Environment.Exit(0);
        }

        private static void 加载配置()
        {
            string 配置文件 = "ProxyConfig.txt";
            if (!File.Exists(配置文件))
            {
                try
                {
                    FileStream fs = File.Create(配置文件);
                    fs.Close();
                }
                catch (Exception ex)
                {
                    c.Error($"创建配置文件时发生错误: {ex.Message}");
                    MessageBox(IntPtr.Zero, "创建配置文件时发生错误:\n" + ex.Message, "错误：", 0x00000010);
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

        private static void 代理开关()
        {
            try
            {
                // 正在运行则关闭代理
                if (ProxyHelper.IsRunning)
                {
                    ProxyHelper.StopProxy();
                }
                else
                {
                    if (监听端口 > 65535 || 监听端口 < 1)
                    {
                        c.Error("监听端口不能大于 65535 或小于 1 。");
                        MessageBox(IntPtr.Zero, "监听端口能大于 65535 或小于 1 。", "错误：", 0x00000010);
                        return;
                    }

                    // 创建根证书并检查信任
                    if (!ProxyHelper.CheckAndCreateCertificate())
                    {
                        MessageBox(IntPtr.Zero, "未能安装根证书。", "错误：", 0x00000010);
                        c.Error("未能安装根证书");
                        return;
                    }

                    // 启动代理
                    ProxyHelper.StartProxy(服务器地址);
                    //使进程保持运行
                    Thread.Sleep(-1);
                }
            }
            catch (Exception ex)
            {
                c.Error("启动代理失败：" + ex);
                MessageBox(IntPtr.Zero, "启动代理失败：\n" + ex.Message, "错误：", 0x00000010);
            }
        }

        private static void 卸载证书()
        {
            c.Log("正在卸载根证书");
            bool DestroyCerted = ProxyHelper.DestroyCertificate();
            if (DestroyCerted)
            {
                c.Log("已卸载根证书");
            }
            else
            {
                c.Warn("未能卸载根证书");
            }
        }
    }
}
