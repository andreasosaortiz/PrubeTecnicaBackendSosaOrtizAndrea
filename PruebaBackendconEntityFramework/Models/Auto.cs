using System;
using System.Collections.Generic;

namespace PruebaBackendconEntityFramework.Models;

public partial class Auto
{
    public int ID { get; set; }

    public string Patente { get; set; } = null!;

    public virtual ICollection<Estancium> Estancia { get; set; } = new List<Estancium>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
