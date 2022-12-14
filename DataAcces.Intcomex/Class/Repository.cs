﻿using DataAcces.Intcomex.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAcces.Intcomex.Class
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Instancia del contexto
        /// </summary>
        private readonly IntcomexContext _context;

        /// <summary>
        /// Tipo de entidad sobre la que opera este conjunto.
        /// </summary>
        private readonly DbSet<TEntity> _dbset;

        /// <summary>
        /// Constructor de la case.
        /// </summary>
        /// <param name="context"></param>
        public Repository(IntcomexContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }

        /// <summary>
        /// Obtiene todos los datos de la entidad desde la base de datos.
        /// </summary>
        /// <returns>Lista de objetos</returns>
        public IEnumerable<TEntity> GetAll() =>
            _dbset.ToList();

        /// <summary>
        /// Obtiene un objeto de la entidad por id
        /// </summary>
        /// <param name="id">id a filtrar</param>
        /// <returns>Objeto buscado</returns>
        public TEntity GetById(int id) =>
            _dbset.Find(id);

        /// <summary>
        /// Crea o inserta registros en la tabla o entidad.
        /// </summary>
        /// <param name="entity"></param>
        public virtual bool Add(TEntity entity)
        {
            bool result = false;
            using (IDbContextTransaction dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _dbset.Add(entity);
                    dbTransaction.Commit();
                    result = true;
                }
                catch (Exception)
                {
                    dbTransaction.Rollback();
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Elimina objetos por id
        /// </summary>
        /// <param name="id">id a filtrar</param>
        public virtual bool Delete(int id)
        {
            bool result = false;
            using (IDbContextTransaction dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _dbset.Remove(_dbset.Find(id));
                    dbTransaction.Commit();
                    result = true;
                }
                catch (Exception)
                {
                    dbTransaction.Rollback();
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Actualiza el objeto o entidad en base de datos.
        /// </summary>
        /// <param name="entity">Entidad a modificar</param>
        public virtual bool Update(TEntity entity)
        {
            bool result = false;
            using (IDbContextTransaction dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _dbset.Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                    dbTransaction.Commit();
                    result = true;
                }
                catch (Exception)
                {
                    dbTransaction.Rollback();
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Guarda los cambios que afecten la base de datos.
        /// </summary>
        public void Save() =>
            _context.SaveChanges();
    }
}