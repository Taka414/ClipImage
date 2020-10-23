//
// (c) 2020 Takap.
//

using System.Drawing;
using System.IO;

namespace Takap.Tools.Imaging
{
    /// <summary>
    /// 引数と処理の入力パラメータを表します。
    /// </summary>
    public class Args
    {
        public Settings Setting { get; private set; }

        public CharMap CharMap { get; private set; }

        public string ImagePath { get; private set; }

        public string DestDir { get; private set; }

        public Bitmap SourceBitmap { get; private set; }

        public static Args Create(string[] args)
        {
            var param = new Args();

            // [0] 設定JSONのファイルパス
            string settingPath = args[0];
            param.Setting = Settings.Load(settingPath);

            // [1] 文字マップのパス
            string mapPath = args[1];
            param.CharMap = CharMap.Load(mapPath);

            // [2] 切り出し対象の画像のファイルパス
            string imagePath = args[2];
            param.ImagePath = imagePath;

            // 出力先は画像パスと同じ場所に子フォルダを作成(固定)
            string parentDir = Path.GetDirectoryName(imagePath);
            string subDir = Path.GetFileNameWithoutExtension(imagePath);
            string destPath = Path.Combine(parentDir, subDir);
            param.DestDir = destPath;

            var bmp = new Bitmap(imagePath);
            param.SourceBitmap = bmp;

            return param;
        }
    }
}
