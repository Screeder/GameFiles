#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class LineStyle2 : ILineStyle
	{
		/*
			LINESTYLE2
			Field				Type				Comment
		 
			Width				UI16				Width of line in twips.			
			StartCapStyle		UB[2]				Start cap style:
													 0 = Round cap
													 1 = No cap
													 2 = Square cap
			JoinStyle			UB[2]				Join style:
													 0 = Round join
													 1 = Bevel join
													 2 = Miter join		 
			HasFillFlag			UB[1]				If 1, fill is defined in FillType.
		 											If 0, uses Color field.	
			NoHScaleFlag		UB[1]				If 1, stroke thickness will not scale if the object is scaled horizontally.			
			NoVScaleFlag		UB[1]				If 1, stroke thickness will not scale if the object is scaled vertically.			
			PixelHintingFlag	UB[1]				If 1, all anchors will be aligned to full pixels.			
			Reserved			UB[5]				Must be 0.			
			NoClose				UB[1]				If 1, stroke will not be closed if the stroke’s last point matches its first point. 
													Flash Player will apply caps instead of a join.			
			EndCapStyle			UB[2]				End cap style:
													 0 = Round cap
													 1 = No cap
													 2 = Square cap			
			MiterLimitFactor	If JoinStyle=2		Miter limit factor is an 8.8 fixed-point value.
		  						UI16						  
			Color				If HasFillFlag=0	Color value including alpha channel.
								RGBA			  
			FillType			If HasFillFlag=1	 Fill style for this stroke.
								FILLSTYLE			
		 */

		internal ushort Width;
		internal CapStyle StartCapStyle;
		internal JoinStyle JoinStyle;
		internal bool HasFillFlag;
		internal bool NoHScaleFlag;
		internal bool NoVScaleFlag;
		internal bool PixelHintingFlag;
		internal bool NoClose;
		internal CapStyle EndCapStyle;
		internal float MiterLimitFactor;
		internal RGBA Color;
		internal IFillStyle FillStyle;

		internal LineStyle2(SwfReader r, ShapeType shapeType)
		{
			this.Width = r.GetUI16();
			this.StartCapStyle = (CapStyle)r.GetBits(2);
			this.JoinStyle = (JoinStyle)r.GetBits(2);
			this.HasFillFlag = r.GetBit();
			this.NoHScaleFlag = r.GetBit();
			this.NoVScaleFlag = r.GetBit();
			this.PixelHintingFlag = r.GetBit();
			r.GetBits(5); // skip
			this.NoClose = r.GetBit();
			this.EndCapStyle = (CapStyle)r.GetBits(2);

			if (this.JoinStyle == JoinStyle.MiterJoin)
			{
				this.MiterLimitFactor = (float)((r.GetByte() / 0x100) + r.GetByte());
			}

			if (this.HasFillFlag)
			{
				this.FillStyle = FillStyleArray.ParseFillStyle2(r, shapeType);
			}
			else
			{
				this.Color = new RGBA(r.GetByte(), r.GetByte(), r.GetByte(), r.GetByte());
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			ToSwf(w, true);
		}
		internal override void ToSwf(SwfWriter w, bool useAlpha)
		{
			w.AppendUI16(this.Width);
			w.AppendBits((uint)this.StartCapStyle, 2);
			w.AppendBits((uint)this.JoinStyle, 2);
			w.AppendBit(this.HasFillFlag);
			w.AppendBit(this.NoHScaleFlag);
			w.AppendBit(this.NoVScaleFlag);
			w.AppendBit(this.PixelHintingFlag);
			w.AppendBits(0, 5);
			w.AppendBit(this.NoClose);
			w.AppendBits((uint)this.EndCapStyle, 2);

			if (this.JoinStyle == JoinStyle.MiterJoin)
			{
				w.AppendFixed8_8(this.MiterLimitFactor);
			}

			if (this.HasFillFlag)
			{
				this.FillStyle.ToSwf(w);
			}
			else
			{
				w.AppendByte(Color.R);
				w.AppendByte(Color.G);
				w.AppendByte(Color.B);
				if (useAlpha)
				{
					w.AppendByte(Color.A);
				}
			}
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.Write("LS2: ");

			if (this.HasFillFlag)
			{
				w.Write(this.FillStyle.ToString());
			}
			else
			{
				this.Color.Dump(w);
			}

			w.Write("w:" + this.Width);
			w.Write(" startCap:" + System.Enum.GetName(typeof(CapStyle), this.StartCapStyle));
            w.Write(" joinStyle:" + System.Enum.GetName(typeof(JoinStyle), this.JoinStyle));
			
			w.Write(" flags:");
			w.Write(HasFillFlag ? "Fill " : "");
			w.Write(NoHScaleFlag ? "HScale " : "");
			w.Write(NoVScaleFlag ? "VScale " : "");
			w.Write(PixelHintingFlag ? "PixelHint " : "");
			w.Write(NoClose ? "NoClose " : "");
			w.Write("  ");

            w.Write("endCap:" + System.Enum.GetName(typeof(CapStyle), this.EndCapStyle));

			if (this.JoinStyle == JoinStyle.MiterJoin)
			{
				w.Write("miter limit:" + this.MiterLimitFactor);
			}
		}
#endif
	}
}
