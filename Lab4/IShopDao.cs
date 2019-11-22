using System;
using System.Collections.Generic;

namespace Lab4
{
    interface IShopDao
    {

        int CreateShop(String title, String address);

        ShopItem CreateItem(int shopId, Item item, int amount, int price);
        
        void AddShopItems(int shop, Item item, int amount);

        void SetShopItemPrice(int shop, Item item, int price);

        List<Shop> GetShopList();

        List<ShopItem> GetItemsInShop(int shopId);

        List<ShopItem> GetItemInAllShop(Item item);
        
        List<ShopItem> GetAllItems();
        
    }
}