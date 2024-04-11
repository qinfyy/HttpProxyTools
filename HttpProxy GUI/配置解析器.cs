using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HttpProxy.GUI
{
    public class 配置解析器
    {
        private Dictionary<string, string> 键值对;
        private string 文件路径;

        public 配置解析器(string 文件路径参数)
        {
            键值对 = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            文件路径 = 文件路径参数;
        }

        public void 解析()
        {
            string[] 所有行 = File.ReadAllLines(文件路径);

            for (int i = 0; i < 所有行.Length; i++)
            {
                string 行 = 所有行[i];
                string 修剪行 = 行.Trim();

                //跳过注释
                if (string.IsNullOrEmpty(修剪行) || 修剪行.StartsWith(";") || 修剪行.StartsWith("#"))
                {
                    continue;
                }
                else
                {
                    string[] 部分 = 修剪行.Split('=');
                    if (部分.Length == 2)
                    {
                        string 键 = 部分[0].Trim();
                        string 值 = 部分[1].Trim();

                        键值对[键] = 值;
                    }
                }
            }
        }

        public string 获取值(string 键)
        {
            if (键值对.ContainsKey(键))
            {
                return 键值对[键];
            }
            else
            {
                return null;
            }
        }

        public void 写入值(string 键, string 值)
        {
            键值对[键] = 值;

            List<string> 所有行 = new List<string>();

            for (int i = 0; i < 键值对.Count; i++)
            {
                KeyValuePair<string, string> 当前键值对 = 键值对.ElementAt(i);
                所有行.Add($"{当前键值对.Key}={当前键值对.Value}");
            }

             File.WriteAllLines(文件路径, 所有行);
        }
    }
}
