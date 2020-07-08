using Cerber.Repository.Models;

namespace Cerber.Repository
{
    public interface IRepository
    {
        void UpdateToken(Token token);

        Token GetToken();
    }
}
