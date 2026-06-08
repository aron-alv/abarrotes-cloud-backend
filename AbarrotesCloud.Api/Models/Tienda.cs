using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AbarrotesCloud.Api.Models;

[Table("tiendas")]
public partial class Tienda
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("nombrenegocio")]
    [StringLength(255)]
    public string Nombrenegocio { get; set; } = null!;

    [Column("fecharegistro")]
    public DateTime? Fecharegistro { get; set; }

    [Column("estatus")]
    [StringLength(50)]
    public string? Estatus { get; set; }

    [InverseProperty("Tienda")]
    public virtual ICollection<Abonoscredito> Abonoscreditos { get; set; } = new List<Abonoscredito>();

    [InverseProperty("Tienda")]
    public virtual ICollection<Categoria> Categoria { get; set; } = new List<Categoria>();

    [InverseProperty("Tienda")]
    public virtual ICollection<Clientescredito> Clientescreditos { get; set; } = new List<Clientescredito>();

    [InverseProperty("Tienda")]
    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

    [InverseProperty("Tienda")]
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    [InverseProperty("Tienda")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    [InverseProperty("Tienda")]
    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
