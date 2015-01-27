#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class ClipActionRecord : IVexConvertable
	{
		internal ClipEvents ClipEvents;
		internal uint ActionRecordSize;
		internal byte KeyCode;
		internal ActionRecords ActionRecords;
		
		internal ClipActionRecord(SwfReader r) : this(r, true)
		{
		}
		internal ClipActionRecord(SwfReader r, bool isSwf6Plus)
		{
			uint highClip = r.GetBits(16) << 16;
			uint lowClip = 0;
			bool isEndRecord = false;
			if (highClip == 0)
			{
				if (isSwf6Plus)
				{
					lowClip = r.GetBits(16);
					if (lowClip == 0)
					{
						ClipEvents = (ClipEvents)0;
						ActionRecordSize = 4;
						isEndRecord = true;
					}
				}
				else
				{
					ClipEvents = (ClipEvents)0;
					ActionRecordSize = 2;
					isEndRecord = true;
				}
			}
			else
			{
				lowClip = r.GetBits(16);
			}

			if (!isEndRecord)
			{
				ClipEvents = (ClipEvents)(lowClip | highClip);
				ActionRecordSize = r.GetUI32();
				if ((ClipEvents & ClipEvents.KeyPress) > 0)
				{
					KeyCode = r.GetByte();
				}
				ActionRecords = new ActionRecords(r, ActionRecordSize); // always is init tag?
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			ToSwf(w, true);
		}
		internal void ToSwf(SwfWriter w, bool isSwf6Plus)
		{
			if ((uint)ClipEvents == 0)
			{
				if (isSwf6Plus)
				{
					w.AppendUI32(0);
				}
				else
				{
					w.AppendUI16(0);
				}
			}
			else
			{
				w.AppendBits((uint)ClipEvents, 32);

				uint start = (uint)w.Position;
				w.AppendUI32(0); // write len after tag written

				if ((ClipEvents & ClipEvents.KeyPress) > 0)
				{
					w.AppendByte(KeyCode);
				}
				ActionRecords.ToSwf(w);

				uint end = (uint)w.Position;
				w.Position = start;
				w.AppendUI32(end - start - 4);
				w.Position = end;
			}
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("ClipActionRecord: ");
        }
#endif
    }
}
