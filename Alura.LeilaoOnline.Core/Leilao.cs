using System;
using System.Collections.Generic;
using System.Linq;

namespace Alura.LeilaoOnline.Core
{
    public enum EstadoLeilao
    {
        LeilaoAntesDoPregao,
        LeilaoEmAndamento,
        LeilaoFinalizado
    }

    public class Leilao
    {
        private Interessada _ultimoCliente;
        private IList<Lance> _lances;
        private IModalidadeAvaliacao _avaliador;

        public IEnumerable<Lance> Lances => _lances;
        public string Peca { get; }
        public Lance Ganhador { get; private set; }
        public EstadoLeilao Estado { get; private set; }
        

        public Leilao(string peca, IModalidadeAvaliacao avaliador)
        {
            Peca = peca;
            _lances = new List<Lance>();
            Estado = EstadoLeilao.LeilaoAntesDoPregao;
            _avaliador = avaliador;
        }

        public void RecebeLance(Interessada cliente, double valor)
        {

            if(NovoLanceEhAceito(cliente))
            {
                _lances.Add(new Lance(cliente, valor));
                _ultimoCliente = cliente;
            }
           
        }

        public void IniciaPregao()
        {
            Estado = EstadoLeilao.LeilaoEmAndamento;

        }

        public void TerminaPregao()
        {
            if(Estado != EstadoLeilao.LeilaoEmAndamento)
            {
                var msg = "Não é possivel terminar um pregão sem ter inciado. Utilize o método IniciaPregao() para incia-lo.";
                throw new System.InvalidOperationException(msg);
            }

            Ganhador = _avaliador.Avalia(this);
            Estado = EstadoLeilao.LeilaoFinalizado;
        }

        private bool NovoLanceEhAceito(Interessada cliente)
        {
            return (Estado == EstadoLeilao.LeilaoEmAndamento &&
                cliente != _ultimoCliente);

        }
    }
}
