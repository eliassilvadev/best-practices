using Best.Practices.Core.Domain.Models;
using Best.Practices.Core.Exceptions;
using Best.Practices.Core.Tests.Common;

namespace Best.Practices.Core.Tests.Domain.Models
{
    public class SampleEntity : BaseEntity
    {
        public virtual string SampleName { get; set; }
        public decimal MonthlySalary { get; protected set; }

        public void SetMonthlySalary(decimal monthlySalary)
        {
            if (monthlySalary <= decimal.Zero)
                throw new ValidationException(CommonTestContants.EntitySalaryMustBeGreaterThanZero);

            MonthlySalary = monthlySalary;
        }
    }
}