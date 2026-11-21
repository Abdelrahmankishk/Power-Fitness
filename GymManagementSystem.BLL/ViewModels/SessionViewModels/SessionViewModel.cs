using GymManagementSystem.BLL.Common;

namespace GymManagementSystem.BLL.ViewModels.SessionViewModels;

public class SessionViewModel
{
    public int Id { get; set; }
    public string Description { get; set; } = default!;
    public int Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string TrainerName { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
    public int AvailableSlots { get; set; }

    // Computed properties
    public DateTime StartDateEgypt => EgyptTimeZone.ToEgyptLocal(StartDate);
    public DateTime EndDateEgypt => EgyptTimeZone.ToEgyptLocal(EndDate);
    public string DateDisplay => $"{StartDateEgypt:MMM dd, yyyy}";
    public string TimeRangeDisplay => $"{StartDateEgypt:hh:mm tt} - {EndDateEgypt:hh:mm tt}";
    public TimeSpan Duration => EndDate - StartDate;

    public string Status
    {
        get
        {
            var nowUtc = DateTime.UtcNow;

            if (StartDate > nowUtc)
                return "Upcoming";
            else if (EndDate >= nowUtc)
                return "Ongoing";
            else
                return "Completed";
        }
    }
}
