#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
using System.Collections.Generic;
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class DoActionTag : IControlTag
	{
		internal ActionRecords ActionRecords;

        private bool isInitTag = false;
        internal bool IsInitTag { get { return isInitTag; } }

        internal DoActionTag()
            : base(TagType.DoAction)
		{
			ActionRecords = new ActionRecords();
		}
		internal DoActionTag(SwfReader r, uint tagLen) : this(r, tagLen, false)
		{
		}
        internal DoActionTag(SwfReader r, uint tagLen, bool isInitTag)
            : base(TagType.DoAction)
		{
            this.isInitTag = isInitTag;
			ActionRecords = new ActionRecords(r, tagLen, isInitTag);
        }
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            if (!isInitTag)
            {
                w.AppendTagIDAndLength(this.TagType, ActionRecords.CodeSize, true);
            }
            else
            {
                w.AppendTagIDAndLength(TagType.DoInitAction, ActionRecords.CodeSize, true);
            }
            ActionRecords.ToSwf(w);
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.Write("DoActionTag: ");
			ActionRecords.Dump(w);
		}
#endif
    }
}
