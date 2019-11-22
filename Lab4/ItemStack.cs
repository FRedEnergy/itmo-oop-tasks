namespace Lab4
{
    class ItemStack
    {
        public readonly Item Item;
        public int Amount;

        public ItemStack(Item item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }
}