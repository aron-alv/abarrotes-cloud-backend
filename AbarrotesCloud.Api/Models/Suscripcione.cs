using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AbarrotesCloud.Api.Models;

[Table("suscripciones")]
public partial class Suscripcione
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("tiendaid")]
    public Guid? Tiendaid { get; set; }

    [Column("monto")]
    public decimal Monto { get; set; }

    [Column("fechapago")]
    public DateTime? Fechapago { get; set; }

    [Column("periodoinicio")]
    public DateTime Periodoinicio { get; set; }

    [Column("metodopago", TypeName = "character varying")]
    public string Metodopago { get; set; } = null!;

    [Column("estatus", TypeName = "character varying")]
    public string Estatus { get; set; } = null!;

    [ForeignKey("Tiendaid")]
    [InverseProperty("Suscripciones")]
    public virtual Tienda? Tienda { get; set; }
}
