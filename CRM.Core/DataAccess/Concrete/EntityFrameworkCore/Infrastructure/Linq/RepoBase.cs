using System.Data;
using System.Linq.Expressions;
using CRM.Core.DataAccess.Abstract.Infrastructure.Linq;
using CRM.Core.Entity.Abstract;
using CRM.Core.ViewModels.Concrete;
using Microsoft.EntityFrameworkCore;

namespace CRM.Core.DataAccess.Concrete.EntityFrameworkCore.Infrastructure.Linq {
    public abstract class RepoBase<TEntity, TContext> : IRepoBase<TEntity>
                                                        where TEntity : EntityBase
                                                        where TContext : DbContext, new() {
        protected RepoBase(TContext context) {
            _dbContext = context;
            _dbSet = context.Set<TEntity>();
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public virtual string[]? IncludePropertyNames { get; set; }

        public virtual string[]? IncludeMinPropertyNames { get; set; }

        internal readonly TContext _dbContext;
        internal readonly DbSet<TEntity> _dbSet;

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null,
                                                string[]? includeProperties = null,
                                                OrderByViewModel<TEntity>? orderBy = null,

                                                int? take = null,
                                                int? skip = null) {
            return GetAsync(filter, includeProperties, orderBy, take, skip, true).GetAwaiter().GetResult();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                                 string[]? includeProperties = null,
                                                                 OrderByViewModel<TEntity>? orderBy = null,
                                                                 int? take = null,
                                                                 int? skip = null,
                                                                 bool? sync = false) {

            var query = _dbSet.AsQueryable();

            for(var i = 0; i < includeProperties?.Length; i++) {
                query = query.Include(includeProperties[i]);
            }

            if(filter != null) {
                query = query.Where(filter);
            }

            if(orderBy?.Direction == OrderDirection.Desc) {
                query = query.OrderByDescending(orderBy?.KeySelector ?? (x => x.Id));
            }
            else if(orderBy?.KeySelector != null) {
                query = query.OrderBy(orderBy.KeySelector);
            }

            if(skip.HasValue) {
                query = query.Skip(skip.Value);
            }

            if(take.HasValue) {
                query = query.Take(take.Value);
            }

            if(sync!.Value) {
                return [.. query];
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(object id) {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                  string[]? includeProperties = null) {
            var query = Where(filter, includeProperties, null);
            return await query.CountAsync();
        }

        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                               string[]? includeProperties = null) {
            var query = Where(filter, includeProperties, null);

            return await query.FirstOrDefaultAsync();
        }


        public virtual async Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                              string[]? includeProperties = null) {
            var query = Where(filter, includeProperties, null);

            return await query.LastOrDefaultAsync();
        }

        public TEntity? Add(TEntity entity) {
            return AddAsync(entity, true).GetAwaiter().GetResult();
        }

        //HACK:Async metodu kod tekrarı olmadan kullanmakiçin sync örnek. Diğer tüm async metodlar bu şekilde sync kullanılabilir.
        public virtual async Task<TEntity?> AddAsync(TEntity entity, bool? sync = false) {
            var result = sync!.Value ? _dbSet.Add(entity) : await _dbSet.AddAsync(entity);
            return result.State == EntityState.Added ? result.Entity : null;
        }

        public async virtual Task<TEntity?> UpdateAsync(TEntity entityToUpdate) {
            _dbSet.Attach(entityToUpdate);
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
            return await Task.FromResult(entityToUpdate);
        }

        public async virtual Task<TEntity?> UpdateByIdAsync(object id) {
            TEntity? entityToUpdate = await _dbSet.FindAsync(id);
            if(entityToUpdate != null) {
                _dbSet.Attach(entityToUpdate);
                _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
                return entityToUpdate;
            }
            return default;
        }

        public virtual async Task<TEntity?> DeleteByIdAsync(object id) {
            TEntity? entityToDelete = await _dbSet.FindAsync(id);
            if(entityToDelete != null) {
                return Delete(entityToDelete);
            }
            return default;
        }

        public virtual TEntity Delete(TEntity entityToDelete) {
            if(_dbContext.Entry(entityToDelete).State == EntityState.Detached) {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
            return entityToDelete;
        }

        public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>>? filter = null,
                                         string[]? includeProperties = null,
                                         OrderByViewModel<TEntity>? orderBy = null) {

            var query = _dbSet.AsQueryable();

            for(var i = 0; i < includeProperties?.Length; i++) {
                query = query.Include(includeProperties[i]);
            }



            if(filter != null) {
                query = query.Where(filter);
            }

            if(orderBy?.Direction == OrderDirection.Desc) {
                query = query.OrderByDescending(orderBy?.KeySelector ?? (x => x.Id));
            }
            else if(orderBy?.KeySelector != null) {
                query = query.OrderBy(orderBy.KeySelector);
            }

            return query;
        }

        public bool HasChanges() {
            return _dbContext.ChangeTracker.HasChanges();
        }

        public int Save() {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveAsync() {
            return await _dbContext.SaveChangesAsync();
        }
    }
}