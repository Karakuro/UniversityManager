using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityManager.Data;
using UniversityManager.Entities;

namespace UniversityManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        ILogger<StudentController> _logger;
        StudentDataService _srv;
        private ExamResultDataService _srvResult;
        private ExamDataService _srvExam;

        public StudentController(ILogger<StudentController> logger, StudentDataService srv, ExamResultDataService srvResult, ExamDataService srvExam)
        {
            this._logger = logger;
            _srv = srv;
            _srvExam = srvExam;
            _srvResult = srvResult;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_srv.GetAll());
        }

        [HttpGet]
        [Route("Median")]
        public IActionResult GetAllMedian()
        {
            List<Student> students = _srv.GetAll();
            List<ExamResult> results = _srvResult.GetAll();
            List<Exam> exams = _srvExam.GetAll();

            var medians = (from s in students
                           join r in results on s.Id equals r.StudentId
                           join e in exams on r.ExamId equals e.Id
                           group new { r.Score, e.Credits } by s into g
                           select new 
                           { 
                               g.Key.Id, 
                               g.Key.Name, 
                               g.Key.Surname, 
                               Median = g.Sum(x => x.Score * (int)x.Credits) / g.Sum(x => (int)x.Credits) 
                           }).ToList();

            return Ok(medians);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(_srv.Get(id));
        }

        [HttpPost]
        public IActionResult Post(Student student)
        {
            try
            {
                if (student.BirthDate > DateTime.Now.AddYears(-18)) { return BadRequest(); }
                if (_srv.Write(student))
                {
                    return Ok();
                }
                return BadRequest("Oh cavolo, qualcosa si è rotto!");
            } 
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
        [HttpPut] 
        public IActionResult Put(Student student)
        {
            if (student.BirthDate > DateTime.Now.AddYears(-18)) { return BadRequest(); }
            if (_srv.Update(student))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(Guid id)
        {
            if(_srv.Delete(id))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
