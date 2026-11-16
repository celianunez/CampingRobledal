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
        public string TextoTipo { get { return Tipo == TipoBungalow.Estandar ? "Estándar" : Tipo.ToString(); } }
        public string TextoEstancia { get { return EstanciaMinima.HasValue ? EstanciaMinima.Value + " noches" : "sin mínimo"; } }

        public string TextoEquipamiento
        {
            get
            {
                System.Collections.Generic.List<string> l = new System.Collections.Generic.List<string>();
                if (AireAcondicionado) l.Add("aire acondicionado");
                if (Cocina) l.Add("cocina");
                if (Terraza) l.Add("terraza");
                if (BanoPropio) l.Add("baño propio");
                return l.Count == 0 ? "ninguno" : string.Join(", ", l);
            }
        }

        public string Descripcion { get; set; }
    }
}
