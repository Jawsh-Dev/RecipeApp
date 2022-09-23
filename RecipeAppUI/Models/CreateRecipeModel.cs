using System.ComponentModel.DataAnnotations;

namespace RecipeAppUI.Models;

public class CreateRecipeModel
{
    [Required]
    [MaxLength(75)]
    public string Title { get; set; }

    [Required]
    [MinLength(1)]
    [Display(Name = "Category")]
    public string CategoryId { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    public List<string> Ingredients { get; set; } = new();

    [Required]
    public List<string> Steps { get; set; } = new();
}
