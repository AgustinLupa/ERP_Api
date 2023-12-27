using ERP.Api.Entity;

namespace ERP.Api.Repository.Contracts;

public interface ISupplierRepository
{
    public Task<int> CreateSupplier(Supplier supplier);
    public Task<bool> UpdateSupplier(Supplier supplier);
    public Task<bool> DeleteSupplier(int id);
    public Task<IEnumerable<Supplier>> GetAll();
    public Task<Supplier> GetById(int id);
    public Task<IEnumerable<Supplier>> GetByName(string name);
    public Task<IEnumerable<Supplier>> GetActiveSupplier();
}
