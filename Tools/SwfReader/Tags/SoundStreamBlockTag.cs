#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class SoundStreamBlockTag : ISwfTag
	{
        internal uint SampleCount;
		//internal Mp3SoundData[] SoundData;
        internal byte[] SoundData;

        internal SoundStreamBlockTag(SwfReader r, uint tagLen)
            : base(TagType.SoundStreamBlock)
		{
			SampleCount = r.GetUI16();
			SoundData = r.GetBytes(tagLen - 2);

			// assume mp3 for now
			//SoundData = new Mp3SoundData[SampleCount];
			//for (int i = 0; i < SampleCount; i++)
			//{
			//    SoundData[i] = new Mp3SoundData(r);
			//}
		}

#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            uint start = (uint)w.Position;
            w.AppendTagIDAndLength(this.TagType, 0, true);

            w.AppendUI16(SampleCount);
            w.AppendBytes(SoundData);

            w.ResetLongTagLength(this.TagType, start);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("SoundStreamBlock " );
        }
#endif
    }
}
