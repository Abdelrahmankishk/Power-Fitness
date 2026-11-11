using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Models
{
    public class Member : GymUser    
    {
        public string? Photo { get; set; }

        #region Relationships
        public HealthRecord HealthRecord { get; set; } = default!;

        public ICollection<MemberShip> Memberships { get; set; } = default!;
        #endregion
    }
}
