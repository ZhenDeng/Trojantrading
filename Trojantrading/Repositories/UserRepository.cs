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

        ApiResponse Update(User user);

        User Get(string userName);
        User GetUserWithAddress(string userName);

        User GetUserByAccount(string account);

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

        public User GetUserWithAddress(string userName)
        {
            var user = trojantradingDbContext.Users
                        .Join(trojantradingDbContext.BillingAddress, x => x.Id, y => y.UserId, (userModel, billingAddress) => new { User = userModel, BillingAddress = billingAddress })
                        .Join(trojantradingDbContext.ShippingAddress, x => x.User.Id, y => y.UserId, (userModel, shippingAddress) => new { JoinUser = userModel, ShippingAddress = shippingAddress })
                        .Where(x => x.JoinUser.User.Account == userName)
                        .Select(join => new User
                        {
                            Id = join.JoinUser.User.Id,
                            CreatedDate = join.JoinUser.User.CreatedDate,
                            Account = join.JoinUser.User.Account,
                            PassswordHash = join.JoinUser.User.PassswordHash,
                            PasswordSalt = join.JoinUser.User.PasswordSalt,
                            Password = join.JoinUser.User.Password,
                            BussinessName = join.JoinUser.User.BussinessName,
                            PostCode = join.JoinUser.User.PostCode,
                            Trn = join.JoinUser.User.Trn,
                            Email = join.JoinUser.User.Email,
                            Mobile = join.JoinUser.User.Mobile,
                            Phone = join.JoinUser.User.Phone,
                            Status = join.JoinUser.User.Status,
                            SendEmail = join.JoinUser.User.SendEmail,
                            ShippingAddress = join.ShippingAddress,
                            BillingAddress = join.JoinUser.BillingAddress
                        }).FirstOrDefault();
            return user;
        }

        //public User GetUserWithRole(string userName)
        //{
        //    var user = trojantradingDbContext.Users
        //                   .Join(trojantradingDbContext, x => x.Id, y => y.UserId, (userModel, billingAddress) => new { User = userModel, BillingAddress = billingAddress })
        //                   .Join(trojantradingDbContext.ShippingAddress, x => x.User.Id, y => y.UserId, (userModel, shippingAddress) => new { JoinUser = userModel, ShippingAddress = shippingAddress })
        //                   .Where(x => x.JoinUser.User.Account == userName)
        //                   .Select(join => new User
        //                   {
        //                       Id = join.JoinUser.User.Id,
        //                       CreatedDate = join.JoinUser.User.CreatedDate,
        //                       Account = join.JoinUser.User.Account,
        //                       PassswordHash = join.JoinUser.User.PassswordHash,
        //                       PasswordSalt = join.JoinUser.User.PasswordSalt,
        //                       Password = join.JoinUser.User.Password,
        //                       BussinessName = join.JoinUser.User.BussinessName,
        //                       PostCode = join.JoinUser.User.PostCode,
        //                       Trn = join.JoinUser.User.Trn,
        //                       Email = join.JoinUser.User.Email,
        //                       Mobile = join.JoinUser.User.Mobile,
        //                       Phone = join.JoinUser.User.Phone,
        //                       Status = join.JoinUser.User.Status,
        //                       SendEmail = join.JoinUser.User.SendEmail,
        //                       ShippingAddress = join.ShippingAddress,
        //                       BillingAddress = join.JoinUser.BillingAddress
        //                   }).FirstOrDefault();
        //    return user;

        //}

        public User GetUserByAccount(string account)
        {
            var user = trojantradingDbContext.Users
                .Where(u=>u.Account == account)
                .FirstOrDefault();
            return user;
        }

        public ApiResponse Update(User user)
        {
            try
            {
                trojantradingDbContext.Users.Update(user);
                trojantradingDbContext.SaveChanges();
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
    }
}