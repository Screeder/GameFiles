#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	/*
	 StartSound
		Header		RECORDHEADER	Tag type = 15.
		SoundId		UI16			ID of sound character to play.
		SoundInfo	SOUNDINFO		Sound style information.
	 */


	internal class StartSoundTag : IControlTag
	{
		internal uint SoundId;
        internal SoundInfo SoundInfo;

        internal StartSoundTag(SwfReader r)
            : base(TagType.StartSound)
        {
			SoundId = r.GetUI16();
            SoundInfo = new SoundInfo(r);
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            uint start = (uint)w.Position;
            w.AppendTagIDAndLength(this.TagType, 0, true);

            w.AppendUI16(SoundId);
            SoundInfo.ToSwf(w);

            w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("Start Sound");
        }
#endif
    }
}
