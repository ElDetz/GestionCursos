namespace API.Dtos.Curso
{
    public class CursoResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Creditos { get; set; }
        public int HorasSemanal { get; set; }
        public string Ciclo { get; set; }
        public string NombreDocente { get; set; }
    }

}
