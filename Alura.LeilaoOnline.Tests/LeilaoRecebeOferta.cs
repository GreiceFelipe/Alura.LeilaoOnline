using System.Linq;
using Alura.LeilaoOnline.Core;
using Xunit;

namespace Alura.LeilaoOnline.Tests
{

    // O xUnit possui outras duas anotações, [MemberData] e[ClassData], onde você pode construir
    // os dados de entrada usando membros da própria classe de teste ou então uma classe específica para isso.
    //Veja mais detalhes nesse artigo (em inglês):
    //https://andrewlock.net/creating-parameterised-tests-in-xunit-with-inlinedata-classdata-and-memberdata/
    public class LeilaoRecebeOferta
    {
        [Theory]
        [InlineData(4, new double[] { 100, 1200, 1400, 1300 })]
        [InlineData(2, new double[] { 800, 900 })]
        public void NaoPermiteNovosLancesDadoLeilaoFinalizado(int qtdeEsperada, double[] ofertas)
        {
            IModalidadeAvaliacao modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();
            for(int i = 0; i < ofertas.Length; i++)
            {
                var valor = ofertas[i];
                if((i%2) == 0)
                {
                    leilao.RecebeLance(fulano, valor);
                    continue;
                }

                leilao.RecebeLance(maria, valor);

            }

            leilao.TerminaPregao();

            leilao.RecebeLance(fulano, 1000);

            var qtdeObtida = leilao.Lances.Count();
            Assert.Equal(qtdeEsperada, qtdeObtida);
        }

        [Fact]
        public void QtdPermaneceZeroDadoQuePregaoNaoFoiIniciado()
        {
            IModalidadeAvaliacao modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);

            leilao.RecebeLance(fulano, 900);

            Assert.Equal(0, leilao.Lances.Count());
        }

        [Fact]
        public void NaoAceitaLanceConsecutivoDeUmMesmoCliente()
        {
            IModalidadeAvaliacao modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);

            leilao.IniciaPregao();
           
           
            leilao.RecebeLance(fulano, 800);

            leilao.RecebeLance(fulano, 1000);

            leilao.TerminaPregao();

            leilao.RecebeLance(fulano, 1000);

            var qtdeObtida = leilao.Lances.Count();
            Assert.Equal(1, qtdeObtida);

        }

    }
}
