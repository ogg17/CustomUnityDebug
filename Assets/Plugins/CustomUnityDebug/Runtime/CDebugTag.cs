using System.Text;
using UnityEngine;

namespace Masev.CustomUnityDebug
{
    /// <summary>
    /// Debug Tag Class
    /// </summary>
    public class CDebugTag
    {
        /// <summary>
        /// Responsible for displaying this debug tag
        /// </summary>
        public bool Enabled { get; set; } = true;
        public string TagStr { get; }

        public CDebugTag(string tag, Color32 color, bool bold, bool italic)
        {
            StringBuilder sb = new();

            if (bold) sb.Append("<b>");
            if (italic) sb.Append("<i>");
            sb.Append("<color=")
                .Append('#')
                .Append(color.r.ToString("X2"))
                .Append(color.g.ToString("X2"))
                .Append(color.b.ToString("X2"))
                .Append(">")
                .Append("[").Append(tag).Append("]")
                .Append("</color>");
            if (italic) sb.Append("</i>");
            if (bold) sb.Append("</b>");

            TagStr = sb.ToString();
        }

        public static implicit operator string(CDebugTag type) => type.TagStr;
    }
}