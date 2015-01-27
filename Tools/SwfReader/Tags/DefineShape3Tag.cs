namespace LeagueSharp.GameFiles.Tools.Swf
{
	/*
		Field		Type 				Comment
		Header		RECORDHEADER		Tag type = 32.
		ShapeId		UI16				ID for this character.
		ShapeBounds RECT				Bounds of the shape.
		Shapes		SHAPEWITHSTYLE		Shape information (RGBA)
	 */
    internal class DefineShape3Tag : DefineShapeTag
	{
        internal DefineShape3Tag(SwfReader r)
            : base(r, TagType.DefineShape3)
		{
		}
	}
}
