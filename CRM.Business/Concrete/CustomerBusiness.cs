using CRM.Business.Abstract;
using CRM.Business.Infrastructer.EntitiyFrameWorkCore;
using CRM.Core.Business.Abstract.Linq;
using CRM.DataAccess.Conctrete.EntityFrameworkCore.Context;
using CRM.Entity.Concrete;

namespace CRM.Business.Concrete {
    public class CustomerBusiness : BusinessBase<CRMUnitOfWork, Customer, CrmDbContext>, ICustomerBusiness {
        public CustomerBusiness(CrmDbContext context) : base(context) { }

        public async override Task<Customer?> AddAsync(Customer entity, bool? sync = false) {
            var addedCustomer = await base.AddAsync(entity);
            if(addedCustomer != null) {
                var affectedRow = await SaveAsync();
                if(affectedRow > 0) {
                    return addedCustomer;
                }
            }
            return default;
        }

        public async override Task<Customer?> UpdateAsync(Customer entityToUpdate) {
            var updatedCustomer = await base.UpdateAsync(entityToUpdate);
            var affectedRow = await SaveAsync();
            if(affectedRow > 0) {
                return updatedCustomer;
            }
            return default;
        }

        public async override Task<Customer?> UpdateByIdAsync(object id) {
            if(id != null && int.TryParse(id.ToString(), out var intId)) {
                var updatedCustomer = await base.UpdateByIdAsync(intId);
                if(updatedCustomer != null) {
                    var affectedRow = await SaveAsync();
                    if(affectedRow > 0) {
                        return updatedCustomer;
                    }
                }
            }
            else {
                throw new InvalidDataException($"{id} format is wrong!");
            }
            return default;
        }

        public async override Task<Customer?> DeleteByIdAsync(object id) {
            if(id != null && int.TryParse(id.ToString(), out var intId)) {
                var deletedCustomer = await base.DeleteByIdAsync(intId);
                if(deletedCustomer != null) {
                    var affectedRow = await SaveAsync();
                    if(affectedRow > 0) {
                        return deletedCustomer;
                    }
                }
            }
            else {
                throw new InvalidDataException($"{id} format is wrong!");
            }
            return default;
        }
    }
}
