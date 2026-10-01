using System.Threading.Tasks;

namespace WorkLifeBalance.Features.Updates
{
    public interface IUpdateCheckerService
    {
        public Task CheckForUpdate();
    }
}
