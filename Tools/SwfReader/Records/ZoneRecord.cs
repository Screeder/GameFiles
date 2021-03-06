namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal struct ZoneRecord
    {
        uint NumZoneData;
        float AlignmentCoordinate1;
		float AlignmentCoordinate2;
		float Range1;
		float Range2;
		bool ZoneMaskX;
		bool ZoneMaskY;

        internal ZoneRecord(SwfReader r)
		{
			NumZoneData = r.GetByte();
			AlignmentCoordinate1 = r.GetFixedNBits(16);
			Range1 = r.GetFixedNBits(16);
			AlignmentCoordinate2 = r.GetFixedNBits(16);
			Range2 = r.GetFixedNBits(16);

            r.GetBits(6); // reserved
			ZoneMaskX = r.GetBit();
			ZoneMaskY = r.GetBit();
			r.Align();
		}
#if SWFWRITER
        internal void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)NumZoneData);

            w.AppendFixedNBits(AlignmentCoordinate1, 16);
            w.AppendFixedNBits(Range1, 16);
            w.AppendFixedNBits(AlignmentCoordinate2, 16);
            w.AppendFixedNBits(Range2, 16);

            w.AppendBits(0, 6); // reserved
            w.AppendBit(ZoneMaskX);
            w.AppendBit(ZoneMaskY);

            w.Align();
        }
#endif
    }
}
