using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAppAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tS;
        public TagController(ITagService tS)
        {
            _tS = tS;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _tS.GetAllTagsAsync();
            return Ok(tags);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById(int id)
        {
            var tag = await _tS.GetTagByIdAsync(id);
            if (tag == null)
                return NotFound();
            return Ok(tag);
        }
        [HttpPost]
        public async Task<IActionResult> AddTag([FromBody] Tag tag)
        {
            if (tag == null)
                return BadRequest("Tag cannot be null");
            await _tS.AddTagAsync(tag);
            return CreatedAtAction(nameof(GetTagById), new { id = tag.TagId }, tag);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTag([FromBody] Tag tag)
        {
            if (tag == null)
                return BadRequest("Tag cannot be null");
            await _tS.UpdateTagAsync(tag);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var existingTag = await _tS.GetTagByIdAsync(id);
            if (existingTag == null)
                return NotFound();
            try
            {
                await _tS.DeleteTagAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("MaxId")]
        public async Task<IActionResult> GetMaxTagId()
        {
            var tags = await _tS.GetAllTagsAsync();
            if (tags == null || !tags.Any())
                return NotFound("No tags found.");
            var maxId = tags.Max(t => t.TagId);
            return Ok(maxId);
        }
    }
}
