using System;

namespace AppStudio.DataProviders.WordPress
{
    public class WordPressCommentsResponse
    {
        public string error { get; set; }
        public WordPressComment[] comments { get; set; }
    }
    public class WordPressComment
    {
        public string id { get; set; }
        public Author author { get; set; }
        public DateTime date { get; set; }
        public string content { get; set; }
    }
}
