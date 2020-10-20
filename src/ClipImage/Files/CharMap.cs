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
            for (int i = 0; i < lines.Length; i++)
            {
                var subList = new List<char>();
                map.items.Add(subList);

                string[] parts = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int p = 0; p < parts.Length; p++)
                {
                    if (parts[p] == "SP") // SPは空白扱いにする
                    {
                        parts[p] = " ";
                    }

                    subList.Add(parts[p][0]);
                }
            }

            return map;
        }
    }
}
