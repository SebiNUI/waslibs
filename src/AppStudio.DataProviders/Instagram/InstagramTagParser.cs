using AppStudio.DataProviders.Instagram.TagData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppStudio.DataProviders.Instagram
{
    public class InstagramTagParser : IParser<InstagramSchema>
    {
        public IEnumerable<InstagramSchema> Parse(string data)
        {
            var response = JsonConvert.DeserializeObject<InstagramTag>(data);
            if (response != null)
            {
                return response.ToSchema();
            }
            return null;
        }
    }
}

namespace AppStudio.DataProviders.Instagram.TagData
{
    public class Dimensions
    {
        [JsonProperty("height")]
        public int height { get; set; }

        [JsonProperty("width")]
        public int width { get; set; }
    }

    public class Owner
    {
        [JsonProperty("id")]
        public string id { get; set; }
    }

    public class Comments
    {
        [JsonProperty("count")]
        public int count { get; set; }
    }

    public class Likes
    {
        [JsonProperty("count")]
        public int count { get; set; }
    }

    public class Node
    {
        private readonly DateTime UnixEpochDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        [JsonProperty("comments_disabled")]
        public bool comments_disabled { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("dimensions")]
        public Dimensions dimensions { get; set; }

        [JsonProperty("owner")]
        public Owner owner { get; set; }

        [JsonProperty("thumbnail_src")]
        public string thumbnail_src { get; set; }

        [JsonProperty("thumbnail_resources")]
        public IList<object> thumbnail_resources { get; set; }

        [JsonProperty("is_video")]
        public bool is_video { get; set; }

        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("date")]
        public int date { get; set; }

        [JsonProperty("display_src")]
        public string display_src { get; set; }

        [JsonProperty("caption")]
        public string caption { get; set; }

        [JsonProperty("comments")]
        public Comments comments { get; set; }

        [JsonProperty("likes")]
        public Likes likes { get; set; }

        [JsonProperty("video_views")]
        public int? video_views { get; set; }

        public InstagramSchema ToSchema()
        {
            var result = new InstagramSchema();

            result._id = this.id;
            result.Published = UnixEpochDate.AddSeconds(this.date);
            result.ThumbnailUrl = this.thumbnail_src;
            result.ImageUrl = this.display_src;
            if (this.caption != null)
            {
                result.Title = this.caption;
            }

            return result;
        }
    }

    public class PageInfo
    {
        [JsonProperty("has_next_page")]
        public bool has_next_page { get; set; }

        [JsonProperty("end_cursor")]
        public string end_cursor { get; set; }
    }

    public class Media
    {
        [JsonProperty("nodes")]
        public IList<Node> nodes { get; set; }

        [JsonProperty("count")]
        public int count { get; set; }

        [JsonProperty("page_info")]
        public PageInfo page_info { get; set; }
    }

    public class TopPosts
    {
        [JsonProperty("nodes")]
        public IList<Node> nodes { get; set; }
    }

    public class Tag
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("content_advisory")]
        public object content_advisory { get; set; }

        [JsonProperty("media")]
        public Media media { get; set; }

        [JsonProperty("top_posts")]
        public TopPosts top_posts { get; set; }
    }

    public class InstagramTag
    {
        [JsonProperty("tag")]
        public Tag tag { get; set; }

        public IEnumerable<InstagramSchema> ToSchema()
        {
            var items = this.tag.media.nodes.Select(d => d.ToSchema()).ToList();

            foreach (var item in items)
            {
                item.Author = "";
                item.UserName = $"#{this.tag.name}";
            }

            return items;
        }
    }
}
