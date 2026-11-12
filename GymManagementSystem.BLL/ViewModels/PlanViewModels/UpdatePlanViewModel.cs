using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.ViewModels.PlanViewModels;

public class UpdatePlanViewModel
{
    public string Name { get; set; }
    [Required(ErrorMessage = "Plan's description Is Required")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Description must be between 2 and 200 characters")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Plan Duration Is Required")]
    [Range(1, 365, ErrorMessage = "Duartion must be greater than 1 and less than 365")]
    public int DurationDays { get; set; }
    [Required(ErrorMessage = "Price Is Required")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a Positive Value")]
    public decimal Price { get; set; }

}
