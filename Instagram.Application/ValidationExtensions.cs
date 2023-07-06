using FluentValidation;

namespace Instagram.Application
{
    internal static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, IEnumerable<string>> MaxItemsLength<T>(this IRuleBuilder<T, IEnumerable<string>> ruleBuilder, int length)
            => ruleBuilder.Must(items => items.All(i => i.Length <= length))
                .WithMessage($"At least one of list items is too long. Max length is {length}");
    }
}
