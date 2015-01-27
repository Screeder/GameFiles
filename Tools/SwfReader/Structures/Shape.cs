using System.Collections.Generic;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class Shape : IVexConvertable
	{
		/*		
			SHAPE
			Field			Type					Comment
			NumFillBits		UB[4]					Number of fill index bits.
			NumLineBits		UB[4]					Number of line index bits.
			ShapeRecords	SHAPERECORD[one or more]		
		*/

		private uint fillBits;
		private uint lineBits;
		internal List<IShapeRecord> ShapeRecords = new List<IShapeRecord>();

        // only used for glyphs
		internal Shape(SwfReader r)
		{
			r.Align();
			fillBits = r.GetBits(4); // always one
			lineBits = r.GetBits(4); // always zero

			ParseShapeRecords(r);
		}


		internal void ParseShapeRecords(SwfReader r)
		{
			bool hasMoreRecords = true;
			while (hasMoreRecords)
			{
				bool typeFlag = r.GetBit();

				if (typeFlag == false) // non edge record
				{
					uint followFlags = r.GetBits(5);
					if (followFlags == 0)
					{
						ShapeRecords.Add(new EndShapeRecord());
						hasMoreRecords = false;
					}
					else
					{
						StyleChangedRecord scr = new StyleChangedRecord(r, followFlags, ref fillBits, ref lineBits, ShapeType.Glyph);
						ShapeRecords.Add(scr);
					}
				}
				else // edge record
				{
					bool isStraight = r.GetBit();
					if (isStraight)		
					{
						StraightEdgeRecord ser = new StraightEdgeRecord(r);
						ShapeRecords.Add(ser);
					}
					else
					{
						CurvedEdgeRecord cer = new CurvedEdgeRecord(r);
						ShapeRecords.Add(cer);
					}
				}
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.Align();
            w.AppendBits(fillBits, 4); // always 1
            w.AppendBits(lineBits, 4); // always 0

            for (int i = 0; i < ShapeRecords.Count; i++)
            {
                ShapeRecords[i].ToSwf(w, ref fillBits, ref lineBits, ShapeType.Glyph);
            }
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("Glyph Records: ");
			w.Indent++;
			for (int i = 0; i < ShapeRecords.Count; i++)
			{
				ShapeRecords[i].Dump(w);
				w.WriteLine();
			}
			w.Indent--;
		}
#endif
	}
}
