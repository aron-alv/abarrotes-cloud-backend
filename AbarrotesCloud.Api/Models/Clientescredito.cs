using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AbarrotesCloud.Api.Models;

[Table("clientescredito")]
public partial class Clientescredito
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("tiendaid")]
    public Guid Tiendaid { get; set; }

    [Column("nombrecompleto")]
    [StringLength(255)]
    public string Nombrecompleto { get; set; } = null!;

    [Column("telefono")]
    [StringLength(20)]
    public string? Telefono { get; set; }

    [Column("limitecredito")]
    [Precision(10, 2)]
    public decimal Limitecredito { get; set; }

    [Column("saldoactual")]
    [Precision(10, 2)]
    public decimal Saldoactual { get; set; }

    [Column("estatus")]
    [StringLength(50)]
    public string? Estatus { get; set; }

    [InverseProperty("Cliente")]
    public virtual ICollection<Abonoscredito> Abonoscreditos { get; set; } = new List<Abonoscredito>();

    [ForeignKey("Tiendaid")]
    [InverseProperty("Clientescreditos")]
    public virtual Tienda Tienda { get; set; } = null!;

    [InverseProperty("Clientecredito")]
    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
