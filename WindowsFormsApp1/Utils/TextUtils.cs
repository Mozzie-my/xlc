﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Utils
{
    public static class TextUtils
    {
        public static string ExtractLink(string text)
        {
            string linkPattern = @"https?://\S+";
            Match linkMatch = Regex.Match(text, linkPattern);
            return linkMatch.Success ? linkMatch.Value : null;
        }
        public static string ExtractPicGuid(string text)
        {
            string picPattern = @"\[pic={([A-Fa-f0-9\-]+)}\.(\w+)\]";
            Match linkMatch = Regex.Match(text, picPattern);
            return linkMatch.Success ? linkMatch.Value : null;
        }

        public static string ExtractTaobaoCode(string text)
        {
            string taobaoCodePattern = @"[￥#*({][A-Za-z0-9]+[￥#*)}]";
            ;
            Match taobaoCodeMatch = Regex.Match(text, taobaoCodePattern);
            return taobaoCodeMatch.Success ? taobaoCodeMatch.Value : null;
        }
        public static string ReplaceLinkAndTaobaoCode(string text, string newLink, string newTaobaoCode)
        {
            string oldLink = ExtractLink(text);
            string oldTaobaoCode = ExtractTaobaoCode(text);

            if (oldTaobaoCode != null)
            {
                // 删除淘口令所在的整行
                int taobaoCodeStartIndex = text.IndexOf(oldTaobaoCode);
                int lineStartIndex = text.LastIndexOf('\n', taobaoCodeStartIndex);
                int lineEndIndex = text.IndexOf('\n', taobaoCodeStartIndex);

                if (lineStartIndex != -1)
                {
                    text = text.Remove(lineStartIndex, oldTaobaoCode.Length);
                }

                // 在淘口令前添加一个换行，插入新的淘口令，并在淘口令和链接之间添加一个换行
                text = text.Insert(taobaoCodeStartIndex, newTaobaoCode);
            }
            if (oldLink != null)
            {
                // 删除淘口令所在的整行
                int LinkStartIndex = text.IndexOf(oldLink);
                int lineStartIndex = text.LastIndexOf('\n', LinkStartIndex);
                int lineEndIndex = text.IndexOf('\n', LinkStartIndex);

                if (lineStartIndex != -1)
                {
                    text = text.Remove(lineStartIndex, oldLink.Length);
                }

                // 在淘口令前添加一个换行，插入新的淘口令，并在淘口令和链接之间添加一个换行
                text = text.Insert(LinkStartIndex, newLink);
            }

            return text;
        }
        // 转换成表情
        public static string ReplaceEmojisWithHex(string text)
        {
            // 正则表达式匹配 [emoji=XXXXXX] 格式，其中XXXXXX是十六进制字符  
            string pattern = @"\[emoji=([A-Fa-f0-9]{6})\]";

            // 使用正则表达式匹配并替换文本中的emoji  
            return Regex.Replace(text, pattern, match =>
            {
                // 获取匹配到的十六进制字符串  
                string hexString = match.Groups[1].Value;

                // 将十六进制字符串转换为字节数组  
                byte[] bytes = StringToByteArray(hexString);

                // 将字节数组转换为UTF-8字符串  
                string emoji = Encoding.UTF8.GetString(bytes);

                // 返回替换后的emoji字符串  
                return emoji;
            });
        }

        // 辅助方法：将十六进制字符串转换为字节数组  
        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

    }



}
