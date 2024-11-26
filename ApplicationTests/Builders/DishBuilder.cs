using Domain;

namespace ApplicationTests.Builders
{
    public class DishBuilder
    {
        private string _name;
        private ServingTime _servingTime;
        private DishType _dishType;
        private int _maxQuantity;
        private int _id;

        public DishBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public DishBuilder WithServingTime(ServingTime servingTime)
        {
            _servingTime = servingTime;
            return this;
        }

        public DishBuilder WithDishType(DishType dishType)
        {
            _dishType = dishType;
            return this;
        }

        public DishBuilder WithMaxQuantity(int maxQuantity)
        {
            _maxQuantity = maxQuantity;
            return this;
        }

        public DishBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public Dish Build()
        {
            return new Dish(_name, _servingTime, _dishType, _maxQuantity) { Id = _id };
        }
    }

}
