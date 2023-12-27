using ERP.Api.Entity;
using System.ComponentModel.DataAnnotations;

namespace ERP.Api.Models.Request;

public class SaveSupplier
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El campo 'nombre' es requerido.")]
    public string Name { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El campo 'direccion' es requerido.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo 'telefono' es requerido.")]
    public int Phone { get; set; }
    public int State { get; set; } = 1;

}
