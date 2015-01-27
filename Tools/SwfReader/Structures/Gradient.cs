using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class Gradient : IFillStyle
	{
		/*
			GradientMatrix		MATRIX					Matrix for gradient fill.
			SpreadMode			UB[2] 
								0 = Pad mode
								1 = Reflect mode
								2 = Repeat mode
								3 = Reserved
			InterpolationMode	UB[2] 
								0 = Normal RGB mode interpolation
								1 = Linear RGB mode interpolation
								2 and 3 = Reserved
			NumGradients		UB[4]					1 to 15
			GradientRecords		GRADRECORD[nGrads]		Gradient records (see following)
		 */

		internal static readonly Rect GradientBaseRect = new Rect(-16384, -16384, 32768, 32768);

		internal Matrix GradientMatrix;
		internal SpreadMode SpreadMode;
		internal InterpolationMode InterpolationMode;
		internal List<GradientRecord> Records;

		internal Gradient(SwfReader r, FillType fillType, bool useAlpha)
		{
			Records = new List<GradientRecord>();
			this.fillType = fillType;

			GradientMatrix = new Matrix(r);
			SpreadMode = (SpreadMode)r.GetBits(2);
			InterpolationMode = (InterpolationMode)(r.GetBits(2) & 1);
			uint numGradients = r.GetBits(4);
			for (int i = 0; i < numGradients; i++)
			{
				Records.Add(new GradientRecord(r, useAlpha));
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			throw new NotSupportedException("Use overload that specifies alpha.");
		}

		internal override void ToSwf(SwfWriter w, bool useAlpha)
		{
			w.AppendByte((byte)this.FillType);

			GradientMatrix.ToSwf(w);
			w.AppendBits((uint)SpreadMode, 2);
			w.AppendBits((uint)InterpolationMode, 2);

			w.AppendBits((uint)Records.Count, 4);

			for (int i = 0; i < Records.Count; i++)
			{
				Records[i].ToSwf(w, useAlpha);
			}
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("Gradient Fill: ");
			w.Write(Enum.GetName(typeof(FillType), this.FillType));
			w.Write(" mx:");
			this.GradientMatrix.Dump(w);
			w.Write(" cnt:" + Records.Count);
			w.Write("|" + Enum.GetName(typeof(SpreadMode), this.SpreadMode));
			w.Write("|" + Enum.GetName(typeof(InterpolationMode), this.InterpolationMode));
			for (int i = 0; i < Records.Count; i++)
			{
				Records[i].Dump(w);
			}
		}
#endif
    }
}
