namespace presupuesto_api.Models.DTOs;

public class RegistroSolicitudDto
    {
        public string Nickname { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
    }