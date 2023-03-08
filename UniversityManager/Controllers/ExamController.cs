using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityManager.Data;
using UniversityManager.Entities;

namespace UniversityManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        ILogger<ExamController> _logger;
        ExamDataService _srv;
        private ExamResultDataService _srvResult;
        private StudentDataService _srvStudent;

        public ExamController(ILogger<ExamController> logger, ExamDataService srv, ExamResultDataService srvResult, StudentDataService srvStudent)
        {
            this._logger = logger;
            _srv = srv;
            _srvResult = srvResult;
            _srvStudent = srvStudent;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_srv.GetAll());
        }

        [HttpGet]
        [Route("Statistics")]
        public IActionResult GetStatistics()
        {
            List<Exam> exams = _srv.GetAll();
            List<ExamResult> results = _srvResult.GetAll();
            var stats = (from e in exams
                         join r in results
                         on e.Id equals r.ExamId
                         group r by e into g
                         select new
                         {
                             g.Key.Id,
                             g.Key.Title,
                             g.Key.Credits,
                             NumPassed = g.Count(x => x.Score >= 18),
                             AverageScore = g.Average(x => x.Score)
                         });
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(_srv.Get(id));
        }

        [HttpPost]
        public IActionResult Post(Exam Exam)
        {
            try
            {
                if (_srv.Write(Exam))
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
        public IActionResult Put(Exam Exam)
        {
            if (_srv.Update(Exam))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            if (_srv.Delete(id))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
