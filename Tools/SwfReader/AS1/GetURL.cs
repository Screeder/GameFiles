#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class GetURL : IAction
	{
		internal ActionKind ActionId{get{return ActionKind.GetURL;}}
		internal uint Version {get{return 3;}}
		internal uint Length
		{
			get
			{
                return (uint)(3 + UrlString.Length + 1 + TargetString.Length + 1);
			}
		}		
		internal string UrlString;		
		internal string TargetString;

		internal GetURL(SwfReader r)
		{
			UrlString = r.GetString();
			TargetString = r.GetString();
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("geturl");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.GetURL);
            w.AppendUI16(Length - 3);// don't incude this part

            w.AppendString(UrlString);
            w.AppendString(TargetString);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("GetUrl: " + UrlString + " targ: " + TargetString);
        }
#endif
    }
}
