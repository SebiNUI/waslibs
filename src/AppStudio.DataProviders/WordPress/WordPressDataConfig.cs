﻿using AppStudio.DataProviders.Core;

namespace AppStudio.DataProviders.WordPress
{
    public class WordPressDataConfig
    {
        public WordPressQueryType QueryType { get; set; }

        public string Query { get; set; }

        public string FilterBy { get; set; }

        public WordPressOrderBy OrderBy { get; set; }

        public SortDirection OrderDirection { get; set; } = SortDirection.Descending;
    }

    public enum WordPressQueryType
    {
        Posts,
        Tag,
        Category
    }

    public enum WordPressOrderBy
    {
        None,
        [StringValue("date")]
        Date,
        [StringValue("modified")]
        Modified,
        [StringValue("title")]
        Title,
        [StringValue("comment_count")]
        CommentCount,
        [StringValue("ID")]
        Id
    }
}
