using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab4
{
    class ShopService
    {

        private const String PropsFile = "./../../../settings.property";
        
        private IShopDao Dao;

        public ShopService()
        {
            if (!File.Exists(PropsFile))
                throw new Exception("Properties file not found: " + PropsFile);
            
            var data = File.ReadAllLines(PropsFile).ToDictionary(row => row.Split('=')[0],
                row => string.Join("=", row.Split('=').Skip(1).ToArray()));
            var daoType = data["dao"];

            Dao = (daoType == "sql") ? (IShopDao) new SqlShopDao(data["host"], data["user"], 
                data["password"], data["database"]) : new FileDao();
        }

        public List<ItemStack> GetAvailableItems(Shop shop, int money)
        {
            var shopItems = Dao.GetItemsInShop(shop.Id);
            var result = new List<ItemStack>();
            foreach (var shopItem in shopItems)
            {
                var possibleAmount = Math.Min(shopItem.Stack.Amount, money / shopItem.UnitPrice);
                result.Add(new ItemStack(shopItem.Stack.Item, possibleAmount));
            }

            return result;
        }

        public Shop FindShopWithCheapestItem(Item item)
        {
            var stacks = new List<ItemStack>();
            stacks.Add(new ItemStack(item, 1));
            return FindShopCheapestBundle(stacks);
        }

        public int GetCost(Shop shop, List<ItemStack> stacks)
        {
            var itemsInShop = Dao.GetItemsInShop(shop.Id);
            var result = 0;

            foreach(var stack in stacks)
            {
                var shopItem = itemsInShop.Find(it => Equals(it.Stack.Item, stack.Item));
                result += shopItem.UnitPrice * stack.Amount;
            }

            return result;
        }

        public Shop FindShopCheapestBundle(List<ItemStack> stacks)
        {
            int min = Int32.MaxValue;
            Shop minShop = null;
            
            var shopList = Dao.GetShopList();
            
            foreach (var shop in shopList)
            {
                var cost = GetCost(shop, stacks);
                if (cost < min)
                {
                    min = cost;
                    minShop = shop;
                }
            }

            return minShop;
        }
        
        public Shop CreateShop(String title, String address)
        {
            return new Shop(Dao.CreateShop(title, address), title, address);
        }

        public ShopItem CreateItem(Shop shop, Item item, int amount, int price)
        {
            return Dao.CreateItem(shop.Id, item, amount, price);
        }

        public void AddShopItems(Shop shop, Item item, int amount)
        {
            Dao.AddShopItems(shop.Id, item, amount);
        }

        public void SetShopItemPrice(Shop shop, Item item, int price)
        {
            Dao.SetShopItemPrice(shop.Id, item, price);
        }
        

    }
}