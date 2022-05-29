using System;
using System.Collections.Generic;
using System.Linq;
using WarCroft.Entities.Items;

namespace WarCroft.Entities.Inventory
{
    public abstract class Bag
    {
        private int capacity;
        private readonly List<Item> items;

        public Bag(int capacity = 100)
        {
            this.Capacity = capacity;
            this.items = new List<Item>();
        }
        public IReadOnlyCollection<Item> Items => this.items;

        protected int Capacity
        {
            get
            {
                return this.capacity;
            }
            set
            {
                this.capacity = value;
            }
        }
        public int Load => this.items.Sum(i => i.Weight);
        public void AddItem(Item item)
        {
            if (this.Load + item.Weight > this.Capacity)
            {
                throw new InvalidOperationException("Bag is full!");
            }

            this.items.Add(item);
        }

        public Item GetItem(string name)
        {
            if (!this.items.Any())
            {
                throw new InvalidOperationException("Bag is empty!");
            }

            Item currItem = this.items.FirstOrDefault(i => i.GetType().Name == name);

            if (currItem == null)
            {
                throw new ArgumentException($"No item with name {name} in bag!");
            }

            this.items.Remove(currItem);

            return currItem;
        }
    }
}
