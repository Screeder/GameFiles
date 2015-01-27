#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
using System.Collections.Generic;
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class PlaceObject3Tag : PlaceObject2Tag 
	{
        internal bool PlaceFlagHasCacheAsBitmap;
        internal bool PlaceFlagHasBlendMode;
        internal bool PlaceFlagHasFilterList;

		private BlendMode BlendMode = BlendMode.Empty;
        internal List<IFilter> FilterList;

		//internal ClipActions ClipActions;

        internal PlaceObject3Tag(SwfReader r)
        {
            tagType = TagType.PlaceObject3;

			HasClipActions = r.GetBit();
			HasClipDepth = r.GetBit();
			HasName = r.GetBit();
			HasRatio = r.GetBit();
			HasColorTransform = r.GetBit();
			HasMatrix = r.GetBit();
			HasCharacter = r.GetBit();
			Move = r.GetBit();

			r.GetBits(5); // reserved
			PlaceFlagHasCacheAsBitmap = r.GetBit();
			PlaceFlagHasBlendMode = r.GetBit();
			PlaceFlagHasFilterList = r.GetBit();

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

			if (PlaceFlagHasFilterList)
			{
				uint filterCount = (uint)r.GetByte();
				FilterList = new List<IFilter>();
				for (int i = 0; i < filterCount; i++)
				{
					FilterKind kind = (FilterKind)r.GetByte();
					switch (kind)
					{
						case FilterKind.Bevel:
							FilterList.Add(new FilterBevel(r));
							break;
						case FilterKind.Blur:
							FilterList.Add(new FilterBlur(r));
							break;
						case FilterKind.ColorMatrix:
							FilterList.Add(new FilterColorMatrix(r));
							break;
						case FilterKind.Convolution:
							FilterList.Add(new FilterConvolution(r));
							break;
						case FilterKind.DropShadow:
							FilterList.Add(new FilterDropShadow(r));
							break;
						case FilterKind.Glow:
							FilterList.Add(new FilterGlow(r));
							break;
						case FilterKind.GradientBevel:
							FilterList.Add(new FilterGradientBevel(r));
							break;
						case FilterKind.GradientGlow:
							FilterList.Add(new FilterGradientGlow(r));
							break;

						default:
							// unsupported filter
							break;
					}
				}
			}
			if (PlaceFlagHasBlendMode)
			{
				BlendMode = (BlendMode)r.GetByte();
			}


			if (HasClipActions)
			{
				//ClipActions = new ClipActions();
			}

		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
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

			w.AppendBits(0, 5); // reserved
			w.AppendBit(PlaceFlagHasCacheAsBitmap);
			w.AppendBit(PlaceFlagHasBlendMode);
			w.AppendBit(PlaceFlagHasFilterList);

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

			if (PlaceFlagHasFilterList)
			{
				w.AppendByte((byte)FilterList.Count);
				for (int i = 0; i < FilterList.Count; i++)
				{
					FilterList[i].ToSwf(w);
				}
			}
			if (PlaceFlagHasBlendMode)
			{
				w.AppendByte((byte)BlendMode);
			}


			if (HasClipActions)
			{
				//todo: ClipActions = new ClipActions();
			}

			w.ResetLongTagLength(this.TagType, start);
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("PlaceObject3 ");
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
