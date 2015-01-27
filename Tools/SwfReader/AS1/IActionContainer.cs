using System.Collections.Generic;
namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class IActionContainer
	{
        internal uint codeSize;
        internal uint CodeSize { get { return codeSize; } set { codeSize = value; } }
        internal List<IAction> statements = new List<IAction>();
        internal List<IAction> Statements { get { return statements; } }
	}
}
