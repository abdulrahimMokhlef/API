using Microsoft.AspNetCore.Mvc;
using API_Project.Context;
using API_Project.DTOs;
using API_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Project.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DepartmentController : ControllerBase
  {
    private readonly InstitutionContext _context;

    public DepartmentController(InstitutionContext context)
    {
      _context = context;
    }

    [HttpGet("getall")]
    public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetAll()
    {
      var departments = await _context.Departments
          .Select(d => new DepartmentDTO
          {
            DeptId = d.DeptId,
            DeptName = d.DeptName,
            MgrName = d.MgrName,
            Location = d.Location
          })
          .ToListAsync();

      return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDTO>> GetById(int id)
    {
      var department = await _context.Departments.FindAsync(id);
      if (department == null)
        return NotFound();

      return Ok(new DepartmentDTO
      {
        DeptId = department.DeptId,
        DeptName = department.DeptName,
        MgrName = department.MgrName,
        Location = department.Location
      });
    }

    [HttpPost("create")]
    public async Task<ActionResult<DepartmentDTO>> Create(DepartmentCreateDTO createDTO)
    {
      var department = new Department
      {
        DeptName = createDTO.DeptName,
        MgrName = createDTO.MgrName,
        Location = createDTO.Location
      };

      _context.Departments.Add(department);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetById), new { id = department.DeptId },
          new DepartmentDTO
          {
            DeptId = department.DeptId,
            DeptName = department.DeptName,
            MgrName = department.MgrName,
            Location = department.Location
          });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var department = await _context.Departments.FindAsync(id);
      if (department == null)
        return NotFound();

      _context.Departments.Remove(department);
      await _context.SaveChangesAsync();

      return NoContent();
    }
  }
}
