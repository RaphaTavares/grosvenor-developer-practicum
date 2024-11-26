using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application;
using Domain;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Moq;
using NUnit.Framework;


namespace ApplicationTests
{
    [TestFixture]
    public class DishManagerTests
    {
        private ServiceProvider _serviceProvider;
        private IDishManager _sut;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            var mockDishRepository = new Mock<IDishRepository>();
            SetupDishMock(mockDishRepository, "egg", ServingTime.Morning, DishType.Entree, 1, 1);
            SetupDishMock(mockDishRepository, "toast", ServingTime.Morning, DishType.Side, 1, 2);
            SetupDishMock(mockDishRepository, "coffee", ServingTime.Morning, DishType.Drink, int.MaxValue, 3);
            SetupDishMock(mockDishRepository, "steak", ServingTime.Evening, DishType.Entree, 1, 4);
            SetupDishMock(mockDishRepository, "potato", ServingTime.Evening, DishType.Side, int.MaxValue, 5);
            SetupDishMock(mockDishRepository, "wine", ServingTime.Evening, DishType.Drink, 1, 6);
            SetupDishMock(mockDishRepository, "cake", ServingTime.Evening, DishType.Dessert, 1, 7);

            services.AddSingleton(mockDishRepository.Object);
            services.AddScoped<IDishManager, DishManager>();
            services.AddScoped<IServer, Server>();

            _serviceProvider = services.BuildServiceProvider();
            _sut = _serviceProvider.GetRequiredService<IDishManager>();
        }

        [TearDown]
        public void TearDown()
        {
            _serviceProvider?.Dispose();
        }

        [Test]
        public async Task EmptyListReturnsEmptyList()
        {
            // Arrange
            var order = new Order();

            // Act
            var actual = await _sut.GetDishesAsync(order);

            // Assert
            Assert.That(actual.Count, Is.EqualTo(0)); // Refactored because I updated NUnit's version
        }

        [Test]
        [TestCase(ServingTime.Morning, new[] { 1 }, new[] { "egg" }, new[] { 1 })]
        [TestCase(ServingTime.Morning, new[] { 1, 2, 3 }, new[] { "egg", "toast", "coffee" }, new[] { 1, 1, 1 })]
        [TestCase(ServingTime.Morning, new[] { 1, 3, 3 }, new[] { "egg", "coffee" }, new[] { 1, 2 })]
        [TestCase(ServingTime.Evening, new[] { 1, 2, 3, 4 }, new[] { "steak", "potato", "wine", "cake" }, new[] { 1, 1, 1, 1 })]
        [TestCase(ServingTime.Evening, new[] { 1, 2, 2, 2 }, new[] { "steak", "potato" }, new[] { 1, 3 })]
        public async Task ReturnExpectedResult_WhenProvidedCorrectInput(
            ServingTime servingTime,
            int[] dishIds,
            string[] expectedDishNames,
            int[] expectedCounts)
        {
            // Arrange
            var order = new Order
            {
                Dishes = dishIds.ToList(),
                ServingTime = servingTime
            };

            // Act
            var actual = await _sut.GetDishesAsync(order);

            // Assert
            for (int i = 0; i < expectedDishNames.Length; i++)
            {
                Assert.That(actual[i].Name, Is.EqualTo(expectedDishNames[i]), $"Dish name mismatch at index {i}");
                Assert.That(actual[i].Count, Is.EqualTo(expectedCounts[i]), $"Dish count mismatch at index {i}");
            }
        }

        [Test]
        [TestCase(ServingTime.Morning, new[] { 99 }, "Dish does not exist")] 
        [TestCase(ServingTime.Evening, new[] { 100 }, "Dish does not exist")] 
        public void ReturnError_WhenDishDoesNotExist(
            ServingTime servingTime,
            int[] dishIds,
            string expectedErrorMessage)
        {
            // Arrange
            var order = new Order
            {
                Dishes = dishIds.ToList(),
                ServingTime = servingTime
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                await _sut.GetDishesAsync(order);
            });

            Assert.That(ex.Message, Is.EqualTo(expectedErrorMessage));
        }


        private void SetupDishMock(
            Mock<IDishRepository> mockDishRepository,
            string name,
            ServingTime servingTime,
            DishType dishType,
            int maxQuantity,
            int id)
        {
            var testDish = new Dish(name, servingTime, dishType, maxQuantity) { Id = id };
            mockDishRepository.Setup(repo => repo.GetByFilterAsync(
                It.Is<Expression<Func<Dish, bool>>>(filter => filter.Compile()(testDish))))
                .ReturnsAsync(new List<Dish> { testDish }.AsReadOnly());
        }


    }
}
