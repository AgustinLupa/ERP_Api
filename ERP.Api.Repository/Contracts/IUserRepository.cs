using ERP.Api.Entity;

namespace ERP.Api.Repository.Contracts;

public interface IUserRepository
{
    public Task<IEnumerable<User>> GetAll();
    public Task<IEnumerable<User>> GetActive();
    public Task<User> Login(User user);
    public Task<int> Create(User user);
    public Task<int> Update(User user);
    public Task<int> Delete(int id);
}
