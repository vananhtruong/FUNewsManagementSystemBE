﻿using BusinessObject.Entities;
using BusinessObject.Model;

public interface ISystemAccountService
{
    Task<IEnumerable<SystemAccount>> GetAllSystemAccountsAsync();
    Task<SystemAccount?> GetSystemAccountByIdAsync(short id);
    Task AddSystemAccountAsync(SystemAccount systemAccount);
    Task UpdateSystemAccountAsync(SystemAccount systemAccount);
    Task DeleteSystemAccountAsync(short id);
    Task<SystemAccount?> Login(string email, string password);
    Task<SystemAccount?> LoginGoogle(LoginGoogleModel loginGoogleModel);
}
