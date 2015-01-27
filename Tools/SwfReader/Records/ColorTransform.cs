#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	/*
		CXFORMWITHALPHA
		HasAddTerms			UB[1] 				Has color addition values if
		equal to 1
		HasMultTerms		UB[1] 				Has color multiply values if
		equal to 1
		Nbits				UB[4] 				Bits in each value field
		RedMultTerm			If HasMultTerms=1 
	 						SB[Nbits] 			Red multiply value
		GreenMultTerm 		If HasMultTerms=1 
	 						SB[Nbits] 			Green multiply value
		BlueMultTerm 		If HasMultTerms=1 
	 						SB[Nbits] 			Blue multiply value
		AlphaMultTerm 		If HasMultTerms=1 
	 						SB[Nbits] 			Alpha multiply value
		RedAddTerm 			If HasAddTerms=1 
	 						SB[Nbits] 			Red addition value
		GreenAddTerm 		If HasAddTerms=1 
	 						SB[Nbits] 			Green addition value 
		BlueAddTerm 		If HasAddTerms=1 
	 						SB[Nbits] 			Blue addition value
		AlphaAddTerm 		If HasAddTerms=1 
	 						SB[Nbits]			Transparency addition value
	 */
	internal class ColorTransform : IRecord
	{
		internal bool HasAddTerms;
		internal bool HasMultTerms;

		internal int RMultTerm;
		internal int GMultTerm;
		internal int BMultTerm;
		internal int AMultTerm;

		internal int RAddTerm;
		internal int GAddTerm;
		internal int BAddTerm;
		internal int AAddTerm;

		internal ColorTransform(SwfReader r, bool useAlpha)
		{
			this.HasAddTerms = r.GetBit();
			this.HasMultTerms = r.GetBit();
			uint nbits = r.GetBits(4);

			if (HasMultTerms)
			{
				RMultTerm = r.GetSignedNBits(nbits);
				GMultTerm = r.GetSignedNBits(nbits);
				BMultTerm = r.GetSignedNBits(nbits);
				AMultTerm = (useAlpha) ? r.GetSignedNBits(nbits) : 0xFF;
			}
			else
			{
				RMultTerm = 0;
				GMultTerm = 0;
				BMultTerm = 0;
				AMultTerm = 0;
			}
			if (HasAddTerms)
			{
				RAddTerm = r.GetSignedNBits(nbits);
				GAddTerm = r.GetSignedNBits(nbits);
				BAddTerm = r.GetSignedNBits(nbits);
				AAddTerm = (useAlpha) ? r.GetSignedNBits(nbits) : 0xFF;
			}
			else
			{
				RAddTerm = 0;
				GAddTerm = 0;
				BAddTerm = 0;
				AAddTerm = 0;
			}
			r.Align();
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			throw new System.NotSupportedException("Use overload that specifies alpha.");
		}

		internal void ToSwf(SwfWriter w, bool useAlpha)
		{
			w.AppendBit(HasAddTerms);
			w.AppendBit(HasMultTerms);
			uint bits = SwfWriter.MinimumBits(RMultTerm, GMultTerm, BMultTerm, AMultTerm, RAddTerm, GAddTerm, BAddTerm, AAddTerm);
			w.AppendBits(bits, 4);

			if (HasMultTerms)
			{
				w.AppendSignedNBits(RMultTerm, bits);
				w.AppendSignedNBits(GMultTerm, bits);
				w.AppendSignedNBits(BMultTerm, bits);
				if (useAlpha)
				{
					w.AppendSignedNBits(AMultTerm, bits);
				}
			}
			if (HasAddTerms)
			{
				w.AppendSignedNBits(RAddTerm, bits);
				w.AppendSignedNBits(GAddTerm, bits);
				w.AppendSignedNBits(BAddTerm, bits);
				if (useAlpha)
				{
					w.AppendSignedNBits(AAddTerm, bits);
				}
			}

			w.Align();
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.Write("CXForm [");

			if (HasMultTerms)
			{
				w.Write(" *" + this.RMultTerm.ToString("X2"));
				w.Write(" *" + this.GMultTerm.ToString("X2"));
				w.Write(" *" + this.BMultTerm.ToString("X2"));
				w.Write(" *" + this.AMultTerm.ToString("X2"));
			}
			if (HasAddTerms)
			{
				w.Write(" +" + this.RAddTerm.ToString("X2"));
				w.Write(" +" + this.GAddTerm.ToString("X2"));
				w.Write(" +" + this.BAddTerm.ToString("X2"));
				w.Write(" +" + this.AAddTerm.ToString("X2"));
			}

			w.WriteLine("]");
        }
#endif
    }
}
