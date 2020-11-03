//
// (c) 2020 Takap.
//

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Takap.Tools.Imaging
{
    /// <summary>
    /// 特定の形式の画像をくりぬくためのツール
    /// </summary>
    internal class AppMain
    {
        // >>>>>>
        //
        // 引数:
        //  [0] 設定JSONのファイルパス
        //  [1] 文字マップのパス
        //  [2] 切り出し対象の画像のファイルパス
        //
        // <<<<<

        public static void Main(string[] args)
        {
            var param = Args.Create(args);

            // 保存先はなければ作成
            if (!Directory.Exists(param.DestDir))
            {
                Directory.CreateDirectory(param.DestDir);
            }

            // 左上 → 右下に等幅な大きさで画像を切り抜いていく

            for (int y = 0; true; y++)
            {
                // 1行の高さは固定
                int ypos = param.Setting.MarginTop;
                if (y != 0)
                {
                    ypos += (param.Setting.CharHeight + param.Setting.CharMarginVertical) * y;
                }

                if (ypos > param.SourceBitmap.Height)
                {
                    break;
                }

                // 水平方向は自動で分割
                int x = 0;
                for (int xpos = param.Setting.MarginLeft; xpos < param.SourceBitmap.Width;)
                {
                    if (!getRectangle(param.SourceBitmap, xpos, ypos, param.Setting.CharHeight, out Rectangle rect))
                    {
                        break;
                    }

                    Console.WriteLine(rect.ToString());

                    // 対応する文字をマップから取り出す
                    if (!param.CharMap.TryGetChar(x++, y, out char _c))
                    {
                        break;
                    }

                    // 画像から領域を切り抜いて保存
                    Bitmap clipedBmp = param.SourceBitmap.Clone(rect, PixelFormat.Format32bppArgb);
                    clipedBmp.Save(createSavePath(param, _c));

                    xpos = rect.X + rect.Width;
                }
            }

            // 空白文字を生成
            if (param.Setting.NeedsSpace)
            {
                var bmp = createSpace(param.Setting.SpaceWidth, param.Setting.CharHeight);
                bmp.Save(createSavePath(param, ' '));
            }
        }

        // 指定した位置から左方向に走査して有効なビットマップ中の有効な文字領域を取得する
        // true : 矩形を選択できた / false : それ以外(右端に到達)
        private static bool getRectangle(Bitmap bmp, int x, int y, int height, out Rectangle rect)
        {
            int beginX = x;
            int width = 1;
            rect = new Rectangle();

            // 開始位置の検索
            for (int xp = x; xp < bmp.Width; xp++, beginX++)
            {
                if (isValidPixel(bmp, xp, y, height))
                {
                    break;
                }
            }

            if (beginX >= bmp.Width)
            {
                return false;
            }

            // 幅の選択
            for (int xp = beginX + 1; xp < bmp.Width; xp++, width++)
            {
                if (!isValidPixel(bmp, xp, y, height) && !isValidPixel(bmp, xp + 1, y, height))
                {
                    break; // 最低2px以上で違う文字と認識する
                }
            }

            if (x + width >= bmp.Width)
            {
                return false;
            }

            rect.X = beginX;
            rect.Y = y;
            rect.Width = width;
            rect.Height = height;

            return true;
        }

        // 指定した位置を上端として下側にheight分の範囲に有効な行が存在するかどうか確認する
        // true : 存在する / false : それ以外
        //   0 1 2
        // 0 0 0 a ↓ こういう方向で検索する 
        // 1 0 0 a ↓
        // 2 0 0 a ↓
        private static bool isValidPixel(Bitmap bmp, int x, int y, int height)
        {
            for (int yp = 0; yp < height; yp++)
            {
                var c = bmp.GetPixel(x, y + yp);
                if (c.A != 0)
                {
                    return true; // 不透明ピクセルは有効
                }
            }
            return false;
        }

        // 画像の保存パスを作成する
        private static string createSavePath(Args param, char c)
        {
            //string fileName = ((byte)c).ToString("X"); // ファイル名は16進数 Asciiコード
            string fileName = convertByteStr(c); // ファイル名は16進数 UTF8コード
            string path = Path.Combine(param.DestDir, fileName + ".png");
            Console.WriteLine($"{c} -> {Path.GetFileName(path)}");
            return path;
        }

        // 文字をUTF8の16進数文字列に変換する
        private static string convertByteStr(char c)
        {
            // バイト配列をUTF8の数値文字列に変換する
            byte[] bs = Encoding.UTF8.GetBytes(new[] { c });
            string str = "";
            foreach (var b in bs)
            {
                str += b.ToString("X");
            }
            //Console.WriteLine($"  ->{name}");
            return str;
        }

        // 空白文字を画像として生成する
        private static Bitmap createSpace(int width, int height)
        {
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bmp.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                }
            }
            return bmp;
        }
    }
}
