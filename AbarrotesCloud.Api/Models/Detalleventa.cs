using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AbarrotesCloud.Api.Models;

[Table("detalleventas")]
public partial class Detalleventa
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("ventaid")]
    public Guid Ventaid { get; set; }

    [Column("productoid")]
    public Guid Productoid { get; set; }

    [Column("cantidad")]
    [Precision(10, 3)]
    public decimal Cantidad { get; set; }

    [Column("preciounitario")]
    [Precision(10, 2)]
    public decimal Preciounitario { get; set; }

    [Column("subtotal")]
    [Precision(10, 2)]
    public decimal Subtotal { get; set; }

    [ForeignKey("Productoid")]
    [InverseProperty("Detalleventa")]
    public virtual Producto Producto { get; set; } = null!;

    [ForeignKey("Ventaid")]
    [InverseProperty("Detalleventa")]
    public virtual Venta Venta { get; set; } = null!;
}
