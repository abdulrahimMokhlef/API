using Microsoft.AspNetCore.Mvc;
using API_Project.Context;
using API_Project.DTOs;
using API_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Project.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class StudentController : ControllerBase
  {
     private readonly InstitutionContext _context;

    public StudentController(InstitutionContext context)
    {
      _context = context;
    }

     [HttpGet("getall")]
    public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAll()
    {
      var students = await _context.Students
          .Include(s => s.Department)
          .Select(s => new StudentDTO
          {
            Id = s.Id,
            Name = s.Name,
            Age = s.Age,
            Address = s.Address,
            Image = s.Image,
            DeptId = s.DeptId,
            DeptName = s.Department.DeptName
          })
          .ToListAsync();

      return Ok(students);
    }

     [HttpGet("{id}")]
    public async Task<ActionResult<StudentDTO>> GetById(int id)
    {
      var student = await _context.Students
          .Include(s => s.Department)
          .FirstOrDefaultAsync(s => s.Id == id);

      if (student == null)
        return NotFound();

      return Ok(new StudentDTO
      {
        Id = student.Id,
        Name = student.Name,
        Age = student.Age,
        Address = student.Address,
        Image = student.Image,
        DeptId = student.DeptId,
        DeptName = student.Department.DeptName
      });
    }

     [HttpPost("create")]
    public async Task<ActionResult<StudentDTO>> Create(StudentCreateDTO createDTO)
    {
      var student = new Student
      {
        Name = createDTO.Name,
        Age = createDTO.Age,
        Address = createDTO.Address,
        Image = createDTO.Image,
        DeptId = createDTO.DeptId
      };

      _context.Students.Add(student);
      await _context.SaveChangesAsync();

      var department = await _context.Departments.FindAsync(student.DeptId);

      return CreatedAtAction(nameof(GetById), new { id = student.Id },
          new StudentDTO
          {
            Id = student.Id,
            Name = student.Name,
            Age = student.Age,
            Address = student.Address,
            Image = student.Image,
            DeptId = student.DeptId,
            DeptName = department?.DeptName
          });
    }

     [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, StudentCreateDTO updateDTO)
    {
      var student = await _context.Students.FindAsync(id);
      if (student == null)
        return NotFound();

      student.Name = updateDTO.Name;
      student.Age = updateDTO.Age;
      student.Address = updateDTO.Address;
      student.Image = updateDTO.Image;
      student.DeptId = updateDTO.DeptId;

      _context.Students.Update(student);
      await _context.SaveChangesAsync();

      return NoContent();
    }

     [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var student = await _context.Students.FindAsync(id);
      if (student == null)
        return NotFound();

      _context.Students.Remove(student);
      await _context.SaveChangesAsync();

      return NoContent();
    }
  }
}
