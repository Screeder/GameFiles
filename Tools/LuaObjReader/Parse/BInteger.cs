﻿using System;

namespace LeagueSharp.GameFiles.Tools.LuaObjReader
{
    public class BInteger : BObject
    {
        private readonly long m_big;
        private readonly int m_number;

        private static long MAX_INT = int.MaxValue;
        private static long MIN_INT = int.MinValue;

        public int AsInteger()
        {
            if (m_big == 0)
                return m_number;
            else if (m_big.CompareTo(MAX_INT) > 0 || m_big.CompareTo(MIN_INT) < 0)
                throw new InvalidOperationException("The size of an integer is outside the range that unluac can handle.");
            else
                return (int)m_big;
        }

        public void Iterate(Action thunk)
        {
            if (m_big == 0)
            {
                var i = m_number;

                while (i-- != 0)
                    thunk.Invoke();
            }
            else
            {
                var i = m_big;

                while (i > 0)
                {
                    thunk.Invoke();
                    i -= 1L;
                }
            }
        }

        public BInteger(BInteger b)
        {
            m_big    = b.m_big;
            m_number = b.m_number;
        }

        public BInteger(int number)
        {
            m_big    = 0L;
            m_number = number;
        }

        public BInteger(long big)
        {
            m_big    = big;
            m_number = 0;
        }
    }
}
