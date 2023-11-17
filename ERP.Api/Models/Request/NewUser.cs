using ERP.Api.Entity;
using System.ComponentModel.DataAnnotations;

namespace ERP.Api.Models.Request;

public class NewUser
{
    [Required(ErrorMessage ="El campo 'usuario' es obligatorio.")]
    [StringLength(20, MinimumLength = 4, ErrorMessage = "El nombre de usuario debe tener una longitud de entre 4 y 20 caracteres.")]
    public string username { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo 'contraseña' es obligatorio.")]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "La contraseña de usuario debe tener una longitud de entre 8 y 30 caracteres.")]
    public string password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe indicar un rol para el usuario.")]
    public Roles id_role { get; set; }

}
