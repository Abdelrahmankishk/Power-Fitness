namespace GymManagementSystem.BLL.ViewModels.HomeViewModels;

public class HomeViewModel
{
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int TotalTrainers { get; set; }
    public int UpcomingSessions { get; set; }
    public int OngoingSessions { get; set; }
    public int CompletedSessions { get; set; }
}
