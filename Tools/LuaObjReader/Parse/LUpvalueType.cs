using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LeagueSharp.GameFiles.Tools.LuaObjReader
{
    public class LUpvalueType : BObjectType<LUpvalue>
    {
        public override LUpvalue Parse(Stream stream, BHeader header)
        {
            return new LUpvalue() {
                InStack = (stream.ReadByte() != 0),
                Index   = 0xFF & stream.ReadByte()
            };;
        }
    }
}
