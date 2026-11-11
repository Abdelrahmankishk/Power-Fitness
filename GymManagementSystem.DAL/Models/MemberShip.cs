using GymManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Models;

public class MemberShip : BaseEntity
{
    public int MemberId { get; set; }
    public Member Member { get; set; } 

    public int PlanId { get; set; }
    public Plan Plan { get; set; }

    public DateTime EndDate { get; set; }

    public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
    public bool IsActive => EndDate > DateTime.Now;
}
