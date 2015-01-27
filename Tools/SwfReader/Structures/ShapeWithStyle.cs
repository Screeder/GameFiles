#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
using System.Collections.Generic;
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class ShapeWithStyle : IVexConvertable
	{
		/*		
			SHAPEWITHSTYLE
			Field			Type					Comment
			FillStyles		FILLSTYLEARRAY			Array of fill styles.
			LineStyles		LINESTYLEARRAY			Array of line styles.
			NumFillBits		UB[4]					Number of fill index bits.
			NumLineBits		UB[4]					Number of line index bits.
			ShapeRecords	SHAPERECORD[one or more]		
		*/

		private uint fillBits;
		private uint lineBits;
		internal FillStyleArray FillStyles;
		internal LineStyleArray LineStyles;
		internal List<IShapeRecord> ShapeRecords = new List<IShapeRecord>();

		private ShapeType shapeType;

		internal ShapeWithStyle(SwfReader r, ShapeType shapeType)
		{
			this.shapeType = shapeType;

			// parse fill defs
			FillStyles = new FillStyleArray(r, shapeType);

			// parse line defs
			LineStyles = new LineStyleArray(r, shapeType);

			r.Align();
			fillBits = r.GetBits(4);
			lineBits = r.GetBits(4);
			r.Align();

			ParseShapeRecords(r);
		}


		private void ParseShapeRecords(SwfReader r)
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
						ShapeRecords.Add(new StyleChangedRecord(r, followFlags, ref fillBits, ref lineBits, shapeType));
					}
				}
				else // edge record
				{
					bool isStraight = r.GetBit();
					if (isStraight)
					{
						ShapeRecords.Add(new StraightEdgeRecord(r));
					}
					else
					{
						ShapeRecords.Add(new CurvedEdgeRecord(r));
					}
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
			FillStyles.ToSwf(w, shapeType);
			LineStyles.ToSwf(w, shapeType);

			w.Align();
			uint fillBits = SwfWriter.MinimumBits((uint)FillStyles.FillStyles.Count);
			uint lineBits = SwfWriter.MinimumBits((uint)LineStyles.LineStyles.Count);
			w.AppendBits(fillBits, 4);
			w.AppendBits(lineBits, 4);
			w.Align();

			for (int i = 0; i < ShapeRecords.Count; i++)
			{
				ShapeRecords[i].ToSwf(w, ref fillBits, ref lineBits, shapeType);
			}
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			FillStyles.Dump(w);
			LineStyles.Dump(w);

			w.WriteLine("Records: ");
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
