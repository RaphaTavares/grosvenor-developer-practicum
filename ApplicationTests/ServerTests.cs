using Application;
using Domain;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationTests
{
    [TestFixture]
    public class ServerTests
    {
        private ServiceProvider _serviceProvider;
        private IServer _sut;
        private Mock<IDishRepository> _mockDishRepository;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            _mockDishRepository = new Mock<IDishRepository>();

            // Configure the mock repository to return dishes based on the filter
            _mockDishRepository.Setup(repo => repo.GetByFilterAsync(It.IsAny<Expression<Func<Dish, bool>>>()))
                .ReturnsAsync((Expression<Func<Dish, bool>> filter) =>
                {
                    var allDishes = GetMockDishes();
                    var compiledFilter = filter.Compile();
                    return allDishes.FindAll(dish => compiledFilter(dish));
                });

            services.AddSingleton(_mockDishRepository.Object);
            services.AddScoped<IDishManager, DishManager>();
            services.AddScoped<IServer, Server>();

            _serviceProvider = services.BuildServiceProvider();

            _sut = _serviceProvider.GetRequiredService<IServer>();
        }

        [TearDown]
        public void Teardown()
        {
            _serviceProvider?.Dispose();
        }

        private List<Dish> GetMockDishes()
        {
            return new List<Dish>
            {
                new Dish("eggs", ServingTime.Morning, DishType.Entree, 1),
                new Dish("toast", ServingTime.Morning, DishType.Side, 1),
                new Dish("coffee", ServingTime.Morning, DishType.Drink, int.MaxValue),
                new Dish("steak", ServingTime.Evening, DishType.Entree, 1),
                new Dish("potato", ServingTime.Evening, DishType.Side, int.MaxValue),
                new Dish("wine", ServingTime.Evening, DishType.Drink, 1),
                new Dish("cake", ServingTime.Evening, DishType.Dessert, 1),
            };
        }

        [Test]
        public async Task TakeOrderAsync_InvalidInput_ReturnsError()
        {
            // Arrange
            var order = "one";
            string expected = "error";

            // Act
            var actual = await _sut.TakeOrderAsync(order);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task TakeOrderAsync_ValidSingleDish_ReturnsDishName()
        {
            // Arrange
            var order = "morning, 1";
            string expected = "eggs";

            // Act
            var actual = await _sut.TakeOrderAsync(order);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task TakeOrderAsync_MultipleAllowedDish_ReturnsDishNameWithCount()
        {
            // Arrange
            var order = "morning, 3, 3, 3";
            string expected = "coffee(x3)";

            // Act
            var actual = await _sut.TakeOrderAsync(order);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task TakeOrderAsync_MixedDishes_ReturnsFormattedDishNames()
        {
            // Arrange
            var order = "evening, 1, 2, 3, 4";
            string expected = "steak,potato,wine,cake";

            // Act
            var actual = await _sut.TakeOrderAsync(order);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task TakeOrderAsync_DishNotAvailable_ReturnsError()
        {
            // Arrange
            var order = "morning, 1, 2, 3, 5";
            string expected = "error";

            // Act
            var actual = await _sut.TakeOrderAsync(order);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task TakeOrderAsync_MultipleNotAllowedDish_ReturnsError()
        {
            // Arrange
            var order = "evening, 1, 1, 2, 3";
            string expected = "error";

            // Act
            var actual = await _sut.TakeOrderAsync(order);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("morning, 1, 2, 3", "eggs,toast,coffee")]
        [TestCase("morning, 2, 1, 3", "eggs,toast,coffee")]
        [TestCase("morning, 1, 2, 3, 3, 3", "eggs,toast,coffee(x3)")]
        [TestCase("morning, 1, 2, 3, 4", "error")]
        [TestCase("evening, 1, 2, 2, 4", "steak,potato(x2),cake")]
        [TestCase("evening, 1, 2, 3, 5", "error")]
        [TestCase("evening, 1, 2, 3, 4, 4", "error")]
        public async Task TakeOrderAsync_VariousOrders_ReturnsExpectedResults(string order, string expected)
        {
            // Act
            var actual = await _sut.TakeOrderAsync(order);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
