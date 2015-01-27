namespace LeagueSharp.GameFiles.Tools.Swf
{
	internal class IStackManipulator : IAction
	{
        internal extern uint StackPops { get; }
        internal extern uint StackPushes { get; }
        internal extern int StackChange { get; }
	}
}
