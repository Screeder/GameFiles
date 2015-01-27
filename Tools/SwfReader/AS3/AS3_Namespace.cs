namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class AS3_Namespace
    {
        const byte ACCESS_ANY = 0x00;
        const byte ACCESS_PRIVATE = 0x05;
        const byte ACCESS_NAMESPACE = 0x08;
        const byte ACCESS_PUBLIC = 0x16;
        const byte ACCESS_PACKAGEINTERNAL = 0x17;
        const byte ACCESS_PROTECTED = 0x18;
        const byte ACCESS_EXPLICIT = 0x19;
        const byte ACCESS_STATICPROTECTED = 0x1A;

        byte access;
        string name;
        internal AS3_Namespace(string name, byte access)
        {
            this.name = name;
            this.access = access;
        }
        internal AS3_Namespace()
        {
            this.name = "*";
            this.access = 0;
        }
        public override string ToString()
        {
            return "[" + access2str(this.access) + "]" + (this.name == null ? "NULL" : (this.name == "" ? "\"\"" : this.name));
        }
        internal string access2str(int type)
        {
            if (type == ACCESS_NAMESPACE) return "namespace";
            else if (type == ACCESS_PUBLIC) return "public";
            else if (type == ACCESS_PACKAGEINTERNAL) return "packageinternal";
            else if (type == ACCESS_PROTECTED) return "protected";
            else if (type == ACCESS_EXPLICIT) return "explicit";
            else if (type == ACCESS_STATICPROTECTED) return "staticprotected";
            else if (type == ACCESS_PRIVATE) return "private";
            else if (type == ACCESS_ANY) return "any";
            else
            {
                //fprintf(stderr, "Undefined access type %02x\n", type);
                return "undefined";
            }
        }
    }
}
