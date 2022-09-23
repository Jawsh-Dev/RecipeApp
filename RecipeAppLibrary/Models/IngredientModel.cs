namespace RecipeAppLibrary.Models;

public class IngredientModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IngredientId { get; set; }
    public string IngredientName { get; set; }
}
