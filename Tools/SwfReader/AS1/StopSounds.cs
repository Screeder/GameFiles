/* Copyright (C) 2008 Robin Debreuil -- Released under the BSD License */

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;

namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class StopSounds : IAction
	{
		internal ActionKind ActionId{get{return ActionKind.StopSounds;}}
		internal uint Version {get{return 3;}}
		internal uint Length { get { return 1; } }
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("stopsounds");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.StopSounds);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("stopAllSounds");
		}
#endif
    }
}
