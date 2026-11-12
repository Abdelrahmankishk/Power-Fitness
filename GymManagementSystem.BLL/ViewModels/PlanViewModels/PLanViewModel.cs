namespace GymManagementSystem.BLL.ViewModels.PlanViewModels;

public class PLanViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int DurationDays { get; set; }
    public decimal Price { get; set; }

    public bool IsActive { get; set; }
}
