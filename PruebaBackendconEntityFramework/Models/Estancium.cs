using System;
using System.Collections.Generic;

namespace PruebaBackendconEntityFramework.Models;

public partial class Estancium
{
    public int Id { get; set; }

    public DateTime? HsEntrada { get; set; }

    public DateTime? HsSalida { get; set; }

    public double? Costo { get; set; }

    public string? IdAuto { get; set; }

    public virtual Auto? IdAutoNavigation { get; set; }
}
