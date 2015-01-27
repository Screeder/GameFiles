#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    /*
     SOUNDINFO
        Reserved		UB[2]					Always 0.
        SyncStop		UB[1]					Stop the sound now.
        SyncNoMultiple	UB[1]					Don’t start the sound if already playing.
        HasEnvelope		UB[1]					Has envelope information.
        HasLoops		UB[1]					Has loop information.
        HasOutPoint		UB[1]					Has out-point information.
        HasInPoint		UB[1]					Has in-point information.
        InPoint			If HasInPoint UI32		Number of samples to skip at beginning of sound.
        OutPoint		If HasOutPoint UI32		Position in samples of last sample to play.
        LoopCount		If HasLoops UI16		Sound loop count.
        EnvPoints		If HasEnvelope UI8		Sound Envelope point count.
        EnvelopeRecords	If HasEnvelope			Sound Envelope records.
                        SOUNDENVELOPE[EnvPoints]	
    */
    internal class SoundInfo
    {
        internal bool IsSyncStop;
        internal bool IsSyncNoMultiple;
        internal bool HasEnvelope;
        internal bool HasLoops;
        internal bool HasOutPoint;
        internal bool HasInPoint;

        internal uint InPoint;
        internal uint OutPoint;
        internal uint LoopCount;
        //internal uint EnvPoints;
        internal SoundEnvelope[] EnvelopeRecords;

        internal SoundInfo(SwfReader r)
        {
			r.GetBits(2); // reserved

			IsSyncStop = r.GetBit();
			IsSyncNoMultiple = r.GetBit();
			HasEnvelope = r.GetBit();
			HasLoops = r.GetBit();
			HasOutPoint = r.GetBit();

			r.Align();
			if(HasInPoint)
			{
				InPoint = r.GetUI32();
			}
			if(HasOutPoint)
			{
				OutPoint = r.GetUI32();
			}
			if(HasLoops)
			{
				LoopCount = r.GetUI16();
			}
			if(HasEnvelope)
			{
				uint count = (uint)r.GetByte();

				uint pos;
				uint left;
				uint right;
				EnvelopeRecords = new SoundEnvelope[count];
				for (int i = 0; i < count; i++)
				{
					pos = r.GetUI32();
					left = r.GetUI16();
					right = r.GetUI16();
					EnvelopeRecords[i] = new SoundEnvelope(pos, left, right);					
				}
            }
        }
#if SWFWRITER
        internal void ToSwf(SwfWriter w)
        {
            w.AppendBits(0, 2); // reserved
            w.AppendBit(IsSyncStop);
            w.AppendBit(IsSyncNoMultiple);
            w.AppendBit(HasEnvelope);
            w.AppendBit(HasLoops);
            w.AppendBit(HasOutPoint);
            w.Align();

            if (HasInPoint)
            {
                w.AppendUI32(InPoint);
            }
            if (HasOutPoint)
            {
                w.AppendUI32(OutPoint);
            }
            if (HasLoops)
            {
                w.AppendUI16(LoopCount);
            }
            if (HasEnvelope)
            {
                w.AppendByte((byte)LoopCount);
                uint count = (uint)EnvelopeRecords.Length;
                for (int i = 0; i < EnvelopeRecords.Length; i++)
                {
                    w.AppendUI32(EnvelopeRecords[i].Pos44);
                    w.AppendUI16(EnvelopeRecords[i].LeftLevel);
                    w.AppendUI16(EnvelopeRecords[i].RightLevel);
                }
            }
        }
#endif
#if SWFDUMPER
        internal void Dump(IndentedTextWriter w)
        {
            w.WriteLine("SoundInfo: ");
        }
#endif
    }
}
