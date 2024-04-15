using AutoFixture;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RPMFuel.Domain.Interfaces;
using RPMFuel.Domain.Models;
using RPMFuel.Domain.Models.Configs;

namespace RPMFuel.Domain.UnitTests
{
    public class PetrolServiceTest
    {
        private PetrolService? _sut;
        private Fixture _fixture;
        private Mock<ILogger<PetrolService>> _loggerMock = new();
        private Mock<IEIAClient> _eiaClientMock = new();
        private Mock<IFuelRepository> _fuelRepositoryMock = new();
        private Mock<IOptions<WorkerConfigOptions>> _configMock = new();
        private Mock<IDateTimeProvider> _dateTimeProviderMock = new();
        private DateTime _now = new DateTime(2024, 04, 08, 01, 00, 00, DateTimeKind.Utc);
        private CancellationToken _cancellationToken = new CancellationToken();

        public PetrolServiceTest()
        {
            _fixture = new Fixture();
            _fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
        }

        [Fact]
        public async Task UpdatePrices_WhenOnlyNewArrived_AllWillBeAdded()
        {
            var fuelDtos = FuelDtosFixture();
            var configFixture = _fixture.Build<WorkerConfigOptions>()
                .With(a => a.DaysBehind, 100)
                .Create();

            _configMock
                .SetupGet(c => c.Value)
                .Returns(configFixture);
            _eiaClientMock
                .Setup(c => c.GetPetrolDataAsync(_cancellationToken))
                .ReturnsAsync(fuelDtos);
            _fuelRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(_fixture.CreateMany<FuelDto>().ToList());
            _dateTimeProviderMock
                .SetupGet(p => p.UtcNow)
                .Returns(_now);

            _sut = new PetrolService(
                _loggerMock.Object,
                _eiaClientMock.Object,
                _fuelRepositoryMock.Object,
                _configMock.Object,
                _dateTimeProviderMock.Object);
            await _sut.UpdatePrices(_cancellationToken);

            _fuelRepositoryMock
                .Verify(e => e.AddManyAsync(
                    It.Is<ICollection<FuelDto>>(a => a.Count == fuelDtos.Count)));
        }

        [Fact]
        public async Task UpdatePrices_WhenSomeAlreadyAreInDb_OnlyNewWillBeAdded()
        {
            var fuelDtos = FuelDtosFixture();
            var notInDbCount = 2;
            var alreadyInDb = fuelDtos.Skip(notInDbCount).ToList();
            var newToAdd = fuelDtos.SkipLast(fuelDtos.Count - notInDbCount);
            var configFixture = _fixture.Build<WorkerConfigOptions>()
                .With(a => a.DaysBehind, 100)
                .Create();

            _configMock
                .SetupGet(c => c.Value)
                .Returns(configFixture);
            _eiaClientMock
                .Setup(c => c.GetPetrolDataAsync(_cancellationToken))
                .ReturnsAsync(fuelDtos);
            _fuelRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(alreadyInDb);
            _dateTimeProviderMock
                .SetupGet(p => p.UtcNow)
                .Returns(_now);

            _sut = new PetrolService(
                _loggerMock.Object,
                _eiaClientMock.Object,
                _fuelRepositoryMock.Object,
                _configMock.Object,
                _dateTimeProviderMock.Object);
            await _sut.UpdatePrices(_cancellationToken);

            _fuelRepositoryMock
                .Verify(e => e.AddManyAsync(
                    It.Is<ICollection<FuelDto>>(a =>
                    a.Count == notInDbCount
                    && a.All(d => newToAdd.Contains(d))
                )));
        }

        [Fact]
        public async Task UpdatePrices_ProceededDataCantBeOlderThanGivenDaysBehind()
        {
            var fuelDtos = FuelDtosFixture();
            var configFixture = _fixture.Build<WorkerConfigOptions>()
                .With(a => a.DaysBehind, 7)
                .Create();

            _configMock
                .SetupGet(c => c.Value)
                .Returns(configFixture);
            _eiaClientMock
                .Setup(c => c.GetPetrolDataAsync(_cancellationToken))
                .ReturnsAsync(fuelDtos);
            _fuelRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(Enumerable.Empty<FuelDto>().ToList());
            _dateTimeProviderMock
                .SetupGet(p => p.UtcNow)
                .Returns(_now);

            _sut = new PetrolService(
                _loggerMock.Object,
                _eiaClientMock.Object,
                _fuelRepositoryMock.Object,
                _configMock.Object,
                _dateTimeProviderMock.Object);

            await _sut.UpdatePrices(_cancellationToken);

            DateOnly dateOnly = DateOnly.FromDateTime(_now);
            var maxAge = dateOnly.DayNumber - _configMock.Object.Value.DaysBehind;
            _fuelRepositoryMock
                .Verify(e => e.AddManyAsync(
                    It.Is<ICollection<FuelDto>>(a =>
                    a.All(a => a.Period.DayNumber >= maxAge))));
        }

        private ICollection<FuelDto> FuelDtosFixture()
        {
            ICollection<FuelDto> fuelDtos =
            [
                new FuelDto(
                    new DateOnly(2024,04,08),
                    _fixture.Create<decimal>(),
                    _fixture.Create<string>()),
                new FuelDto(
                    new DateOnly(2024,04,01),
                    _fixture.Create<decimal>(),
                    _fixture.Create<string>()),
                new FuelDto(
                    new DateOnly(2024,03,25),
                    _fixture.Create<decimal>(),
                    _fixture.Create<string>()),
                new FuelDto(
                    new DateOnly(2024,03,18),
                    _fixture.Create<decimal>(),
                    _fixture.Create<string>()),
                new FuelDto(
                    new DateOnly(2024,03,11),
                    _fixture.Create<decimal>(),
                    _fixture.Create<string>()),
            ];

            return fuelDtos;
        }
    }
}