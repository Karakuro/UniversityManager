using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityManager.Data;
using UniversityManager.Entities;
using UniversityManager.Models;

namespace UniversityManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamResultController : ControllerBase
    {
        private ILogger<ExamResultController> _logger;
        private ExamResultDataService _srv;
        private ExamDataService _srvExam;
        private StudentDataService _srvStud;

        public ExamResultController(ILogger<ExamResultController> logger, ExamResultDataService srv, ExamDataService srvExam, StudentDataService srvStud)
        {
            this._logger = logger;
            _srv = srv;
            _srvExam = srvExam;
            _srvStud = srvStud;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_srv.GetAll());
        }

        [HttpGet]
        [Route("/exam/{id}")]
        public IActionResult GetByExam(Guid id)
        {
            return Ok(_srv.GetAll().FindAll(er => er.ExamId == id));
        }

        [HttpGet]
        [Route("/student/{id}")]
        public IActionResult GetByStudent(Guid id)
        {
            return Ok(_srv.GetAll().FindAll(er => er.StudentId == id));
        }

        [HttpGet]
        [Route("/Libretto/{id}")]
        public IActionResult Libretto(Guid id)
        {
            return Ok(GetLibretto(id));
        }

        [HttpGet]
        [Route("/both/{studentId}/{examId}")]
        public IActionResult Get(Guid studentId, Guid examId)
        {
            return Ok(_srv.Get(examId, studentId));
        }

        [HttpPost]
        public IActionResult Post(ExamResult ExamResult)
        {
            try
            {
                if (_srv.Write(ExamResult))
                {
                    return Ok();
                }
                return BadRequest("Oh cavolo, qualcosa si è rotto!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
        [HttpPut]
        public IActionResult Put(ExamResult ExamResult)
        {
            if (_srv.Update(ExamResult))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("/{studentId}/{examId}")]
        public IActionResult Delete(Guid studentId, Guid examId)
        {
            if (_srv.Delete(examId, studentId))
            {
                return Ok();
            }
            return BadRequest();
        }

        private List<LibrettoModel> GetLibretto(Guid id)
        {
            List<ExamResult> results = _srv.GetAll().FindAll(er => er.StudentId == id);
            List<Exam> exams = _srvExam.GetAll();
            List<LibrettoModel> libretto = (from r in results
                            join e in exams
                            on r.ExamId equals e.Id
                            select new LibrettoModel { ExamTitle = e.Title, Score = r.Score }).ToList();
            return libretto;
        }
    }
}
