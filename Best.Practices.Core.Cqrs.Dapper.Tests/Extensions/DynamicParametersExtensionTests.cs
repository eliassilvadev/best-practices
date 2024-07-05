using Best.Practices.Core.Cqrs.Dapper.Extensions;
using Best.Practices.Core.Domain.Entities.Interfaces;
using Dapper;
using FluentAssertions;
using Moq;
using Xunit;

namespace Best.Practices.Core.Cqrs.Dapper.Tests.Extensions
{
    public class DynamicParametersExtensionTests
    {
        private readonly DynamicParameters _parameters;

        public DynamicParametersExtensionTests()
        {
            _parameters = new DynamicParameters();
        }

        [Theory]
        [InlineData("Test Value")]
        [InlineData("")]
        [InlineData(null)]
        public void AddNullable_GivenAStringParameter_ShouldAddParameter(string parameterValue)
        {
            _parameters.AddNullable("@ParameterName", parameterValue);

            var value = _parameters.Get<string>("ParameterName");

            value.Should().Be(parameterValue);
        }

        [Fact]
        public void AddNullable_GivenAIntegerParameter_ShouldAddParameter()
        {
            int parameterValue = 10;

            _parameters.AddNullable<int>("@ParameterName", parameterValue);

            var value = _parameters.Get<int?>("ParameterName");

            value.Should().Be(parameterValue);
        }

        [Fact]
        public void AddNullable_GivenANullIntegerParameter_ShouldAddParameter()
        {
            int? parameterValue = null;

            _parameters.AddNullable("@ParameterName", parameterValue);

            var value = _parameters.Get<int?>("ParameterName");

            value.Should().Be(parameterValue);
        }

        [Fact]
        public void AddNullable_GivenADateTimeParameter_ShouldAddParameter()
        {
            DateTime parameterValue = DateTime.Now;

            _parameters.AddNullable("@ParameterName", parameterValue);

            var value = _parameters.Get<DateTime>("@ParameterName");

            value.Should().Be(parameterValue);
        }

        [Fact]
        public void AddNullable_GivenANullDateTimeParameter_ShouldAddParameter()
        {
            DateTime? parameterValue = null;

            _parameters.AddNullable("@ParameterName", parameterValue);

            var value = _parameters.Get<DateTime?>("@ParameterName");

            value.HasValue.Should().BeFalse();
        }

        [Fact]
        public void AddNullable_GivenADateTimeOffsetParameter_ShouldAddParameter()
        {
            DateTimeOffset parameterValue = DateTimeOffset.Now;

            _parameters.AddNullable("@ParameterName", parameterValue);

            var value = _parameters.Get<DateTimeOffset>("@ParameterName");

            value.Should().Be(parameterValue);
        }

        [Fact]
        public void AddNullable_GivenANullDateTimeOffsetParameter_ShouldAddParameter()
        {
            DateTimeOffset? parameterValue = null;

            _parameters.AddNullable("@ParameterName", parameterValue);

            var value = _parameters.Get<DateTimeOffset?>("@ParameterName");

            value.HasValue.Should().BeFalse();
        }

        [Fact]
        public void AddNullable_GivenAnEntityParameter_ShouldAddParameter()
        {
            var entity = new Mock<IBaseEntity>();
            var entityId = Guid.NewGuid();

            entity.Setup(e => e.Id).Returns(entityId);

            _parameters.AddNullable("@ParameterName", entity.Object);

            var value = _parameters.Get<Guid>("@ParameterName");

            value.Should().Be(entityId);
        }

        [Fact]
        public void AddNullable_GivenAnNullEntityParameter_ShouldAddParameter()
        {
            IBaseEntity entity = null;

            _parameters.AddNullable("@ParameterName", entity);

            var value = _parameters.Get<Guid?>("@ParameterName");

            value.Should().Be(null);
        }
    }
}