using System.Threading.Tasks;
using Trojantrading.Models;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Trojantrading.Repositories
{
    public interface IUserRepository
    {
        ApiResponse Add(User user);
        ApiResponse Delete(int id);
        ApiResponse Update(User user);
        User Get(string userName);
        User GetUserWithAddress(string userName);
        User GetUserByAccount(string account);
        User GetUserWithRole(string userName);
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

        public ApiResponse Add(User user)
        {
            try
            {
                trojantradingDbContext.Users.Add(user);
                trojantradingDbContext.SaveChanges();
                return new ApiResponse() {
                    Status = "success",
                    Message = "Successfully add user"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "fail",
                    Message = "Fail to add user"
                };
            }
            
        }

        public ApiResponse Delete(int id)
        {
            try
            {
                var user = trojantradingDbContext.Users.Where(u => u.Id == id).FirstOrDefault();
                trojantradingDbContext.Users.Remove(user);
                trojantradingDbContext.SaveChanges();
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully delete user"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "fail",
                    Message = "Fail to delete user"
                };
            }
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
                        .Join(trojantradingDbContext.BillingAddresses, x => x.BillingAddressId, y => y.Id, (userModel, billingAddress) => new { User = userModel, BillingAddress = billingAddress })
                        .Join(trojantradingDbContext.ShippingAddresses, x => x.User.ShippingAddressId, y => y.Id, (userModel, shippingAddress) => new { JoinUser = userModel, ShippingAddress = shippingAddress })
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

        public User GetUserWithRole(string userName)
        {
            var user = trojantradingDbContext.Users
                           .Join(trojantradingDbContext.Roles, x => x.RoleId, y => y.Id, (userModel, role) => new { User = userModel, Role = role })
                           .Where(x => x.User.Account == userName)
                           .Select(join => new User
                           {
                               Id = join.User.Id,
                               CreatedDate = join.User.CreatedDate,
                               Account = join.User.Account,
                               PassswordHash = join.User.PassswordHash,
                               PasswordSalt = join.User.PasswordSalt,
                               Password = join.User.Password,
                               BussinessName = join.User.BussinessName,
                               PostCode = join.User.PostCode,
                               Trn = join.User.Trn,
                               Email = join.User.Email,
                               Mobile = join.User.Mobile,
                               Phone = join.User.Phone,
                               Status = join.User.Status,
                               SendEmail = join.User.SendEmail,
                               Role = join.Role
                           }).FirstOrDefault();
            return user;

        }

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