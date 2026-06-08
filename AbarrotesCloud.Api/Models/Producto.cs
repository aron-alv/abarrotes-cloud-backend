using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AbarrotesCloud.Api.Models;

[Table("productos")]
public partial class Producto
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("tiendaid")]
    public Guid Tiendaid { get; set; }

    [Column("categoriaid")]
    public Guid Categoriaid { get; set; }

    [Column("codigobarras")]
    [StringLength(100)]
    public string? Codigobarras { get; set; }

    [Column("nombre")]
    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    [Column("preciocompra")]
    [Precision(10, 2)]
    public decimal Preciocompra { get; set; }

    [Column("precioventa")]
    [Precision(10, 2)]
    public decimal Precioventa { get; set; }

    [Column("tipounidad")]
    [StringLength(50)]
    public string Tipounidad { get; set; } = null!;

    [Column("stockactual")]
    [Precision(10, 3)]
    public decimal Stockactual { get; set; }

    [Column("stockminimo")]
    [Precision(10, 3)]
    public decimal Stockminimo { get; set; }

    [ForeignKey("Categoriaid")]
    [InverseProperty("Productos")]
    public virtual Categoria Categoria { get; set; } = null!;

    [InverseProperty("Producto")]
    public virtual ICollection<Detalleventa> Detalleventa { get; set; } = new List<Detalleventa>();

    [InverseProperty("Producto")]
    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

    [ForeignKey("Tiendaid")]
    [InverseProperty("Productos")]
    public virtual Tienda Tienda { get; set; } = null!;
}
