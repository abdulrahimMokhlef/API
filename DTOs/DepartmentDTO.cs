namespace API_Project.DTOs
{
  public class DepartmentDTO
  {
    public int DeptId { get; set; }
    public string? DeptName { get; set; }
    public string? MgrName { get; set; }
    public string? Location { get; set; }
  }

  public class DepartmentCreateDTO
  {
    public string? DeptName { get; set; }
    public string? MgrName { get; set; }
    public string? Location { get; set; }
  }
}
