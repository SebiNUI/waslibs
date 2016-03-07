﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Instagram;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public partial class InstagramTestLibrary
    {
        [TestMethod]
        public async Task LoadInstagramTags()
        {
            var config = new InstagramDataConfig
            {
                QueryType = InstagramQueryType.Tag,
                Query = "windowsappstudio"
            };
            var dataProvider = new InstagramDataProvider(OAuthKeys.InstagramValidKeys);
            IEnumerable<InstagramSchema> result = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadInstagramValidUserId()
        {
            var config = new InstagramDataConfig
            {
                QueryType = InstagramQueryType.Id,
                Query = "239684951"
            };
            var dataProvider = new InstagramDataProvider(OAuthKeys.InstagramValidKeys);
            IEnumerable<InstagramSchema> result = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadInstagramInvalidUserId()
        {
            var config = new InstagramDataConfig
            {
                QueryType = InstagramQueryType.Id,
                Query = "-1234"
            };
            var dataProvider = new InstagramDataProvider(OAuthKeys.InstagramValidKeys);
            await ExceptionsAssert.ThrowsAsync<RequestFailedException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestRevokedOAuth()
        {
            var config = new InstagramDataConfig
            {
                QueryType = InstagramQueryType.Tag,
                Query = "windowsappstudio"
            };

            var dataProvider = new InstagramDataProvider(OAuthKeys.InstagramRevokedKeys);

            await ExceptionsAssert.ThrowsAsync<OAuthKeysRevokedException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestInvalidOAuth()
        {
            var config = new InstagramDataConfig
            {
                QueryType = InstagramQueryType.Tag,
                Query = "windowsappstudio"
            };

            var tokens = new InstagramOAuthTokens { ClientId = "INVALID" };

            var dataProvider = new InstagramDataProvider(tokens);

            await ExceptionsAssert.ThrowsAsync<OAuthKeysRevokedException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestEmptyOAuth()
        {
            var config = new InstagramDataConfig
            {
                QueryType = InstagramQueryType.Tag,
                Query = "windowsappstudio"
            };

            var dataProvider = new InstagramDataProvider(new InstagramOAuthTokens());

            OAuthKeysNotPresentException exception = await ExceptionsAssert.ThrowsAsync<OAuthKeysNotPresentException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("ClientId"));
        }

        [TestMethod]
        public async Task TestNullQuery()
        {
            var config = new InstagramDataConfig
            {
                QueryType = InstagramQueryType.Tag,
                Query = null
            };

            var dataProvider = new InstagramDataProvider(OAuthKeys.InstagramValidKeys);

            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("Query"));
        }

        [TestMethod]
        public async Task TestNullOAuth()
        {
            var config = new InstagramDataConfig
            {
                QueryType = InstagramQueryType.Tag,
                Query = "windowsappstudio"
            };

            var dataProvider = new InstagramDataProvider(null);

            await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
        }

        [TestMethod]
        public async Task TestNullConfig()
        {
            var dataProvider = new InstagramDataProvider(OAuthKeys.InstagramValidKeys);

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestNullParser()
        {
            var dataProvider = new InstagramDataProvider(OAuthKeys.InstagramValidKeys);

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync<InstagramSchema>(new InstagramDataConfig(), 20, null));
        }
    }
}
