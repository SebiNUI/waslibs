using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AppStudio.DataProviders.Core;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.Twitter
{
    public class TwitterSearchParser : IParser<TwitterSchema>
    {
        public IEnumerable<TwitterSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var result = JsonConvert.DeserializeObject<TwitterSearchResult>(data);

            return result.statuses.Select(r => r.Parse()).ToList();
        }
    }

    internal static class TwitterParser
    {
        public static TwitterSchema Parse(this TwitterTimelineItem item)
        {
            TwitterSchema tweet = new TwitterSchema
            {
                _id = item.Id,
                CreationDateTime = TryParse(item.CreatedAt)
            };
            
            if (item.User == null)
            {
                tweet.UserId = string.Empty;
                tweet.UserName = string.Empty;
                tweet.UserScreenName = string.Empty;
                tweet.UserProfileImageUrl = string.Empty;
                tweet.Url = string.Empty;
            }
            else
            {
                tweet.UserId = item.User.Id;
                tweet.UserName = item.User.Name.DecodeHtml();
                tweet.UserScreenName = string.Concat("@", item.User.ScreenName.DecodeHtml());
                tweet.UserProfileImageUrl = item.User.ProfileImageUrl;
                tweet.Url = string.Format("https://twitter.com/{0}/status/{1}", item.User.ScreenName, item.Id);
                if (!string.IsNullOrEmpty(tweet.UserProfileImageUrl))
                {
                    tweet.UserProfileImageUrl = tweet.UserProfileImageUrl.Replace("_normal", string.Empty);
                }
            }

            var text = item.Text;
            if (item.Entities?.Urls?.Count > 0)
            {
                foreach (TwitterUrl url in item.Entities.Urls)
                {
                    // special case where the URL is a Twitter video
                    if (url.DisplayUrl.Contains("amp.twimg.com/v/"))
                    {
                        text = text.Replace(url.Url, string.Empty);
                    }
                    else
                    {
                        text = text.Replace(url.Url, url.DisplayUrl);
                    }
                }
            }
            
            if (item.Entities?.Media?.Count > 0)
            {     
                foreach (TwitterMedia media in item.Entities.Media)
                {
                    text = text.Replace(media.Url, string.Empty);

                    if ((media.Type == "photo") && (tweet.ImageUrl == null))
                    {
                        tweet.ImageUrl = new Uri(media.MediaUrl);
                    }
                   
                }
            }

            // a '\n' could be at thye end of the tweet if we removed a Twitter video
            tweet.Text = text.TrimEnd('\n').DecodeHtml();

            return tweet;
        }

        private static DateTime TryParse(string dateTime)
        {
            DateTime dt;
            if (!DateTime.TryParseExact(dateTime, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                dt = DateTime.Today;
            }

            return dt;
        }
    }

    public class TwitterTimelineParser : IParser<TwitterSchema>
    {
        public IEnumerable<TwitterSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var result = JsonConvert.DeserializeObject<TwitterTimelineItem[]>(data);
            return result.Select(r => r.Parse()).ToList();
        }
    }

    public class TwitterTimelineItem
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("id_str")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("user")]
        public TwitterUser User { get; set; }

        [JsonProperty("entities")]
        public TwitterEntity Entities { get; set; }
    }

    public class TwitterEntity
    {
        [JsonProperty("hashtags")]
        public List<TwitterHashtags> Hashtags { get; set; }

        [JsonProperty("user_mentions")]
        public List<TwitterUserMention> UserMentions { get; set; }

        [JsonProperty("urls")]
        public List<TwitterUrl> Urls { get; set; }

        [JsonProperty("media")]
        public List<TwitterMedia> Media { get; set; }
    }

    public class TwitterHashtags
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("indices")]
        public int[] Indices { get; set; }
    }

    public class TwitterUserMention
    {
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("id_str")]
        public string IdStr { get; set; }

        [JsonProperty("indices")]
        public int[] Indices { get; set; }
    }

    public class TwitterUrl
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("expanded_url")]
        public string ExpandedUrl { get; set; }

        [JsonProperty("display_url")]
        public string DisplayUrl { get; set; }

        [JsonProperty("indices")]
        public int[] indices { get; set; }
    }

    public class TwitterSize
    {
        [JsonProperty("w")]
        public int Width { get; set; }

        [JsonProperty("h")]
        public int Height { get; set; }

        [JsonProperty("resize")]
        public string Resize { get; set; }
    }

    public class Sizes
    {
        [JsonProperty("small")]
        public TwitterSize Small { get; set; }

        [JsonProperty("medium")]
        public TwitterSize Medium { get; set; }

        [JsonProperty("large")]
        public TwitterSize Large { get; set; }
    }

    public class TwitterMedia
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("id_str")]
        public string IdStr { get; set; }

        [JsonProperty("indices")]
        public int[] Indices { get; set; }

        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }

        [JsonProperty("media_url_https")]
        public string MediaUrlHttps { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("display_url")]
        public string DisplayUrl { get; set; }

        [JsonProperty("expanded_url")]
        public string ExpandedUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sizes")]
        public Sizes Sizes { get; set; }
    }

 

    public class TwitterUser
    {
        [JsonProperty("id_str")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }
    }

    internal class TwitterSearchResult
    {
        public TwitterTimelineItem[] statuses { get; set; }
    }
}
