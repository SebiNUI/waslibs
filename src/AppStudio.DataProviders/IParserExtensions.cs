using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppStudio.DataProviders
{
    static class IParserExtensions
    {
        internal static async Task<IEnumerable<TSchema>> ParseAsync<TSchema>(this IParser<TSchema> parser, string data) where TSchema : SchemaBase
        {
            return await Task.Run<IEnumerable<TSchema>>(() =>
            {
                return parser.Parse(data);
            });
        }
    }
}
