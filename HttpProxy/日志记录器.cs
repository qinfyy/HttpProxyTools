using System;
using System.Diagnostics;
using System.Linq;

namespace HttpProxy
{
    public class 日志记录器
    {
        private readonly string _名称;
        private readonly bool _跟踪错误;
        private readonly ConsoleColor _颜色;
        private static readonly object 控制台锁 = new object();

        public 日志记录器(string 名称, ConsoleColor 颜色 = ConsoleColor.White, bool 跟踪错误 = true)
        {
            _名称 = 名称;
            _颜色 = 颜色;
            _跟踪错误 = 跟踪错误;
        }

        public bool 是否调试()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        public void Log(params string[] 内容)
        {
            lock (控制台锁)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(DateTime.Now.ToString("HH:mm:ss "));
                Console.ResetColor();
                Console.Write("<");
                Console.ForegroundColor = _颜色;
                Console.Write(_名称);
                Console.ResetColor();
                Console.Write("> ");
                Console.WriteLine(string.Join("\t", 内容));
                Console.ResetColor();
            }
        }

        public void Warn(params string[] 内容)
        {
            lock (控制台锁)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(DateTime.Now.ToString("HH:mm:ss "));
                Console.ResetColor();
                Console.Write("<");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(_名称);
                Console.ResetColor();
                Console.Write("> ");
                Console.WriteLine(string.Join("\t", 内容));
                Console.ResetColor();
            }
        }

        public void Trail(params string[] 消息)
        {
            lock (控制台锁)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\t└──" + string.Join(" ", 消息.Select(c => c.ToString()).ToArray()));
                Console.ResetColor();
            }
        }

        public void Error(params string[] 内容)
        {
            lock (控制台锁)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(DateTime.Now.ToString("HH:mm:ss "));
                Console.ResetColor();
                Console.Write("<");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(_名称);
                Console.ResetColor();
                Console.Write("> ");
                Console.ForegroundColor = ConsoleColor.White;
                if (_跟踪错误)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                }
                Console.WriteLine(string.Join("\t", 内容));
                Console.ResetColor();
                if (是否调试())
                {
                    StackTrace 跟踪 = new StackTrace(true);
                    if (_跟踪错误)
                    {
                        Trail(跟踪.ToString());
                    }
                }
            }
        }

        public void Debug(params string[] 内容)
        {
            if (是否调试())
            {
                lock (控制台锁)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(DateTime.Now.ToString("HH:mm:ss "));
                    Console.ResetColor();
                    Console.Write("<");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(_名称);
                    Console.ResetColor();
                    Console.Write("> ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine(string.Join("\t", 内容));
                    Console.ResetColor();
                }
            }
        }
    }
}
