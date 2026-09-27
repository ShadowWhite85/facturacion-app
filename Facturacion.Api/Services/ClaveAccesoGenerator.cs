using System.Text;

namespace Facturacion.Api.Services;

/// <summary>
/// Genera la clave de acceso de 49 dígitos según la normativa del SRI:
/// fecha(ddmmaaaa) + codDoc + RUC + ambiente + serie(estab+ptoEmi) +
/// secuencial + código numérico + tipo de emisión + dígito verificador (módulo 11).
/// </summary>
public static class ClaveAccesoGenerator
{
    public static string Generar(
        DateTime fechaEmision,
        string codigoDocumento,   // 01 = factura
        string ruc,
        string ambiente,          // 1 = pruebas, 2 = producción
        string establecimiento,   // 001
        string puntoEmision,      // 001
        string secuencial,        // 9 dígitos
        string tipoEmision = "1") // 1 = normal
    {
        var codigoNumerico = Random.Shared.Next(10000000, 99999999).ToString();

        var clave = new StringBuilder(49)
            .Append(fechaEmision.ToString("ddMMyyyy"))
            .Append(codigoDocumento)
            .Append(ruc)
            .Append(ambiente)
            .Append(establecimiento)
            .Append(puntoEmision)
            .Append(secuencial)
            .Append(codigoNumerico)
            .Append(tipoEmision)
            .ToString();

        return clave + CalcularDigitoVerificador(clave);
    }

    /// <summary>Módulo 11: ponderadores 2..7 de derecha a izquierda.</summary>
    private static int CalcularDigitoVerificador(string clave48)
    {
        var suma = 0;
        var factor = 2;
        for (var i = clave48.Length - 1; i >= 0; i--)
        {
            suma += (clave48[i] - '0') * factor;
            factor = factor == 7 ? 2 : factor + 1;
        }

        var residuo = suma % 11;
        var digito = 11 - residuo;
        return digito == 11 ? 0 : digito == 10 ? 1 : digito;
    }
}
