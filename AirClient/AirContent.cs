using LeagueSharp.GameFiles.Tools.Swf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace LeagueSharp.GameFiles.AirClient
{
    static public class AirGeneratedContent
    {
        static ChampionsList _champions;
        static public ChampionsList Champions
        {
            get
            {
                if (_champions == null)
                {
                    SetIfNeeded();
                }
                return _champions;
            }
        }
        
        static RunesList _runes;
        static public RunesList Runes
        {
            get
            {
                if (_runes == null)
                {
                    SetIfNeeded();
                }
                return _runes;
            }
        }
        
        static MapsList _maps;
        static public MapsList Maps
        {
            get
            {
                if (_maps == null)
                {
                    SetIfNeeded();
                }
                return _maps;
            }
        }
        
        static FeaturedGamesList _featuredGames;
        static public FeaturedGamesList FeaturedGames
        {
            get
            {
                if (_featuredGames == null)
                {
                    SetIfNeeded();
                }
                return _featuredGames;
            }
        }

        static SpellsList _spells;
        static public SpellsList Spells
        {
            get
            {
                if (_spells == null)
                {
                    SetIfNeeded();
                }
                return _spells;
            }
        }
        
        static GameItemList _items;
        static public GameItemList Items
        {
            get
            {
                if (_items == null)
                {
                    SetIfNeeded();
                }
                return _items;
            }
        }
        
        static string _AirGeneratedContent = Path.Combine("assets", "swfs", "AirGeneratedContent.swf");

        static private List<object[]> GetParams(AS3_MethodInfo method, string function)
        {
            List<object[]> result = new List<object[]>();
            bool inFunctionCall = false;
            int __index = 0;
            object[] __res = new object[method.max_stack];
            foreach (AS3_Code.OpCode __code in method.code.Codes)
            {
                if (__code.opcode == 93 && __code.datas.Length > 0 && __code.datas[0] is QName && ((QName)__code.datas[0]).Name == function) //findpropstrict
                {
                    inFunctionCall = true;
                    __index = 0;
                    __res = new object[method.max_stack];
                    continue;
                }
                else if (inFunctionCall)
                {
                    //(param1:int, param2:String, param3:int, param4:int, param5:int, param6:int, param7:String, param8:String, param9:String, param10:String, param11:String, param12:Array, param13:Array, param14:Object)
                    if ((__code.opcode == 36 || __code.opcode == 37 || __code.opcode == 44 || __code.opcode == 45 || __code.opcode == 47) && __code.datas.Length > 0)
                    {
                        __res[__index] = __code.datas[0];
                        __index++;
                    }
                    else if ((__code.opcode == 85 && __code.datas.Length > 0)) //object
                    {
                        Dictionary<string, object> __object = new Dictionary<string, object>();
                        int __arraylength = (int)__code.datas[0];
                        __index -= (__arraylength * 2);
                        for (int __i = 0; __i < __arraylength; __i++)
                        {
                            string __key = (string)__res[__index + (__i * 2)];
                            __object[__key] = __res[__index + (__i * 2) + 1];
                            __res[__index + (__i * 2)] = null;
                            __res[__index + (__i * 2) + 1] = null;
                        }
                        __res[__index] = __object;
                        __index++;
                    }
                    else if ((__code.opcode == 86 && __code.datas.Length > 0)) //array
                    {
                        int __arraylength = (int)__code.datas[0];
                        __index -= __arraylength;
                        object[] __array = new object[__arraylength];
                        for (int __i = 0; __i < __arraylength; __i++)
                        {
                            __array[__i] = __res[__index + __i];
                            __res[__index + __i] = null;
                        }
                        __res[__index] = __array;
                        __index++;
                    }
                    else if (__code.opcode == 70 && __code.datas.Length > 0 && __code.datas[0] is QName && ((QName)__code.datas[0]).Name == function) //callproperty
                    {
                        result.Add(__res);
                        inFunctionCall = false;
                    }
                    else if (__code.opcode == 42)
                    {
                        __res[__index] = __res[__index - 1];
                        __index++;
                    }
                    else if (__code.opcode == 32)
                    {
                        __res[__index] = null;
                        __index++;
                    }
                    else if (__code.opcode == 38)
                    {
                        __res[__index] = true;
                        __index++;
                    }
                    else if (__code.opcode == 39)
                    {
                        __res[__index] = false;
                        __index++;
                    }
                    else
                    {

                    }
                }
            }
            return result;
        }
        static private void Set()
        {
            string __airFileName = Path.Combine(Directories.AirClientFolder, _AirGeneratedContent);
            if (File.Exists(__airFileName))
            {
                _champions = new ChampionsList();
                _runes = new RunesList();
                _maps = new MapsList();
                _featuredGames = new FeaturedGamesList();
                _spells = new SpellsList();
                _items = new GameItemList();
                Tools.Swf.SwfReader __swfReader = new Tools.Swf.SwfReader(__airFileName);
                __swfReader.DeCompile();
                if (__swfReader.CompilationUnit.IsValid)
                {
                    if (__swfReader.CompilationUnit.Symbols.Count > 0)
                    {
                        foreach (ISwfTag __iSwfTag in __swfReader.CompilationUnit.Tags)
                        {
                            if (__iSwfTag is DoABCTag)
                            {
                                DoABCTag _abctag = (DoABCTag)__iSwfTag;
                                foreach (KeyValuePair<uint, AS3_Class> __class in _abctag.Classes)
                                {
                                    if (__class.Value.classname is RTQName)
                                    {
                                        switch (((RTQName)__class.Value.classname).Name)
                                        {
                                            case "ChampionGeneratedData":
                                                foreach (KeyValuePair<uint, AS3_MethodInfo> __member in __class.Value.traits.methods)
                                                {
                                                    List<object[]> __results = GetParams(__member.Value, "createChampion");
                                                    if (__results.Count > 0)
                                                    {
                                                        foreach (object[] __result in __results)
                                                        {
                                                            _champions.Add(new Champion(__result));
                                                        }
                                                    }
                                                }
                                                break;
                                            case "RuneGeneratedData":
                                                foreach (KeyValuePair<uint, AS3_MethodInfo> __member in __class.Value.traits.methods)
                                                {
                                                    List<object[]> __results = GetParams(__member.Value, "createRune");
                                                    if (__results.Count > 0)
                                                    {
                                                        foreach (object[] __result in __results)
                                                        {
                                                            _runes.Add(new Rune(__result));
                                                        }
                                                    }
                                                }
                                                break;
                                            case "MapGeneratedData":
                                                foreach (KeyValuePair<uint, AS3_MethodInfo> __member in __class.Value.traits.methods)
                                                {
                                                    List<object[]> __results = GetParams(__member.Value, "createMap");
                                                    if (__results.Count > 0)
                                                    {
                                                        foreach (object[] __result in __results)
                                                        {
                                                            _maps.Add(new Map(__result));
                                                        }
                                                    }
                                                }
                                                break;
                                            case "FeaturedGameGeneratedData":
                                                foreach (KeyValuePair<uint, AS3_MethodInfo> __member in __class.Value.traits.methods)
                                                {
                                                    List<object[]> __results = GetParams(__member.Value, "createFeaturedGame");
                                                    if (__results.Count > 0)
                                                    {
                                                        foreach (object[] __result in __results)
                                                        {
                                                            _featuredGames.Add(new FeaturedGame(__result));
                                                        }
                                                    }
                                                }
                                                break;
                                            case "SpellGeneratedData":
                                                foreach (KeyValuePair<uint, AS3_MethodInfo> __member in __class.Value.traits.methods)
                                                {
                                                    List<object[]> __results = GetParams(__member.Value, "createSpell");
                                                    if (__results.Count > 0)
                                                    {
                                                        foreach (object[] __result in __results)
                                                        {
                                                            _spells.Add(new Spell(__result));
                                                        }
                                                    }
                                                }
                                                break;
                                            case "GameItemGeneratedData":
                                                foreach (KeyValuePair<uint, AS3_MethodInfo> __member in __class.Value.traits.methods)
                                                {
                                                    List<object[]> __results = GetParams(__member.Value, "createGameItem");
                                                    if (__results.Count > 0)
                                                    {
                                                        foreach (object[] __result in __results)
                                                        {
                                                            _items.Add(new GameItem(__result));
                                                        }
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
		
		public static void Init()
		{
		    if (_items == null)
		    {
		        SetIfNeeded();
		    }
		}
		
        static ManualResetEvent oSetEvent;
        static private void SetIfNeeded()
        {
            if (oSetEvent != null) oSetEvent.WaitOne();
            if (_items == null)
            {
                oSetEvent = new ManualResetEvent(false);
                Set();
                oSetEvent.Set();
                oSetEvent.Close();
                oSetEvent.Dispose();
                oSetEvent = null;
            }
        }
        public class ChampionsList : IEnumerable<Champion>
        {
            List<Champion> _champions = new List<Champion>();
            Dictionary<string, int> _championsNames = new Dictionary<string, int>();
            Dictionary<int, int> _championsId = new Dictionary<int, int>();

            public void Add(Champion champion)
            {
                int __index = _champions.Count;
                _champions.Add(champion);
                _championsNames[champion.skinName.ToLowerInvariant()] = __index;
                _championsId[champion.championId] = __index;
            }
            public Champion this[int value]
            {
                get
                {
                    if (_championsId.ContainsKey(value)) { return _champions[_championsId[value]]; }
                    return null;
                }
            }
            public Champion this[string value]
            {
                get
                {
                    if (_championsNames.ContainsKey(value.ToLowerInvariant())) { return _champions[_championsNames[value.ToLowerInvariant()]]; }
                    return null;
                }
            }

            public int Count { get { return _champions.Count; } }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return _champions.GetEnumerator(); }
            IEnumerator<Champion> IEnumerable<Champion>.GetEnumerator() { return _champions.GetEnumerator(); }

        }
        public class Champion
        {
            public int championId { get; private set; }
            public string skinName { get; private set; }
            public int attackRank { get; private set; }
            public int defenseRank { get; private set; }
            public int magicRank { get; private set; }
            public int difficultyRank { get; private set; }
            public string passiveIcon { get; private set; }
            public string abilityIcon1 { get; private set; }
            public string abilityIcon2 { get; private set; }
            public string abilityIcon3 { get; private set; }
            public string abilityIcon4 { get; private set; }
            public string[] searchTags { get; private set; }
            public string[] searchTagsSecondary { get; private set; }
            public Dictionary<string, Skin> skinInfo { get; private set; }
            public Champion(object[] entry)
            {
                this.championId = Convert.ToInt32(entry[0]);
                this.skinName = (string)entry[1];
                this.attackRank = Convert.ToInt32(entry[2]);
                this.defenseRank = Convert.ToInt32(entry[3]);
                this.magicRank = Convert.ToInt32(entry[4]);
                this.difficultyRank = Convert.ToInt32(entry[5]);
                this.passiveIcon = (string)entry[6];
                this.abilityIcon1 = (string)entry[7];
                this.abilityIcon2 = (string)entry[8];
                this.abilityIcon3 = (string)entry[9];
                this.abilityIcon4 = (string)entry[10];
                int __searchTagsLen = (entry[11] != null && entry[11] is object[]) ? ((object[])entry[11]).Length : 0;
                this.searchTags = new string[__searchTagsLen];
                if (__searchTagsLen > 0)
                {
                    for (int __i = 0; __i < __searchTagsLen; __i++)
                    {
                        this.searchTags[__i] = (string)((object[])entry[11])[__i];
                    }
                }
                int __searchTagsSecondaryLen = (entry[11] != null && entry[12] is object[]) ? ((object[])entry[12]).Length : 0;
                this.searchTagsSecondary = new string[__searchTagsSecondaryLen];
                if (__searchTagsSecondaryLen > 0)
                {
                    for (int __i = 0; __i < __searchTagsSecondaryLen; __i++)
                    {
                        this.searchTagsSecondary[__i] = (string)((object[])entry[12])[__i];
                    }
                }
                this.skinInfo = new Dictionary<string, Skin>();
                if (entry[13] is Dictionary<string, object>)
                {
                    foreach (KeyValuePair<string, object> __entry in (Dictionary<string, object>)entry[13])
                    {
                        this.skinInfo.Add(__entry.Key, new Skin(__entry.Value));
                    }
                }
            }
            public class Skin
            {
                public int skinId { get; private set; }
                public string name { get; private set; }
                public int skinIndex { get; private set; }
                public Skin(object entry)
                {
                    if (entry is Dictionary<string, object>)
                    {
                        Dictionary<string, object> __entry = (Dictionary<string, object>)entry;
                        this.skinId = __entry.ContainsKey("skinId") ? Convert.ToInt32(__entry["skinId"]) : 0;
                        this.name = __entry.ContainsKey("name") ? (string)__entry["name"] : "";
                        this.skinIndex = __entry.ContainsKey("skinIndex") ? Convert.ToInt32(__entry["skinIndex"]) : 0;
                    }
                }
            }
        }

        public class RunesList : IEnumerable<Rune>
        {
            List<Rune> _runes = new List<Rune>();
            Dictionary<int, int> _runesId = new Dictionary<int, int>();
            public void Add(Rune rune)
            {
                int __index = _runes.Count;
                _runes.Add(rune);
                _runesId[rune.itemId] = __index;
            }
            public Rune this[int value]
            {
                get
                {
                    if (_runesId.ContainsKey(value)) { return _runes[_runesId[value]]; }
                    return null;
                }
            }
            public int Count { get { return _runes.Count; } }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return _runes.GetEnumerator(); }
            IEnumerator<Rune> IEnumerable<Rune>.GetEnumerator() { return _runes.GetEnumerator(); }
        }
        public class Rune
        {
            public int itemId { get; private set; }
            public string name { get; private set; }
            public string description { get; private set; }
            public int runeTypeId { get; private set; }
            public int tier { get; private set; }
            public string imagePath { get; private set; }
            public List<ItemEffect> itemEffects { get; private set; }
            public Rune(object[] entry)
            {
                this.itemId = Convert.ToInt32(entry[0]);
                this.name = (string)entry[1];
                this.description = (string)entry[2];
                this.runeTypeId = Convert.ToInt32(entry[3]);
                this.tier = Convert.ToInt32(entry[4]);
                this.imagePath = (string)entry[5];
                this.itemEffects = new List<ItemEffect>();
                if (entry[6] != null && entry[6] is object[])
                {
                    foreach (object __entry in (object[])entry[6])
                    {
                        this.itemEffects.Add(new ItemEffect(__entry));
                    }
                }
            }
            public class ItemEffect
            {
                public string name { get; private set; }
                public double value { get; private set; }
                public int categoryId { get; private set; }
                public int runeType { get; private set; }
                public ItemEffect(object entry)
                {
                    if (entry is Dictionary<string, object>)
                    {
                        Dictionary<string, object> __entry = (Dictionary<string, object>)entry;
                        this.name = __entry.ContainsKey("name") ? (string)__entry["name"] : "";
                        this.value = __entry.ContainsKey("value") ? Convert.ToDouble(__entry["value"]) : 0;
                        this.categoryId = __entry.ContainsKey("categoryId") ? Convert.ToInt32(__entry["categoryId"]) : 0;
                        this.runeType = __entry.ContainsKey("runeType") ? Convert.ToInt32(__entry["runeType"]) : 0;
                    }
                }
            }
        }

        public class MapsList : IEnumerable<Map>
        {
            List<Map> _maps = new List<Map>();
            Dictionary<string, int> _mapsNames = new Dictionary<string, int>();
            Dictionary<int, int> _mapsId = new Dictionary<int, int>();
            public void Add(Map map)
            {
                int __index = _maps.Count;
                _maps.Add(map);
                _mapsNames[map.name.ToLowerInvariant()] = __index;
                _mapsId[map.mapId] = __index;
            }
            public Map this[int value]
            {
                get
                {
                    if (_mapsId.ContainsKey(value)) { return _maps[_mapsId[value]]; }
                    return null;
                }
            }
            public Map this[string value]
            {
                get
                {
                    if (_mapsNames.ContainsKey(value.ToLowerInvariant())) { return _maps[_mapsNames[value.ToLowerInvariant()]]; }
                    return null;
                }
            }
            public int Count { get { return _maps.Count; } }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return _maps.GetEnumerator(); }
            IEnumerator<Map> IEnumerable<Map>.GetEnumerator() { return _maps.GetEnumerator(); }
        }
        public class Map
        {
            public int mapId { get; private set; }
            public string name { get; private set; }
            public string displayName { get; private set; }
            public string description { get; private set; }
            public int totalPlayers { get; private set; }
            public Map(object[] entry)
            {
                this.mapId = Convert.ToInt32(entry[0]);
                this.name = (string)entry[1];
                this.displayName = (string)entry[2];
                this.description = (string)entry[3];
                this.totalPlayers = Convert.ToInt32(entry[4]);
            }
        }

        public class FeaturedGamesList : IEnumerable<FeaturedGame>
        {
            List<FeaturedGame> _featuredGames = new List<FeaturedGame>();
            Dictionary<string, int> _featuredGamesNames = new Dictionary<string, int>();
            public void Add(FeaturedGame featuredGame)
            {
                int __index = _featuredGames.Count;
                _featuredGames.Add(featuredGame);
                _featuredGamesNames[featuredGame.key.ToLowerInvariant()] = __index;
            }
            public FeaturedGame this[string value]
            {
                get
                {
                    if (_featuredGamesNames.ContainsKey(value.ToLowerInvariant())) { return _featuredGames[_featuredGamesNames[value.ToLowerInvariant()]]; }
                    return null;
                }
            }
            public int Count { get { return _featuredGames.Count; } }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return _featuredGames.GetEnumerator(); }
            IEnumerator<FeaturedGame> IEnumerable<FeaturedGame>.GetEnumerator() { return _featuredGames.GetEnumerator(); }
        }
        public class FeaturedGame
        {
            public string key { get; private set; }
            public string gameMode { get; private set; }
            public string[] gameMutators { get; private set; }
            public FeaturedGame(object[] entry)
            {
                this.key = (string)entry[0];
                this.gameMode = (string)entry[1];
                int __gameMutatorsLen = (entry[2] != null && entry[2] is object[]) ? ((object[])entry[2]).Length : 0;
                this.gameMutators = new string[__gameMutatorsLen];
                if (__gameMutatorsLen > 0)
                {
                    for (int __i = 0; __i < __gameMutatorsLen; __i++)
                    {
                        this.gameMutators[__i] = (string)((object[])entry[2])[__i];
                    }
                }
            }
        }

        public class SpellsList : IEnumerable<Spell>
        {
            List<Spell> _spells = new List<Spell>();
            Dictionary<int, int> _spellsIds = new Dictionary<int, int>();
            Dictionary<string, int> _spellsNames = new Dictionary<string, int>();
            public void Add(Spell spell)
            {
                int __index = _spells.Count;
                _spells.Add(spell);
                _spellsNames[spell.name.ToLowerInvariant()] = __index;
                _spellsIds[spell.spellId] = __index;
            }
            public Spell this[int value]
            {
                get
                {
                    if (_spellsIds.ContainsKey(value)) { return _spells[_spellsIds[value]]; }
                    return null;
                }
            }
            public Spell this[string value]
            {
                get
                {
                    if (_spellsNames.ContainsKey(value.ToLowerInvariant())) { return _spells[_spellsNames[value.ToLowerInvariant()]]; }
                    return null;
                }
            }
            public int Count { get { return _spells.Count; } }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return _spells.GetEnumerator(); }
            IEnumerator<Spell> IEnumerable<Spell>.GetEnumerator() { return _spells.GetEnumerator(); }
        }
        public class Spell
        {
            public int spellId { get; private set; }
            public string name { get; private set; }
            public string displayName { get; private set; }
            public string description { get; private set; }
            public int minLevel { get; private set; }
            public bool active { get; private set; }
            public string[] gameModes { get; private set; }

            public Spell(object[] entry)
            {
                this.spellId = Convert.ToInt32(entry[0]);
                this.name = (string)entry[1];
                this.displayName = (string)entry[2];
                this.description = (string)entry[3];
                this.minLevel = Convert.ToInt32(entry[4]);
                this.active = Convert.ToBoolean(entry[5]);
                int __gameModesLen = (entry[6] != null && entry[6] is object[]) ? ((object[])entry[6]).Length : 0;
                this.gameModes = new string[__gameModesLen];
                if (__gameModesLen > 0)
                {
                    for (int __i = 0; __i < __gameModesLen; __i++)
                    {
                        this.gameModes[__i] = (string)((object[])entry[6])[__i];
                    }
                }
            }
        }

        public class GameItemList : IEnumerable<GameItem>
        {
            List<GameItem> _gameItems = new List<GameItem>();
            Dictionary<int, int> _gameItemsIds = new Dictionary<int, int>();
            Dictionary<string, int> _gameItemsNames = new Dictionary<string, int>();
            public void Add(GameItem gameItem)
            {
                int __index = _gameItems.Count;
                _gameItems.Add(gameItem);
                _gameItemsNames[gameItem.name.ToLowerInvariant()] = __index;
                _gameItemsIds[gameItem.itemId] = __index;
            }
            public GameItem this[int value]
            {
                get
                {
                    if (_gameItemsIds.ContainsKey(value)) { return _gameItems[_gameItemsIds[value]]; }
                    return null;
                }
            }
            public GameItem this[string value]
            {
                get
                {
                    if (_gameItemsNames.ContainsKey(value.ToLowerInvariant())) { return _gameItems[_gameItemsNames[value.ToLowerInvariant()]]; }
                    return null;
                }
            }
            public int Count { get { return _gameItems.Count; } }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return _gameItems.GetEnumerator(); }
            IEnumerator<GameItem> IEnumerable<GameItem>.GetEnumerator() { return _gameItems.GetEnumerator(); }
        }
        public class GameItem
        {
            public int itemId { get; private set; }
            public string name { get; private set; }
            public string description { get; private set; }
            public string iconName { get; private set; }
            public int cost { get; private set; }
            public int[] recipeItems { get; private set; }
            public int[] buildsIntoItems { get; private set; }
            public string[] categories { get; private set; }
            public GameItem(object[] entry)
            {
                this.itemId = Convert.ToInt32(entry[0]);
                this.name = (string)entry[1];
                this.description = (string)entry[2];
                this.iconName = (string)entry[3];
                this.cost = Convert.ToInt32(entry[4]);
                int __recipeItemsLen = (entry[5] != null && entry[5] is object[]) ? ((object[])entry[5]).Length : 0;
                this.recipeItems = new int[__recipeItemsLen];
                if (__recipeItemsLen > 0)
                {
                    for (int __i = 0; __i < __recipeItemsLen; __i++)
                    {
                        this.recipeItems[__i] = Convert.ToInt32(((object[])entry[5])[__i]);
                    }
                }
                int __buildsIntoItemsLen = (entry[6] != null && entry[6] is object[]) ? ((object[])entry[6]).Length : 0;
                this.buildsIntoItems = new int[__buildsIntoItemsLen];
                if (__buildsIntoItemsLen > 0)
                {
                    for (int __i = 0; __i < __buildsIntoItemsLen; __i++)
                    {
                        this.buildsIntoItems[__i] = Convert.ToInt32(((object[])entry[6])[__i]);
                    }
                }
                int __categoriesLen = (entry[7] != null && entry[7] is object[]) ? ((object[])entry[7]).Length : 0;
                this.categories = new string[__categoriesLen];
                if (__categoriesLen > 0)
                {
                    for (int __i = 0; __i < __categoriesLen; __i++)
                    {
                        this.categories[__i] = (string)((object[])entry[7])[__i];
                    }
                }
            }
        }

    }
}
