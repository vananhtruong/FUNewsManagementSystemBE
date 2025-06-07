using BusinessObject.Entities;
using FUNews.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using BusinessLogicLayer.UnitOfWorks;
using BusinessObject.Model;

namespace FUNews.BLL.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<SystemAccount, short> _systemAccountRepository;
        private readonly IGenericRepository<NewsArticle, string> _newsArticleRepository;

        public SystemAccountService(IGenericRepository<SystemAccount, short> systemAccountRepository, IUnitOfWork unitOfWork, IGenericRepository<NewsArticle, string> newsArticleRepository)
        {
            _systemAccountRepository = systemAccountRepository;
            _unitOfWork = unitOfWork;
            _newsArticleRepository = newsArticleRepository;
        }

        // Get all system accounts
        public async Task<IEnumerable<SystemAccount>> GetAllSystemAccountsAsync()
        {
            return await _systemAccountRepository.FindAll().ToListAsync();
        }

        // Get system account by ID
        public async Task<SystemAccount?> GetSystemAccountByIdAsync(short id)
        {
            return await _systemAccountRepository.FindById(id, "AccountId");
        }

        // Add a new system account
        public async Task AddSystemAccountAsync(SystemAccount systemAccount)
        {
            if (systemAccount == null)
                throw new ArgumentNullException(nameof(systemAccount));

            _systemAccountRepository.Create(systemAccount);
            await _unitOfWork.SaveChange(); // Save changes using UnitOfWork
        }

        // Update an existing system account
        public async Task UpdateSystemAccountAsync(SystemAccount systemAccount)
        {
            if (systemAccount == null)
                throw new ArgumentNullException(nameof(systemAccount));

            _systemAccountRepository.Update(systemAccount);
            await _unitOfWork.SaveChange(); // Save changes using UnitOfWork
        }

        // Delete a system account by ID
        public async Task DeleteSystemAccountAsync(short id)
        {
            var systemAccount = await _systemAccountRepository.FindById(id, "AccountId");
            if (systemAccount == null)
                throw new ArgumentException("System account not found");
            var newsArticles = await _newsArticleRepository.FindAll(x => x.CreatedById == id).ToListAsync();
            if (newsArticles.Any())
                throw new InvalidOperationException("Cannot delete system account with existing news articles.");
            _systemAccountRepository.Delete(systemAccount);
            await _unitOfWork.SaveChange(); // Save changes using UnitOfWork
        }
        // Check Login
        public async Task<SystemAccount> Login(string email, string password)
        {
            return await _systemAccountRepository.FindAll().Where(x => x.AccountEmail.Equals(email) && x.AccountPassword.Equals(password)).FirstOrDefaultAsync();
        }

        public async Task<SystemAccount> LoginGoogle(LoginGoogleModel loginGoogleModel)
        {
            var isexist = _systemAccountRepository.FindAll(x => x.AccountEmail.Equals(loginGoogleModel.AccountEmail));
            if (isexist.Any())
            {
                return await isexist.FirstOrDefaultAsync();
            }
            var random = new Random();
            var newAccount = new SystemAccount
            {
                AccountId = (short)random.Next(1000, 9999),
                AccountEmail = loginGoogleModel.AccountEmail,
                AccountName = loginGoogleModel.AccountName,
                AccountRole = 2,
            };
            _systemAccountRepository.Create(newAccount);
            _unitOfWork.SaveChange();
            return await _systemAccountRepository.FindAll(x => x.AccountEmail.Equals(loginGoogleModel.AccountEmail)).FirstOrDefaultAsync();
        }
    }
}
