namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class TagType
	{
        internal const uint End = 0x00;
        internal const uint ShowFrame = 0x01;
        internal const uint DefineShape = 0x02;
        internal const uint FreeCharacter = 0x03;
        internal const uint PlaceObject = 0x04;
        internal const uint RemoveObject = 0x05;
        internal const uint DefineBits = 0x06;
        internal const uint DefineButton = 0x07;
        internal const uint JPEGTables = 0x08;
        internal const uint BackgroundColor = 0x09;
        internal const uint DefineFont = 0x0A;
        internal const uint DefineText = 0x0B;
        internal const uint DoAction = 0x0C;
        internal const uint DefineFontInfo = 0x0D;
        internal const uint DefineSound = 0x0E;
        internal const uint StartSound = 0x0F;
        internal const uint Undefined10 = 0x10; //undefined
        internal const uint DefineButtonSound = 0x11;
        internal const uint SoundStreamHead = 0x12;
        internal const uint SoundStreamBlock = 0x13;
        internal const uint DefineBitsLossless = 0x14;
        internal const uint DefineBitsJPEG2 = 0x15;
        internal const uint DefineShape2 = 0x16;
        internal const uint DefineButtonCxform = 0x17;
        internal const uint Protect = 0x18;
        internal const uint PathsArePostScript = 0x19;

        // **** Flash 3 ****
        internal const uint PlaceObject2 = 0x1A;
        internal const uint Undefined1B = 0x1B; //undefined
        internal const uint RemoveObject2 = 0x1C;
        internal const uint SyncFrame = 0x1D;
        internal const uint Undefined1E = 0x1E; //undefined
        internal const uint FreeAll = 0x1F;
        internal const uint DefineShape3 = 0x20;
        internal const uint DefineText2 = 0x21;
        internal const uint DefineButton2 = 0x22;
        internal const uint DefineBitsJPEG3 = 0x23;
        internal const uint DefineBitsLossless2 = 0x24;
        internal const uint DefineEditText = 0x25; // **** Flash 4 ****
        internal const uint DefineVideo = 0x26; // **** Flash 4 ****
        internal const uint DefineSprite = 0x27;
        internal const uint NameCharacter = 0x28;
        internal const uint SerialNumber = 0x29;
        internal const uint DefineTextFormat = 0x2A;
        internal const uint FrameLabel = 0x2B;
        internal const uint Undefined2C = 0x2C; //undefined
        internal const uint SoundStreamHead2 = 0x2D;
        internal const uint DefineMorphShape = 0x2E;
        internal const uint FrameTag = 0x2F;
        internal const uint DefineFont2 = 0x30;
        internal const uint GenCommand = 0x31;
        internal const uint DefineCommandObj = 0x32;
        internal const uint CharacterSet = 0x33;
        internal const uint FontRef = 0x34;
        internal const uint Undefined35 = 0x35; //undefined
        internal const uint Undefined36 = 0x36; //undefined
        internal const uint Undefined37 = 0x37; //undefined
        internal const uint ExportAssets = 0x38;
        internal const uint ImportAssets = 0x39;
        internal const uint EnableDebugger = 0x3A;
        internal const uint DoInitAction = 0x3B;// **** Flash 5+ ****
        internal const uint DefineVideoStream = 0x3C;// **** Flash 5+ ****
        internal const uint DefineFontInfo2 = 0x3E;// **** Flash 5+ ****

        internal const uint EnableDebugger2 = 0x40;
        internal const uint ScriptLimits = 0x41;
        internal const uint SetTabIndex = 0x42;

        internal const uint FileAttributes = 0x45;// **** Flash 5+ ****
        internal const uint PlaceObject3 = 0x46;// **** Flash 5+ ****
        internal const uint ImportAssets2 = 0x47;// **** Flash 5+ ****
        internal const uint DoABC = 0x48;// **** Flash 5+ ****
        internal const uint DefineFontAlignZones = 0x49;// **** Flash 5+ ****
        internal const uint CSMTextSettings = 0x4A;// **** Flash 5+ ****
        internal const uint DefineFont3 = 0x4B;// **** Flash 5+ ****
        internal const uint SymbolClass = 0x4C;// **** Flash 5+ ****
        internal const uint Metadata = 0x4D;  // Flash 5+ XML blob with comments; description; copyright; etc
        internal const uint DoABC2 = 0x52;// Flash 9+
        internal const uint DefineShape4 = 0x53;// **** Flash 5+ ****

        internal const uint DefineBinaryData = 0x57;// Flash 9+
        internal const uint DefineFontName = 0x58;// Flash 9+
	};
}








