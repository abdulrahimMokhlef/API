namespace API_Project.DTOs
{
  public class StudentDTO
  {
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? Address { get; set; }
    public string? Image { get; set; }
    public int DeptId { get; set; }
    public string? DeptName { get; set; }
  }

  public class StudentCreateDTO
  {
    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? Address { get; set; }
    public string? Image { get; set; }
    public int DeptId { get; set; }
  }
}
