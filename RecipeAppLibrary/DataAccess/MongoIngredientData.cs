namespace RecipeAppLibrary.DataAccess;

public class MongoIngredientData : IIngredientData
{
    private readonly IMongoCollection<IngredientModel> _ingredients;
    private readonly IMemoryCache _cache;
    private const string CacheName = "IngredientData";

    public MongoIngredientData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _ingredients = db.IngredientCollection;

    }

    public async Task<List<IngredientModel>> GetAllIngredients()
    {
        var output = _cache.Get<List<IngredientModel>>(CacheName);
        if (output is null)
        {
            var results = await _ingredients.FindAsync(_ => true);
            output = results.ToList();

            _cache.Set(CacheName, output, TimeSpan.FromDays(1));
        }
        return output;
    }

    public async Task<List<IngredientModel>> GetIngredient(string ingredientName)
    {
        var output = _cache.Get<List<IngredientModel>>(ingredientName);
        if (output is null)
        {
            var results = await _ingredients.FindAsync(i => i.IngredientName == ingredientName);
            output = results.ToList();

            _cache.Set(ingredientName, output, TimeSpan.FromMinutes(1));
        }

        return output;
    }

    public Task CreateIngredient(IngredientModel ingredient)
    {
        return _ingredients.InsertOneAsync(ingredient);
    }

    public Task UpdateIngredient(IngredientModel ingredient)
    {
        var filter = Builders<IngredientModel>.Filter.Eq("IngredientId", ingredient.IngredientId);
        return _ingredients.ReplaceOneAsync(filter, ingredient, new ReplaceOptions { IsUpsert = true });
    }
}
