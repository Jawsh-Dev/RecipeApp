using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace RecipeAppLibrary.DataAccess;

public class DbConnection : IDbConnection
{
    private readonly IConfiguration _config;
    private readonly IMongoDatabase _db;
    private string _connectionId = "MongoDB";

    public string DbName { get; private set; }
    public string CategoryCollectionName { get; private set; } = "categories";
    public string IngredientCollectionName { get; private set; } = "ingredients";
    public string UserCollectionName { get; private set; } = "users";
    public string RecipeCollectionName { get; private set; } = "recipes";

    public MongoClient Client { get; private set; }
    public IMongoCollection<CategoryModel> CategoryCollection { get; private set; }
    public IMongoCollection<IngredientModel> IngredientCollection { get; private set; }
    public IMongoCollection<UserModel> UserCollection { get; private set; }
    public IMongoCollection<RecipeModel> RecipeCollection { get; private set; }

    public DbConnection(IConfiguration config)
    {
        _config = config;
        Client = new MongoClient(_config.GetConnectionString(_connectionId));
        DbName = _config["DatabaseName"];
        _db = Client.GetDatabase(DbName);

        CategoryCollection = _db.GetCollection<CategoryModel>(CategoryCollectionName);
        IngredientCollection = _db.GetCollection<IngredientModel>(IngredientCollectionName);
        UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
        RecipeCollection = _db.GetCollection<RecipeModel>(RecipeCollectionName);
    }
}
