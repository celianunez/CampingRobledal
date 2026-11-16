using System.Globalization;

namespace CampingRobledal.Utilidades
{
    public static class Texto
    {
        /// <summary>Búsqueda sin distinguir mayúsculas ni tildes. Una búsqueda vacía coincide siempre.</summary>
        public static bool Contiene(string texto, string busqueda)
        {
            if (string.IsNullOrWhiteSpace(busqueda)) return true;
            if (texto == null) return false;
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(texto, busqueda.Trim(),
                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }
    }
}
