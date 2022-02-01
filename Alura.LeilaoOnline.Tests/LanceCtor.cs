using Alura.LeilaoOnline.Core;
using Xunit;

namespace Alura.LeilaoOnline.Tests
{
    public class LanceCtor
    {
        [Fact]
        public void LancaArgumentExceptionDadoValorNegativo()
        {
            var valorNegativo = -100;

            var excecaoObtida = Assert.Throws<System.ArgumentException>(
                    () => new Lance(null, valorNegativo)
                );

            var msgEsperada = "Valor do lance deve ser maior ou igual a zero.";

            Assert.Equal(msgEsperada, excecaoObtida.Message);
        }

    }
}
