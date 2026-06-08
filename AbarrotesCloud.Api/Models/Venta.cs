using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AbarrotesCloud.Api.Models;

[Table("ventas")]
public partial class Venta
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("tiendaid")]
    public Guid Tiendaid { get; set; }

    [Column("usuarioid")]
    public Guid Usuarioid { get; set; }

    [Column("clientecreditoid")]
    public Guid? Clientecreditoid { get; set; }

    [Column("fechaventa")]
    public DateTime? Fechaventa { get; set; }

    [Column("total")]
    [Precision(10, 2)]
    public decimal Total { get; set; }

    [Column("metodopago")]
    [StringLength(50)]
    public string Metodopago { get; set; } = null!;

    [Column("estatus")]
    [StringLength(50)]
    public string? Estatus { get; set; }

    [ForeignKey("Clientecreditoid")]
    [InverseProperty("Venta")]
    public virtual Clientescredito? Clientecredito { get; set; }

    [InverseProperty("Venta")]
    public virtual ICollection<Detalleventa> Detalleventa { get; set; } = new List<Detalleventa>();

    [ForeignKey("Tiendaid")]
    [InverseProperty("Venta")]
    public virtual Tienda Tienda { get; set; } = null!;

    [ForeignKey("Usuarioid")]
    [InverseProperty("Venta")]
    public virtual Usuario Usuario { get; set; } = null!;
}
