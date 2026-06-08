using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AbarrotesCloud.Api.Models;

[Table("kardex")]
public partial class Kardex
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("tiendaid")]
    public Guid Tiendaid { get; set; }

    [Column("productoid")]
    public Guid Productoid { get; set; }

    [Column("usuarioid")]
    public Guid Usuarioid { get; set; }

    [Column("tipomovimiento")]
    [StringLength(50)]
    public string Tipomovimiento { get; set; } = null!;

    [Column("motivo")]
    [StringLength(100)]
    public string Motivo { get; set; } = null!;

    [Column("cantidad")]
    [Precision(10, 3)]
    public decimal Cantidad { get; set; }

    [Column("fechamovimiento")]
    public DateTime? Fechamovimiento { get; set; }

    [ForeignKey("Productoid")]
    [InverseProperty("Kardices")]
    public virtual Producto Producto { get; set; } = null!;

    [ForeignKey("Tiendaid")]
    [InverseProperty("Kardices")]
    public virtual Tienda Tienda { get; set; } = null!;

    [ForeignKey("Usuarioid")]
    [InverseProperty("Kardices")]
    public virtual Usuario Usuario { get; set; } = null!;
}
