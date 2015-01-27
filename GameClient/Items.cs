using LeagueSharp.GameFiles.GameClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LeagueSharp.GameFiles
{
    public class Items
    {
        static private void Set()
        {
            if (Archives.Files.ContainsKey("data/items/items.json"))
            {
                string __jsonned = Encoding.UTF8.GetString(Archives.Files["data/items/items.json"].GetLastContent());
                System.Web.Script.Serialization.JavaScriptSerializer __ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                object result = __ser.DeserializeObject(__jsonned);
                if (result is Dictionary<string, object>)
                {
                    Dictionary<string, object> datas = (Dictionary<string, object>)result;
                    if (datas.ContainsKey("items") && datas["items"] is IList<object>)
                    {
                        _items = new ItemsList((IList<object>)datas["items"]);
                    }
                }
            }
        }
        static private ItemsList _items;
        /// <summary>
        /// Item list from archive
        /// </summary>
        static public ItemsList items
        {
            get
            {
                SetIfNeeded();
                return _items;
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
    }
    public class ItemsList : IEnumerable<Item>
    {
        private List<Item> _items = new List<Item>();
        private Dictionary<int, int> _itemsIdInt = new Dictionary<int, int>();
        private Dictionary<string, int> _itemsIdString = new Dictionary<string, int>();
        public ItemsList(IList<object> entries)
        {
            if (entries != null && entries.Count > 0)
            {
                foreach (object __item in entries)
                {
                    Add(new Item(__item));
                }
            }
        }
        private void Add(Item entry)
        {
            int __index = _items.Count;
            int __id = Convert.ToInt32(entry.id);
            _items.Add(entry);
            _itemsIdInt[__id] = __index;
            _itemsIdString[entry.id] = __index;
        }
        public int Count
        {
            get { return _items.Count; }
        }

        public Item this[int id]
        {
            get
            {
                if (_itemsIdInt.ContainsKey(id))
                {
                    return _items[_itemsIdInt[id]];
                }
                return null;
            }
        }
        public Item this[string id]
        {
            get
            {
                if (_itemsIdString.ContainsKey(id))
                {
                    return _items[_itemsIdString[id]];
                }
                return null;
            }
        }
        public List<Item>.Enumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator<Item> IEnumerable<Item>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }

    public class Item
    {
        public string id { get; private set; }
        public string name { get; private set; }
        public string description { get; private set; }
        public string icon { get; private set; }
        public Gold gold { get; private set; }
        public bool canBeSold { get; private set; }
        public bool consumed { get; private set; }
        public int depth { get; private set; }
        public List<string> from { get; private set; }
        public bool hideFromAll { get; private set; }
        public bool inStore { get; private set; }
        public List<string> into { get; private set; }
        public string itemgroup { get; private set; }
        public Array notes { get; private set; }
        public string requiredChampion { get; private set; }
        public string requiredSpellName { get; private set; }
        public int requiredLevel { get; private set; }
        public string specialRecipe { get; private set; }
        public int stacks { get; private set; }
        public bool usableInStore { get; private set; }
        public Stats stats { get; private set; }
        public List<string> tags { get; private set; }
        public Item(object raw)
        {
            //default values
            specialRecipe = "0";
            inStore = true;
            canBeSold = true;
            consumed = true;
            depth = 1;
            stacks = 1;
            if (raw is Dictionary<string, object>)
            {
                Dictionary<string, object> datas = (Dictionary<string, object>)raw;
                if (datas.ContainsKey("id")) this.id = (string)datas["id"];
                if (datas.ContainsKey("name")) this.name = (string)datas["name"];
                if (datas.ContainsKey("description")) this.description = (string)datas["description"];
                if (datas.ContainsKey("icon")) this.icon = (string)datas["icon"];
                if (datas.ContainsKey("gold")) this.gold = new Gold(datas["gold"]);
                if (datas.ContainsKey("canBeSold")) this.canBeSold = (bool)datas["canBeSold"];
                if (datas.ContainsKey("consumed")) this.consumed = (bool)datas["consumed"];
                if (datas.ContainsKey("depth")) this.depth = (int)datas["depth"];
                from = new List<string>();
                if (datas.ContainsKey("from") && datas["from"] is IList<object>)
                {
                    foreach (object __from in (IList<object>)datas["from"]) { from.Add((string)__from); }
                }
                if (datas.ContainsKey("hideFromAll")) this.hideFromAll = (bool)datas["hideFromAll"];
                if (datas.ContainsKey("inStore")) this.inStore = (bool)datas["inStore"];
                into = new List<string>();
                if (datas.ContainsKey("into") && datas["into"] is IList<object>)
                {
                    foreach (object __into in (IList<object>)datas["into"]) { into.Add((string)__into); }
                }
                if (datas.ContainsKey("itemgroup")) this.itemgroup = (string)datas["itemgroup"];
                if (datas.ContainsKey("notes")) this.notes = (Array)datas["notes"];
                if (datas.ContainsKey("requiredChampion")) this.requiredChampion = (string)datas["requiredChampion"];
                if (datas.ContainsKey("requiredSpellName")) this.requiredSpellName = (string)datas["requiredSpellName"];
                if (datas.ContainsKey("requiredLevel")) this.requiredLevel = (int)datas["requiredLevel"];
                if (datas.ContainsKey("specialRecipe")) this.specialRecipe = (string)datas["specialRecipe"];
                if (datas.ContainsKey("stacks")) this.stacks = (int)datas["stacks"];
                if (datas.ContainsKey("usableInStore")) this.usableInStore = (bool)datas["usableInStore"];
                if (datas.ContainsKey("stats")) this.stats = new Stats(datas["stats"]);
                tags = new List<string>();
                if (datas.ContainsKey("tags") && datas["tags"] is IList<object>)
                {
                    foreach (object __tag in (IList<object>)datas["tags"]) { tags.Add((string)__tag); }
                }
            }
        }
    
        private byte[] _iconBytes;
        public byte[] GetIconBytes()
        {
            if (_iconBytes == null)
            {
                if (Archives.Files.ContainsKey(@"data\items\icons2d\" + icon))
                {
                    _iconBytes = Archives.Files[@"data\items\icons2d\" + icon].GetLastContent();
                }
            }
            return _iconBytes;
        }
    }
    public class Stats
    {
        public double FlatArmorMod { get; private set; }
        public double FlatAttackSpeedMod { get; private set; }
        public double FlatCritChanceMod { get; private set; }
        public double FlatCritDamageMod { get; private set; }
        public double FlatHPRegenMod { get; private set; }
        public double FlatMPPoolMod { get; private set; }
        public double FlatMPRegenMod { get; private set; }
        public double FlatMagicDamageMod { get; private set; }
        public double FlatMovementSpeedMod { get; private set; }
        public double FlatPhysicalDamageMod { get; private set; }
        public double FlatSpellBlockMod { get; private set; }
        public double PercentArmorMod { get; private set; }
        public double PercentAttackSpeedMod { get; private set; }
        public double PercentCritDamageMod { get; private set; }
        public double PercentEXPBonus { get; private set; }
        public double PercentHPPoolMod { get; private set; }
        public double PercentHPRegenMod { get; private set; }
        public double PercentBaseHPRegenMod { get; private set; }
        public double PercentMPPoolMod { get; private set; }
        public double PercentMPRegenMod { get; private set; }
        public double PercentBaseMPRegenMod { get; private set; }
        public double PercentMagicDamageMod { get; private set; }
        public double PercentMovementSpeedMod { get; private set; }
        public double PercentPhysicalDamageMod { get; private set; }
        public double PercentSpellBlockMod { get; private set; }
        public Stats(object raw)
        {
            if (raw is Dictionary<string, object>)
            {
                Dictionary<string, object> datas = (Dictionary<string, object>)raw;
                if (datas.ContainsKey("FlatArmorMod")) this.FlatArmorMod = Convert.ToDouble(datas["FlatArmorMod"]);
                if (datas.ContainsKey("FlatAttackSpeedMod")) this.FlatAttackSpeedMod = Convert.ToDouble(datas["FlatAttackSpeedMod"]);
                if (datas.ContainsKey("FlatCritChanceMod")) this.FlatCritChanceMod = Convert.ToDouble(datas["FlatCritChanceMod"]);
                if (datas.ContainsKey("FlatCritDamageMod")) this.FlatCritDamageMod = Convert.ToDouble(datas["FlatCritDamageMod"]);
                if (datas.ContainsKey("FlatHPRegenMod")) this.FlatHPRegenMod = Convert.ToDouble(datas["FlatHPRegenMod"]);
                if (datas.ContainsKey("FlatMPPoolMod")) this.FlatMPPoolMod = Convert.ToDouble(datas["FlatMPPoolMod"]);
                if (datas.ContainsKey("FlatMPRegenMod")) this.FlatMPRegenMod = Convert.ToDouble(datas["FlatMPRegenMod"]);
                if (datas.ContainsKey("FlatMagicDamageMod")) this.FlatMagicDamageMod = Convert.ToDouble(datas["FlatMagicDamageMod"]);
                if (datas.ContainsKey("FlatMovementSpeedMod")) this.FlatMovementSpeedMod = Convert.ToDouble(datas["FlatMovementSpeedMod"]);
                if (datas.ContainsKey("FlatPhysicalDamageMod")) this.FlatPhysicalDamageMod = Convert.ToDouble(datas["FlatPhysicalDamageMod"]);
                if (datas.ContainsKey("FlatSpellBlockMod")) this.FlatSpellBlockMod = Convert.ToDouble(datas["FlatSpellBlockMod"]);
                if (datas.ContainsKey("PercentArmorMod")) this.PercentArmorMod = Convert.ToDouble(datas["PercentArmorMod"]);
                if (datas.ContainsKey("PercentAttackSpeedMod")) this.PercentAttackSpeedMod = Convert.ToDouble(datas["PercentAttackSpeedMod"]);
                if (datas.ContainsKey("PercentCritDamageMod")) this.PercentCritDamageMod = Convert.ToDouble(datas["PercentCritDamageMod"]);
                if (datas.ContainsKey("PercentEXPBonus")) this.PercentEXPBonus = Convert.ToDouble(datas["PercentEXPBonus"]);
                if (datas.ContainsKey("PercentHPPoolMod")) this.PercentHPPoolMod = Convert.ToDouble(datas["PercentHPPoolMod"]);
                if (datas.ContainsKey("PercentHPRegenMod")) this.PercentHPRegenMod = Convert.ToDouble(datas["PercentHPRegenMod"]);
                if (datas.ContainsKey("PercentBaseHPRegenMod")) this.PercentBaseHPRegenMod = Convert.ToDouble(datas["PercentBaseHPRegenMod"]);
                if (datas.ContainsKey("PercentMPPoolMod")) this.PercentMPPoolMod = Convert.ToDouble(datas["PercentMPPoolMod"]);
                if (datas.ContainsKey("PercentMPRegenMod")) this.PercentMPRegenMod = Convert.ToDouble(datas["PercentMPRegenMod"]);
                if (datas.ContainsKey("PercentBaseMPRegenMod")) this.PercentBaseMPRegenMod = Convert.ToDouble(datas["PercentBaseMPRegenMod"]);
                if (datas.ContainsKey("PercentMagicDamageMod")) this.PercentMagicDamageMod = Convert.ToDouble(datas["PercentMagicDamageMod"]);
                if (datas.ContainsKey("PercentMovementSpeedMod")) this.PercentMovementSpeedMod = Convert.ToDouble(datas["PercentMovementSpeedMod"]);
                if (datas.ContainsKey("PercentPhysicalDamageMod")) this.PercentPhysicalDamageMod = Convert.ToDouble(datas["PercentPhysicalDamageMod"]);
                if (datas.ContainsKey("PercentSpellBlockMod")) this.PercentSpellBlockMod = Convert.ToDouble(datas["PercentSpellBlockMod"]);
            }
        }
    }
    public class Gold
    {
        public int @base { get; private set; }
        public int total { get; private set; }
        public int sell { get; private set; }
        public Gold(object raw)
        {
            if (raw is Dictionary<string, object>)
            {
                Dictionary<string, object> datas = (Dictionary<string, object>)raw;
                if (datas.ContainsKey("base")) this.@base = (int)datas["base"];
                if (datas.ContainsKey("total")) this.total = (int)datas["total"];
                if (datas.ContainsKey("sell")) this.sell = (int)datas["sell"];
            }
        }
    }
    
}
