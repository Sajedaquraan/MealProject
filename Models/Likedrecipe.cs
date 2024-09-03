using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealProject.Models;

public partial class Likedrecipe
{
    public decimal Likedid { get; set; }

    public string RecipeLabel { get; set; } = null!;

    public string RecipeData { get; set; } = null!;

    public string? RecipeImage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public decimal Userloginid { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; }
    public virtual Userlogin Userlogin { get; set; } = null!;
}
