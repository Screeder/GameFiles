#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class BitmapFill : IFillStyle
	{
		internal uint CharacterId;
		internal Matrix Matrix;

		internal BitmapFill(uint characterId, Matrix matrix, FillType fillType)
		{
			this.CharacterId = characterId;
			this.Matrix = matrix;
			this.fillType = fillType;
        }
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			throw new System.NotSupportedException("Use overload that specifies alpha.");
		}

		internal override void ToSwf(SwfWriter w, bool useAlpha)
		{
            w.AppendByte((byte)fillType);
            w.AppendUI16(CharacterId);
            Matrix.ToSwf(w);
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.Write("Bitmap Fill id_" + this.CharacterId + " type: ");
            w.Write(System.Enum.GetName(typeof(FillType), this.FillType));
			w.Write(" ");
			Matrix.Dump(w);
		}
#endif
    }
}