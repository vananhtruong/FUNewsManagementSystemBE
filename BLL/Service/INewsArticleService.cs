using BusinessObject.Entities;

public interface INewsArticleService
{
    Task<IEnumerable<NewsArticle>> GetAllNewsArticlesAsync();
    Task<IEnumerable<NewsArticle>> GetNewsArticleByStaffIdAsync(short id);
    Task<NewsArticle?> GetNewsArticleByIdAsync(string id);
    Task AddNewsArticleAsync(NewsArticle newsArticle, List<int> tagIds);
    Task UpdateNewsArticleAsync(NewsArticle newsArticle, List<int> tagIds);
    Task DeleteNewsArticleAsync(string id);
    Task UpdateNewsArticleAsync(NewsArticle newsArticle);
    Task<IEnumerable<NewsArticle>> GetActiveNewsForLecturerAsync();
}
