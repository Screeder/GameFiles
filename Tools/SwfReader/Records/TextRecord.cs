namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class TextRecord
	{
		private bool _TextRecordType;			// UB[1]	
		private uint _StyleFlagsReserved;		// UB[3]
        internal bool StyleFlagsHasFont;		// UB[1]
        internal bool StyleFlagsHasColor;		// UB[1]
        internal bool StyleFlagsHasYOffset;	// UB[1]
        internal bool StyleFlagsHasXOffset;	// UB[1]
        internal uint FontID;					// If StyleFlagsHasFont UI16
        internal RGBA TextColor;				// If StyleFlagsHasColor RGB. If this record is part of a DefineText2 tag then RGBA
        internal int XOffset;					// If StyleFlagsHasXOffset SI16
        internal int YOffset;					// If StyleFlagsHasYOffset SI16
        internal uint TextHeight;				// If hasFont UI16
        internal uint GlyphCount;				// UI8
        internal GlyphEntry[] GlyphEntries;	// GLYPHENTRY[GlyphCount]

        internal TextRecord(SwfReader r, uint glyphBits, uint advanceBits, bool hasAlpha)
		{
			_TextRecordType = r.GetBit();
			_StyleFlagsReserved = r.GetBits(3);
			StyleFlagsHasFont = r.GetBit(); 
			StyleFlagsHasColor = r.GetBit(); 
			StyleFlagsHasYOffset = r.GetBit(); 
			StyleFlagsHasXOffset = r.GetBit(); 

			if(StyleFlagsHasFont)
			{
				FontID = r.GetUI16();
			}
			if(StyleFlagsHasColor)
			{
				TextColor = new RGBA(r.GetByte(), r.GetByte(), r.GetByte());
				if(hasAlpha)
				{
					TextColor.A = r.GetByte();
				}
			}
			if(StyleFlagsHasXOffset)
			{
				XOffset = r.GetInt16();
			}
			if(StyleFlagsHasYOffset)
			{
				YOffset = r.GetInt16();
			}
			if(StyleFlagsHasFont)
			{
				TextHeight = r.GetUI16();
			}

			GlyphCount = (uint)r.GetByte();
			GlyphEntries = new GlyphEntry[GlyphCount];
			for (int i = 0; i < GlyphCount; i++)
			{
				uint index = r.GetBits(glyphBits);
				int advance = r.GetSignedNBits(advanceBits);
				GlyphEntries[i] = new GlyphEntry(index, advance);
			}
            r.Align();//
		}
#if SWFWRITER
        internal void ToSwf(SwfWriter w, uint glyphBits, uint advanceBits, bool hasAlpha)
        {
            w.AppendBit(_TextRecordType);
            w.AppendBits(_StyleFlagsReserved, 3);
            w.AppendBit(StyleFlagsHasFont);
            w.AppendBit(StyleFlagsHasColor);
            w.AppendBit(StyleFlagsHasYOffset);
            w.AppendBit(StyleFlagsHasXOffset);
            w.Align();

            if (StyleFlagsHasFont)
            {
                w.AppendUI16(FontID);
            }
            if (StyleFlagsHasColor)
            {
                w.AppendByte(TextColor.R);
                w.AppendByte(TextColor.G);
                w.AppendByte(TextColor.B);
                if (hasAlpha)
                {
                    w.AppendByte(TextColor.A);
                }
            }
            if (StyleFlagsHasXOffset)
            {
                w.AppendInt16(XOffset);
            }
            if (StyleFlagsHasYOffset)
            {
                w.AppendInt16(YOffset);
            }
            if (StyleFlagsHasFont)
            {
                w.AppendUI16(TextHeight);
            }

            w.AppendByte((byte)GlyphEntries.Length);

            for (int i = 0; i < GlyphEntries.Length; i++)
            {
                w.AppendBits(GlyphEntries[i].GlyphIndex, glyphBits);
                w.AppendSignedNBits(GlyphEntries[i].GlyphAdvance, advanceBits);
            }
            w.Align();
        }
#endif
    }
}














