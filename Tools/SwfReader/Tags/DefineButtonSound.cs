
/* Copyright (C) 2008 Robin Debreuil -- Released under the BSD License */

using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;

namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class DefineButtonSound : ISwfTag
	{

        /*
            ButtonId            UI16                    The ID of the button these sounds apply to.           
            ButtonSoundChar0    UI16                    Sound ID for OverUpToIdle
            ButtonSoundInfo0    SOUNDINFO (if           Sound style for OverUpToIdle
                                ButtonSoundChar0 
                                is nonzero)
            ButtonSoundChar1    UI16                    Sound ID for IdleToOverUp
            ButtonSoundInfo1    SOUNDINFO               Sound style for IdleToOverUp
                               (if ButtonSoundChar1 
                                is nonzero)            
            ButtonSoundChar2    UI16                    Sound ID for OverUpToOverDown
           
            ButtonSoundInfo2    SOUNDINFO (if           Sound style for OverUpToOverDown
                                ButtonSoundChar2 
                                is nonzero)            
            ButtonSoundChar3    UI16                    Sound ID for OverDownToOverUp           
            ButtonSoundInfo3    SOUNDINFO (if           Sound style for OverDownToOverUp
                                ButtonSoundChar3
                                is nonzero)
            
         */
        internal uint ButtonId;         
        internal uint ButtonSoundChar0; 
        internal SoundInfo ButtonSoundInfo0; 
        internal uint ButtonSoundChar1; 
        internal SoundInfo ButtonSoundInfo1; 
        internal uint ButtonSoundChar2; 
        internal SoundInfo ButtonSoundInfo2; 
        internal uint ButtonSoundChar3;
        internal SoundInfo ButtonSoundInfo3;

        internal DefineButtonSound(SwfReader r)
            : base(TagType.DefineButtonSound)
		{
            ButtonId = r.GetUI16();

            ButtonSoundChar0 = r.GetUI16();
            ButtonSoundInfo0 = new SoundInfo(r);
            ButtonSoundChar1 = r.GetUI16();
            ButtonSoundInfo1 = new SoundInfo(r);
            ButtonSoundChar2 = r.GetUI16();
            ButtonSoundInfo2 = new SoundInfo(r);
            ButtonSoundChar3 = r.GetUI16();
            ButtonSoundInfo3 = new SoundInfo(r);
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            uint start = (uint)w.Position;
            w.AppendTagIDAndLength(this.TagType, 0, true);

            w.AppendUI16(ButtonId);

            w.AppendUI16(ButtonSoundChar0);
            ButtonSoundInfo0.ToSwf(w);
            w.AppendUI16(ButtonSoundChar1);
            ButtonSoundInfo1.ToSwf(w);
            w.AppendUI16(ButtonSoundChar2);
            ButtonSoundInfo2.ToSwf(w);
            w.AppendUI16(ButtonSoundChar3);
            ButtonSoundInfo3.ToSwf(w);

            w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("DefineButtonSound: ");
			w.WriteLine();
		}
#endif
    }
}
