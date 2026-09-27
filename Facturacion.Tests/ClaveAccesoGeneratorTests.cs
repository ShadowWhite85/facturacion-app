using Facturacion.Api.Services;

namespace Facturacion.Tests;

public class ClaveAccesoGeneratorTests
{
    [Fact]
    public void Generar_ClaveDe49Digitos()
    {
        var clave = ClaveAccesoGenerator.Generar(
            new DateTime(2026, 9, 27), "01", "0601234567001", "1", "001", "001", "000000001");

        Assert.Equal(49, clave.Length);
        Assert.All(clave, c => Assert.True(char.IsDigit(c)));
    }

    [Fact]
    public void Generar_EstructuraSegunNormativaSri()
    {
        var clave = ClaveAccesoGenerator.Generar(
            new DateTime(2026, 9, 27), "01", "0601234567001", "1", "001", "001", "000000001");

        Assert.Equal("27092026", clave.Substring(0, 8));    // fecha ddmmaaaa
        Assert.Equal("01", clave.Substring(8, 2));          // codDoc factura
        Assert.Equal("0601234567001", clave.Substring(10, 13)); // RUC
        Assert.Equal("1", clave.Substring(23, 1));          // ambiente pruebas
        Assert.Equal("001001", clave.Substring(24, 6));     // série estab+ptoEmi
        Assert.Equal("000000001", clave.Substring(30, 9));  // secuencial
    }

    [Fact]
    public void Generar_DigitoVerificadorEsModulo11()
    {
        var clave = ClaveAccesoGenerator.Generar(
            new DateTime(2026, 9, 27), "01", "0601234567001", "1", "001", "001", "000000001");

        // Verifica el dígito recomputando módulo 11 con ponderadores 2..7
        var cuerpo = clave.Substring(0, 48);
        var suma = 0;
        var factor = 2;
        for (var i = cuerpo.Length - 1; i >= 0; i--)
        {
            suma += (cuerpo[i] - '0') * factor;
            factor = factor == 7 ? 2 : factor + 1;
        }
        var esperado = 11 - (suma % 11);
        esperado = esperado == 11 ? 0 : esperado == 10 ? 1 : esperado;

        Assert.Equal(esperado.ToString(), clave.Substring(48, 1));
    }
}
