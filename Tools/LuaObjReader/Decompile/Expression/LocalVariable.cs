﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeagueSharp.GameFiles.Tools.LuaObjReader
{
    public class LocalVariable : Expression
    {
        private readonly Declaration m_decl;

        public override int ConstantIndex
        {
            get { return -1; }
        }

        public override bool IsBrief
        {
            get { return true; }
        }

        public override bool IsDotChain
        {
            get { return true; }
        }

        public override void Print(Output output)
        {
            output.Print(m_decl.Name);
        }

        public LocalVariable(Declaration decl)
            : base(PRECEDENCE_ATOMIC)
        {
            m_decl = decl;
        }
    }
}
