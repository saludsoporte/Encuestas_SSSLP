using SQLite;

namespace EncuestaApp.Models;

[Table("Localidad")]
public class Localidad
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public String Cve_Ent { get; set; }  
    public String Cve_Mun { get; set; }  
    public String Cve_Loc { get; set; } 

    public string LocalidadNombre { get; set; } = string.Empty;

    public string Ambito { get; set; } = string.Empty; 
}