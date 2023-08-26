using Instagram.Models;
using System.Text;

namespace Instagram.Mobile
{
    public static class PagedRequestExtensions
    {
        public static string BuildQuery<TRequest>(this TRequest request)
            where TRequest : PagedRequest
        {
            var builder = new StringBuilder("?");

            var props = typeof(TRequest)
                .GetProperties()
                .Where(prop => prop.GetValue(request) is not null);

            foreach(var prop in props) 
            {
                if (typeof(IEnumerable<string>).IsAssignableFrom(prop.PropertyType))
                {
                    var value = prop.GetValue(request) as IEnumerable<string>;
                    var names = value.Select(x => $"{Uri.EscapeDataString(prop.Name)}={Uri.EscapeDataString(x)}&");
                    builder.Append(string.Join("", names));
                    continue;
                }
                else if (typeof(IEnumerable<long>).IsAssignableFrom(prop.PropertyType))
                {
                    var value = prop.GetValue(request) as IEnumerable<long>;
                    var names = value.Select(x => $"{Uri.EscapeDataString(prop.Name)}={x}&");
                    builder.Append(string.Join("", names));
                    continue;
                }

                builder.Append($"{Uri.EscapeDataString(prop.Name)}={Uri.EscapeDataString(prop.GetValue(request).ToString())}&");
            }

            return builder.ToString();
        }

        public static string BuildQuery<TRequest>(this TRequest request, string endpoint)
            where TRequest : PagedRequest
        {
            return $"{endpoint}{request.BuildQuery()}";
        }
    }
}
