namespace CosmosAPI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CosmosAPI.Models;

    public interface ICosmosDbService
    {
        Task<string> GetFilesAsync(string query);
        Task<string> GetFileAsync(Guid id);
        Task AddFileAsync(Files item);
        Task<string> UpdateFileAsync(Guid id, Files item);
        Task<string> DeleteFileAsync(Guid id);



        Task<string> GetCategorysAsync(string query);
        Task<string> GetCategoryAsync(Guid id);
        Task AddCategoryAsync(Category item);
        Task<string> UpdateCategoryAsync(Guid id, Category item);
        Task<string> DeleteCategoryAsync(Guid id);





        Task<string> GetMediumsAsync(string query);
        Task<string> GetMediumAsync(Guid id);
        Task AddMediumAsync(Medium item);
        Task<string> UpdateMediumAsync(Guid id, Medium item);
        Task<string> DeleteMediumAsync(Guid id);






        Task<string> GetCreatedBysAsync(string query);
        Task<string> GetCreatedByAsync(Guid id);
        Task AddCreatedByAsync(CreatedBy item);
        Task<string> UpdateCreatedByAsync(Guid id, CreatedBy item);
        Task<string> DeleteCreatedByAsync(Guid id);






        Task<string> GetPostsAsync(string query);
        Task<string> GetPostAsync(Guid id);
        Task AddPostAsync(Post item);
        Task<string> UpdatePostAsync(Guid id, Post item);
        Task<string> DeletePostAsync(Guid id);


        Task<string> GetRatingsAsync(string query);
        Task<string> GetRatingAsync(Guid id);
        //Task AddRatingAsync(Rating item);
        Task<string> UpdateRatingAsync(Guid id, Rating item);
        //Task<string> DeleteRatingAsync(Guid id);

    }
}