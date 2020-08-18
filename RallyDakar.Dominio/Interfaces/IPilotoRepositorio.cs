using RallyDakar.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RallyDakar.Dominio.Interfaces
{
    public interface IPilotoRepositorio
    {
        void Adicionar(Piloto piloto);
        void Atualizar(Piloto piloto);
        IEnumerable<Piloto> ObterTodos();
        Piloto Obter(int pilotoId);
        bool Existe(int pilotoId);
        void Deletar(Piloto piloto);
    }
}
