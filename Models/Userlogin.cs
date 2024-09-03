using System;
using System.Collections.Generic;

namespace MealProject.Models;

public partial class Userlogin
{
    public decimal Userloginid { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Roleid { get; set; }

    public virtual ICollection<Likedrecipe> Likedrecipes { get; set; } = new List<Likedrecipe>();

    public virtual Role? Role { get; set; }

    public virtual Customer? User { get; set; }

    public virtual ICollection<Userrecipe> Userrecipes { get; set; } = new List<Userrecipe>();
}
