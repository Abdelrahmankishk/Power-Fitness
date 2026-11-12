using GymManagementSystem.DAL.Models.Enums;

namespace GymManagementSystem.BLL.ViewModels.TrainerViewModels;

public class TrainerViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string DateofBirth { get; set; }
    public string Gender { get; set; }
    public string specialtie {  get; set; }
    public string? Address {  get; set; }
}
