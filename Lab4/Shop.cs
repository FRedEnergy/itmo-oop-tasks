using System;

namespace Lab4
{
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
}