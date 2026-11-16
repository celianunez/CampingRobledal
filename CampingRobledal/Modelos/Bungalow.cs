namespace CampingRobledal
{
    public class Bungalow : Alojamiento
    {
        public TipoBungalow Tipo { get; set; }
        public decimal PrecioNoche { get; set; }
        /// <summary>Estancia mínima en noches; null si no hay mínimo.</summary>
        public int? EstanciaMinima { get; set; }
        public bool AireAcondicionado { get; set; }
        public bool Cocina { get; set; }
        public bool Terraza { get; set; }
        public bool BanoPropio { get; set; }
        public string Descripcion { get; set; }
    }
}
