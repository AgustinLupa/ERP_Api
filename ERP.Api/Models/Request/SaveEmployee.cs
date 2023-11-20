using System.ComponentModel.DataAnnotations;

namespace ERP.Api.Models.Request
{
    public class SaveEmployee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo 'nombre' es obligatorio.")]
        [StringLength(60,MinimumLength = 4, ErrorMessage = "El nombre de empleado debe tener una longitud de entre 4 y 60 caracteres.")]
        public string Name { get; set; }=string.Empty;

        [Required(ErrorMessage = "El campo 'apellido' es obligatorio.")]
        [StringLength(60, MinimumLength = 4, ErrorMessage = "El apellido del empleado debe tener una longitud de entre 4 y 60 caracteres.")]
        public string LastName { get; set; } = string.Empty;

        public int State { get; set; } = 1;

        [Required(ErrorMessage = "El campo 'dni' es obligatorio.")]
        [Range(10000000, 100000000, ErrorMessage = "eL campo 'dni' debe encontrarse entre el rango 10000000 a 100000000")]
        public int Dni { get; set; }

        [Required(ErrorMessage = "El campo 'codigo de empleado' es obligatorio.")]
        public int Code_Employee { get; set; }
    }
}
