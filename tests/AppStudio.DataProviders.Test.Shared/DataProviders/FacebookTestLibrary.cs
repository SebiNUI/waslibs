﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Facebook;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public partial class FacebookTestLibrary
    {
        [TestMethod]
        public async Task LoadFacebook()
        {
            var config = new FacebookDataConfig
            {
                UserId = "8195378771",
            };
            var dataProvider = new FacebookDataProvider(OAuthKeys.FacebookValidKeys);
            IEnumerable<FacebookSchema> result = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task TestRevokedOAuth()
        {
            var config = new FacebookDataConfig
            {
                UserId = "8195378771",
            };

            var dataProvider = new FacebookDataProvider(OAuthKeys.FacebookRevokedKeys);

            await ExceptionsAssert.ThrowsAsync<OAuthKeysRevokedException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestInvalidOAuth()
        {
            var config = new FacebookDataConfig
            {
                UserId = "8195378771",
            };

            var tokens = new FacebookOAuthTokens
            {
                AppId = "INVALID",
                AppSecret = "INVALID"
            };

            var dataProvider = new FacebookDataProvider(tokens);

            await ExceptionsAssert.ThrowsAsync<OAuthKeysRevokedException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestEmptyOAuth()
        {
            var config = new FacebookDataConfig
            {
                UserId = "8195378771",
            };

            var dataProvider = new FacebookDataProvider(new FacebookOAuthTokens());

            OAuthKeysNotPresentException exception = await ExceptionsAssert.ThrowsAsync<OAuthKeysNotPresentException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("AppId"));
        }

        [TestMethod]
        public async Task TestNullUserId()
        {
            var config = new FacebookDataConfig
            {
                UserId = null,
            };

            var dataProvider = new FacebookDataProvider(OAuthKeys.FacebookValidKeys);

            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("UserId"));
        }

        [TestMethod]
        public async Task TestNullOAuth()
        {
            var config = new FacebookDataConfig
            {
                UserId = "8195378771",
            };

            var dataProvider = new FacebookDataProvider(null);

            await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestNullConfig()
        {
            var dataProvider = new FacebookDataProvider(OAuthKeys.FacebookValidKeys);

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestNullParser()
        {
            var dataProvider = new FacebookDataProvider(OAuthKeys.FacebookValidKeys);

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync<FacebookSchema>(new FacebookDataConfig(), 20, null));
        }

        [TestMethod]
        public async Task TestMaxRecords_Min()
        {
            int maxRecords = 1;
            var config = new FacebookDataConfig
            {
                UserId = "8195378771",
            };
            var dataProvider = new FacebookDataProvider(OAuthKeys.FacebookValidKeys);
            IEnumerable<FacebookSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, result.Count());
        }

        [TestMethod]
        public async Task TestMaxRecords()
        {
            int maxRecords = 70;
            var config = new FacebookDataConfig
            {
                UserId = "150135720497",
            };
            var dataProvider = new FacebookDataProvider(OAuthKeys.FacebookValidKeys);
            IEnumerable<FacebookSchema> result = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsTrue(result.Count() > 25);
        }

        [TestMethod]
        public async Task LoadPaginationFacebook()
        {
            var config = new FacebookDataConfig
            {
                UserId = "8195378771",
            };
            var dataProvider = new FacebookDataProvider(OAuthKeys.FacebookValidKeys);
            await dataProvider.LoadDataAsync(config, 5);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<FacebookSchema> result = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationFacebook()
        {
            var config = new FacebookDataConfig
            {
                UserId = "8195378771",
            };
            var dataProvider = new FacebookDataProvider(OAuthKeys.FacebookValidKeys);
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
        }
    }
}
