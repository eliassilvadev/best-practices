using Best.Practices.Core.Application.Dtos.Input;
using Best.Practices.Core.Domain.Enumerators;

namespace Best.Practices.Core.Cqrs.Dapper.Extensions
{
    public static class SearchFilterInputExtension
    {
        public static Dictionary<string, object> GetParameters(this SearchFilterInput filter)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            switch (filter.FilterType)
            {
                case FilterType.Containing:
                    dictionary.Add(filter.FilterProperty, "%" + filter.FilterValue + "%");
                    break;
                case FilterType.Equals:
                    dictionary.Add(filter.FilterProperty, filter.FilterValue ?? "");
                    break;
                case FilterType.DifferentFrom:
                    dictionary.Add(filter.FilterProperty, filter.FilterValue ?? "");
                    break;
                case FilterType.StartsWith:
                    dictionary.Add(filter.FilterProperty, filter.FilterValue + "%");
                    break;
                case FilterType.EndsWith:
                    dictionary.Add(filter.FilterProperty, "%" + filter.FilterValue);
                    break;
                case FilterType.LessThan:
                    dictionary.Add(filter.FilterProperty, filter.FilterValue ?? "");
                    break;
                case FilterType.LessThanOrEqualTo:
                    dictionary.Add(filter.FilterProperty, filter.FilterValue ?? "");
                    break;
                case FilterType.GreaterThan:
                    dictionary.Add(filter.FilterProperty, filter.FilterValue ?? "");
                    break;
                case FilterType.GreaterThanOrEqualTo:
                    dictionary.Add(filter.FilterProperty, filter.FilterValue ?? "");
                    break;
                case FilterType.Empty:
                    dictionary.Add(filter.FilterProperty, "");
                    break;
                case FilterType.NotEmpty:
                    dictionary.Add(filter.FilterProperty, "");
                    break;
                default:
                    dictionary.Add(filter.FilterProperty, filter.FilterValue ?? "");
                    break;
            }

            return dictionary;
        }

        public static string GetSqlFilter(this SearchFilterInput filter)
        {
            string filterProperty = filter.FilterProperty;
            return filter.FilterType switch
            {
                FilterType.Containing => filterProperty + " LIKE ",
                FilterType.Equals => filterProperty + " = ",
                FilterType.DifferentFrom => filterProperty + " <> ",
                FilterType.StartsWith => filterProperty + " LIKE ",
                FilterType.EndsWith => filterProperty + " LIKE ",
                FilterType.LessThan => filterProperty + " < ",
                FilterType.LessThanOrEqualTo => filterProperty + " <= ",
                FilterType.GreaterThan => filterProperty + " > ",
                FilterType.GreaterThanOrEqualTo => filterProperty + " >= ",
                FilterType.Empty => filterProperty + " = ",
                FilterType.NotEmpty => filterProperty + " <> ",
                _ => filterProperty + " = ",
            } + "@" + filter.FilterProperty;
        }
    }
}
