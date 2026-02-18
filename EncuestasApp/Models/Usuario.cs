using SQLite;
using System.ComponentModel.DataAnnotations;

namespace EncuestaApp.Models;

public class Usuario
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    [Unique] // SQLite-net
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Password { get; set; } = "";

    public string NombreCompleto { get; set; } = "";
}