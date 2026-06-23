namespace presupuesto_api.Models.DTOs;

    public class LoginSolicitudDto
    {
        public string Correo { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
    }