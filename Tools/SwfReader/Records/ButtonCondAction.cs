using System.Collections.Generic;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class ButtonCondAction
    {
        /*
            CondActionSize          UI16        Offset in bytes from start of this field to next BUTTONCONDACTION, or 0 if last action
            CondIdleToOverDown      UB[1]       Idle to OverDown
            CondOutDownToIdle       UB[1]       OutDown to Idle
            CondOutDownToOverDown   UB[1]       OutDown to OverDown
            CondOverDownToOutDown   UB[1]       OverDown to OutDown
            CondOverDownToOverUp    UB[1]       OverDown to OverUp
            CondOverUpToOverDown    UB[1]       OverUp to OverDown
            CondOverUpToIdle        UB[1]       OverUp to Idle
            CondIdleToOverUp        UB[1]       Idle to OverUp

            CondKeyPress            UB[7]       SWF 4 or later: key code
                                                Otherwise: always 0
                                                Valid key codes:
                                                1 = left arrow
                                                2 = right arrow
                                                3 = home
                                                4 = end
                                                5 = insert
                                                6 = delete
                                                8 = backspace
                                                13 = enter
                                                14 = up arrow
                                                15 = down arrow
                                                16 = page up
                                                17 = page down
                                                18 = tab
                                                19 = escape
                                                32 to 126: follows ASCII

            CondOverDownToIdle  UB[1]           OverDown to Idle

            Actions             ACTIONRECORD    Actions to perform. See DoAction.
                                [zero or more]
            
            ActionEndFlag       UI8             Must be 0
         */
        internal uint CondActionSize;              
        internal bool CondIdleToOverDown;  
        internal bool CondOutDownToIdle;    
        internal bool CondOutDownToOverDown;
        internal bool CondOverDownToOutDown;
        internal bool CondOverDownToOverUp; 
        internal bool CondOverUpToOverDown; 
        internal bool CondOverUpToIdle;     
        internal bool CondIdleToOverUp; 
        internal uint CondKeyPress;
        internal bool CondOverDownToIdle;
        internal ActionRecords ActionRecords;
        internal List<IAction> Actions { get { return ActionRecords.Statements; } }

        internal ButtonCondAction(SwfReader r)
		{
            CondActionSize = r.GetUI16();
            CondIdleToOverDown = r.GetBit();
            CondOutDownToIdle = r.GetBit();
            CondOutDownToOverDown = r.GetBit();
            CondOverDownToOutDown = r.GetBit();
            CondOverDownToOverUp = r.GetBit();
            CondOverUpToOverDown = r.GetBit();
            CondOverUpToIdle = r.GetBit();
            CondIdleToOverUp = r.GetBit();
            CondKeyPress = r.GetBits(7);
            CondOverDownToIdle = r.GetBit();

            uint start = r.Position;
            ActionRecords = new ActionRecords(r, int.MaxValue);
            ActionRecords.CodeSize = r.Position - start;
        }
#if SWFWRITER
        internal void ToSwf(SwfWriter w)
        {
            w.AppendUI16(CondActionSize);
            w.AppendBit(CondIdleToOverDown);
            w.AppendBit(CondOutDownToIdle);
            w.AppendBit(CondOutDownToOverDown);
            w.AppendBit(CondOverDownToOutDown);
            w.AppendBit(CondOverDownToOverUp);
            w.AppendBit(CondOverUpToOverDown);
            w.AppendBit(CondOverUpToIdle);
            w.AppendBit(CondIdleToOverUp);
            w.AppendBits(CondKeyPress, 7);
            w.AppendBit(CondOverDownToIdle);

            ActionRecords.ToSwf(w);
        }
#endif
#if SWFDUMPER
        internal void Dump(IndentedTextWriter w)
        {
            w.Write("ButtonCondAction: ");
            w.WriteLine();
        }
#endif
    }
}
