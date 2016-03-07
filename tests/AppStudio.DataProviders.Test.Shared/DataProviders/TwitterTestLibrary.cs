﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Twitter;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public class TwitterTestLibrary
    {
        [TestMethod]
        public async Task TestHomeTimeLine()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.Home
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            IEnumerable<TwitterSchema> result = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task TestUserTimeLine()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = "lumia"
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            IEnumerable<TwitterSchema> result = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task TestSearch()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.Search,
                Query = "#lumia"
            };
            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterValidKeys);
            IEnumerable<TwitterSchema> result = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task TestRevokedOAuth()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = @"lumia"
            };

            var dataProvider = new TwitterDataProvider(OAuthKeys.TwitterRevokedKeys);

            await ExceptionsAssert.ThrowsAsync<OAuthKeysRevokedException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestInvalidOAuth()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = @"lumia"
            };

            var tokens = new TwitterOAuthTokens
            {
                ConsumerKey = "INVALID",
                ConsumerSecret = "INVALID",
                AccessToken = "INVALID",
                AccessTokenSecret = "INVALID"
            };

            var dataProvider = new TwitterDataProvider(tokens);

            await ExceptionsAssert.ThrowsAsync<OAuthKeysRevokedException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestEmptyOAuth()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = @"lumia"
            };
            var dataProvider = new TwitterDataProvider(new TwitterOAuthTokens());

            OAuthKeysNotPresentException exception = await ExceptionsAssert.ThrowsAsync<OAuthKeysNotPresentException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("ConsumerKey"));
        }

        [TestMethod]
        public async Task TestNullQuery()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User
            };
            var dataProvider = new TwitterDataProvider(new TwitterOAuthTokens());

            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("Query"));
        }

        [TestMethod]
        public async Task TestNullOAuth()
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.User,
                Query = @"lumia"
            };
            var dataProvider = new TwitterDataProvider(null);

            await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestNullConfig()
        {
            TwitterDataProvider dataProvider = new TwitterDataProvider(new TwitterOAuthTokens());

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestNullParser()
        {
            TwitterDataProvider dataProvider = new TwitterDataProvider(new TwitterOAuthTokens());

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync<TwitterSchema>(new TwitterDataConfig(), 20, null));
        }
    }
}
