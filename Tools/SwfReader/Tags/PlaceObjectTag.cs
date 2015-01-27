#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class PlaceObjectTag : IPlaceObject
	{
        internal bool HasCharacter = true;
        internal bool HasMatrix = true;
        internal bool HasColorTransform = false;

        internal uint Character;
        internal uint Depth;
        internal Matrix Matrix = Matrix.Empty;
        internal ColorTransform ColorTransform;

        internal PlaceObjectTag()
            : base(TagType.PlaceObject)
		{
		}
        internal PlaceObjectTag(SwfReader r)
            : base(TagType.PlaceObject)
		{
			Character = r.GetUI16();
			Depth = r.GetUI16();
			Matrix = new Matrix(r);
		}
        internal PlaceObjectTag(SwfReader r, uint tagEnd)
            : base(TagType.PlaceObject)
		{
			Character = r.GetUI16();
			Depth = r.GetUI16();
			Matrix = new Matrix(r);

			if (tagEnd != r.Position)
			{
				HasColorTransform = true;
				ColorTransform = new ColorTransform(r, false);
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			uint start = (uint)w.Position;
			w.AppendTagIDAndLength(this.TagType, 0, true);

			w.AppendUI16(this.Character);
			w.AppendUI16(this.Depth);
			Matrix.ToSwf(w);
			if (HasColorTransform)
			{
				ColorTransform.ToSwf(w, false);
			}

			w.ResetLongTagLength(this.TagType, start);
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.Write("PlaceObject ");
			w.Write("id:" + Character);
			w.Write(" dp:" + Depth);
			w.Write(" ");
			Matrix.Dump(w);
			if (HasColorTransform)
			{
				//ColorTransform.Dump(w);
			}
			w.WriteLine();
        }
#endif
	}
}
