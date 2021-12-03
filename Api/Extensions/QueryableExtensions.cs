using System;
using System.Linq;
using System.Linq.Expressions;

namespace Api.Extensions
{
    public static partial class QueryableExtensions
    {
        public static IQueryable<Type> SelectSingleField<T, Type>(this IQueryable<T> source, string field)
        {
            var parameter = Expression.Parameter(typeof(T), "e");

            var member = Expression.PropertyOrField(parameter, field);

            var newSelector = Expression.Lambda<Func<T, Type>>(member, parameter);

            return source.Select(newSelector);
        }
    }
}
