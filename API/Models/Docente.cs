
namespace API.Models
{
    public class Docente
    {
        public int Id { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string Profesion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Correo { get; set; }

        public ICollection<Curso> Cursos { get; set; }
    }

}
