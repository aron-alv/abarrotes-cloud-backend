using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AbarrotesCloud.Api.Models;

[Table("listablanca")]
[Index("Email", Name = "listablanca_email_key", IsUnique = true)]
public partial class Listablanca
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("email")]
    public string Email { get; set; } = null!;

    [Column("tienda_id")]
    public Guid TiendaId { get; set; }

    [Column("fecha_agregado")]
    public DateTime? FechaAgregado { get; set; }
}
