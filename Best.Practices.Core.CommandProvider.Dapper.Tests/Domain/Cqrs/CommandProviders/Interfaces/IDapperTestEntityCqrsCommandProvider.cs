﻿using Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Models;
using Best.Practices.Core.Domain.Cqrs.CommandProviders;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Cqrs.CommandProviders.Interfaces
{
    public interface IDapperTestEntityCqrsCommandProvider : ICqrsCommandProvider<DapperTestEntity>
    {
        DapperTestEntity GetByCode(string sampleName);
    }
}