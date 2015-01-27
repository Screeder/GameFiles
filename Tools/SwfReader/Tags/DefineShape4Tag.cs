using System;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class DefineShape4Tag : ISwfTag
	{
		/*
			DefineShape4
			Field					Type			Comment
			Header					RECORDHEADER	Tag type = 83.
			ShapeId					UI16			ID for this character.
			ShapeBounds				RECT			Bounds of the shape.
			EdgeBounds				RECT			Bounds of the shape, excluding strokes.
			Reserved				UB[6]			Must be 0.
			UsesNonScalingStrokes	UB[1]			If 1, the shape contains at least one non-scaling stroke.
			UsesScalingStrokes		UB[1]			If 1, the shape contains at least one scaling stroke.
			Shapes					SHAPEWITHSTYLE	Shape information.
		*/
        internal UInt16 CharacterId;
		internal Rect ShapeBounds;
		internal Rect EdgeBounds;
		internal bool UsesNonScalingStrokes;
		internal bool UsesScalingStrokes;
		internal ShapeWithStyle Shapes;

        internal DefineShape4Tag(SwfReader r)
            : base(TagType.DefineShape4)
		{
            this.CharacterId = r.GetUI16();
			this.ShapeBounds = new Rect(r);
			this.EdgeBounds = new Rect(r);

			r.SkipBits(6);
			this.UsesNonScalingStrokes = r.GetBit();
			this.UsesScalingStrokes = r.GetBit();
			this.Shapes = new ShapeWithStyle(r, ShapeType.DefineShape4);
			r.Align();
		}

#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			uint start = (uint)w.Position;
			w.AppendTagIDAndLength(this.TagType, 0, true); // rewrite len after tag 

			w.AppendUI16(this.CharacterId);
			this.ShapeBounds.ToSwf(w);
			this.EdgeBounds.ToSwf(w);

			w.AppendBits(0, 6);
			w.AppendBit(this.UsesNonScalingStrokes);
			w.AppendBit(this.UsesScalingStrokes);
			this.Shapes.ToSwf(w, ShapeType.DefineShape4);
			w.Align();

			w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
            w.WriteLine("DefineShape4 id_" + CharacterId + ":");
			w.Indent++;

			w.Write("shape bounds:");
			this.ShapeBounds.Dump(w);
			w.WriteLine();

			w.Write("edge bounds:");
			this.EdgeBounds.Dump(w);
			w.WriteLine();

			if (UsesNonScalingStrokes)
			{
				w.Write("Uses non scaling strokes. ");
			}
			else
			{
				w.Write("No non scaling strokes. ");
			}

			if (UsesScalingStrokes)
			{
				w.Write("Uses scaling strokes. ");
			}
			else
			{
				w.Write("No scaling strokes. ");
			}
			
			w.WriteLine();
			this.Shapes.Dump(w);

			w.WriteLine();
			w.Indent--;
        }
#endif
    }
}