using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AbarrotesCloud.Api.Models;

[Table("abonoscredito")]
public partial class Abonoscredito
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("clienteid")]
    public Guid Clienteid { get; set; }

    [Column("tiendaid")]
    public Guid Tiendaid { get; set; }

    [Column("usuarioid")]
    public Guid Usuarioid { get; set; }

    [Column("montoabonado")]
    [Precision(10, 2)]
    public decimal Montoabonado { get; set; }

    [Column("fechaabono")]
    public DateTime? Fechaabono { get; set; }

    [ForeignKey("Clienteid")]
    [InverseProperty("Abonoscreditos")]
    public virtual Clientescredito Cliente { get; set; } = null!;

    [ForeignKey("Tiendaid")]
    [InverseProperty("Abonoscreditos")]
    public virtual Tienda Tienda { get; set; } = null!;

    [ForeignKey("Usuarioid")]
    [InverseProperty("Abonoscreditos")]
    public virtual Usuario Usuario { get; set; } = null!;
}
