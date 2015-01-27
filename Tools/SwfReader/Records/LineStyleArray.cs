using System.Collections.Generic;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class LineStyleArray : IRecord
	{
		internal List<ILineStyle> LineStyles = new List<ILineStyle>();

		internal LineStyleArray()
		{
		}
		internal LineStyleArray(SwfReader r, ShapeType shapeType)
		{
			int lineCount = (int)r.GetByte();
			if (lineCount == 0xFF)
			{
				lineCount = (int)r.GetUI16();
			}

			for (int i = 0; i < lineCount; i++)
			{
				ParseLineStyle(r, shapeType);
			}
		}

		private void ParseLineStyle(SwfReader r, ShapeType shapeType)
		{
			if (shapeType > ShapeType.DefineShape3)
			{
				LineStyles.Add(new LineStyle2(r, shapeType));
			}
			else
			{
				if (shapeType > ShapeType.DefineShape2)
				{
					LineStyles.Add(new LineStyle(r, true));
				}
				else
				{
					LineStyles.Add(new LineStyle(r, false));
				}
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			throw new System.NotSupportedException("Use overload that specifies shapeType.");
		}
		internal void ToSwf(SwfWriter w, ShapeType shapeType)
		{
			if (LineStyles.Count > 0xFE)
			{
				w.AppendByte(0xFF);
				w.AppendUI16((uint)LineStyles.Count);
			}
			else
			{
				w.AppendByte((byte)LineStyles.Count);
			}

			bool useAlpha = shapeType > ShapeType.DefineShape2;
			for (int i = 0; i < LineStyles.Count; i++)
			{
				LineStyles[i].ToSwf(w, useAlpha);
			}
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("Line Styles: ");
			w.Indent++;
			for (int i = 0; i < LineStyles.Count; i++)
			{
				LineStyles[i].Dump(w);
				w.WriteLine();
			}
			w.Indent--;
        }
#endif
    }
}
