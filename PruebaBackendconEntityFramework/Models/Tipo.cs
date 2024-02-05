using System;
using System.Collections.Generic;

namespace PruebaBackendconEntityFramework.Models;

public partial class Tipo
{
    public int Idtipo { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
