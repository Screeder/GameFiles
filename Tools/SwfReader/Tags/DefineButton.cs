
/* Copyright (C) 2008 Robin Debreuil -- Released under the BSD License */

using System.Collections.Generic;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class DefineButton : ISwfTag
	{
        /*
		    Header              RECORDHEADER                Tag type = 7
            ButtonId            UI16                        ID for this character
            Characters          BUTTONRECORD[one or more]   Characters that make up the button
            CharacterEndFlag    UI8                         Must be 0
            Actions             ACTIONRECORD[zero or more]  Actions to perform
            ActionEndFlag       UI8                         Must be 0
         */
        internal uint ButtonId;
        internal List<ButtonRecord> Characters;
        internal uint CharacterEndFlag;
        internal ActionRecords ActionRecords;
        internal List<IAction> Actions { get { return ActionRecords.Statements; } }


        internal DefineButton(SwfReader r)
            : base(TagType.DefineButton)
        {
            ButtonId = r.GetByte();
            while (r.PeekByte() != 0)
            {
                Characters.Add(new ButtonRecord(r, TagType.DefineButton));
            }
            r.GetByte();// 0, end ButtonRecords

            uint start = r.Position;
            ActionRecords = new ActionRecords(r, int.MaxValue);
            ActionRecords.CodeSize = r.Position - start;
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            uint start = (uint)w.Position;
            w.AppendTagIDAndLength(this.TagType, 0, true);

            w.AppendByte((byte)ButtonId);
            for (int i = 0; i < Characters.Count; i++)
            {
                Characters[i].ToSwf(w);
            }

            ActionRecords.ToSwf(w);

            w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.Write("DefineButton: ");
			w.WriteLine();
		}
#endif
	}
}
