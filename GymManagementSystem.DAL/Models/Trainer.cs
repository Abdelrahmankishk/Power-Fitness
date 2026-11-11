using GymManagementSystem.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Models
{
    public class Trainer: GymUser
    {
        public TrainerSpecialties Specialtie { get; set; }

        #region Relationships
        public ICollection<Session> Sessions { get; set; } = default!;
        #endregion
    }
}
