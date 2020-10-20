//
// (c) 2020 Takap.
//

using System.Drawing;
using System.IO;

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
                int ypos = param.Setting.MarginTop;
                if (y != 0)
                {
                    ypos += (param.Setting.CharHeight + param.Setting.CharMarginVertical) * y;
                }

                if (ypos > param.SourceBitmap.Height)
                {
                    break;
                }

                for (int x = 0; true; x++)
                {
                    int xpos = param.Setting.MarginLeft;
                    if (x != 0)
                    {
                        xpos += (param.Setting.CharWidth + param.Setting.CharMarginHorizontal) * x;
                    }

                    if (xpos > param.SourceBitmap.Width)
                    {
                        break;
                    }

                    // 対応する
                    if (!param.CharMap.TryGetChar(x, y, out char _c))
                    {
                        continue;
                    }

                    // 等幅と仮定して切り抜く
                    var rect = new Rectangle(xpos, ypos, param.Setting.CharWidth, param.Setting.CharHeight);
                    Bitmap clipedBmp = param.SourceBitmap.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    if (_c != ' ')
                    {
                        // 横幅の空白を削除する(高さはそのまま)
                        int left = filndLeft(clipedBmp);
                        int right = findRight(clipedBmp);
                        var rect2 = new Rectangle(left, 0, clipedBmp.Width - left - (clipedBmp.Width - 1 - right), clipedBmp.Height);
                        if (rect2.Width != rect.Width)
                        {
                            clipedBmp = clipedBmp.Clone(rect2, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        }
                    }

                    // ファイル名は16進数 Asciiコード
                    string fileName = ((byte)_c).ToString("X");
                    clipedBmp.Save(Path.Combine(param.DestDir, fileName + ".png"));
                }
            }
        }

        // 左からの有効なピクセル開始位置
        private static int filndLeft(Bitmap bmp)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    var c = bmp.GetPixel(x, y);
                    if (c.A != 0)
                    {
                        return x; // 半透明のピクセルが見つかれば有効とする
                    }
                }
            }
            return 0;
        }

        // 右からの有効なピクセル開始位置
        private static int findRight(Bitmap bmp)
        {
            for (int x = bmp.Width - 1; x >= 0; x--)
            {
                for (int y = bmp.Height - 1; y >= 0; y--)
                {
                    var c = bmp.GetPixel(x, y);
                    if (c.A != 0)
                    {
                        return x;
                    }
                }
            }
            return 0;
        }
    }
}
