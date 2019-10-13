using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MySql;
using MySql.Data.MySqlClient;

namespace Lab4
{
    class Item
    {
        private String name;

        public Item(string name)
        {
            this.name = name;
        }

        protected bool Equals(Item other)
        {
            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Item) obj);
        }

        public override int GetHashCode()
        {
            return (name != null ? name.GetHashCode() : 0);
        }
    }

    class ItemStack
    {
        public Item Item;
        public int Amount;

        public ItemStack(Item item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }
    
    class ShopItem
    {
        public Shop Owner;
        public ItemStack Stack;
        public int UnitPrice;
    
    }
    class Shop
    {
        public readonly int Id;
        public readonly String Name;
        public readonly String Address;

        public Shop(int id, string name, string address)
        {
            Id = id;
            Name = name;
            Address = address;
        }
    }
    
    interface IShopDao
    {

        int CreateShop(String title, String address);

        Item CreateItem(String name);

        void AddShopItems(int shop, Item item, int amount);

        void SetShopItemPrice(int shop, Item item, int price);

        List<Shop> GetShopList();

        List<ShopItem> GetItemsInShop(int shopId);

        List<ShopItem> GetItemInAllShop(Item item);
        
        List<ShopItem> GetAllItems();
        
    }

    class SQLShopDao: IShopDao
    {

        private MySqlConnection Connection = null;
        private bool IsInitialized => Connection != null;

        private void CheckConnection()
        {
            if (IsInitialized) return;
            
            Connection = new MySqlConnection("Server=localhost; database=lab4; UID=root; password=mysql;");
            Connection.Open();
        }
        

        public int CreateShop(string title, string address)
        {
            CheckConnection();
            var command = new MySqlCommand($"INSERT INFO shop(title, address) VALUES ({title},{address}", Connection);
            command.ExecuteNonQuery();
            return (int) command.LastInsertedId;
        }

        public Item CreateItem(string name)
        {
            return new Item(name);
        }

        public void AddShopItems(int shop, Item item, int amount)
        {
            CheckConnection();
            
        }

        public void SetShopItemPrice(int shop, Item item, int price)
        {
            throw new NotImplementedException();
        }

        public List<Shop> GetShopList()
        {
            throw new NotImplementedException();
        }

        public List<ShopItem> GetItemsInShop(int shopId)
        {
            throw new NotImplementedException();
        }

        public List<ShopItem> GetItemInAllShop(Item item)
        {
            throw new NotImplementedException();
        }

        public List<ShopItem> GetAllItems()
        {
            throw new NotImplementedException();
        }
    }

    class ShopService
    {

        private IShopDao Dao;

        List<ItemStack> GetPossibleItems(Shop shop, int money)
        {
            var shopItems = Dao.GetItemsInShop(shop.Id);
            var result = new List<ItemStack>();
            foreach (var shopItem in shopItems)
            {
                var possibleAmount = Math.Max(shopItem.Stack.Amount, money / shopItem.UnitPrice);
                result.Add(new ItemStack(shopItem.Stack.Item, possibleAmount));
            }

            return result;
        }

        Shop FindShopWithCheapestItem(Item item)
        {
            return FindShopCheapestBundle(new List<ItemStack>({new ItemStack(item, 1)});
        }

        int GetCost(Shop shop, List<ItemStack> stacks)
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

        Shop FindShopCheapestBundle(List<ItemStack> stacks)
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
        
        Shop CreateShop(String title, String address)
        {
            return new Shop(Dao.CreateShop(title, address), title, address);
        }

        Item CreateItem(String name)
        {
            return Dao.CreateItem(name);
        }

        void AddShopItems(Shop shop, Item item, int amount)
        {
            Dao.AddShopItems(shop.Id, item, amount);
        }

        void SetShopItemPrice(Shop shop, Item item, int price)
        {
            Dao.SetShopItemPrice(shop.Id, item, price);
        }
        

    }
    
    class Program
    {
        public static void Main(string[] args)
        {
        }
    }
}