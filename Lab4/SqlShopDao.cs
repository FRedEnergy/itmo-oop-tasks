using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Lab4
{
    class SqlShopDao: IShopDao
    {

        private readonly String host;
        private readonly String user;
        private readonly String password;
        private readonly String database;

        private MySqlConnection Connection = null;
        private bool IsInitialized => Connection != null;

        public SqlShopDao(string host, string user, string password, string database)
        {
            this.host = host;
            this.user = user;
            this.password = password;
            this.database = database;
        }

        private void CheckConnection()
        {
            if (IsInitialized) return;
            
            Connection = new MySqlConnection($"Server={host}; database={database}; UID={user}; password={password};");
            Connection.Open();
            
            new MySqlCommand($"create table if not exists shops\n(\n\tid int auto_increment,\n\ttitle varchar(64) null," +
                             $"\n\taddress varchar(64) null,\n\tconstraint shops_pk\n\t\tprimary key (id)\n);",
                Connection).ExecuteNonQuery();
            new MySqlCommand($"create table if not exists items\n(\n\tname varchar(64) null," +
                             $"\n\tshop_id int null,\n\tamount int null,\n\tcost float null\n);\n\n",
                Connection).ExecuteNonQuery();
        }
        
        

        public int CreateShop(string title, string address)
        {
            CheckConnection();
            var command = PrepareCommand($"INSERT INTO shops (title, address) VALUES (@title, @address)", 
                new Dictionary<string, object> {{"@title", title}, {"@address", address}});
            command.ExecuteNonQuery();
            return (int) command.LastInsertedId;
        }
        
        public ShopItem CreateItem(int shopId, Item item, int amount, int price)
        {
            CheckConnection();
            MySqlDataReader reader = PrepareCommand($"SELECT * FROM shops WHERE id = @id",
                new Dictionary<string, object>{{"@id", shopId}}).ExecuteReader();
            
            if (!reader.Read())
            {
                reader.Close();
                return null;
            }

            var shop = ReadShop(reader);
            reader.Close();

            PrepareCommand($"INSERT INTO items (name, shop_id, amount, cost) VALUES(@name,@shop,@amount,@price)",
                    new Dictionary<string, object>{{"@name", item.name}, {"@shop", shopId}, {"@amount", amount}, {"@price", price}})
                .ExecuteNonQuery();
            
            return new ShopItem(shop, new ItemStack(item, amount), price);
        }

        public void AddShopItems(int shop, Item item, int amount)
        {
            CheckConnection();
            PrepareCommand($"UPDATE items SET amount = amount + @amount WHERE shop_id = @shop AND name = @name",
                new Dictionary<string, object> {{"@amount", amount}, {"@shop", shop}, {"@name", item.name}}).ExecuteNonQuery();
        }

        public void SetShopItemPrice(int shop, Item item, int price)
        {
            CheckConnection();
            new MySqlCommand($"UPDATE items SET cost = {price} WHERE shop_id = {shop} AND name = {item.name}",
                Connection).ExecuteNonQuery();
        }

        public List<Shop> GetShopList()
        {
            CheckConnection();
            MySqlDataReader reader = new MySqlCommand("SELECT * FROM shops", Connection).ExecuteReader();
            
            var shopList = new List<Shop>();
            while (reader.Read())
                shopList.Add(ReadShop(reader));
            
            reader.Close();
            return shopList;
        }

        public List<ShopItem> GetItemsInShop(int shopId)
        {
            CheckConnection();
            MySqlDataReader reader = PrepareCommand($"SELECT * FROM shops WHERE id = @id",
                new Dictionary<string, object>{{"@id", shopId}}).ExecuteReader();
            if (!reader.Read())
            {
                reader.Close();
                return new List<ShopItem>();
            }

            var shop = ReadShop(reader);
            reader.Close();
            
            reader = PrepareCommand($"SELECT * FROM items WHERE shop_id = @id",
                new Dictionary<string, object>{{"@id", shopId}}).ExecuteReader();
            
            var shopItems = new List<ShopItem>();
            while (reader.Read())
                shopItems.Add(ReadShopItem(shop, reader));
            
            reader.Close();

            return shopItems;
        }

        public List<ShopItem> GetItemInAllShop(Item item)
        {
          
            CheckConnection();
            MySqlDataReader reader = PrepareCommand($"SELECT * FROM items JOIN shops ON items.shop_id = shops.id" +
                                                    $" WHERE items.name = @name",
                new Dictionary<string, object>{{"name", item.name}}).ExecuteReader();
            
            var shopItems = ReadShopItems(reader);
            reader.Close();
            return shopItems;
        }

        public List<ShopItem> GetAllItems()
        {
            CheckConnection();
            MySqlDataReader reader = new MySqlCommand($"SELECT * FROM items JOIN shops ON items.shop_id = shops.id",
                Connection).ExecuteReader();
            var shopItems = ReadShopItems(reader);
            reader.Close();
            return shopItems;
        }

        private List<ShopItem> ReadShopItems(MySqlDataReader reader)
        {
            var shopItems = new List<ShopItem>();
            while (reader.Read())
                shopItems.Add(ReadShopItem(ReadShop(reader), reader));
                
            return shopItems;
        }

        private ShopItem ReadShopItem(Shop shop, MySqlDataReader reader)
        {
            var name = reader.GetString("name");
            var amount = reader.GetInt32("amount");
            var cost = reader.GetInt32("cost");
            return new ShopItem(shop, new ItemStack(new Item(name), amount), cost);
        }
        
        private Shop ReadShop(MySqlDataReader reader)
        {
            return new Shop(reader.GetInt32("id"), reader.GetString("title"), 
                reader.GetString("address"));
        }

        private MySqlCommand PrepareCommand(String query, Dictionary<string, object> data)
        {
            var command = new MySqlCommand(query, Connection);
            foreach (var pair in data)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            return command;
        }
        
    }
}