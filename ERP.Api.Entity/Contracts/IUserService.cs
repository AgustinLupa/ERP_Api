namespace ERP.Api.Entity.Contracts;

public interface IUserService
{
    public Task<IEnumerable<User>> GetAll();
    public Task<IEnumerable<User>> GetActive();
    public Task<User> Login(User user);
}
