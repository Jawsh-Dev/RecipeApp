
namespace RecipeAppLibrary.DataAccess;

public interface IRecipeData
{
    Task CreateRecipe(RecipeModel recipe);
    Task<List<RecipeModel>> GetAllRecipes();
    Task<RecipeModel> GetRecipe(string id);
    Task<List<RecipeModel>> GetUsersRecipes(string userId);
    Task UpdateRecipe(RecipeModel recipe);
}