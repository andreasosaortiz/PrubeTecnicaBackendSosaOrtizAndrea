using System;
using System.Collections.Generic;

namespace PruebaBackendconEntityFramework.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public double? Acumulado { get; set; }

    public int? Idtipo { get; set; }

    public string? IdAuto { get; set; }

    public virtual Auto? IdAutoNavigation { get; set; }

    public virtual Tipo? IdtipoNavigation { get; set; }
}
