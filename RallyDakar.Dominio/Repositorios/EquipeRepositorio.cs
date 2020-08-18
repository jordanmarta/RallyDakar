using Microsoft.EntityFrameworkCore;
using RallyDakar.Dominio.DbContexto;
using RallyDakar.Dominio.Entidades;
using RallyDakar.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RallyDakar.Dominio.Repositorios
{
    public class EquipeRepositorio : IEquipeRepositorio
    {
        private readonly RallyDbContexto _rallyDbContexto;

        public EquipeRepositorio(RallyDbContexto rallyDbContexto)
        {
            _rallyDbContexto = rallyDbContexto;
        }

        public void Adicionar(Equipe equipe)
        {
            _rallyDbContexto.Equipes.Add(equipe);
            _rallyDbContexto.SaveChanges();
        }

        public void Atualizar(Equipe equipe)
        {
            // PUT
            if(_rallyDbContexto.Entry(equipe).State == EntityState.Detached)
            {
                _rallyDbContexto.Attach(equipe);
                _rallyDbContexto.Entry(equipe).State = EntityState.Modified;
            }
            else
            {
                _rallyDbContexto.Update(equipe);
            }

            _rallyDbContexto.SaveChanges();
        }

        public void Deletar(Equipe equipe)
        {
            _rallyDbContexto.Equipes.Remove(equipe);
            _rallyDbContexto.SaveChanges();
        }

        public bool Existe(int equipeId)
        {
            return _rallyDbContexto.Equipes.Any(p => p.Id == equipeId);
        }

        public Equipe Obter(int equipeId)
        {
            return _rallyDbContexto.Equipes.FirstOrDefault(p => p.Id == equipeId);
        }

        public IEnumerable<Equipe> ObterTodos()
        {
            return _rallyDbContexto.Equipes.ToList();
        }

        public IEnumerable<Equipe> ObterTodos(string nome)
        {
            return _rallyDbContexto.Equipes
                .Where(p => p.Nome.Contains(nome))
                .ToList();
        }
    }
}
