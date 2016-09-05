﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.WordPress;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test.DataProviders
{
    [TestClass]
    public partial class WordPressTestLibrary
    {
        [TestMethod]
        public async Task LoadWordPressPost()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Posts
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadWordPressCategory()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task LoadWordPressTag()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Tag,
                FilterBy = "apps"
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config);

            Assert.IsNotNull(data);
            Assert.AreNotEqual(data.Count(), 0);
        }

        [TestMethod]
        public async Task GetComments()
        {
            var dataProvider = new WordPressDataProvider();
            var result = await dataProvider.GetComments("en.blog.wordpress.com", "32497", 20);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(result.Count(), 0);
        }

        [TestMethod]
        public async Task TestNullQueryConfig()
        {
            var config = new WordPressDataConfig
            {
                Query = null,
                QueryType = WordPressQueryType.Posts
            };

            var dataProvider = new WordPressDataProvider();

            ConfigParameterNullException exception = await ExceptionsAssert.ThrowsAsync<ConfigParameterNullException>(async () => await dataProvider.LoadDataAsync(config));
            Assert.IsTrue(exception.Message.Contains("Query"));
        }

        [TestMethod]
        public async Task TestNullConfig()
        {
            var dataProvider = new WordPressDataProvider();

            await ExceptionsAssert.ThrowsAsync<ConfigNullException>(async () => await dataProvider.LoadDataAsync(null));
        }

        [TestMethod]
        public async Task TestNullParser()
        {
            var dataProvider = new WordPressDataProvider();

            await ExceptionsAssert.ThrowsAsync<ParserNullException>(async () => await dataProvider.LoadDataAsync<WordPressSchema>(new WordPressDataConfig(), 20, null));
        }

        [TestMethod]
        public async Task TestMaxRecordsWordPressPost_Min()
        {
            int maxRecords = 1;
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Posts
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsWordPressPost()
        {
            int maxRecords = 25;
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Posts
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsTrue(data.Count() > 20);
        }

        [TestMethod]
        public async Task TestMaxRecordsWordPressTag_Min()
        {
            int maxRecords = 1;
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Tag,
                FilterBy = "apps"
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsWordPressTag()
        {
            int maxRecords = 25;
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Tag,
                FilterBy = "wordpress"
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsTrue(data.Count() > 20);
        }

        [TestMethod]
        public async Task TestMaxRecordsWordPressCategory_Min()
        {
            int maxRecords = 1;
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.AreEqual(maxRecords, data.Count());
        }

        [TestMethod]
        public async Task TestMaxRecordsWordPressCategory()
        {
            int maxRecords = 25;
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProvider.LoadDataAsync(config, maxRecords);

            Assert.IsTrue(data.Count() > 20);
        }

        [TestMethod]
        public async Task LoadPaginationWordPressPost()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Posts
            };

            var dataProvider = new WordPressDataProvider();
            await dataProvider.LoadDataAsync(config, 5);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<WordPressSchema> data = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataBeforeLoadDataWordPressPost()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Posts
            };

            var dataProvider = new WordPressDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
        }

        [TestMethod]
        public async Task LoadPaginationWordPressCategory()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProvider = new WordPressDataProvider();
            await dataProvider.LoadDataAsync(config, 5);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<WordPressSchema> data = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationWordPressCategory()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProvider = new WordPressDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
        }


        [TestMethod]
        public async Task LoadPaginationWordPressTag()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProvider = new WordPressDataProvider();
            await dataProvider.LoadDataAsync(config, 5);

            Assert.IsTrue(dataProvider.HasMoreItems);

            IEnumerable<WordPressSchema> data = await dataProvider.LoadMoreDataAsync();

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Any());
        }

        [TestMethod]
        public async Task LoadMoreDataInvalidOperationWordPressTag()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Tag,
                FilterBy = "apps"
            };

            var dataProvider = new WordPressDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.LoadMoreDataAsync());
        }

        [TestMethod]
        public async Task LoadCommentsPaginationWordPress()
        {
            var dataProvider = new WordPressDataProvider();
            var site = "en.blog.wordpress.com";
            var postId = "35160";
            var maxId = 20;
            await dataProvider.GetComments(site, postId, maxId);
            Assert.IsTrue(dataProvider.HasMoreComments, nameof(dataProvider.HasMoreComments));

            var result = await dataProvider.GetMoreComments();
            Assert.IsFalse(dataProvider.HasMoreComments, nameof(dataProvider.HasMoreComments));

            Assert.IsNotNull(result, $"{nameof(result)} is not null");
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task LoadMoreCommentsPagination_InvalidOperationWordPress()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProvider = new WordPressDataProvider();
            InvalidOperationException exception = await ExceptionsAssert.ThrowsAsync<InvalidOperationException>(async () => await dataProvider.GetMoreComments());
        }        

        [TestMethod]
        public async Task LoadWordPressPost_Sorting()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Posts
            };

            var dataProviderNoSorting = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProviderNoSorting.LoadDataAsync(config);
            IEnumerable<WordPressSchema> moreData = await dataProviderNoSorting.LoadMoreDataAsync();

            config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Posts,
                OrderBy = WordPressOrderBy.Id,
                OrderDirection = SortDirection.Ascending
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> sortedData = await dataProvider.LoadDataAsync(config);
            IEnumerable<WordPressSchema> moreSortedData = await dataProvider.LoadMoreDataAsync();

            config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Posts,
                OrderBy = WordPressOrderBy.Id,
                OrderDirection = SortDirection.Descending
            };

            var dataProviderDesc = new WordPressDataProvider();
            IEnumerable<WordPressSchema> sortedDescData = await dataProviderDesc.LoadDataAsync(config);
            IEnumerable<WordPressSchema> moreSortedDescData = await dataProviderDesc.LoadMoreDataAsync();

            
            Assert.AreNotEqual(data.FirstOrDefault().Title, sortedData.FirstOrDefault().Title, "LoadDataAsync: WordPress sorting (ascending) is not working");
            Assert.AreNotEqual(moreData.FirstOrDefault().Title, moreSortedData.FirstOrDefault().Title, "LoadMoreDataAsync: WordPress sorting (ascending) is not working");
            Assert.AreNotEqual(sortedData.FirstOrDefault().Title, sortedDescData.FirstOrDefault().Title, "LoadDataAsync: WordPress sorting (descending) is not working");
            Assert.AreNotEqual(moreSortedData.FirstOrDefault().Title, moreSortedDescData.FirstOrDefault().Title, "LoadMoreDataAsync: WordPress sorting (descending) is not working");
        }

        [TestMethod]
        public async Task LoadWordPressCategory_Sorting()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes"
            };

            var dataProviderNoSorting = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProviderNoSorting.LoadDataAsync(config);
            IEnumerable<WordPressSchema> moreData = await dataProviderNoSorting.LoadMoreDataAsync();

            config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes",
                OrderBy = WordPressOrderBy.Id,
                OrderDirection = SortDirection.Ascending
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> sortedData = await dataProvider.LoadDataAsync(config);
            IEnumerable<WordPressSchema> moreSortedData = await dataProvider.LoadMoreDataAsync();

            config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Category,
                FilterBy = "themes",
                OrderBy = WordPressOrderBy.Id,
                OrderDirection = SortDirection.Descending
            };

            var dataProviderDesc = new WordPressDataProvider();
            IEnumerable<WordPressSchema> sortedDescData = await dataProviderDesc.LoadDataAsync(config);
            IEnumerable<WordPressSchema> moreSortedDescData = await dataProviderDesc.LoadMoreDataAsync();


            Assert.AreNotEqual(data.FirstOrDefault().Title, sortedData.FirstOrDefault().Title, "LoadDataAsync: WordPress sorting (ascending) is not working");
            Assert.AreNotEqual(moreData.FirstOrDefault().Title, moreSortedData.FirstOrDefault().Title, "LoadMoreDataAsync: WordPress sorting (ascending) is not working");
            Assert.AreNotEqual(sortedData.FirstOrDefault().Title, sortedDescData.FirstOrDefault().Title, "LoadDataAsync: WordPress sorting (descending) is not working");
            Assert.AreNotEqual(moreSortedData.FirstOrDefault().Title, moreSortedDescData.FirstOrDefault().Title, "LoadMoreDataAsync: WordPress sorting (descending) is not working");
        }

        [TestMethod]
        public async Task LoadWordPressTag_Sorting()
        {
            var config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Tag,
                FilterBy = "wordpress"
            };
            var dataProviderNoSorting = new WordPressDataProvider();
            IEnumerable<WordPressSchema> data = await dataProviderNoSorting.LoadDataAsync(config);
            IEnumerable<WordPressSchema> moreData = await dataProviderNoSorting.LoadMoreDataAsync();

            config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Tag,
                FilterBy = "wordpress",
                OrderBy = WordPressOrderBy.Id,
                OrderDirection = SortDirection.Ascending
            };

            var dataProvider = new WordPressDataProvider();
            IEnumerable<WordPressSchema> sortedData = await dataProvider.LoadDataAsync(config);
            IEnumerable<WordPressSchema> moreSortedData = await dataProvider.LoadMoreDataAsync();

            config = new WordPressDataConfig
            {
                Query = "en.blog.wordpress.com",
                QueryType = WordPressQueryType.Tag,
                FilterBy = "wordpress",
                OrderBy = WordPressOrderBy.Id,
                OrderDirection = SortDirection.Descending
            };

            var dataProviderDesc = new WordPressDataProvider();
            IEnumerable<WordPressSchema> sortedDescData = await dataProviderDesc.LoadDataAsync(config);
            IEnumerable<WordPressSchema> moreSortedDescData = await dataProviderDesc.LoadMoreDataAsync();


            Assert.AreNotEqual(data.FirstOrDefault().Title, sortedData.FirstOrDefault().Title, "LoadDataAsync: WordPress sorting (ascending) is not working");
            Assert.AreNotEqual(moreData.FirstOrDefault().Title, moreSortedData.FirstOrDefault().Title, "LoadMoreDataAsync: WordPress sorting (ascending) is not working");
            Assert.AreNotEqual(sortedData.FirstOrDefault().Title, sortedDescData.FirstOrDefault().Title, "LoadDataAsync: WordPress sorting (descending) is not working");
            Assert.AreNotEqual(moreSortedData.FirstOrDefault().Title, moreSortedDescData.FirstOrDefault().Title, "LoadMoreDataAsync: WordPress sorting (descending) is not working");
        }   
    }
}
