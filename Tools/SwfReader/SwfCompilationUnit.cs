using System;
#if SWFDUMPER
using System.CodeDom.Compiler;
#endif
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
namespace LeagueSharp.GameFiles.Tools.Swf
{
    internal class SwfCompilationUnit : IVexConvertable
	{
        internal SwfReader _r;
        internal uint _curTag;
        internal uint _curTagLen;

        internal string Name = "Nameless";
        internal string FullPath;
        internal SwfHeader Header;
        internal List<ISwfTag> Tags = new List<ISwfTag>();
        internal JPEGTables JpegTable;
        internal Dictionary<uint, byte[]> BinaryDatas = new Dictionary<uint, byte[]>();
        internal string Metadata;
        internal bool IsValid;
        internal bool Tagged;
        internal List<byte[]> TimelineStream = new List<byte[]>();
        internal Dictionary<uint, DefineFont2_3> Fonts = new Dictionary<uint, DefineFont2_3>();
        internal Dictionary<string, uint> Symbols = new Dictionary<string, uint>();
        internal StringBuilder Log;

        internal SwfCompilationUnit(SwfReader swfReader)
		{
            this._r = swfReader;
            this.Name = this._r.Name;
            this.FullPath = this._r.FullPath;
            this.Log = new StringBuilder();
		}
        internal void GetTags()
        {
            this.IsValid = _ParseHeader();
            if (IsValid)
            {
                _ParseTags();
            }
            this._r = null;
            this._curTagLen = 0;
            this.Tagged = true;
        }
        internal void GetDoABC()
        {
            if (this.IsValid && this.Tagged)
            {
                foreach (ISwfTag __tag in Tags)
                {
                    if (__tag is DoABCTag)
                    {
                        (__tag as DoABCTag).Extract();
                    }
                }
            }
        }
        internal void Decompile()
        {
            GetTags();
            GetDoABC();
        }

        internal bool _ParseHeader()
		{
			this.Header = new SwfHeader(_r);
			return this.Header.IsSwf;
		}

        internal void _ParseTags()
		{
			bool __tagsRemain = true;
            Tags = new List<ISwfTag>();
            TimelineStream = new List<byte[]>();
            BinaryDatas = new Dictionary<uint, byte[]>();
            Fonts = new Dictionary<uint, DefineFont2_3>();
            Metadata = null;
            Symbols = new Dictionary<string, uint>();
			while (__tagsRemain)
			{
				/*
					RECORDHEADER (short)
					Field				Type	Comment
					TagCodeAndLength	UI16	Upper 10 bits: tag typeLower 6 bits: tag length
				 * 
					RECORDHEADER (long)
					Field				Type	Comment
					TagCodeAndLength	UI16	Tag type and length of 0x3F Packed together as in short header
					Length				SI32	Length of tag
				*/
				uint __b = _r.GetUI16();
                _curTag = (uint)(__b >> 6);
				_curTagLen = __b & 0x3F;
				if (_curTagLen == 0x3F)
				{
					_curTagLen = _r.GetUI32();
				}
				uint __tagEnd = _r.Position + _curTagLen;
                //Debug.WriteLine(r.Position + " type: " + ((uint)curTag).ToString("X2") + " -- " + Enum.GetName(typeof(TagType), curTag));
					
				switch (_curTag)
				{
					case TagType.End:
                        Tags.Add(new EndTag());
						__tagsRemain = false;
						break;

					case TagType.FileAttributes:
						Tags.Add(new FileAttributesTag(_r));
						break;

					case TagType.BackgroundColor:
						Tags.Add(new BackgroundColorTag(_r));
						break;

                    case TagType.SerialNumber:
                        Tags.Add(new SerialNumberTag(_r));
                        break;

                    case TagType.ScriptLimits:
                        Tags.Add(new ScriptLimitsTag(_r));
                        break;

                    case TagType.Metadata:
                        MetadataTag __metadataTag = new MetadataTag(_r);
                        Metadata = __metadataTag.xml;
                        Tags.Add(__metadataTag);
                        break;

                    case TagType.DefineBinaryData:
                        DefineBinaryDataTag __defineBinaryDataTag = new DefineBinaryDataTag(_r, _curTagLen);
                        BinaryDatas.Add(__defineBinaryDataTag.id, __defineBinaryDataTag.data);
                        Tags.Add(__defineBinaryDataTag);
                        break;

                    case TagType.SymbolClass:
                        SymbolClassTag __symbolClassTag = new SymbolClassTag(_r, _curTagLen);
                        Symbols = __symbolClassTag.Symbols;
                        Tags.Add(__symbolClassTag);
                        break;
                        
					case TagType.DefineShape:
						Tags.Add(new DefineShapeTag(_r));
						break;

					case TagType.DefineShape2:
						Tags.Add(new DefineShape2Tag(_r));
						break;

					case TagType.DefineShape3:
						Tags.Add(new DefineShape3Tag(_r));
						break;

					case TagType.DefineShape4:
						Tags.Add(new DefineShape4Tag(_r));
						break;

					case TagType.PlaceObject:
						Tags.Add(new PlaceObjectTag(_r, __tagEnd));
						break;

					case TagType.PlaceObject2:
						Tags.Add(new PlaceObject2Tag(_r, this.Header.Version));
						break;

					case TagType.PlaceObject3:
						Tags.Add(new PlaceObject3Tag(_r));
						break;

					case TagType.RemoveObject:
						Tags.Add(new RemoveObjectTag(_r));
						break;

					case TagType.RemoveObject2:
						Tags.Add(new RemoveObject2Tag(_r));
						break;

					case TagType.ShowFrame:
						Tags.Add(new ShowFrame(_r));
						break;

					case TagType.FrameLabel:
						Tags.Add(new FrameLabelTag(_r));
						break;

					case TagType.DefineSprite:
						DefineSpriteTag sp = new DefineSpriteTag(_r, this.Header.Version);
						Tags.Add(sp);
						break;

					// Bitmaps

					case TagType.JPEGTables:
                        JpegTable = new JPEGTables(_r, _curTagLen);
                        Tags.Add(JpegTable);
						break;

					case TagType.DefineBits:
						Tags.Add(new DefineBitsTag(_r, _curTagLen, false, false));
					    break;

					case TagType.DefineBitsJPEG2:
						Tags.Add(new DefineBitsTag(_r, _curTagLen, true, false));
						break;

					case TagType.DefineBitsJPEG3:
						Tags.Add(new DefineBitsTag(_r, _curTagLen, true, true));
					    break;

					case TagType.DefineBitsLossless:
						Tags.Add(new DefineBitsLosslessTag(_r, _curTagLen, false));
					    break;

					case TagType.DefineBitsLossless2:
						Tags.Add(new DefineBitsLosslessTag(_r, _curTagLen, true));
					    break;

					// Sound

					case TagType.DefineSound:
						Tags.Add(new DefineSoundTag(_r, _curTagLen));
						break;

					case TagType.StartSound:
						Tags.Add(new StartSoundTag(_r));
						break;

					case TagType.SoundStreamHead:
					case TagType.SoundStreamHead2:
					    Tags.Add(new SoundStreamHeadTag(_r));
					    break;

					case TagType.SoundStreamBlock:
						SoundStreamBlockTag ssb = new SoundStreamBlockTag(_r, _curTagLen);
						TimelineStream.Add(ssb.SoundData);
						Tags.Add(ssb);
						break;

                    // text

					case TagType.DefineFontInfo:
						break;
					case TagType.DefineFontInfo2:
						break;
					case TagType.DefineFont:
						break;
					case TagType.DefineFont2:
						DefineFont2_3 df2 = new DefineFont2_3(_r, false);
						Tags.Add(df2);
						Fonts.Add(df2.FontId, df2);
						break;
					case TagType.DefineFont3:
						DefineFont2_3 df3 = new DefineFont2_3(_r, true);
						Tags.Add(df3);
						Fonts.Add(df3.FontId, df3);
						break;
					case TagType.DefineFontAlignZones:
						DefineFontAlignZonesTag dfaz = new DefineFontAlignZonesTag(_r, Fonts);
						Tags.Add(dfaz);
						break;
					case TagType.CSMTextSettings:
						CSMTextSettingsTag csm = new CSMTextSettingsTag(_r);
						Tags.Add(csm);
						break;
					case TagType.DefineText:
						DefineTextTag dt = new DefineTextTag(_r, false);
						Tags.Add(dt);
						break;
					case TagType.DefineText2:
						DefineTextTag dt2 = new DefineTextTag(_r, true);
						Tags.Add(dt2);
                        break;
                    case TagType.DefineEditText:
                        Tags.Add(new DefineEditTextTag(_r));
                        break;
                    case TagType.DefineFontName:
                        Tags.Add(new DefineFontName(_r));
                        break;

                    // buttons
                    case TagType.DefineButton:
                        Tags.Add(new DefineButton(_r));
                        break;
                    case TagType.DefineButton2:
                        Tags.Add(new DefineButton2(_r));
                        break;
                    case TagType.DefineButtonCxform:
                        Tags.Add(new DefineButtonCxform(_r));
                        break;
                    case TagType.DefineButtonSound:
                        Tags.Add(new DefineButtonSound(_r));
                        break;

                    // actions
					case TagType.ExportAssets:
						Tags.Add(new ExportAssetsTag(_r));
						break;

					case TagType.DoAction:
						Tags.Add(new DoActionTag(_r, _curTagLen));
						break;

					case TagType.DoInitAction:
						Tags.Add(new DoActionTag(_r, _curTagLen, true));
						break;

					// todo: defineMorphShape
					case TagType.DefineMorphShape:
                        Tags.Add(new UnsupportedDefinitionTag(_r, _curTag, "Morphs not supported"));
						_r.SkipBytes(_curTagLen); 
						break;
					// todo: defineVideoStream
					case TagType.DefineVideoStream:
                        Tags.Add(new UnsupportedDefinitionTag(_r, _curTag, "Video not supported"));
						_r.SkipBytes(_curTagLen); 
						break;

                    case TagType.ImportAssets:
                    case TagType.ImportAssets2:
                        Tags.Add(new UnsupportedDefinitionTag(_r, _curTag, "Import Assets not yet supported"));
						_r.SkipBytes(_curTagLen); 
                        break;
                        // todo: ImportAssets tags
                    case TagType.DoABC:
                        Tags.Add(new DoABCTag(_r, _curTagLen));
                        //_r.SkipBytes(_curTagLen); 
                        break;
                    case TagType.DoABC2:
                        Tags.Add(new DoABCTag(_r, _curTagLen, true));
						//_r.SkipBytes(_curTagLen); 
                        break;
                        
					default:
						// skip if unknown
                        Debug.WriteLine("unknown type: " + ((uint)_curTag).ToString("X2") + " -- ");
                        Log.AppendLine("Unhandled swf tag: " + ((uint)_curTag).ToString("X2") + " -- ");
                        Tags.Add(new UnsupportedDefinitionTag(_r, (uint)_curTag, "Unhandled swf tag: " + ((uint)_curTag).ToString("X2")));
                        _r.SkipBytes(_curTagLen);
						break;
				}
				if (__tagEnd != _r.Position)
				{
                    Debug.WriteLine("bad tag: " + ((uint)_curTag).ToString("X2"));
                    Log.AppendLine("Tag not fully parsed: " + ((uint)_curTag).ToString("X2")); 
						
					_r.Position = __tagEnd;
				}
			}
		}
#if SWFWRITER
		internal override void ToSwf(SwfWriter w)
		{
			// write header
			this.Header.ToSwf(w);
            
			for (int i = 0; i < Tags.Count; i++)
			{
                //if (i == 0x2de)//w.Position >= 0x3cd20)//
                //{
                //    i = i;
                //}
				if (Tags[i] is PlaceObject2Tag)
				{
					bool is6Plus = this.Header.Version > 5;
					((PlaceObject2Tag)Tags[i]).ToSwf(w, is6Plus);
				}
				else
				{
					Tags[i].ToSwf(w);
				}
			}

			if (Header.IsCompressed)
			{
				w.Zip();
			}

			uint len = (uint)w.Position;
			w.Position = 4;
			w.AppendUI32(len);
			w.Position = len;
		}
#endif
#if SWFDUMPER
        static internal string DumpWrite(TagType tagType, uint size, string description)
        {
            string __tagString = tagType.ToString("X");
            __tagString = (__tagString.Substring(__tagString.Length - 3));
            __tagString = "[" + __tagString + "]           ";
            string __sizeString = size.ToString() + " ";
            __tagString = __tagString.Substring(0, 16 - __sizeString.Length) + __sizeString;
            return __tagString + description;
        }

        static internal string TagTypeString(TagType tagType)
        {
            string __tagString = tagType.ToString("X");
            return (__tagString.Substring(__tagString.Length - 3));
        }

		internal override void Dump(IndentedTextWriter w)
		{
			this.Header.Dump(w);
			foreach (ISwfTag tag in this.Tags)
			{
				tag.Dump(w);
			}
        }
#endif
    }
}
