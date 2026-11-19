using System.Linq.Expressions;
using Habita.Domain.ValueObjects;
using PropertyEntity = Habita.Domain.Entities.Property;

namespace Habita.Application.Dtos.Property.Requests;

public sealed record PropertySearchCriteria(
    int? PropertyTypeId, decimal? PriceMin, decimal? PriceMax, int? Bedrooms, int? Bathrooms, string? Code
)
{
    // Inline both predicates so EF applies the same scope and filters to the count and the page.
    public Expression<Func<T, bool>> Within<T>(
        Expression<Func<T, bool>> scope, Expression<Func<T, PropertyEntity>> property)
    {
        Expression<Func<PropertyEntity, bool>> matches = p =>
            (!PropertyTypeId.HasValue || p.PropertyTypeId == PropertyTypeId.Value)
            && (!PriceMin.HasValue || p.Price.Amount >= PriceMin.Value)
            && (!PriceMax.HasValue || p.Price.Amount <= PriceMax.Value)
            && (!Bedrooms.HasValue || p.Bedrooms == Bedrooms.Value)
            && (!Bathrooms.HasValue || p.Bathrooms == Bathrooms.Value)
            && (string.IsNullOrEmpty(Code) || p.Code == PropertyCode.Unsafe(Code));

        var row = property.Parameters[0];
        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(Expression.Invoke(scope, row), Expression.Invoke(matches, property.Body)), row);
    }
}
