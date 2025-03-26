using System.Linq.Expressions;
using CRM.Core.Entity.Abstract;
using CRM.Core.ViewModels.Abstract;
using CRM.Core.ViewModels.Concrete;

namespace CRM.Core.DataAccess.Abstract.Infrastructure.Linq {
    public interface IRepoBase<TEntity> where TEntity : EntityBase {

        Guid Id { get; }

        /// <summary>
        /// Join sorgularında gönderilecek property isimleri.
        /// </summary>
        string[]? IncludePropertyNames { get; set; }

        /// <summary>
        /// Join sorgularında gönderilecek minimum property isimleri.
        /// </summary>
        string[]? IncludeMinPropertyNames { get; set; }

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null,
                                 string[]? includeProperties = null,
                                 OrderByViewModel<TEntity>? orderBy = null,
                                 int? take = null,
                                 int? skip = null);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
                                            string[]? includeProperties = null,
                                            OrderByViewModel<TEntity>? orderBy = null,
                                            int? take = null,
                                            int? skip = null,
                                            bool? sync = false);

        /// <summary>
        /// Dikkat! Tehlikeli, yanlış kullanım performans katili olabilir!!!
        /// </summary>
        /// <param name="filter">Expression<Func<TEntity, bool>></param>
        /// <param name="includeProperties">Join FK</param>
        /// <param name="orderBy">OrderByViewModel<TEntity></param>
        /// <param name="includePassives"></param>
        /// <returns></returns>
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>>? filter = null,
                                  string[]? includeProperties = null,
                                  OrderByViewModel<TEntity>? orderBy = null);
        Task<TEntity?> GetByIdAsync(object id);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null, string[]? includeProperties = null);

        /// <summary>
        /// Gönderilen Expression ile context içinde Async arama yapılır. Bulunan ilk entity, bulunamazsa default olarak null döndürür. 
        /// </summary>
        /// <param name="filter">Expression</param>
        /// <param name="includePassives">True gönderilirse tüm aktif olmayan entity içinde de aranır. Default false'dur.</param>
        /// <returns>T</returns>
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, string[]? includeProperties = null);

        /// <summary>
        /// Gönderilen Expression ile context içinde Async arama yapılır. Bulunan son entity, bulunamazsa default olarak null döndürür. 
        /// </summary>
        /// <param name="filter">Expression</param>
        /// <param name="includePassives">True gönderilirse tüm aktif olmayan entity içinde de aranır. Default false'dur.</param>
        /// <returns>T</returns>
        Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, string[]? includeProperties = null);     

        /// <summary>
        /// Gönderilen entity contexte eklenerek değiştirilen state ile geri döndürülür.
        /// </summary>
        /// <param name="entity">EntityBase</param>
        /// <returns>T</returns>
        TEntity? Add(TEntity entity);

        /// <summary>
        /// Gönderilen entity contexte eklenerek değiştirilen state ile geri döndürülür.
        /// </summary>
        /// <param name="entity">EntityBase</param>
        /// <returns>T</returns>
        Task<TEntity?> AddAsync(TEntity entity, bool? sync = false);

        Task<TEntity?> DeleteByIdAsync(object id);

        TEntity Delete(TEntity entityToDelete);
        Task<TEntity?> UpdateAsync(TEntity entityToUpdate);

        Task<TEntity?> UpdateByIdAsync(object id);
        
        bool HasChanges();

        int Save();

        Task<int> SaveAsync();
    }
}
