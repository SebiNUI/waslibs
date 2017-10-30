using AppStudio.DataProviders.Instagram.UsernameData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppStudio.DataProviders.Instagram
{
    public class InstagramUsernameParser : IParser<InstagramSchema>
    {
        public IEnumerable<InstagramSchema> Parse(string data)
        {
            var response = JsonConvert.DeserializeObject<InstagramUsername>(data);
            if (response != null)
            {
                return response.ToSchema();
            }
            return null;
        }
    }
}

namespace AppStudio.DataProviders.Instagram.UsernameData
{
    public class FollowedBy
    {
        [JsonProperty("count")]
        public int count { get; set; }
    }

    public class Follows
    {
        [JsonProperty("count")]
        public int count { get; set; }
    }

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

        [JsonProperty("__typename")]
        public string __typename { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("comments_disabled")]
        public bool comments_disabled { get; set; }

        [JsonProperty("dimensions")]
        public Dimensions dimensions { get; set; }

        [JsonProperty("gating_info")]
        public object gating_info { get; set; }

        [JsonProperty("media_preview")]
        public string media_preview { get; set; }

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

        [JsonProperty("video_views")]
        public int video_views { get; set; }

        [JsonProperty("caption")]
        public string caption { get; set; }

        [JsonProperty("comments")]
        public Comments comments { get; set; }

        [JsonProperty("likes")]
        public Likes likes { get; set; }

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

    public class User
    {

        [JsonProperty("biography")]
        public string biography { get; set; }

        [JsonProperty("blocked_by_viewer")]
        public bool blocked_by_viewer { get; set; }

        [JsonProperty("country_block")]
        public bool country_block { get; set; }

        [JsonProperty("external_url")]
        public object external_url { get; set; }

        [JsonProperty("external_url_linkshimmed")]
        public object external_url_linkshimmed { get; set; }

        [JsonProperty("followed_by")]
        public FollowedBy followed_by { get; set; }

        [JsonProperty("followed_by_viewer")]
        public bool followed_by_viewer { get; set; }

        [JsonProperty("follows")]
        public Follows follows { get; set; }

        [JsonProperty("follows_viewer")]
        public bool follows_viewer { get; set; }

        [JsonProperty("full_name")]
        public string full_name { get; set; }

        [JsonProperty("has_blocked_viewer")]
        public bool has_blocked_viewer { get; set; }

        [JsonProperty("has_requested_viewer")]
        public bool has_requested_viewer { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("is_private")]
        public bool is_private { get; set; }

        [JsonProperty("is_verified")]
        public bool is_verified { get; set; }

        [JsonProperty("profile_pic_url")]
        public string profile_pic_url { get; set; }

        [JsonProperty("profile_pic_url_hd")]
        public string profile_pic_url_hd { get; set; }

        [JsonProperty("requested_by_viewer")]
        public bool requested_by_viewer { get; set; }

        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("connected_fb_page")]
        public object connected_fb_page { get; set; }

        [JsonProperty("media")]
        public Media media { get; set; }

        [JsonProperty("saved_media")]
        public Media saved_media { get; set; }
    }

    public class InstagramUsername
    {
        [JsonProperty("user")]
        public User user { get; set; }

        [JsonProperty("logging_page_id")]
        public string logging_page_id { get; set; }

        public IEnumerable<InstagramSchema> ToSchema()
        {
            var items = this.user.media.nodes.Select(d => d.ToSchema()).ToList();

            foreach (var item in items)
            {
                item.Author = this.user.full_name;
                item.UserName = $"@{this.user.username}";
            }

            return items;
        }
    }
}
