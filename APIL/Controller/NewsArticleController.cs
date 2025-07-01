using BusinessObject.Entities;
using BusinessObject.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace WebAppAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticleController : ControllerBase
    {
        private readonly INewsArticleService _naS;
        public NewsArticleController(INewsArticleService naS)
        {
            _naS = naS;
        }
        [HttpGet]
        [Authorize(Roles = "1,2,3")]
        [EnableQuery]
        public async Task<IActionResult> GetAllNewsArticles()
        {
            var newsArticles = await _naS.GetAllNewsArticlesAsync();
            return Ok(newsArticles);
        }
        [HttpGet("staff/{staffId}")]
        public async Task<IActionResult> GetNewsArticleByStaffId(short staffId)
        {
            var newsArticles = await _naS.GetNewsArticleByStaffIdAsync(staffId);
            return Ok(newsArticles);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsArticleById(string id)
        {
            var newsArticle = await _naS.GetNewsArticleByIdAsync(id);
            if (newsArticle == null)
            {
                return NotFound();
            }
            return Ok(newsArticle);
        }
        [HttpPost]
        public async Task<IActionResult> AddNewsArticle([FromBody] NewArticleModel model)
        {
            if (model.NewsArticle == null || model.TagIds == null || !model.TagIds.Any())
            {
                return BadRequest("Invalid news article or tag IDs.");
            }
            await _naS.AddNewsArticleAsync(model.NewsArticle, model.TagIds);
            return CreatedAtAction(nameof(GetNewsArticleById), new { id = model.NewsArticle.NewsArticleId }, model.NewsArticle);
        }
        [HttpPut("v1")]
        public async Task<IActionResult> UpdateNewsArticle([FromBody] NewArticleModel model)
        {
            if (model.NewsArticle == null || model.TagIds == null || !model.TagIds.Any())
            {
                return BadRequest("Invalid news article or tag IDs.");
            }
            await _naS.UpdateNewsArticleAsync(model.NewsArticle, model.TagIds);
            return NoContent();
        }
        [HttpPut("v2")]
        public async Task<IActionResult> UpdateNewsArticle([FromBody] NewsArticle newsArticle)
        {
            if (newsArticle == null)
            {
                return BadRequest("Invalid news article.");
            }
            await _naS.UpdateNewsArticleAsync(newsArticle);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewsArticle(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid ID.");
            }
            await _naS.DeleteNewsArticleAsync(id);
            return NoContent();
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveNewsForLecturer()
        {
            var activeNews = await _naS.GetActiveNewsForLecturerAsync();
            return Ok(activeNews);
        }
    }
}
