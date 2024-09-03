using System;
using System.Collections.Generic;

namespace MealProject.Models;

public partial class Contact
{
    public decimal Contactid { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Contant { get; set; }

    public DateTime? CreatedAt { get; set; }
}
