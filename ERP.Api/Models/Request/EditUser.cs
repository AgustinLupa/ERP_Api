using ERP.Api.Entity;
using System.ComponentModel.DataAnnotations;

namespace ERP.Api.Models.Request;

public class EditUser
{
    [Required(ErrorMessage = "El campo 'usuario' es obligatorio.")]
    [StringLength(20, MinimumLength = 4, ErrorMessage = "El nombre de usuario debe tener una longitud de entre 4 y 20 caracteres.")]
    public string username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe indicar un rol para el usuario.")]
    public Roles id_role { get; set; }
    public int state { get; set; } = 1;

}
