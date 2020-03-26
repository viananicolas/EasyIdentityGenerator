namespace EasyIdentityGenerator.Data.Services.Interfaces
{
    public interface IPasswordHasherMvc
    {
        string HashPassword(string password);
    }
}