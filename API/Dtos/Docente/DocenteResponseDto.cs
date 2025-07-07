namespace API.Dtos.Docente
{
    public class DocenteResponseDto
    {
        public int Id { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string Profesion { get; set; }
        public string Correo { get; set; }
        public int TotalCursos { get; set; }
    }
}
