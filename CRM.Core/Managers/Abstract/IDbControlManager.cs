using Microsoft.EntityFrameworkCore;

namespace CRM.Core.Managers.Abstract {
    public interface IDbControlManager<out TContext> where TContext : DbContext, new() {
        TContext Context { get; }

        /// <summary>
        /// Veri tabanı durumunu kontrol ederek gerektiğinde oluşturur.
        /// </summary>
        /// <param name="dbCreate">Veri tabanı zaten mevcutsa false döner</param>
        /// <param name="dbDeleteByForce">DİKKAT!!! Mevcut tüm veri silinecektir! dbCretate ile birlikte true gönderildiğinde veri tabanı zorla tekrar oluşturulur.</param>
        /// <returns>Task</returns>
        Task CheckDbStateAsync(bool dbCreate = false, bool dbDeleteByForce = false, bool sync = false);
        void CheckDbState(bool dbCreate = false, bool dbDeleteByForce = false);
    }
}
