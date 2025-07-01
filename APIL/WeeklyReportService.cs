using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Text;

namespace WebAppAPI
{
    public class WeeklyReportService
    {
        private readonly ILogger<WeeklyReportService> _logger;
        private readonly INewsArticleService _newsService;

        public WeeklyReportService(ILogger<WeeklyReportService> logger, INewsArticleService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        public async Task SendWeeklyReportToAdminAsync()
        {
            var today = DateTime.Today;
            var lastMonday = today.AddDays(-(int)today.DayOfWeek - 7); 

            var allArticles = await _newsService.GetAllNewsArticlesAsync();
            var weeklyArticles = allArticles
                .Where(a => a.CreatedDate >= lastMonday && a.CreatedDate <= today)
                .ToList();

            var sb = new StringBuilder();
            sb.Append("<h2>📰 Weekly News Report</h2>");
            sb.Append($"<p>🗓️ From {lastMonday:dd/MM/yyyy} to {today:dd/MM/yyyy}</p>");

            if (weeklyArticles.Any())
            {
                sb.Append($"<p>Total Articles: <b>{weeklyArticles.Count}</b></p><ul>");
                foreach (var a in weeklyArticles)
                {
                    sb.Append($"<li><b>{a.NewsTitle}</b> – {a.CreatedDate:dd/MM/yyyy}</li>");
                }
                sb.Append("</ul>");
            }
            else
            {
                sb.Append("<p><i>📭 No articles were created from last week to today.</i></p>");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("FPT News System", "noreply@yourdomain.com"));
            message.To.Add(new MailboxAddress("shy", "tvanh170315@gmail.com"));
            message.Subject = "📊 Weekly Report – News System";

            var bodyBuilder = new BodyBuilder { HtmlBody = sb.ToString() };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("anhtvde170315@fpt.edu.vn", "rmig dayq jxtj hvbe");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("✅ Weekly report sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("❌ Failed to send weekly report: " + ex.Message);
            }
        }
    }
}
