using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CosmosAPI.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;

namespace CosmosAPI.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _containerFiles;
        private Container _containerCategory;
        private Container _containerCreator;
        private Container _containerMedium;
        private Container _containerPost;
        private Container _containerRatings;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._containerFiles = dbClient.GetContainer(databaseName, "Files");
            this._containerCategory = dbClient.GetContainer(databaseName, "Category");
            this._containerCreator = dbClient.GetContainer(databaseName, "Creator");
            this._containerMedium = dbClient.GetContainer(databaseName, "Medium");
            this._containerPost = dbClient.GetContainer(databaseName, "Post");
            this._containerRatings = dbClient.GetContainer(databaseName, "Identity_FileRating");

        }

        public async Task AddFileAsync(Files item)
        {
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Files> Response = await this._containerFiles.ReadItemAsync<Files>(item.id.ToString(), new PartitionKey(item.id.ToString()));
                Console.WriteLine("Item in database with id: {0} already exists\n", Response.Resource.id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ItemResponse<Files> Response = await this._containerFiles.CreateItemAsync<Files>(item, new PartitionKey(item.id.ToString()));

                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", Response.Resource.id, Response.RequestCharge);
            }

        }

        public async Task<string> DeleteFileAsync(Guid id)
        {
            
            try
            {
                await this._containerFiles.DeleteItemAsync<Files>(id.ToString(), new PartitionKey(id.ToString()));
                return "Deleted file " + id;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }
        }

        public async Task<string> GetFileAsync(Guid id)
        {
            try
            {
                ItemResponse<Files> response = await this._containerFiles.ReadItemAsync<Files>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource.ToString();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }

        public async Task<string> GetFilesAsync(string queryString)
        {
            try
            {
            var query = this._containerFiles.GetItemQueryIterator<Files>(new QueryDefinition(queryString));
            ManyItems results = new ManyItems();
            Files[] LotsOfFiles = new Files[400];
            int iterator = 0;
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                    foreach(Files file in response.Resource)
                    {
                        LotsOfFiles[iterator]=file;
                        iterator++;
                    }
                
            }
            Files[] FinalFiles = new Files[iterator];
            for (int i = 0; i < iterator; i++)
            {
                FinalFiles[i] = LotsOfFiles[i];
            }
            
            results.Items = FinalFiles;
            return results.ToString();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }

        public async Task<string> UpdateFileAsync(Guid id, Files item)
        {
            try
            {
                await this._containerFiles.UpsertItemAsync<Files>(item, new PartitionKey(id.ToString()));
                return "Updated file "+id;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }








        public async Task AddCategoryAsync(Category item)
        {
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Category> Response = await this._containerCategory.ReadItemAsync<Category>(item.id.ToString(), new PartitionKey(item.id.ToString()));
                Console.WriteLine("Item in database with id: {0} already exists\n", Response.Resource.id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                ItemResponse<Category> Response = await this._containerCategory.CreateItemAsync<Category>(item, new PartitionKey(item.id.ToString()));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", Response.Resource.id, Response.RequestCharge);
            }

        }

        public async Task<string> DeleteCategoryAsync(Guid id)
        {

            try
            {
                await this._containerCategory.DeleteItemAsync<Category>(id.ToString(), new PartitionKey(id.ToString()));
                return "Deleted file " + id;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }
        }

        public async Task<string> GetCategoryAsync(Guid id)
        {
            try
            {
                ItemResponse<Category> response = await this._containerCategory.ReadItemAsync<Category>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource.ToString();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }

        public async Task<string> GetCategorysAsync(string queryString)
        {
            try
            {
                var query = this._containerCategory.GetItemQueryIterator<Category>(new QueryDefinition(queryString));
                ManyItems results = new ManyItems();
                Category[] LotsOfItems = new Category[400];
                int iterator = 0;
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    foreach (Category item in response.Resource)
                    {
                        LotsOfItems[iterator] = item;
                        iterator++;
                    }

                }
                Category[] FinalItems = new Category[iterator];
                for (int i = 0; i < iterator; i++)
                {
                    FinalItems[i] = LotsOfItems[i];
                }

                results.Items = FinalItems;
                return results.ToString();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }

        public async Task<string> UpdateCategoryAsync(Guid id, Category item)
        {
            try
            {
                await this._containerCategory.UpsertItemAsync<Category>(item, new PartitionKey(id.ToString()));
                return "Updated file " + id;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }








        public async Task AddPostAsync(Post item)
        {
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Post> Response = await this._containerPost.ReadItemAsync<Post>(item.id.ToString(), new PartitionKey(item.id.ToString()));
                Console.WriteLine("Item in database with id: {0} already exists\n", Response.Resource.id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                ItemResponse<Post> Response = await this._containerPost.CreateItemAsync<Post>(item, new PartitionKey(item.id.ToString()));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", Response.Resource.id, Response.RequestCharge);
            }

        }

        public async Task<string> DeletePostAsync(Guid id)
        {

            try
            {
                await this._containerPost.DeleteItemAsync<Post>(id.ToString(), new PartitionKey(id.ToString()));
                return "Deleted file " + id;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }
        }

        public async Task<string> GetPostAsync(Guid id)
        {
            try
            {
                ItemResponse<Post> response = await this._containerPost.ReadItemAsync<Post>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource.ToString();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }

        public async Task<string> GetPostsAsync(string queryString)
        {
            try
            {
                var query = this._containerPost.GetItemQueryIterator<Post>(new QueryDefinition(queryString));
                ManyItems results = new ManyItems();
                Post[] LotsOfItems = new Post[400];
                int iterator = 0;
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    foreach (Post item in response.Resource)
                    {
                        LotsOfItems[iterator] = item;
                        iterator++;
                    }

                }
                Post[] FinalItems = new Post[iterator];
                for (int i = 0; i < iterator; i++)
                {
                    FinalItems[i] = LotsOfItems[i];
                }

                results.Items = FinalItems;
                return results.ToString();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }

        public async Task<string> UpdatePostAsync(Guid id, Post item)
        {
            try
            {
                await this._containerPost.UpsertItemAsync<Post>(item, new PartitionKey(id.ToString()));
                return "Updated file " + id;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }





        public async Task AddCreatedByAsync(CreatedBy item)
        {
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<CreatedBy> Response = await this._containerCreator.ReadItemAsync<CreatedBy>(item.id.ToString(), new PartitionKey(item.id.ToString()));
                Console.WriteLine("Item in database with id: {0} already exists\n", Response.Resource.id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                ItemResponse<CreatedBy> Response = await this._containerCreator.CreateItemAsync<CreatedBy>(item, new PartitionKey(item.id.ToString()));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", Response.Resource.id, Response.RequestCharge);
            }

        }

        public async Task<string> DeleteCreatedByAsync(Guid id)
        {

            try
            {
                await this._containerCreator.DeleteItemAsync<CreatedBy>(id.ToString(), new PartitionKey(id.ToString()));
                return "Deleted file " + id;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }
        }

        public async Task<string> GetCreatedByAsync(Guid id)
        {
            try
            {
                ItemResponse<CreatedBy> response = await this._containerCreator.ReadItemAsync<CreatedBy>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource.ToString();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }

        public async Task<string> GetCreatedBysAsync(string queryString)
        {
            try
            {
                var query = this._containerCreator.GetItemQueryIterator<CreatedBy>(new QueryDefinition(queryString));
                ManyItems results = new ManyItems();
                CreatedBy[] LotsOfItems = new CreatedBy[400];
                int iterator = 0;
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    foreach (CreatedBy item in response.Resource)
                    {
                        LotsOfItems[iterator] = item;
                        iterator++;
                    }

                }
                CreatedBy[] FinalItems = new CreatedBy[iterator];
                for (int i = 0; i < iterator; i++)
                {
                    FinalItems[i] = LotsOfItems[i];
                }

                results.Items = FinalItems;
                return results.ToString();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }

        public async Task<string> UpdateCreatedByAsync(Guid id, CreatedBy item)
        {
            try
            {
                await this._containerCreator.UpsertItemAsync<CreatedBy>(item, new PartitionKey(id.ToString()));
                return "Updated file " + id;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }








        public async Task AddMediumAsync(Medium item)
        {
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Medium> Response = await this._containerMedium.ReadItemAsync<Medium>(item.id.ToString(), new PartitionKey(item.id.ToString()));
                Console.WriteLine("Item in database with id: {0} already exists\n", Response.Resource.id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                ItemResponse<Medium> Response = await this._containerMedium.CreateItemAsync<Medium>(item, new PartitionKey(item.id.ToString()));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", Response.Resource.id, Response.RequestCharge);
            }

        }

        public async Task<string> DeleteMediumAsync(Guid id)
        {

            try
            {
                await this._containerMedium.DeleteItemAsync<Medium>(id.ToString(), new PartitionKey(id.ToString()));
                return "Deleted file " + id;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }
        }

        public async Task<string> GetMediumAsync(Guid id)
        {
            try
            {
                ItemResponse<Medium> response = await this._containerMedium.ReadItemAsync<Medium>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource.ToString();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }

        public async Task<string> GetMediumsAsync(string queryString)
        {
            try
            {
                var query = this._containerMedium.GetItemQueryIterator<Medium>(new QueryDefinition(queryString));
                ManyItems results = new ManyItems();
                Medium[] LotsOfItems = new Medium[400];
                int iterator = 0;
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    foreach (Medium item in response.Resource)
                    {
                        LotsOfItems[iterator] = item;
                        iterator++;
                    }

                }
                Medium[] FinalItems = new Medium[iterator];
                for (int i = 0; i < iterator; i++)
                {
                    FinalItems[i] = LotsOfItems[i];
                }

                results.Items = FinalItems;
                return results.ToString();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }

        public async Task<string> UpdateMediumAsync(Guid id, Medium item)
        {
            try
            {
                await this._containerMedium.UpsertItemAsync<Medium>(item, new PartitionKey(id.ToString()));
                return "Updated file " + id;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }


        public async Task AddRatingAsync(Rating item)
        {
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Rating> Response = await this._containerFiles.ReadItemAsync<Rating>(item.id.ToString(), new PartitionKey(item.id.ToString()));
                Console.WriteLine("Item in database with id: {0} already exists\n", Response.Resource.id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ItemResponse<Rating> Response = await this._containerFiles.CreateItemAsync<Rating>(item, new PartitionKey(item.id.ToString()));

                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", Response.Resource.id, Response.RequestCharge);
            }

        }

        //public async Task<string> DeleteFileAsync(Guid id)
        //{

        //    try
        //    {
        //        await this._containerFiles.DeleteItemAsync<Files>(id.ToString(), new PartitionKey(id.ToString()));
        //        return "Deleted file " + id;
        //    }
        //    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        //    {
        //        return ex.Message;
        //    }
        //}

        public async Task<string> GetRatingAsync(Guid id)
        {
            try
            {
                ItemResponse<Rating> response = await this._containerRatings.ReadItemAsync<Rating>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource.ToString();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }

        public async Task<string> GetRatingsAsync(string queryString)
        {
            try
            {
                var query = this._containerRatings.GetItemQueryIterator<Rating>(new QueryDefinition(queryString));
                ManyItems results = new ManyItems();
                Rating[] LotsOfFiles = new Rating[400];
                int iterator = 0;
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    foreach (Rating file in response.Resource)
                    {
                        LotsOfFiles[iterator] = file;
                        iterator++;
                    }

                }
                Rating[] FinalFiles = new Rating[iterator];
                for (int i = 0; i < iterator; i++)
                {
                    FinalFiles[i] = LotsOfFiles[i];
                }

                results.Items = FinalFiles;
                return results.ToString();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }

        public async Task<string> UpdateRatingAsync(Guid id, Rating item)
        {
            try
            {
                await this._containerRatings.UpsertItemAsync<Rating>(item, new PartitionKey(id.ToString()));
                return "Updated file " + id;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return ex.Message;
            }

        }


    }
}
