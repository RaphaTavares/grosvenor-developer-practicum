using System.Collections.Generic;
using System.Linq;
using Domain;
using NUnit.Framework;


namespace ApplicationTests
{
    [TestFixture]
    public class DishManagerTests
    {
        private DishManager _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new DishManager();
        }

        [Test]
        public void EmptyListReturnsEmptyList()
        {
            var order = new Order();
            var actual = _sut.GetDishes(order);
            Assert.That(actual.Count, Is.EqualTo(0)); // Refactored because I updated NUnit's version
        }

        [Test]
        public void ListWith1ReturnsOneSteak()
        {
            var order = new Order
            {
                Dishes = new List<int>
                {
                    1
                }
            };

            var actual = _sut.GetDishes(order);
            Assert.That(actual.Count, Is.EqualTo(1));                     // Refactored because I updated NUnit's version
            Assert.That(actual.First().DishName, Is.EqualTo("steak"));    // Refactored because I updated NUnit's version
            Assert.That(actual.First().Count, Is.EqualTo(1));             // Refactored because I updated NUnit's version
        }
    }
}
