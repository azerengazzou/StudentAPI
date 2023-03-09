using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudentAPI.Data;
using StudentAPI.Model;
using System.ComponentModel.DataAnnotations;

namespace StudentAPI.Controllers
{
    [Route("api/StudentAPI")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;
        public StudentAPIController(ApplicationDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudents()
        {
            IEnumerable<Student> StudentsList=await _db.Students.ToListAsync();
            return Ok(_mapper.Map<List<StudentDTO>>(StudentsList));
        }

        [HttpGet("{id:int}", Name = "GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StudentDTO>> GetStudent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var student= await _db.Students.FirstOrDefaultAsync(s=>s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<StudentDTO>(student));
        }

        [HttpGet("{Student_Name}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<StudentDTO>> GetStudentByName(string name)
        {
            if (name == null)
            {
                return BadRequest();
            }
            IEnumerable<Student> StudentsList = _db.Students.Where(s=>s.Name.Contains(name.ToLower()));
            if (StudentsList.Count()== 0)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<List<StudentDTO>>(StudentsList));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentCreateDTO studentCreateDTO)
        {
            if (await _db.Students.FirstOrDefaultAsync(u => u.Matricule == studentCreateDTO.Matricule) != null)
            {
                ModelState.AddModelError("CustomError", "Student already exist");
                return BadRequest(ModelState);
            }
            if (studentCreateDTO == null)
            {
                return BadRequest(studentCreateDTO);
            }

            Student modelStudent = _mapper.Map<Student>(studentCreateDTO);
            await _db.Students.AddAsync(modelStudent);
            await _db.SaveChangesAsync();
            return Ok("Success");
        }

        [HttpDelete("{id:int}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var student = await _db.Students.FirstOrDefaultAsync(x => x.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            _db.Students.Remove(student);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateStudent([FromBody] StudentUpdateDTO studentUpdateDTO, int id)
        {
            if (studentUpdateDTO == null || id != studentUpdateDTO.Id)
            {
                return BadRequest();
            }

            Student studentModel = _mapper.Map<Student>(studentUpdateDTO);
            _db.Update(studentModel);
            await _db.SaveChangesAsync();
            return NoContent();

        }

        [HttpPatch("{id:int}", Name = "UpdateStudentPartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStudentPartial(int id, JsonPatchDocument<StudentUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var student = await _db.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            StudentUpdateDTO studentToPatch = _mapper.Map<StudentUpdateDTO>(student);
            if (student == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(studentToPatch,ModelState);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Student studentToDB = _mapper.Map<Student>(studentToPatch);
            _db.Students.Update(studentToDB);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}