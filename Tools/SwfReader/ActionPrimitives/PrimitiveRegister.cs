#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class PrimitiveRegister : IPrimitive
    {
        internal PrimitiveType PrimitiveType { get { return PrimitiveType.Register; } }
        internal object Value { get { return RegisterValue; } }
        internal int Length { get { return 1 + 1; } }
		internal byte RegisterValue;

		internal PrimitiveRegister(SwfReader r)
		{
			RegisterValue = r.GetByte();
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.Write("r:" + RegisterValue);
			/* 
			 * // THIS ROUTINE WILL WRITE THE ACTUAL REG VALUE (THIS, SUPER ETC) FOR WHEN ELEMENTS ARE PRELOADED
			 * 
			if (RegisterValue < 6 && DoActionTag.CurrentDumpStatement is DefineFunction2)
			{
				int pf = (int)((DefineFunction2)DoActionTag.CurrentDumpStatement).Preloads;
				int active = 0;
				uint index = 0;
				for (int pow = 1; pow < (int)PreloadFlags.End; pow*=2)
				{
					if ((pf & pow) > 0)
					{
						active = pow;
						if (++index == RegisterValue)
						{
							break;
						}
					}
				}
				if (index == RegisterValue)
				{
					w.Write(DefineFunction2.PreloadFlagToString((PreloadFlags)active));
				}
				else
				{
					w.Write("r:" + RegisterValue);
				}
			}
			else
			{
				w.Write("r:" + RegisterValue);
			}
			*/
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)PrimitiveType);
            w.AppendByte(RegisterValue);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine(RegisterValue + " ");
		}
#endif
	}
}
