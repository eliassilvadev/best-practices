using Best.Practices.Core.Domain.Enumerators;

namespace Best.Practices.Core.Application.Dtos.Input
{
    public class SearchFilterInput
    {
        public FilterType FilterType { get; set; }

        public string FilterProperty { get; set; }

        public object FilterValue { get; set; }
    }
}