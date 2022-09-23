using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeAppLibrary.DataAccess;

public class MongoRecipeData : IRecipeData
{
    private readonly IDbConnection _db;
    private readonly IUserData _userData;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<RecipeModel> _recipes;
    private const string CacheName = "RecipeData";

    public MongoRecipeData(IDbConnection db, IUserData userData, IMemoryCache cache)
    {
        _db = db;
        _userData = userData;
        _cache = cache;
        _recipes = db.RecipeCollection;
    }

    public async Task<List<RecipeModel>> GetAllRecipes()
    {
        var output = _cache.Get<List<RecipeModel>>(CacheName);
        if (output is null)
        {
            var results = await _recipes.FindAsync(_ => true);
            output = results.ToList();

            _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
        }

        return output;
    }

    public async Task<List<RecipeModel>> GetUsersRecipes(string userId)
    {
        var output = _cache.Get<List<RecipeModel>>(userId);
        if (output is null)
        {
            var results = await _recipes.FindAsync(r => r.Author.Id == userId);
            output = results.ToList();

            _cache.Set(userId, output, TimeSpan.FromMinutes(1));
        }

        return output;
    }

    public async Task<RecipeModel> GetRecipe(string id)
    {
        var results = await _recipes.FindAsync(r => r.Id == id);
        return results.FirstOrDefault();
    }

    public async Task UpdateRecipe(RecipeModel recipe)
    {
        await _recipes.ReplaceOneAsync(r => r.Id == recipe.Id, recipe);
        _cache.Remove(CacheName);
    }

    public async Task CreateRecipe(RecipeModel recipe)
    {
        var client = _db.Client;

        using var session = await client.StartSessionAsync();

        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_db.DbName);
            var recipesInTransaction = db.GetCollection<RecipeModel>(_db.RecipeCollectionName);
            await recipesInTransaction.InsertOneAsync(session, recipe);

            var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
            var user = await _userData.GetUser(recipe.Author.Id);
            user.AuthoredRecipes.Add(new BasicRecipeModel(recipe));
            await usersInTransaction.ReplaceOneAsync(session, u => u.Id == user.Id, user);

            await session.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }
}
