using IntelligentResumeParsingSystem.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

[Route("api/[controller]")]
[ApiController]
public class JobPositionController : ControllerBase
{
    private readonly IJobPositionService _jobPositionService;

    public JobPositionController(IJobPositionService jobPositionService)
    {
        _jobPositionService = jobPositionService;
    }

    // 获取所有职位
    [HttpGet]
    public ActionResult<List<JobPosition>> GetAllPositions()
    {
        return Ok(_jobPositionService.GetAll());
    }

    // 获取特定职位
    [HttpGet("{id}")]
    public ActionResult<JobPosition> GetPositionById(int id)
    {
        var jobPosition = _jobPositionService.GetById(id);

        if (jobPosition == null)
        {
            return NotFound();
        }

        return Ok(jobPosition);
    }

    // 创建职位
    [HttpPost]
    public ActionResult<JobPosition> CreatePosition([FromBody] JobPosition jobPosition)
    {
        _jobPositionService.Create(jobPosition);

        return CreatedAtAction(nameof(GetPositionById), new { id = jobPosition.Id }, jobPosition);
    }

    // 更新职位
    [HttpPut("{id}")]
    public ActionResult UpdatePosition(int id, [FromBody] JobPosition jobPosition)
    {
        if (id != jobPosition.Id)
        {
            return BadRequest();
        }

        _jobPositionService.Update(jobPosition);

        return NoContent();
    }

    // 删除职位
    [HttpDelete("{id}")]
    public ActionResult DeletePosition(int id)
    {
        var existingJobPosition = _jobPositionService.GetById(id);

        if (existingJobPosition == null)
        {
            return NotFound();
        }

        _jobPositionService.Delete(id);

        return NoContent();
    }
}
