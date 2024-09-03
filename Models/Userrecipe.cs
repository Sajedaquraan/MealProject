using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealProject.Models;

public partial class Userrecipe
{
    public decimal Recipesid { get; set; }

    public string Title { get; set; } = null!;

    public string Ingredients { get; set; } = null!;

    public string? Video { get; set; }

    public string? Image { get; set; }

    public DateTime? CreatedAt { get; set; }

    public decimal Userloginid { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; }
    public virtual Userlogin Userlogin { get; set; } = null!;
}
