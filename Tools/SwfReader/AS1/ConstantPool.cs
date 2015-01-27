#if SWFASM || SWFDUMPER
using System.CodeDom.Compiler;
#endif
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class ConstantPool : IStackManipulator
	{
		internal ActionKind ActionId{get{return ActionKind.ConstantPool;}}
		internal uint Version {get{return 5;}}
		internal uint Length
		{
			get
			{
				uint len = 5;
				foreach (string s in Constants)
				{
					len += (uint)s.Length + 1;
				}
				return len;
			}
		}

		internal uint StackPops { get { return 0; } }
		internal uint StackPushes { get { return 0; } }
		internal int StackChange { get { return 0; } }


		internal string[] Constants;

		internal ConstantPool(SwfReader r)
		{
            uint len = r.GetUI16();

			Constants = new string[len];
			for (int i = 0; i < len; i++)
			{
				Constants[i] = r.GetString();
			}
		}
		// todo: getIndex
#if SWFASM
		internal override void ToFlashAsm(IndentedTextWriter w)
		{
			string comma = Constants.Length > 1 ? ", " : "";
			w.Write("constants ");
			for (int i = 0; i < Constants.Length; i++)
			{
				w.Write("'" + PrimitiveString.EscapeString(Constants[i]) + "'" + comma);
				if (i == Constants.Length - 2)
				{
					comma = "";
				}
			}
			w.WriteLine("");
		}
#endif
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
            w.AppendByte((byte)ActionKind.ConstantPool);

            long lenPos = w.Position;
            w.AppendUI16(0); // length

            w.AppendUI16((uint)Constants.Length);

            for (int i = 0; i < Constants.Length; i++)
            {
                w.AppendString(Constants[i]);
            }
            long temp = w.Position;
            w.Position = lenPos;
            w.AppendUI16((uint)(temp - lenPos - 2)); // skip len bytes
            w.Position = temp;

		}
#endif
#if SWFDUMPER
		internal override void Dump(IndentedTextWriter w)
		{
			w.WriteLine("Action: " + System.Enum.GetName(typeof(ActionKind), this.ActionId));
			for (int i = 0; i < Constants.Length; i++)
			{
				w.Write(i + ":" + PrimitiveString.EscapeString(Constants[i]) + "\t");
			}
			w.WriteLine("");
        }
#endif
    }
}
