using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab4
{
    class FileDao : IShopDao
    {
        private const string ShopsFile = "./shops.csv";
        private const string ItemsFile = "./items.csv";

        private readonly Dictionary<int, Shop> Shops = new Dictionary<int, Shop>();
        private readonly Dictionary<int, Dictionary<string, ShopItem>> ShopItems = new Dictionary<int, Dictionary<string, ShopItem>>();

        private int NextId => Shops.Count == 0 ? 0 : Shops.Keys.Max();

        public FileDao()
        {
            if (File.Exists(ShopsFile))
            {
                using (var reader = new StreamReader(ShopsFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var spitted = line.Split(',');
                        var id = int.Parse(spitted[0]);
                        Shops[id] = new Shop(id, spitted[1], spitted[2]);
                    }
                }
            }

            if (File.Exists(ItemsFile))
            {
                using (var reader = new StreamReader(ItemsFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var spitted = line.Split(',');
                        var name = spitted[0];
                        var shopId = int.Parse(spitted[1]);
                        var amount = int.Parse(spitted[2]);
                        var price = int.Parse(spitted[3]);

                        var shop = Shops[shopId];

                        if (!ShopItems.ContainsKey(shopId))
                            ShopItems[shopId] = new Dictionary<string, ShopItem>();

                        ShopItems[shopId][name] = new ShopItem(shop, new ItemStack(new Item(name), amount), price);
                    }
                }
            }
        }

        private void DumpDataOnDisk()
        {
            using (var file = new StreamWriter(ShopsFile))
                foreach (var shop in GetShopList())
                    file.WriteLine($"{shop.Id},{shop.Name},{shop.Address}");

            using (var file = new StreamWriter(ItemsFile))
                foreach (var item in GetAllItems())
                    file.WriteLine($"{item.Stack.Item.name},{item.Owner.Id},{item.Stack.Amount},{item.UnitPrice}");
        }
        
        
        
        public int CreateShop(string title, string address)
        {
            var id = NextId + 1;
            Shops[id] = new Shop(id, title, address);
            DumpDataOnDisk();
            return id;
        }

        public ShopItem CreateItem(int shopId, Item item, int amount, int price)
        {
            if(!ShopItems.ContainsKey(shopId))
                ShopItems[shopId] = new Dictionary<string, ShopItem>();

            var shopRef = Shops[shopId];
            var shopItem = new ShopItem(shopRef, new ItemStack(item, amount), price);
            ShopItems[shopId][item.name] = shopItem;
            DumpDataOnDisk();
            
            return shopItem;
        }

        public void AddShopItems(int shop, Item item, int amount)
        {
            if(!ShopItems.ContainsKey(shop))
                return;
            
            var shopItems = ShopItems[shop];
            if (!shopItems.ContainsKey(item.name)) return;
            
            shopItems[item.name].Stack.Amount += amount;
            DumpDataOnDisk();

        }

        public void SetShopItemPrice(int shop, Item item, int price)
        {
            if(!ShopItems.ContainsKey(shop))
                return;
            
            var shopItems = ShopItems[shop];
            if (!shopItems.ContainsKey(item.name)) return;
            
            shopItems[item.name].UnitPrice = price;
            DumpDataOnDisk();
        }

        public List<Shop> GetShopList()
        { 
            return new List<Shop>(Shops.Values);
        }

        public List<ShopItem> GetItemsInShop(int shopId)
        {
            return !ShopItems.ContainsKey(shopId) ? new List<ShopItem>() : new List<ShopItem>(ShopItems[shopId].Values);
        }

        public List<ShopItem> GetItemInAllShop(Item item)
        {
            return ShopItems.Select(pair => pair.Value[item.name]).ToList();
        }

        public List<ShopItem> GetAllItems()
        {
            return new List<ShopItem>(ShopItems.Values.SelectMany(it => it.Values));
        }
    }
}