#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	/*
		Field		Type 				Comment
		Header		RECORDHEADER		Tag type = 2.
		ShapeId		UI16				ID for this character.
		ShapeBounds RECT				Bounds of the shape.
		Shapes		SHAPEWITHSTYLE		Shape information
	 */
    internal class DefineShapeTag : ISwfTag
	{
        internal uint CharacterId;
        internal Rect ShapeBounds;
        internal ShapeWithStyle Shapes;

        internal DefineShapeTag(SwfReader r)
            : base(TagType.DefineShape)
		{
            this.CharacterId = r.GetUI16();
			this.ShapeBounds = new Rect(r);
            this.Shapes = new ShapeWithStyle(r, ShapeType.DefineShape1);
			r.Align();
		}
        internal DefineShapeTag(SwfReader r, uint tagType)
            : base(tagType)
        {
            this.CharacterId = r.GetUI16();
            this.ShapeBounds = new Rect(r);
            if (tagType == TagType.DefineShape)
            {
                this.Shapes = new ShapeWithStyle(r, ShapeType.DefineShape1);
            }
            else if (tagType == TagType.DefineShape2)
            {
                this.Shapes = new ShapeWithStyle(r, ShapeType.DefineShape2);
            }
            else if (tagType == TagType.DefineShape3)
            {
                this.Shapes = new ShapeWithStyle(r, ShapeType.DefineShape3);
            }
            r.Align();
        }

#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			uint start = (uint)w.Position;
			w.AppendTagIDAndLength(this.TagType, 0, true); // rewrite len after tag 

			w.AppendUI16(this.CharacterId);
			this.ShapeBounds.ToSwf(w);
            if (tagType == TagType.DefineShape)
            {
                this.Shapes.ToSwf(w, ShapeType.DefineShape1);
            }
            else if (tagType == TagType.DefineShape2)
            {
                this.Shapes.ToSwf(w, ShapeType.DefineShape2);
            }
            else if (tagType == TagType.DefineShape3)
            {
                this.Shapes.ToSwf(w, ShapeType.DefineShape3);
            }
			w.Align();

			w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
            w.Write("DefineShape");
            if (tagType == TagType.DefineShape2)
            {
                w.Write("2");
            }
            else if (tagType == TagType.DefineShape3)
            {
                w.Write("3");
            }
            else if (tagType == TagType.DefineShape4)
            {
                w.Write("4");
            }
            w.WriteLine(" id_" + CharacterId + ":");
			w.Indent++;

			w.Write("shape bounds:");
			this.ShapeBounds.Dump(w);
			w.WriteLine();
			this.Shapes.Dump(w);

			w.WriteLine();
			w.Indent--;
        }
#endif
    }
}
