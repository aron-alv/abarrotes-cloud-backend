using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AbarrotesCloud.Api.Models;

[Table("usuarios")]
public partial class Usuario
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("tiendaid")]
    public Guid Tiendaid { get; set; }

    [Column("nombrecompleto")]
    [StringLength(255)]
    public string Nombrecompleto { get; set; } = null!;

    [Column("rol")]
    [StringLength(50)]
    public string Rol { get; set; } = null!;

    [Column("pinventa")]
    [StringLength(4)]
    public string? Pinventa { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [InverseProperty("Usuario")]
    public virtual ICollection<Abonoscredito> Abonoscreditos { get; set; } = new List<Abonoscredito>();

    [InverseProperty("Usuario")]
    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

    [ForeignKey("Tiendaid")]
    [InverseProperty("Usuarios")]
    public virtual Tienda Tienda { get; set; } = null!;

    [InverseProperty("Usuario")]
    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
