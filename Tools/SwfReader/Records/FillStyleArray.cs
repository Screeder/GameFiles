using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class FillStyleArray : IRecord
	{
		internal List<IFillStyle> FillStyles = new List<IFillStyle>();

		internal FillStyleArray()
		{
		}
		internal FillStyleArray(SwfReader r, ShapeType shapeType)
		{
			int fillCount = (int)r.GetByte();
			if (fillCount == 0xFF)
			{
				fillCount = (int)r.GetUI16();
			}

			for (int i = 0; i < fillCount; i++)
			{
				FillStyles.Add(ParseFillStyle2(r, shapeType));
			}
		}

		internal static IFillStyle ParseFillStyle2(SwfReader r, ShapeType shapeType)
		{
			IFillStyle result = null;

			FillType fsType = (FillType)r.GetByte();
			bool useAlpha = shapeType > ShapeType.DefineShape2;
			switch (fsType)
			{
				case FillType.Solid:
					result = new SolidFill(r, useAlpha);
					break;

				case FillType.Linear:
					result = new Gradient(r, fsType, useAlpha);
					break;

				case FillType.Radial:
					result = new Gradient(r, fsType, useAlpha);
					break;

				case FillType.Focal:
					result = null;
					//throw new NotSupportedException("Currently FillType.Focal is not supported");
					break;

				case FillType.RepeatingBitmap:
				case FillType.ClippedBitmap:
				case FillType.NSRepeatingBitmap:
				case FillType.NSClippedBitmap:
					uint charId = r.GetUI16();
					Matrix bmpMatrix = new Matrix(r);
					result = new BitmapFill(charId, bmpMatrix, fsType);
					break;
			}
			return result;
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			throw new NotSupportedException("Use overload that specifies shapeType.");
		}
		internal void ToSwf(SwfWriter w, ShapeType shapeType)
		{
			if (FillStyles.Count > 0xFE)
			{
				w.AppendByte(0xFF);
				w.AppendUI16((uint)FillStyles.Count);
			}
			else
			{
				w.AppendByte((byte)FillStyles.Count);
			}

			bool useAlpha = shapeType > ShapeType.DefineShape2;
			for (int i = 0; i < FillStyles.Count; i++)
			{
				FillStyles[i].ToSwf(w, useAlpha);
			}
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("Fill Styles: ");
			w.Indent++;
			for (int i = 0; i < FillStyles.Count; i++)
			{
				FillStyles[i].Dump(w);
				w.WriteLine();
			}
			w.Indent--;
		}
#endif
    }
}
