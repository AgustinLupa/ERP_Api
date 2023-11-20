using System.ComponentModel.DataAnnotations;

namespace ERP.Api.Models.Request;

public class LoginCredentials
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El campo 'usuario' es requerido")]
    public string username { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El campo 'contraseña' es requerido")]
    public string password { get; set; } = string.Empty;
}
