//
// (c) 2020 Takap.
//

using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Takap.Tools.Imaging
{
    [DataContract]
    public class Settings
    {
        [DataMember(Name = "margin-top")]
        public int MarginTop { get; set; }

        [DataMember(Name = "margin-left")]
        public int MarginLeft { get; set; }

        [DataMember(Name = "char-margin-vertical")]
        public int CharMarginVertical { get; set; }

        [DataMember(Name = "char-height")]
        public int CharHeight { get; set; }

        [DataMember(Name = "needs-space-img")]
        public bool NeedsSpace { get; set; }

        [DataMember(Name = "space-width")]
        public int SpaceWidth { get; set; }

        // 指定したファイルパスからオブジェクトを作成する
        public static Settings Load(string filePath)
        {
            return JsonSerializer.DeserializingJsonFronString<Settings>(readJsonExcludeComment(filePath));
        }

        // コメント行を除いてファイルを読み込む
        private static string readJsonExcludeComment(string filePath)
        {
            var sb = new StringBuilder();
            using (var sr = new StreamReader(filePath))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue; // 空行は無視
                    }

                    if (line.Contains("//"))
                    {
                        continue; // '//' が含まれていたらコメント
                    }
                    sb.AppendLine(line);
                }
            }
            return sb.ToString();
        }
    }
}
