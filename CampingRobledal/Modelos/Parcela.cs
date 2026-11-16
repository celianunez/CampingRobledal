namespace CampingRobledal
{
    public class Parcela : Alojamiento
    {
        public CategoriaParcela Categoria { get; set; }
        public string Ubicacion { get; set; }
        public bool Electricidad { get; set; }
        public bool Agua { get; set; }
        public bool Desague { get; set; }
        public bool Sombra { get; set; }
        public decimal PrecioBaja { get; set; }
        public decimal PrecioMedia { get; set; }
        public decimal PrecioAlta { get; set; }
        public string Observaciones { get; set; }
        public string Plano { get; set; }

        public decimal PrecioPara(Temporada temporada)
        {
            switch (temporada)
            {
                case Temporada.Baja: return PrecioBaja;
                case Temporada.Media: return PrecioMedia;
                default: return PrecioAlta;
            }
        }
    }
}
