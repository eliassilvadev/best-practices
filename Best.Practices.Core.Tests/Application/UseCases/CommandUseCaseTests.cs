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
        public void Execute_InputIsValid_ReturnsSuccess()
        {
            // Arrange
            var input = new SampleChildUseCaseInputBuilder()
                .WithSampleName("Sample Name Test")
                .Build();

            var sampleEntity = new SampleEntityBuilder().Build();

            _sampleRepository.Setup(s => s.GetBySampleName(input.SampleName))
                .Returns(null as SampleEntity);

            _sampleRepository.Setup(s => s.GetById(input.SampleLookUpId))
                .Returns(sampleEntity);

            _unitOfWork.Setup(s => s.SaveChanges())
                .Returns(true);

            // Act
            var output = _useCase.Execute(input);

            // Assert
            output.HasErros.Should().BeFalse();
            _sampleRepository.Verify(x => x.GetById(input.SampleLookUpId), Times.Once);
            _sampleRepository.Verify(x => x.GetBySampleName(input.SampleName), Times.Once);
            _unitOfWork.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Execute_SampleNameAlreadyExists_ReturnsError()
        {
            // Arrange
            var input = new SampleChildUseCaseInputBuilder()
                .WithSampleName("Sample Name Test")
                .Build();

            var sampleEntity = new SampleEntityBuilder().Build();

            _sampleRepository.Setup(s => s.GetBySampleName(input.SampleName))
                .Returns(sampleEntity);
            // Act
            var output = _useCase.Execute(input);

            // Assert
            output.HasErros.Should().BeTrue();
            output.Errors.Should().ContainEquivalentOf(new ErrorMessage(CommonTestContants.EntityWithNameAlreadyExists.Format(input.SampleName)));
            _sampleRepository.Verify(x => x.GetById(input.SampleLookUpId), Times.Never);
            _sampleRepository.Verify(x => x.GetBySampleName(input.SampleName), Times.Once);
            _unitOfWork.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public void Execute_SampleIdAreadyExists_ReturnsError()
        {
            // Arrange
            var input = new SampleChildUseCaseInputBuilder()
                .WithSampleName("Sample Name Test")
                .Build();

            _sampleRepository.Setup(s => s.GetBySampleName(input.SampleName))
                .Returns(null as SampleEntity);

            _sampleRepository.Setup(s => s.GetById(input.SampleLookUpId))
                .Returns(null as SampleEntity);

            _unitOfWork.Setup(s => s.SaveChanges())
                .Returns(true);

            // Act
            var output = _useCase.Execute(input);

            // Assert
            output.HasErros.Should().BeTrue();
            output.Errors.Should().ContainEquivalentOf(new ErrorMessage(CommonTestContants.EntityWithIdDoesNotExists.Format(input.SampleLookUpId)));
            _sampleRepository.Verify(x => x.GetById(input.SampleLookUpId), Times.Once);
            _sampleRepository.Verify(x => x.GetBySampleName(input.SampleName), Times.Once);
            _unitOfWork.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public void Execute_SaveChangesReturnsFalse_ReturnsError()
        {
            // Arrange
            var input = new SampleChildUseCaseInputBuilder()
                .WithSampleName("Sample Name Test")
                .Build();

            var sampleEntity = new SampleEntityBuilder().Build();

            _sampleRepository.Setup(s => s.GetBySampleName(input.SampleName))
                .Returns(null as SampleEntity);

            _sampleRepository.Setup(s => s.GetById(input.SampleLookUpId))
                .Returns(sampleEntity);

            _unitOfWork.Setup(s => s.SaveChanges())
                .Returns(false);

            // Act
            var output = _useCase.Execute(input);

            // Assert
            output.HasErros.Should().BeTrue();
            _sampleRepository.Verify(x => x.GetById(input.SampleLookUpId), Times.Once);
            _sampleRepository.Verify(x => x.GetBySampleName(input.SampleName), Times.Once);
            _unitOfWork.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}