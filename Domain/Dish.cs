using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    /// <summary>
    /// Contains a dish by name and number of times the dish has been ordered
    /// </summary>
    public sealed class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MaxQuantity { get; private set; }
        public DishType DishType { get; private set; }
        public ServingTime ServingTime {  get; private set; } 
        [NotMapped]
        public int Count { get; private set; }


        public Dish(string dishName, ServingTime servingTime, DishType dishType, int maxQuantity)
        {
            Name = dishName.ToLower();
            Count = 1;
            ServingTime = servingTime;
            DishType = dishType;
            MaxQuantity = maxQuantity;
        }

        public void Add(int amount)
        {
            if (Count + amount <= MaxQuantity)
                Count += amount;

            else
                throw new Exception("You tried adding more than a dish allows.");
        }

        public bool IsQuantityAllowed(int quantity)
        {
            if (Count + quantity <= MaxQuantity)
                return true;

            return false;
        }

        public Dish()
        {
            ResetCount(); // Redefine Count sempre que uma nova instância é criada
        }

        public void ResetCount()
        {
            Count = 1;
        }
    }
}
