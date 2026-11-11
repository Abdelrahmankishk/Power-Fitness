using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Models
{
    public class Session : BaseEntity
    {
        public string? Description { get; set; }
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region Relationships
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = default!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        public ICollection<Booking> SessionMembers { get; set; } = default!;
        #endregion
    }
}
