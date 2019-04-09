using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trojantrading.Models;
using System.Linq;
using Trojantrading.Util;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Trojantrading.Repositories
{
    public interface IUserRepository
    {
        Task<User> Add(User user);

        void Delete(int id);

        Task<ApiResponse> Update(User user);

        Task<User> Get(string userName);

        Task<User> GetUserByAccount(string account);

        int GetTotalUserNumber();

        int GetNewUserNumber();

        IEnumerable<User> GetAll();
    }

    public class UserRepository:IUserRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;

        public UserRepository(TrojantradingDbContext trojantradingDbContext)
        {
            this.trojantradingDbContext = trojantradingDbContext;
        }

        public async Task<User> Add(User user)
        {
            await trojantradingDbContext.Users.AddAsync(user);  
            await trojantradingDbContext.SaveChangesAsync();
            return user;
        }

        public async void Delete(int id)
        {
            var user = new User();
            trojantradingDbContext.Users.Remove(user);
            trojantradingDbContext.SaveChanges();
        }

        public async Task<User> Get(string userName)
        {
            var user = trojantradingDbContext.Users
                .Where(u=>u.Account == userName)
                .FirstOrDefault();
            return user;
        }

        public async Task<User> GetUserByAccount(string account)
        {
            var user = trojantradingDbContext.Users
                .Where(u=>u.Account == account)
                .FirstOrDefault();
            return user;
        }

        public int GetTotalUserNumber()
        {
            var result = trojantradingDbContext.Users
                .Where(u => u.Status == Constrants.USER_STATUS_ACTIVE)
                .Count();
            return result;
        }

        public int GetNewUserNumber()
        {
            var result = trojantradingDbContext.Users
                .Where(u => u.Status == Constrants.USER_STATUS_INACTIVE)
                .Count();
            return result;
        }

        public async Task<ApiResponse> Update(User user)
        {
            try
            {
                trojantradingDbContext.Users.Update(user);
                await trojantradingDbContext.SaveChangesAsync();
                return new ApiResponse() {
                    Status = "success",
                    Message = "Successfully update user"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "fail",
                    Message = ex.Message
                };
            }
        }

        public IEnumerable<User> GetAll()
        {
            var users = trojantradingDbContext.Users;
            return users;
        }
    }
    
}