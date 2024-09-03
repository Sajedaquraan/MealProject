using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealProject.Models;

public partial class Customer
{
    public decimal Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Useremail { get; set; } = null!;

    public string Userpassword { get; set; } = null!;

    public string? Userimage { get; set; }

    public decimal? Googleid { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }
    public DateTime? Registerdate { get; set; }

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();
}
