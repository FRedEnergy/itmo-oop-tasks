using System;
using System.Collections;
using MySql;

namespace Lab4
{
    class Program
    {
        public static void Main(string[] args)
        {
            var service = new ShopService();

            var shop = service.CreateShop("Some Shiny Shop", "ul. Lva Tolstogo 7");
            Console.WriteLine("Created new shop");

            var bananaItem = new Item("Banana");
            var appleItem = new Item("Apple");
            service.CreateItem(shop, bananaItem, 10, 17);
            service.CreateItem(shop, appleItem, 2, 5);
            Console.WriteLine("Added bananas (17 per unit) and apples (5 per unit) into the shop");

            var availableItems = service.GetAvailableItems(shop, 20);
            Console.WriteLine("Checking what we can buy for 20 units of currency");
            foreach (var stack in availableItems)
                Console.WriteLine($"{stack.Amount} of {stack.Item.name}");
        
            service.AddShopItems(shop, bananaItem, 10);
            Console.WriteLine("Added 10 bananas");

            var secondShop = service.CreateShop("Second Shop", "ul. Kronverkskaya 14");
            Console.WriteLine("Created second shop");
            
            service.CreateItem(secondShop, bananaItem, 10, 12);
            Console.WriteLine("Added bananas in second shop for price of 12 per unit");

            var shopWithCheapestBanana = service.FindShopWithCheapestItem(bananaItem);
            Console.WriteLine($"Shop with cheapest banana is {shopWithCheapestBanana.Name} at {shopWithCheapestBanana.Address}");
        }
    }
}