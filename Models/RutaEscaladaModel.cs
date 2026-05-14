using System;

namespace ProyectoEscalada.Models{
    public class RutaEscaladaModel
    {
        public int Id { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public string GradoDificultad { get; set; } = string.Empty; 
        public string TipoAgarre { get; set; } = string.Empty; 
        public bool EsBoulder { get; set; }
    }
}