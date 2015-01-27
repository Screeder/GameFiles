namespace LeagueSharp.GameFiles.Tools.Swf
{
	/*
		Field		Type 				Comment
		Header		RECORDHEADER		Tag type = 22.
		ShapeId		UI16				ID for this character.
		ShapeBounds RECT				Bounds of the shape.
		Shapes		SHAPEWITHSTYLE		Shape information
	 */
    internal class DefineShape2Tag : DefineShapeTag
	{
        internal DefineShape2Tag(SwfReader r)
            : base(r, TagType.DefineShape2)
		{
		}
    }
}
