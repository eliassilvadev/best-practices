using Best.Practices.Core.Common;
using Best.Practices.Core.Extensions;
using Best.Practices.Core.Tests.Application.Dtos.Builders;
using Best.Practices.Core.Tests.Application.UseCases.SampleUseCases;
using Best.Practices.Core.Tests.Common;
using Best.Practices.Core.Tests.Domain.Models;
using Best.Practices.Core.Tests.Domain.Repositories.SampleRepository;
using Best.Practices.Core.UnitOfWork.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Best.Practices.Core.Tests.Application.UseCases
{
    public class CommandUseCaseTests
    {
        private readonly SampleCommandUseCase _useCase;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ISampleRepository> _sampleRepository;

        public CommandUseCaseTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _sampleRepository = new Mock<ISampleRepository>();

            _useCase = new SampleCommandUseCase(_sampleRepository.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task ExecuteAsync_InputIsValid_ReturnsSuccess()
        {
            // Arrange
            var input = new SampleChildUseCaseInputBuilder()
                .WithSampleName("Sample Name Test")
                .Build();

            var sampleEntity = new SampleEntityBuilder().Build();

            _sampleRepository.Setup(s => s.GetBySampleName(input.SampleName))
                .ReturnsAsync(null as SampleEntity);

            _sampleRepository.Setup(s => s.GetById(input.SampleLookUpId))
                .ReturnsAsync(sampleEntity);

            _unitOfWork.Setup(s => s.SaveChangesAsync())
                .ReturnsAsync(true);

            // Act
            var output = await _useCase.ExecuteAsync(input);

            // Assert
            output.HasErros.Should().BeFalse();
            _sampleRepository.Verify(x => x.GetById(input.SampleLookUpId), Times.Once);
            _sampleRepository.Verify(x => x.GetBySampleName(input.SampleName), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_SampleNameAlreadyExists_ReturnsError()
        {
            // Arrange
            var input = new SampleChildUseCaseInputBuilder()
                .WithSampleName("Sample Name Test")
                .Build();

            var sampleEntity = new SampleEntityBuilder().Build();

            _sampleRepository.Setup(s => s.GetBySampleName(input.SampleName))
                .ReturnsAsync(sampleEntity);
            // Act
            var output = await _useCase.ExecuteAsync(input);

            // Assert
            output.HasErros.Should().BeTrue();
            output.Errors.Should().ContainEquivalentOf(new ErrorMessage(CommonTestContants.EntityWithNameAlreadyExists.Format(input.SampleName)));
            _sampleRepository.Verify(x => x.GetById(input.SampleLookUpId), Times.Never);
            _sampleRepository.Verify(x => x.GetBySampleName(input.SampleName), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_SampleIdAreadyExists_ReturnsError()
        {
            // Arrange
            var input = new SampleChildUseCaseInputBuilder()
                .WithSampleName("Sample Name Test")
                .Build();

            _sampleRepository.Setup(s => s.GetBySampleName(input.SampleName))
                .ReturnsAsync(null as SampleEntity);

            _sampleRepository.Setup(s => s.GetById(input.SampleLookUpId))
                .ReturnsAsync(null as SampleEntity);

            _unitOfWork.Setup(s => s.SaveChangesAsync())
                .ReturnsAsync(true);

            // Act
            var output = await _useCase.ExecuteAsync(input);

            // Assert
            output.HasErros.Should().BeTrue();
            output.Errors.Should().ContainEquivalentOf(new ErrorMessage(CommonTestContants.EntityWithIdDoesNotExists.Format(input.SampleLookUpId)));
            _sampleRepository.Verify(x => x.GetById(input.SampleLookUpId), Times.Once);
            _sampleRepository.Verify(x => x.GetBySampleName(input.SampleName), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_SaveChangesReturnsFalse_ReturnsError()
        {
            // Arrange
            var input = new SampleChildUseCaseInputBuilder()
                .WithSampleName("Sample Name Test")
                .Build();

            var sampleEntity = new SampleEntityBuilder().Build();

            _sampleRepository.Setup(s => s.GetBySampleName(input.SampleName))
                .ReturnsAsync(null as SampleEntity);

            _sampleRepository.Setup(s => s.GetById(input.SampleLookUpId))
                .ReturnsAsync(sampleEntity);

            _unitOfWork.Setup(s => s.SaveChangesAsync())
                .ReturnsAsync(false);

            // Act
            var output = await _useCase.ExecuteAsync(input);

            // Assert
            output.HasErros.Should().BeTrue();
            _sampleRepository.Verify(x => x.GetById(input.SampleLookUpId), Times.Once);
            _sampleRepository.Verify(x => x.GetBySampleName(input.SampleName), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}