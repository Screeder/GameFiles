#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class GotoFrame2 : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.GotoFrame2;}}
		internal uint Version {get{return 4;}}
		internal uint Length
		{
			get
			{
				uint len = 4;
				if(SceneBiasFlag) len += 2;
				return len;
			}
		}

		internal uint StackPops { get { return 1; } }
		internal uint StackPushes { get { return 0; } }
		internal int StackChange { get { return -1; } }

		
		internal bool SceneBiasFlag;		
		internal bool PlayFlag;		
		internal uint SceneBias;

		internal GotoFrame2(SwfReader r)
		{
			r.GetBits(6); // reserved
			SceneBiasFlag = r.GetBit();
			PlayFlag = r.GetBit();
			r.Align();
			if (SceneBiasFlag)
			{
				SceneBias = r.GetUI16();
			}
			else
			{
				SceneBias = 0;
			}
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("gotoframe");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)ActionKind.GotoFrame2);
            w.AppendUI16(Length - 3); // don't incude def byte and len

            w.AppendBits(0, 6);
            w.AppendBit(SceneBiasFlag);
            w.AppendBit(PlayFlag);
            w.Align();
			if (SceneBiasFlag)
			{
                w.AppendUI16(SceneBias);
			}
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			string play = this.PlayFlag ? "play" : "stop";
			w.WriteLine("GotoFrame2 (stack): " + play);
        }
#endif
    }
}
