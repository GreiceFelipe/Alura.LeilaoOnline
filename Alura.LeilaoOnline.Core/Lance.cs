using System;
namespace Alura.LeilaoOnline.Core
{
    public class Lance
    {
        public Interessada Cliente { get; }
        public double Valor { get; }

        public Lance(Interessada cliente, double valor)
        {
            if(valor < 0)
            {
                var msg = "Valor do lance deve ser maior ou igual a zero.";
                throw new System.ArgumentException(msg);
            }
            Cliente = cliente;
            Valor = valor;
        }
    }
}
