[Route("api/[controller]")]
[ApiController]
public class ResumeController : ControllerBase
{   
    private readonly ResumeService _resumeService;
    // 获取简历
    [HttpGet("{id}")]
    public ActionResult<Resume> GetResume(int id)
    {
        try
        {
            var resume = _resumeService.Get(id);

            if (resume == null)
            {
                return NotFound();
            }

            return resume;
        }
        catch (ArgumentException ex)
        {
            // log the error here
            return BadRequest("Invalid request parameters");
        }
        catch (InvalidOperationException ex)
        {
            // log the error here   
            return NotFound("Resume not found");
        }
        catch (Exception ex)
        {
            // log the error here
            return StatusCode(500, "Internal server error");
        }
    }


    // 创建简历
    [HttpPost]
    public ActionResult<Resume> CreateResume([FromBody] Resume resume)
    {
        try
        {
            // Validate the incoming Resume object
            if (resume == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid resume details");
            }

            // Use the ResumeService to create a new resume
            var createdResume = _resumeService.Create(resume);

            // Return the created resume with 201 Created status code
            return CreatedAtAction(nameof(GetResume), new { id = createdResume.Id }, createdResume);
        }
        catch (Exception ex)
        {
            // Log the error here
            return StatusCode(500, "Internal server error");
        }
    }


    // 更新简历
    [HttpPut("{id}")]
    public IActionResult UpdateResume(int id, [FromBody] Resume resume)
    {
        try
        {
            // Validate the incoming Resume object
            if (resume == null || id != resume.Id || !ModelState.IsValid)
            {
                return BadRequest("Invalid resume details");
            }

            // Use the ResumeService to get the resume by id
            var resumeFromDb = _resumeService.GetById(id);
            if (resumeFromDb == null)
            {
                return NotFound();
            }

            // Update the resume
            _resumeService.Update(resume);

            // Return NoContent status code
            return NoContent();
        }
        catch (Exception ex)
        {
            // Log the error here
            return StatusCode(500, "Internal server error");
        }
    }


    // 删除简历
    [HttpDelete("{id}")]
    public IActionResult DeleteResume(int id)
    {
        try
        {
            // Use the ResumeService to get the resume by id
            var resume = _resumeService.GetById(id);
            if (resume == null)
            {
                return NotFound();
            }

            // Delete the resume
            _resumeService.Delete(id);

            // Return NoContent status code
            return NoContent();
        }
        catch (Exception ex)
        {
            // Log the error here
            return StatusCode(500, "Internal server error");
        }
    }

}
