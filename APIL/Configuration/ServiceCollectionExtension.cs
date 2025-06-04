
using BusinessLogicLayer.UnitOfWorks;
using DataAccessLayer;
using FUNews.BLL.Services;
using FUNews.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace WebAppAPI.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static void AddRepositoryUOW(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ITagService, TagService>();
            services.AddScoped<INewsArticleService, NewsArticleService>();
            services.AddScoped<ISystemAccountService, SystemAccountService>();
        }
    }
}
