#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class FilterColorMatrix : IFilter
	{
		internal FilterKind FilterKind { get { return FilterKind.ColorMatrix; } }

		float[] Matrix;
		internal FilterColorMatrix(SwfReader r)
		{
			this.Matrix = new float[20];
			for (int i = 0; i < 20; i++)
			{
				this.Matrix[i] = r.GetFloat32();
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine(this);
        }
#endif
    }
}
