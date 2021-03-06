#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	/*
		Header			RECORDHEADER	Tag type = 74.
		TextID			UI16			ID for the DefineText,DefineText2, or DefineEditText to which this tag applies.
	 	UseFlashType	UB[2]			0 = use normal renderer.
	 									1 = use FlashType renderer.
		GridFit			UB[3]			0 = Do not use grid fitting. AlignmentZones and LCD sub-pixel information will not be used. 
	 									1 = Pixel grid fit. Only supported for left-aligned dynamic text. This setting provides the ultimate in FlashType readability, with crisp letters aligned to pixels. 
	 									2 = Sub-pixel grid fit. Align letters to the 1/3 pixel used by LCD monitors. Can also improve quality for CRT output. 
		Reserved		UB[3]			Must be 0. 
		Thickness		F32				The thickness attribute for the associated text field. Set to 0.0 to use the default (anti-aliasing table) value. 
		Sharpness		F32				The sharpness attribute for the associated text field. Set to 0.0 to use the default (anti-aliasing table) value. 
		Reserved		UI8				Must be 0.	 
	 */
	internal class CSMTextSettingsTag : ISwfTag
	{
		internal uint  TextId;
		internal uint  UseFlashType;
		internal uint  GridFit;
		internal float Thickness;
		internal float Sharpness;

        internal CSMTextSettingsTag(SwfReader r)
            : base(TagType.CSMTextSettings)
		{
			TextId = r.GetUI16();
			UseFlashType = r.GetBits(2);
			GridFit = r.GetBits(3);
			r.GetBits(3); // reserved
			r.Align();

			Thickness = r.GetFixedNBits(32);
			Sharpness = r.GetFixedNBits(32);

			r.GetByte(); // reserved
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            uint len = 12;
            w.AppendTagIDAndLength(this.TagType, len, false);

            w.AppendUI16(TextId);
            w.AppendBits(UseFlashType, 2);
            w.AppendBits(GridFit, 3);
            w.AppendBits(0, 3); // reserved
            w.Align();

            w.AppendFixedNBits(Thickness, 32);
            w.AppendFixedNBits(Sharpness, 32);

            w.AppendByte(0); // reserved
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("CSMTextSettings: ");
			w.WriteLine();
        }
#endif
    }
}



















