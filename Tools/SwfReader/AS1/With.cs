using System.Collections.Generic;
#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class With : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.With;}}
		internal uint Version { get { return 5; } }
		internal uint Length
		{
			get
			{
				uint len = 3 + 2;
				len += (uint)WithBlock.Length + 1;
				return len;
			}
		}
        internal IActionContainer ActionContainer;
		internal uint StackPops { get { return 1; } }
		internal uint StackPushes { get { return 0; } }
		internal int StackChange { get { return -1; } }
        /*
		private uint codeSize;
        public uint CodeSize { get { return codeSize; } set { codeSize = value; } }
		private List<IAction> statements = new List<IAction>();
        public List<IAction> Statements { get { return statements; } }
        */
		internal uint Size
		{
			get
			{
                if (ActionContainer.Statements == null) return 0;
                return (uint)ActionContainer.Statements.Count;
			}
		}
		
		internal string WithBlock;
		
		internal List<IAction>[] ActionCollections
		{
			get
			{
				List<IAction>[] actions =
                    new List<IAction>[1] { ActionContainer.Statements };
				return actions;
			}
		}
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("with");
        }
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.With);
            w.AppendUI16(Length - 3); // don't incude def byte and len

            // todo: ctor missing
		}
#endif
#if SWFDUMPER
        internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine(System.Enum.GetName(typeof(ActionKind), this.ActionId));
		}
#endif
    }
}
