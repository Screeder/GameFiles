using System.Collections.Generic;
#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class DefineFunction : IStackManipulator
	{
		/*
			FunctionName STRING
			NumParams UI16
			param 1 STRING
			param 2 STRING
			...
			param N STRING
			codeSize UI16
		 */
        internal IActionContainer ActionContainer;
		internal ActionKind ActionId{get{return ActionKind.DefineFunction;}}
		internal uint Version {get{return 5;}}
		internal uint Length
		{
			get
			{
				uint len = 7;
				len += (uint)FunctionName.Length + 1;
				for(int i = 0; i < Params.Length; i++)
				{
					len += (uint)Params[i].Length + 1;
				}
				return len;
			}
		}

		internal uint StackPops { get { return 0; } }
		internal uint StackPushes { get { return 0; } }
		internal int StackChange { get { return 0; } }

		private uint codeSize;
		internal uint CodeSize { get { return codeSize; } set { codeSize = value; } } // todo: calc code size
		private List<IAction> statements = new List<IAction>();
		internal List<IAction> Statements { get { return statements; } }

		internal string FunctionName;
		internal string[] Params;

		private ConstantPool cp;

		internal DefineFunction(SwfReader r, ConstantPool cp)
		{
			this.cp = cp;
			FunctionName = r.GetString();
			uint paramCount = r.GetUI16();
			Params = new string[paramCount];
			for (int i = 0; i < paramCount; i++)
			{
				Params[i] = r.GetString();
			}

            CodeSize = r.GetUI16();
		}

#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			w.WriteLine("function '" + FunctionName + "' " + ActionRecords.GetLabel((int)CodeSize)); // todo: function end code label
			ActionRecords.CurrentConstantPool = cp;
			for (int i = 0; i < Statements.Count; i++)
			{
				ActionRecords.AutoLineLabel(w);
				Statements[i].ToFlashAsm(w);
			}
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
        {
            w.AppendByte((byte)ActionKind.DefineFunction);
            w.AppendUI16(Length - 3); // don't incude def byte and len

            w.AppendString(FunctionName);
            w.AppendUI16((uint)Params.Length);
            for (int i = 0; i < Params.Length; i++)
            {
                w.AppendString(Params[i]);
            }

            w.AppendUI16(CodeSize); // temp
            long startPos = w.Position;

            for (int i = 0; i < Statements.Count; i++)
            {
                Statements[i].ToSwf(w);
            }

            // adjust code size
            long curPos = w.Position;
            if (codeSize != (curPos - startPos))
            {
                codeSize = (uint)(curPos - startPos);
                w.Position = startPos - 2;
                w.AppendUI16(CodeSize); // acutal
                w.Position = curPos;
            }
		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
            w.WriteLine(System.Enum.GetName(typeof(ActionKind), this.ActionId));
			ActionRecords.CurrentConstantPool = cp;
			for (int i = 0; i < Statements.Count; i++)
			{
				ActionRecords.AutoLineLabel(w);
				Statements[i].Dump(w);
			}
		}
#endif
    }
}
