namespace Lab4
{
    class ShopItem
    {
        public readonly Shop Owner;
        public readonly ItemStack Stack;
        public int UnitPrice;

        public ShopItem(Shop owner, ItemStack stack, int unitPrice)
        {
            Owner = owner;
            Stack = stack;
            UnitPrice = unitPrice;
        }
    }
}