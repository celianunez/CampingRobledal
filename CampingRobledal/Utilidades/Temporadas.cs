using System;

namespace CampingRobledal.Utilidades
{
    /// <summary>Temporada de una fecha y precio de una parcela en ese periodo (D-010).</summary>
    public static class Temporadas
    {
        public static Temporada DeFecha(DateTime fecha)
        {
            int mes = fecha.Month;
            if (mes == 7 || mes == 8) return Temporada.Alta;
            if (mes == 6 || mes == 9 || mes == 4 || mes == 5) return Temporada.Media;
            return Temporada.Baja;
        }

        /// <summary>Suma el precio de cada noche entre entrada (incluida) y salida (excluida).</summary>
        public static decimal ImporteParcela(Parcela parcela, DateTime entrada, DateTime salida)
        {
            decimal total = 0;
            for (DateTime dia = entrada.Date; dia < salida.Date; dia = dia.AddDays(1))
                total += parcela.PrecioPara(DeFecha(dia));
            return total;
        }
    }
}
