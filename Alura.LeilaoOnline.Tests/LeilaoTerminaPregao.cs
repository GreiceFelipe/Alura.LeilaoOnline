using Alura.LeilaoOnline.Core;
using Xunit;

namespace Alura.LeilaoOnline.Tests
{
    public class LeilaoTerminaPregao
    {
        [Theory]
        [InlineData(1000, new double[] { 800, 900, 1000, 990 })]
        [InlineData(1000, new double[] { 800, 900, 1000 })]
        [InlineData(800, new double[] { 800 })]
        public void RetornaMaiorValorDadoLeilaoComPeloMenosUmLance(double valorEsperado, double[] ofertas)
        {
            IModalidadeAvaliacao modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();
            for (int i = 0; i < ofertas.Length; i++)
            {
                var valor = ofertas[i];
                if ((i % 2) == 0)
                {
                    leilao.RecebeLance(fulano, valor);
                    continue;
                }

                leilao.RecebeLance(maria, valor);

            }

            leilao.TerminaPregao();

            var valorObtido = leilao.Ganhador.Valor;
            Assert.Equal(valorEsperado, valorObtido);
            Assert.Equal(fulano, leilao.Ganhador.Cliente);
            Assert.Equal(EstadoLeilao.LeilaoFinalizado, leilao.Estado);

        }

        [Fact]
        public void LancaInvalidOperationExptionDadoPregaoNaoIniciado()
        {
            IModalidadeAvaliacao modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);

            var excecaoObtida = Assert.Throws<System.InvalidOperationException>(
                () => leilao.TerminaPregao()
            );

            var msgEsperada = "Não é possivel terminar um pregão sem ter inciado. Utilize o método IniciaPregao() para incia-lo.";

            Assert.Equal(msgEsperada, excecaoObtida.Message);

        }

        [Fact]
        public void RetornaZeroDadoLeilaoSemLances()
        {
            IModalidadeAvaliacao modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);


            leilao.IniciaPregao();
            leilao.TerminaPregao();

            var valorEsperado = 0;
            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
            Assert.Equal(EstadoLeilao.LeilaoFinalizado, leilao.Estado);
        }

        [Theory]
        [InlineData(1200, 1250,new double[] {800,1150,1400,1250})]
        public void RetornaValorMaisProximoDadoLeilaoNessaModalidade(
            double valorDestino,
            double valorEsperado,
            double[] ofertas)
        {
            IModalidadeAvaliacao modalidade = new OfertaSuperiorMaisProxima(valorDestino);
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();
            for (int i = 0; i < ofertas.Length; i++)
            {
                var valor = ofertas[i];
                if ((i % 2) == 0)
                {
                    leilao.RecebeLance(fulano, valor);
                    continue;
                }

                leilao.RecebeLance(maria, valor);

            }

            leilao.TerminaPregao();

            var valorObtido = leilao.Ganhador.Valor;
            Assert.Equal(valorEsperado, valorObtido);
            Assert.Equal(EstadoLeilao.LeilaoFinalizado, leilao.Estado);
        }
    }
}
