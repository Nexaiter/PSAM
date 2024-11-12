﻿using Microsoft.EntityFrameworkCore;
using PSAM.Entities;
using PSAM.Exceptions;
using PSAM.Repositories.IRepositories;

namespace PSAM.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public readonly AppDbContext _appDbContext;

        public AccountRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<bool> CheckAccountExistence(string login)
        {
            var account = await _appDbContext.Accounts.FirstOrDefaultAsync(p => p.Login == login);
            return account != null;
        }

        public async Task<bool> CheckLoginExistence(string login)
        {
            return await _appDbContext.Accounts.AnyAsync(p => p.Login == login);
        }

        public async Task<bool> CheckUsernameExistence(string username)
        {
            return await _appDbContext.Accounts.AnyAsync(p => p.Username == username);
        }
        public async Task<bool> CheckAccount(string login, string password)
        {
            var account = await _appDbContext.Accounts.FirstOrDefaultAsync(p => p.Login == login && p.Password == password);
            return account != null;
        }
        public async Task<AccountEntity> GetAccountById(int accountId)
        {
            var account = await _appDbContext.Accounts.FirstOrDefaultAsync(p => p.AccountId == accountId);
            return account;
        }

        public async Task<int> GetId(string login, string password)
        {
            var account = await _appDbContext.Accounts.FirstOrDefaultAsync(p => p.Login == login && p.Password == password);
            return account.AccountId;
        }

        public async Task<string> GetUsername(int accountId)
        {
            var account = await _appDbContext.Accounts.FirstOrDefaultAsync(p => p.AccountId == accountId);
            return account.Username;
        }

        public async Task RegisterAccount(AccountEntity accountEntity)
        {
            _appDbContext.Accounts.Add(accountEntity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<AccountEntity>> GetAllAccounts(int pageNumber, int pageSize)
        {
            return await _appDbContext.Accounts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> DeleteAccountById(int accountId)
        {
            var account = await _appDbContext.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
            if (account == null)
                throw new AccountDoesntExistException(); // Rzuca wyjątek, gdy konto nie istnieje
            _appDbContext.Accounts.Remove(account);
            await _appDbContext.SaveChangesAsync();

            return true; // Pomyślnie usunięto konto
        }

        public async Task UpdateAccount(AccountEntity account)
        {
            _appDbContext.Accounts.Update(account);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task SaveImageBase64(int accountId, string base64Image)
        {
            if (string.IsNullOrWhiteSpace(base64Image))
                throw new ArgumentException("Image cannot be empty");

            var account = await _appDbContext.Accounts.FindAsync(accountId);
            if (account == null)
                throw new AccountDoesntExistException();

            account.ImageBase64 = base64Image;
            await _appDbContext.SaveChangesAsync();
        }

        public async Task RemoveProfileImage(int accountId)
        {
            var account = await _appDbContext.Accounts.FindAsync(accountId);
            if (account == null)
            {
                throw new AccountDoesntExistException();
            }

            account.ImageBase64 = null; // Ustawienie na null w bazie danych
            await _appDbContext.SaveChangesAsync();


        }
    }
}
