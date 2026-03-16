using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EIKON.UI.Models;


public partial class Trhem000old
{
    public string? EmCodigo { get; set; } = null!;

    public string? EmDescri { get; set; } = null!;

    public string? Activo { get; set; } = null!;

    public string? Direcc { get; set; } = null!;

    public string? Telefono { get; set; } = null!;

    public string? Fax { get; set; } = null!;

    public string? Apostal { get; set; } = null!;

    public string? Status { get; set; } = null!;

    public string? Registro { get; set; } = null!;

    public string? Provincia { get; set; } = null!;

    public string? Actividad { get; set; } = null!;

    public decimal Capital { get; set; }

    public string? Clase { get; set; } = null!;

    public string? EmFiscal { get; set; } = null!;

    public string? EmComenta { get; set; } = null!;

    public DateTime? EmAnivers { get; set; }

    public string? EmNombrec { get; set; } = null!;

    public string? EmRnc { get; set; } = null!;

    public string? EmEMail { get; set; } = null!;

    public string? EmBmp { get; set; } = null!;

    public string? EmTimbre { get; set; } = null!;

    public string? EmNoidss { get; set; } = null!;

    public string? EmForfech { get; set; } = null!;

    public string? EmPoliza { get; set; } = null!;
           
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdentityColumn { get; set; } 

    public string? EmSplash { get; set; } = null!;

    public string? CyCodigo { get; set; } = null!;

    public DateTime? EmDesde { get; set; } = null!;

    public DateTime? EmHasta { get; set; }= null!;

    public string? CreatedBy { get; set; } = null!;

    public DateTime? CreatedOn { get; set; } = DateTime.Now;

    public string? ChangedBy { get; set; } = null!;

    public DateTime? ChangedOn { get; set; }

    public string? NcCodigo { get; set; } = null!;

    public string? EmDesalterno { get; set; } = null!;

    public string? LcCodigo { get; set; } = null!;
}
