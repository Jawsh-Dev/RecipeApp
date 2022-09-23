namespace RecipeAppLibrary.Models;

public class BasicRecipeModel
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }

    public BasicRecipeModel()
    {

    }

    public BasicRecipeModel(RecipeModel recipe)
    {
        Id = recipe.Id;
        Title = recipe.Title;
    }
}
