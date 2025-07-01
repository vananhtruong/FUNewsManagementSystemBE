using Hangfire;

namespace WebAppAPI
{
    public static class HangfireJobScheduler
    {
        public static void ScheduleJobs()
        {
            RecurringJob.AddOrUpdate<WeeklyReportService>(
            "weekly-report-job",
            x => x.SendWeeklyReportToAdminAsync(),
            Cron.Weekly(DayOfWeek.Monday, 8));
        }
    }

}
