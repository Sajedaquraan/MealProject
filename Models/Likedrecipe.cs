using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealProject.Models;

public partial class Likedrecipe
{
    public decimal Likedid { get; set; }

    public string? Recipelabel { get; set; }

    public string? Recipedata { get; set; }

    public string? Recipeimage { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public DateTime? Createdat { get; set; }

    public decimal? Userloginid { get; set; }

    public virtual Userlogin? Userlogin { get; set; }
}
