using Best.Practices.Core.Application.Dtos.Input;
using Best.Practices.Core.Domain.Enumerators;

namespace Best.Practices.Core.Tests.Application.Dtos.Builders
{
    public class SearchFilterInputBuilder
    {
        private FilterType _filterType;
        private string _filterProperty;
        private object _filterValue;

        public SearchFilterInputBuilder()
        {
            _filterType = FilterType.Equals;
            _filterProperty = "Name";
            _filterValue = "Name Test";
        }

        public SearchFilterInputBuilder WithFilterType(FilterType filterType)
        {
            _filterType = filterType;
            return this;
        }

        public SearchFilterInputBuilder WithFilterProperty(string filterProperty)
        {
            _filterProperty = filterProperty;
            return this;
        }

        public SearchFilterInputBuilder WithFilterValue(object filterValue)
        {
            _filterValue = filterValue;
            return this;
        }

        public SearchFilterInput Build()
        {
            return new SearchFilterInput
            {
                FilterType = _filterType,
                FilterProperty = _filterProperty,
                FilterValue = _filterValue
            };
        }
    }
}