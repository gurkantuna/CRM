using CRM.Core.Managers.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CRM.Core.Managers.Concrete {
    public abstract class DbControlManager<TContext> : IDbControlManager<TContext> where TContext : DbContext, new() {

        protected DbControlManager(TContext context) {
            Context = context;
        }

        public TContext Context { get; }

        public void CheckDbState(bool dbCreate = false, bool dbDeleteByForce = false) {
            this.CheckDbStateAsync(dbCreate, dbDeleteByForce, true).GetAwaiter().GetResult();
        }

        public async Task CheckDbStateAsync(bool dbCreate = false, bool dbDeleteByForce = false, bool sync = false) {
            try {
                if(dbDeleteByForce && !dbCreate) {
                    throw new InvalidOperationException("Veri tabanının zorla silinmesi için önlem olarak dbCreate paramteresi true gönderilmelidir!");
                }
                else if(dbDeleteByForce && dbCreate) {
                    await Context.Database.EnsureDeletedAsync();
                }

                if(dbCreate) {
                    var resultCreate = await Context.Database.EnsureCreatedAsync();
                    if(resultCreate) {
                        Console.WriteLine("Veri tabanı başarıyla oluşturuldu;");
                    }
                }

                await WriteMockDataAfterCreate();
            }
            catch {
                throw;//Environment.Development aşamasında olduğu için hatayı özellikle fırlatıyoruz
            }
        }

        public abstract Task WriteMockDataAfterCreate();
    }
}
