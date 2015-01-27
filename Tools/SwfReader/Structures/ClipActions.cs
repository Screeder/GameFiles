#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
using System.Collections.Generic;
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class ClipActions : IVexConvertable
	{
		internal ClipEvents ClipEvents;
		internal List<ClipActionRecord> ClipActionRecords;

		internal ClipActions(SwfReader r) : this(r, true)
		{
		}
		internal ClipActions(SwfReader r, bool isSwf6Plus)
		{
			r.GetUI16(); // reserved
			ClipEvents = (ClipEvents)r.GetBits(32);
			ClipActionRecords = new List<ClipActionRecord>();

			bool hasMoreRecords = true;
			while (hasMoreRecords)
			{
				ClipActionRecord car = new ClipActionRecord(r, isSwf6Plus);
				ClipActionRecords.Add(car);
				if ((uint)car.ClipEvents == 0)
				{
					hasMoreRecords = false;
				}
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			ToSwf(w, true);
		}
		internal void ToSwf(SwfWriter w, bool isSwf6Plus)
		{
			w.AppendUI16(0); // reserved
			w.AppendBits((uint)ClipEvents, 32);

			for (int i = 0; i < ClipActionRecords.Count; i++)
			{
				ClipActionRecords[i].ToSwf(w, isSwf6Plus);
			}
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("ClipActions: ");
		}
#endif
	}
}
