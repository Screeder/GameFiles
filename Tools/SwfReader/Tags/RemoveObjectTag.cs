#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class RemoveObjectTag : IControlTag
	{
		internal uint Character;
		internal uint Depth;

        internal RemoveObjectTag(SwfReader r)
            : base(TagType.RemoveObject)
		{
			this.Character = r.GetUI16();
			this.Depth = r.GetUI16();
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			w.AppendTagIDAndLength(this.TagType, 2);
			w.AppendUI16(Character);
			w.AppendUI16(Depth);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("RemoveObject: " + this.Depth);
		}
#endif
    }
}
