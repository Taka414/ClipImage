//
// (c) 2020 Takap.
//

using System;
using System.Collections.Generic;
using System.IO;

namespace Takap.Tools.Imaging
{
    /// <summary>
    /// どういう風に文字が並んでいるかを表します。
    /// </summary>
    public class CharMap
    {
        private readonly List<List<char>> items = new List<List<char>>();

        public bool TryGetChar(int x, int y, out char c)
        {
            c = default;
            try
            {
                c = this.items[y][x];
                return true;
            }
            catch (Exception) // 範囲外は例外で処理
            {
                return false;
            }
        }

        public static CharMap Load(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            var map = new CharMap();
            foreach (var line in lines)
            {
                string trimedLine = line.Trim();
                if (string.IsNullOrEmpty(trimedLine) || trimedLine.StartsWith("!"))
                {
                    continue; // コメント行、空行の読み飛ばし
                }

                var subList = new List<char>();
                map.items.Add(subList);

                string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int p = 0; p < parts.Length; p++)
                {
                    subList.Add(parts[p][0]);
                }
            }

            return map;
        }
    }
}
