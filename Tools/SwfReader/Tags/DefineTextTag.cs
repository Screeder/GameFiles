using System.Collections.Generic;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif

namespace LeagueSharp.GameFiles.Tools.Swf
{
	/*
		Header				RECORDHEADER		Tag type = 11.
		CharacterID			UI16				ID for this text character.
		TextBounds			RECT				Bounds of the text.
		TextMatrix			MATRIX				Transformation matrix for the text.
		GlyphBits			UI8					Bits in each glyph index.
		AdvanceBits			UI8					Bits in each advance value.
		TextRecords			TEXTRECORD[0+]		Text records.
		EndOfRecordsFlag	UI8					Must be 0.
	 */
	internal class DefineTextTag : ISwfTag
	{
		internal uint CharacterId;
		internal Rect TextBounds;
		internal Matrix TextMatrix;		
		//internal uint GlyphBits;	
		//internal uint AdvanceBits;	
		internal List<TextRecord> TextRecords = new List<TextRecord>();
		//internal uint EndOfRecordsFlag;

        private uint glyphBits;
        private uint advanceBits;

        internal DefineTextTag(SwfReader r, bool useAlpha)
            : base(TagType.DefineText)
		{
			if (useAlpha)
			{
				tagType = TagType.DefineText2;
			}
			CharacterId = r.GetUI16();
			TextBounds = new Rect(r);
			TextMatrix = new Matrix(r);
			glyphBits = (uint)r.GetByte();
			advanceBits = (uint)r.GetByte();

			while (r.PeekByte() != 0x00)
			{
				TextRecords.Add(new TextRecord(r, glyphBits, advanceBits, useAlpha));
			}
			byte end = r.GetByte();
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            uint start = (uint)w.Position;
            w.AppendTagIDAndLength(this.TagType, 0, true);

            w.AppendUI16(CharacterId); 
			TextBounds.ToSwf(w);
			TextMatrix.ToSwf(w);

            w.AppendByte((byte)glyphBits); // TODO: gen nbits
            w.AppendByte((byte)advanceBits); // TODO: gen nbits

            for (int i = 0; i < TextRecords.Count; i++)
            {
                TextRecords[i].ToSwf(w, glyphBits, advanceBits, tagType >= TagType.DefineText2);
            }

            w.AppendByte(0); // end

            w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("DefineText: ");
			w.WriteLine();
		}
#endif
    }
}
