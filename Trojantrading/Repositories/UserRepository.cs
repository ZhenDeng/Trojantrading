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

        User Get(string userName);

        User GetUserByAccount(string account);

        int GetTotalUserNumber();

        int GetNewUserNumber();

        IEnumerable<User> GetAll();

        ApiResponse ValidateEmail(string email);
        Task<ApiResponse> UpdatePassword(string userName, string newPassword);

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

        public void Delete(int id)
        {
            var user = new User();
            trojantradingDbContext.Users.Remove(user);
            trojantradingDbContext.SaveChanges();
        }

        public User Get(string userName)
        {
            var user = trojantradingDbContext.Users
                .Where(u=>u.Account == userName)
                .FirstOrDefault();
            return user;
        }

        public User GetUserByAccount(string account)
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

        public async Task<ApiResponse> UpdatePassword(string userName, string newPassword)
        {
            try
            {
                User user = trojantradingDbContext.Users.Where(item => item.Account == userName).FirstOrDefault();
                user.Password = newPassword;
                trojantradingDbContext.Users.Update(user);
                await trojantradingDbContext.SaveChangesAsync();

                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully update the password"
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

        public ApiResponse ValidateEmail(string email)
        {
            try
            {
                string userName = trojantradingDbContext.Users.Where(user => user.Email == email).FirstOrDefault().Account;
                if (!string.IsNullOrEmpty(userName))
                {
                    return new ApiResponse()
                    {
                        Status = "success",
                        Message = "Email Address is Valid"
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Status = "fail",
                        Message = "This email address is not register in our website"
                    };
                }

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