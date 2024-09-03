using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealProject.Models;

public partial class Customer
{
    public decimal Userid { get; set; }

    public string? Username { get; set; }

    public string? Useremail { get; set; }

    public string? Userpassword { get; set; }

    public string? Userimage { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public decimal? Googleid { get; set; }

    public DateTime? Registerdate { get; set; }

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();
}
