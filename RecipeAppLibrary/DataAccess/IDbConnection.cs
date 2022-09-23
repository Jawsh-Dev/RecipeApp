using MongoDB.Driver;

namespace RecipeAppLibrary.DataAccess;

public interface IDbConnection
{
    IMongoCollection<CategoryModel> CategoryCollection { get; }
    string CategoryCollectionName { get; }
    MongoClient Client { get; }
    string DbName { get; }
    IMongoCollection<IngredientModel> IngredientCollection { get; }
    string IngredientCollectionName { get; }
    IMongoCollection<RecipeModel> RecipeCollection { get; }
    string RecipeCollectionName { get; }
    IMongoCollection<UserModel> UserCollection { get; }
    string UserCollectionName { get; }
}