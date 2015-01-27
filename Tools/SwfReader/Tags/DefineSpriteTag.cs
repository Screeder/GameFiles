using System;
using System.Collections.Generic;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class DefineSpriteTag : ISwfTag
	{
		/*
			Header			RECORDHEADER		Tag type = 39
			Sprite			ID UI16				Character ID of sprite
			FrameCount		UI16				Number of frames in sprite
			ControlTags		TAG[one or more]	A series of tags
		*/

        internal UInt16 CharacterId;
		internal UInt16 FrameCount;
		internal Rect ShapeBounds;
		internal List<IControlTag> ControlTags = new List<IControlTag>();

		internal List<IPlaceObject> FirstFrameObjects = new List<IPlaceObject>();
		private uint curTag;
		private uint curTagLen;

        internal DefineSpriteTag(SwfReader r, byte swfVersion)
            : base(TagType.DefineSprite)
		{
            this.CharacterId = r.GetUI16();
			this.FrameCount = r.GetUI16();
			ParseTags(r, swfVersion);
		}

		private void ParseTags(SwfReader r, byte swfVersion)
		{
			bool tagsRemain = true;

			uint curFrame = 0;
			while (tagsRemain)
			{
				uint b = r.GetUI16();
                curTag = (uint)(b >> 6);
				curTagLen = b & 0x3F;
				if (curTagLen == 0x3F)
				{
					curTagLen = r.GetUI32();
				}
				uint tagEnd = r.Position + curTagLen;
				//Debug.WriteLine("sprite type: " + ((uint)curTag).ToString("X2") + " -- " + Enum.GetName(typeof(TagType), curTag));

				switch (curTag)
				{
					case TagType.End:
						tagsRemain = false;
						ControlTags.Add(new EndTag());
						break;

					case TagType.PlaceObject:
						PlaceObjectTag pot = new PlaceObjectTag(r, tagEnd);
						FirstFrameObjects.Add(pot);
						ControlTags.Add(pot);
						break;
					case TagType.PlaceObject2:
						PlaceObject2Tag po2t = new PlaceObject2Tag(r, swfVersion);
						if (po2t.HasCharacter)
						{
							FirstFrameObjects.Add(po2t);
						}
						ControlTags.Add(po2t);
						break;
					case TagType.PlaceObject3:
						PlaceObject3Tag po3t = new PlaceObject3Tag(r);
						if (po3t.HasCharacter)
						{
							FirstFrameObjects.Add(po3t);
						}
						ControlTags.Add(po3t);
						break;

					case TagType.RemoveObject:
						ControlTags.Add(new RemoveObjectTag(r));
						break;

					case TagType.RemoveObject2:
						ControlTags.Add(new RemoveObject2Tag(r));
						break;

					case TagType.ShowFrame:
						ControlTags.Add(new ShowFrame(r));
						curFrame++;
						break;

					case TagType.SoundStreamHead:
					case TagType.SoundStreamHead2:
						ControlTags.Add(new SoundStreamHeadTag(r));
						break;

					case TagType.FrameLabel:
						ControlTags.Add(new FrameLabelTag(r));
						break;

					case TagType.DoAction:
						ControlTags.Add(new DoActionTag(r, curTagLen));
						break;

					case TagType.DoInitAction:
						ControlTags.Add(new DoActionTag(r, curTagLen, true));
						break;

					default:
						// skip if unknown
						//Debug.WriteLine("invalid sprite tag: " + ((uint)curTag).ToString("X2") + " -- " + Enum.GetName(typeof(TagType), curTag));
						r.SkipBytes(curTagLen);
						break;
				}
				if (tagEnd != r.Position)
				{
					Console.WriteLine("bad tag in sprite: " + Enum.GetName(typeof(TagType), curTag));
				}
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			uint start = (uint)w.Position;
			w.AppendTagIDAndLength(this.TagType, 0, true);

			w.AppendUI16(CharacterId);
			w.AppendUI16(FrameCount);

			for (int i = 0; i < ControlTags.Count; i++)
			{
				ControlTags[i].ToSwf(w);
			}

			// note: Flash always writes this as a long tag.
			w.ResetLongTagLength(this.TagType, start, true);
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
            w.WriteLine("DefineSprite id_" + CharacterId + " fc: " + FrameCount);
			w.Indent++;

			foreach (ISwfTag tag in ControlTags)
			{
				tag.Dump(w);
			}

			w.Indent--;
        }
#endif
    }
}
