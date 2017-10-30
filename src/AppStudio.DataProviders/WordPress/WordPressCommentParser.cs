using AppStudio.DataProviders.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AppStudio.DataProviders.WordPress
{
    public class WordPressCommentParser : IParser<WordPressCommentSchema>
    {
        public IEnumerable<WordPressCommentSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var wordPressResponse = JsonConvert.DeserializeObject<WordPressCommentsResponse>(data);

            return wordPressResponse.comments
                                        .OrderByDescending(c => c.date)
                                        .Select( r => new WordPressCommentSchema()
                                        {
                                            _id = r.id,
                                            Content = r.content.DecodeHtml(),
                                            Author = r.author.name.DecodeHtml(),
                                            AuthorImage = r.author.avatar_url,
                                            PublishDate = r.date
                                        });
        }
    }
}
