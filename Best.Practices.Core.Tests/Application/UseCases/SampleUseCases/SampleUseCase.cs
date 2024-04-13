﻿using Best.Practices.Core.Application.UseCases;
using Best.Practices.Core.Tests.Application.SampleUseCasesDtos;
using Best.Practices.Core.Tests.Domain.Repositories.SampleRepository;
using FluentValidation;

namespace Best.Practices.Core.Tests.Application.UseCases.SampleUseCases
{
    public class SampleUseCase : BaseUseCase<SampleChildUseCaseInput, bool>
    {
        private readonly ISampleRepository _sampleRepository;
        private readonly IValidator<SampleChildUseCaseInput> _validator;

        public SampleUseCase(
            IValidator<SampleChildUseCaseInput> validator,
            ISampleRepository sampleRepository) : base()
        {
            _sampleRepository = sampleRepository;
            _validator = validator;
        }

        public override UseCaseOutput<bool> InternalExecute(SampleChildUseCaseInput input)
        {
            _validator.ValidateAndThrow(input);

            var entity = _sampleRepository.GetById(input.SampleId);

            var sucess = entity is not null;

            return CreateSuccessOutput(sucess);
        }
    }
}