using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

namespace Imag.Demo.Shared;

[Index("Name", Name = "Name")]
public class Customer
{
    /*
    Model danych: Utwórz model danych Customer z następującymi polami:
    - Id: Unikalny identyfikator kontrahenta.
    - Name: Nazwa kontrahenta.
    - Address: Adres kontrahenta.
    - NIP: Numer Identyfikacji Podatkowej kontrahenta. 
    */

    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "nvarchar")]
    [StringLength(50)]
    public string? Adress { get; set; }

    [Column(TypeName = "nvarchar")]
    [StringLength(20)]
    //NIP normalized to EU -> XX 0000000000+
    public string? NIP { get; set; }

    
}

