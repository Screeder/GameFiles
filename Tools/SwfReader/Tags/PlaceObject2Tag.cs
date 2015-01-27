#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class PlaceObject2Tag : PlaceObjectTag 
	{
		/*
			Field 						Type 							Comment
			Header 						RECORDHEADER 					Tag type = 26
			PlaceFlagHasClipActions 	UB[1] 							SWF 5 and later: has clip actions (sprite characters only)
																		Otherwise: always 0
			PlaceFlagHasClipDepth 		UB[1] 							Has clip depth
			PlaceFlagHasName 			UB[1] 							Has name
			PlaceFlagHasRatio 			UB[1] 							Has ratio
			PlaceFlagHasColorTransform 	UB[1] 							Has color transform
			PlaceFlagHasMatrix 			UB[1] 							Has matrix
			PlaceFlagHasCharacter 		UB[1] 							Places a character
			PlaceFlagMove 				UB[1] 							Defines a character to be moved
			Depth 						UI16 							Depth of character
		 
			CharacterId 				If PlaceFlagHasCharacter		ID of character to place
											UI16

			Matrix 						If PlaceFlagHasMatrix			Transform matrix data
											MATRIX
											
			ColorTransform 				If								Color transform data
										PlaceFlagHasColorTransform
										CXFORMWITHALPHA

			Ratio 						If PlaceFlagHasRatio UI16

			Name 						If PlaceFlagHasName				Name of character
										STRING
										
			ClipDepth 					If PlaceFlagHasClipDepth UI16 	Clip depth

			ClipActions 				If PlaceFlagHasClipActions		SWF 5 and later:
										CLIPACTIONS						Clip Actions Data
		*/
        internal bool HasClipActions;
        internal bool HasClipDepth;
        internal bool HasName;
        internal bool HasRatio;
        internal bool Move;

        internal uint Ratio;
        internal string Name;
        internal uint ClipDepth;

        internal ClipActions ClipActions;

        internal PlaceObject2Tag()
        {
            tagType = TagType.PlaceObject2;
		}
        internal PlaceObject2Tag(SwfReader r, byte swfVersion)
		{
			tagType = TagType.PlaceObject2;
			HasClipActions = r.GetBit();
			HasClipDepth = r.GetBit();
			HasName = r.GetBit();
			HasRatio = r.GetBit();
			HasColorTransform = r.GetBit();
			HasMatrix = r.GetBit();
			HasCharacter = r.GetBit();
			Move = r.GetBit();

			Depth = r.GetUI16();

			if (HasCharacter)
			{
				Character = r.GetUI16();
			}
			if (HasMatrix)
			{
				Matrix = new Matrix(r);
			}
			if (HasColorTransform)
			{
				ColorTransform = new ColorTransform(r, true);
			}
			if (HasRatio)
			{
				Ratio = r.GetUI16();
			}
			if (HasName)
			{
				Name = r.GetString();
			}
			if (HasClipDepth)
			{
				ClipDepth = r.GetUI16();
			}

			if (HasClipActions)
			{
				ClipActions = new ClipActions(r, (swfVersion > 5));
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			ToSwf(w, true);
		}
		internal void ToSwf(SwfWriter w, bool isSwf6Plus)
		{
			uint start = (uint)w.Position;
			w.AppendTagIDAndLength(this.TagType, 0, true);

			w.AppendBit(HasClipActions);
			w.AppendBit(HasClipDepth);
			w.AppendBit(HasName);
			w.AppendBit(HasRatio);
			w.AppendBit(HasColorTransform);
			w.AppendBit(HasMatrix);
			w.AppendBit(HasCharacter);
			w.AppendBit(Move);

			w.AppendUI16(Depth);

			if (HasCharacter)
			{
				w.AppendUI16(Character);
			}
			if (HasMatrix)
			{
				Matrix.ToSwf(w);
			}
			if (HasColorTransform)
			{
				ColorTransform.ToSwf(w, true);
			}
			if (HasRatio)
			{
				w.AppendUI16(Ratio);
			}
			if (HasName)
			{
				w.AppendString(Name);
			}
			if (HasClipDepth)
			{
				w.AppendUI16(ClipDepth);
			}

			if (HasClipActions)
			{
				ClipActions.ToSwf(w, isSwf6Plus);
			}

            //w.ResetLongTagLength(this.TagType, start, true);
            // this may be always long tag?
            if (HasClipActions || HasName)
            {
                w.ResetLongTagLength(this.TagType, start, true); // flash always makes long tags is clip actions are present
            }
            else
            {
                w.ResetLongTagLength(this.TagType, start);
            }
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("PlaceObject2 ");
			if (HasCharacter)
			{
				w.Write("id:" + Character);
			}
			w.Write(" dp:" + Depth);
			if (HasMatrix)
			{
				w.Write(" ");
				Matrix.Dump(w);
			}
			if (HasColorTransform)
			{
				//ColorTransform.Dump(w);
			}
			if (HasRatio)
			{
				w.Write(" r:" + Ratio);
			}
			if (HasName)
			{
				w.Write(" n:" + Name);
			}
			if (HasClipDepth)
			{
				w.Write(" cd:" + ClipDepth);
			}
			if (HasClipActions)
			{
				//ClipActions.Dump(w);
			}
			w.WriteLine();
        }
#endif
    }
}
