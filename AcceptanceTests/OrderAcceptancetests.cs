using Application;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AcceptanceTests
{
    [TestFixture]
    public class OrderAcceptanceTests
    {
        private IServer _server;
        private SqliteConnection _connection;
        private RestaurantDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            // Crie uma conexão SQLite em memória
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            // Configure o DbContext para usar a conexão em memória
            var options = new DbContextOptionsBuilder<RestaurantDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Instancie o DbContext com as opções configuradas
            _dbContext = new RestaurantDbContext(options);

            // Certifique-se de que o banco de dados é criado e os dados são semeados
            _dbContext.Database.EnsureCreated();

            // Configure as dependências
            var dishRepository = new DishRepository(_dbContext);
            var dishManager = new DishManager(dishRepository);
            _server = new Server(dishManager);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }

        [Test]
        public async Task MorningOrder_ValidInput_ReturnsExpectedOutput()
        {
            // Arrange
            var orderInput = "morning, 1, 2, 3";
            var expectedOutput = "egg,toast,coffee";

            // Act
            var actualOutput = await _server.TakeOrderAsync(orderInput);

            // Assert
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }

        [Test]
        public async Task EveningOrder_InvalidDish_ReturnsError()
        {
            // Arrange
            var orderInput = "evening, 1, 2, 3, 5";
            var expectedOutput = "error";

            // Act
            var actualOutput = await _server.TakeOrderAsync(orderInput);

            // Assert
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }

        // Adicione mais testes conforme necessário
    }
}
