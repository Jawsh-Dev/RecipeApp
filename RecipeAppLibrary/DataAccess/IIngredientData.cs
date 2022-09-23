
namespace RecipeAppLibrary.DataAccess;

public interface IIngredientData
{
    Task CreateIngredient(IngredientModel ingredient);
    Task<List<IngredientModel>> GetAllIngredients();
    Task<List<IngredientModel>> GetIngredient(string ingredientName);
    Task UpdateIngredient(IngredientModel ingredient);
}