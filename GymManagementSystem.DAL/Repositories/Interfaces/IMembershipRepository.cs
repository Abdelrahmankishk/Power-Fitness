using GymManagementSystem.DAL.Models;
using System.Linq.Expressions;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IMembershipRepository : IGenericRepository<MemberShip>
{
    Task<List<MemberShip>> GetAllMembershipsWithMemberAndPlanAsync(Expression<Func<MemberShip, bool>>? condition = null, CancellationToken ct = default);
}
