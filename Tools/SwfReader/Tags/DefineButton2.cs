using System.Collections.Generic;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class DefineButton2 : ISwfTag
	{
        /*
            Header              RECORDHEADER    Tag type = 34
            ButtonId            UI16            ID for this character
            ReservedFlags       UB[7]           Always 0
            TrackAsMenu         UB[1]           0 = track as normal button
                                                1 = track as menu button
            ActionOffset        UI16            Offset in bytes from start of this
                                                field to the first
                                                BUTTONCONDACTION, or 0
                                                if no actions occur
            Characters          BUTTONRECORD    Characters that make up the button
                                [one or more]
           
            CharacterEndFlag    UI8                 Must be 0
            Actions             BUTTONCONDACTION    Actions to execute at particular button events
                                [zero or more]
                                
            
         */
        internal uint ButtonId;
        internal bool TrackAsMenu;
        internal uint ActionOffset;
        internal List<ButtonRecord> Characters = new List<ButtonRecord>();
        internal List<ButtonCondAction> ButtonCondActions = new List<ButtonCondAction>();

        internal DefineButton2(SwfReader r)
            : base(TagType.DefineButton2)
        {
            ButtonId = r.GetUI16();
            r.GetBits(7);
            TrackAsMenu = r.GetBit();
            ActionOffset = r.GetUI16();

            while (r.PeekByte() != 0)
            {
                Characters.Add(new ButtonRecord(r, TagType.DefineButton2));
            }
            r.GetByte();// 0, end ButtonRecords

            if (ActionOffset > 0)
            {
                ButtonCondAction bca;
                do
                {
                    bca = new ButtonCondAction(r);
                    ButtonCondActions.Add(bca);
                }
                while (bca.CondActionSize > 0);
            }
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            uint start = (uint)w.Position;
            w.AppendTagIDAndLength(this.TagType, 0, true);

            w.AppendUI16(ButtonId);
            w.AppendBits(0, 7);
            w.AppendBit(TrackAsMenu);
            w.AppendUI16(ActionOffset); // todo: calc offset

            for (int i = 0; i < Characters.Count; i++)
            {
                Characters[i].ToSwf(w);
            }
            w.AppendByte(0);

            if(ActionOffset > 0)
            {
                ButtonCondActions[ButtonCondActions.Count - 1].CondActionSize = 0;
                for (int i = 0; i < ButtonCondActions.Count; i++)
                {
                    ButtonCondActions[i].ToSwf(w);
                }
            }

            w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("DefineButton2: ");
			w.WriteLine();
        }
#endif
    }
}
