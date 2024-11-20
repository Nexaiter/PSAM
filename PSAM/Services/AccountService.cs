using AutoMapper;
using Microsoft.Identity.Client;
using PSAM.DTOs.AccountDTOs;
using PSAM.Entities;
using PSAM.Exceptions; // Dodano przestrzeń nazw dla wyjątków
using PSAM.Models;
using PSAM.Repositories.IRepositories;
using PSAM.Services.IServices;

namespace PSAM.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly ITechnologyRepository _technologyRepository;
        private readonly ISubscribersRepository _subscribersRepository;

        public AccountService(IMapper mapper, IAccountRepository accountRepository, ITechnologyRepository technologyRepository, ISubscribersRepository subscribersRepository)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _technologyRepository = technologyRepository;
            _subscribersRepository = subscribersRepository;
        }

        public async Task<bool> CheckAccountExistence(string login)
        {
            return await _accountRepository.CheckAccountExistence(login);
        }
        public async Task<bool> CheckAccount(string login, string password)
        {
            return await _accountRepository.CheckAccount(login, password);
        }
        private async Task IsAccountDataValid(RegisterModel registerModel)
        {
            if (await _accountRepository.CheckLoginExistence(registerModel.Login))
            {
                throw new LoginTakenException();
            }
            if (await _accountRepository.CheckUsernameExistence(registerModel.Username))
            {
                throw new UsernameTakenException();
            }
        }
        public async Task<AccountDTO> GetPlayerById(int accountId)
        {
            var account = await _accountRepository.GetAccountById(accountId);
            var accountDto = _mapper.Map<AccountDTO>(account);

            // Pobieranie technologii użytkownika i mapowanie na TechnologyDTOs
            var technologies = await _technologyRepository.GetAccountTechnologies(accountId);
            accountDto.Technologies = technologies.Select(t => new TechnologyDTOs
            {
                TechnologyId = t.TechnologyId,
                AccountId = t.AccountId,
                Technology = t.Technology
            }).ToList();

            return accountDto;
        }

        /*public async Task<List<AccountDTO>> GetFilteredAccounts(int pageNumber, int pageSize, string? username, string? firstName, string? lastName, string? city, string? technology)
        {
            var accounts = await _accountRepository.GetFilteredAccounts(pageNumber, pageSize, username, firstName, lastName, city, technology);
            return _mapper.Map<List<AccountDTO>>(accounts);
        }*/

        public async Task<List<AccountDTO>> GetFilteredAccounts(int pageNumber, int pageSize, string? username, string? firstName, string? lastName, string? city, string? technology)
        {
            var accounts = await _accountRepository.GetFilteredAccounts(pageNumber, pageSize, username, firstName, lastName, city, technology);

            // Pobranie technologii dla każdego konta
            var accountDtos = _mapper.Map<List<AccountDTO>>(accounts);

            foreach (var accountDto in accountDtos)
            {
                var technologies = await _technologyRepository.GetAccountTechnologies(accountDto.AccountId);
                accountDto.Technologies = technologies.Select(t => new TechnologyDTOs
                {
                    TechnologyId = t.TechnologyId,
                    AccountId = t.AccountId,
                    Technology = t.Technology
                }).ToList();
            }

            return accountDtos;
        }

        public async Task<string> GetUsername(int accountId)
        {
            return await _accountRepository.GetUsername(accountId);
        }
        public async Task<int> GetId(string login, string password)
        {
            return await _accountRepository.GetId(login, password);
        }
        public async Task RegisterAccount(RegisterModel registerModel)
        {
            await IsAccountDataValid(registerModel);

            var newAccount = new AccountEntity
            {
                Username = registerModel.Username,
                Login = registerModel.Login,
                Password = registerModel.Password,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                City = registerModel.City,
                Description = registerModel.Description
            };
            await _accountRepository.RegisterAccount(newAccount);
        }
        public async Task DeleteAccountById(int accountId)
        {
            await _accountRepository.DeleteAccountById(accountId);
        }
        public async Task UpdateById(int accountId, AccountUpdateDTO updateDTO)
        {
            var account = await _accountRepository.GetAccountById(accountId);

            if (account == null)
                throw new AccountDoesntExistException();

            // Tylko aktualizuj, jeśli `updateDTO` posiada wartość
            if (!string.IsNullOrEmpty(updateDTO.FirstName))
                account.FirstName = updateDTO.FirstName;

            if (!string.IsNullOrEmpty(updateDTO.LastName))
                account.LastName = updateDTO.LastName;

            if (!string.IsNullOrEmpty(updateDTO.City))
                account.City = updateDTO.City;

            if (!string.IsNullOrEmpty(updateDTO.Description))
                account.Description = updateDTO.Description;


            await _accountRepository.UpdateAccount(account);
        }
        public async Task AddTechnology(int accountId, string technology)
        {
            bool alreadyHasTechnology = await _technologyRepository.AccountHasTechnology(accountId, technology);
            if (alreadyHasTechnology)
            {
                throw new AccountHasTechException();
            }
            try
            {
                await _technologyRepository.AddTechnology(accountId, technology);
            }
            catch (Exception ex)
            {
                throw new AccountExceptions($"Account tech error: {ex.Message}");
            }
        }
        public async Task RemoveTechnology(int technologyId)
        {
            try
            {
                await _technologyRepository.RemoveTechnology(technologyId);
            }
            catch (Exception ex)
            {
                throw new AccountExceptions($"Account tech error: {ex.Message}");

            }
        }
        public async Task<List<TechnologyDTOs>> GetAccountTechs(int accountId)
        {
            var techs = await _technologyRepository.GetAccountTechnologies(accountId);
            return _mapper.Map<List<TechnologyDTOs>>(techs);
        }
        public async Task Subscribe(int accountId, int subscribeeId)
        {
            if (await _subscribersRepository.AlreadySubbed(accountId, subscribeeId))
            {
                throw new AccountExceptions("Already subscribing!");
            }

            var sub = new SubscribersEntity
            {
                SubscribeeId = subscribeeId,
                SubscriberId = accountId
            };
            await _subscribersRepository.Subscribe(sub);
        }
        public async Task Unsubscribe(int accountId, int subscribeeId)
        {
            await _subscribersRepository.Unsubscribe(accountId, subscribeeId);
        }
        public async Task<List<AccountDTO>> GetAccountsSubscriptions(int accountId, int pageNumber, int pageSize)
        {
            var subscriptions = await _subscribersRepository.GetAccountsSubscriptions(accountId, pageNumber, pageSize);
            return _mapper.Map<List<AccountDTO>>(subscriptions);
        }
        public async Task<List<AccountDTO>> GetAllAccounts(int pageNumber, int pageSize)
        {
            var accounts = await _accountRepository.GetAllAccounts(pageNumber, pageSize);
            return _mapper.Map<List<AccountDTO>>(accounts);
        }
        public async Task<List<AccountDTO>> GetAccountsSubscribers(int accountId, int pageNumber, int pageSize)
        {
            var subscribers = await _subscribersRepository.GetAccountsSubscribers(accountId, pageNumber, pageSize);
            return _mapper.Map<List<AccountDTO>>(subscribers);
        }
        public async Task<int> GetSubscriberAmount(int accountId)
        {
            return await _subscribersRepository.GetSubscriberAmount(accountId);
        }

        public async Task UpdateProfileImage(int accountId, string base64Image)
        {
            // Validate or process the Base64 string if needed
            await _accountRepository.SaveImageBase64(accountId, base64Image);
        }

        public async Task DeleteProfileImage(int accountId)
        {
            var account = await _accountRepository.GetAccountById(accountId);
            if (account == null)
            {
                throw new AccountDoesntExistException();
            }

            account.ImageBase64 = ""; // Usuwanie obrazu poprzez ustawienie na null
            await _accountRepository.UpdateAccount(account);
        }

        


    }
}

