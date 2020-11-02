using System.Threading.Tasks;
using Trojantrading.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text;
using Trojantrading.Service;
using Trojantrading.Repositories.Generic;
using Trojantrading.Utilities;
using Trojantrading.Util;

namespace Trojantrading.Repositories
{
    public interface IUserRepository
    {
        Task<ApiResponse> AddUser(User user);
        Task<ApiResponse> DeleteUser(int id);
        Task<ApiResponse> Update(User user);
        Task<User> GetUserByAccount(int userId);
        Task<List<User>> GetUsers();
        Task<ApiResponse> ValidateEmail(string email);
        Task<ApiResponse> UpdatePassword(int userId, string newPassword);
        Task<ApiResponse> ValidatePassword(int userId, string password);
    }

    public class UserRepository:IUserRepository
    {
        private readonly IShare _share;
        private readonly IRepositoryV2<User> _userRepository;

        public UserRepository(IShare share, IRepositoryV2<User> userRepository)
        {
            _share = share;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse> AddUser(User user)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(_share.RandomString(4, true));
                builder.Append(_share.RandomNumber(1000, 9999));
                builder.Append(_share.RandomString(2, false));
                user.Password = builder.ToString();
                _userRepository.Create(user);
                await _userRepository.SaveChangesAsync();
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
                    Message = ex.Message
                };
            }
            
        }

        public async Task<ApiResponse> DeleteUser(int id)
        {
            try
            {
                var user = await _userRepository.Queryable.Where(u => u.Id == id).GetFirstOrDefaultAsync();
                user.Status = Constrants.USER_STATUS_INACTIVE;
                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();
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
                    Message = ex.Message
                };
            }
        }

        public async Task<User> GetUserByAccount(int userId)
        {
            var user = await _userRepository.Queryable.Where(u => u.Id == userId).GetFirstOrDefaultAsync();
            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _userRepository.Queryable.GetListAsync();
            return users;
        }

        public async Task<ApiResponse> Update(User user)
        {
            try
            {
                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();
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

        public async Task<ApiResponse> UpdatePassword(int userId, string newPassword)
        {
            try
            {
                User user = await _userRepository.Queryable.Where(item => item.Id == userId).GetFirstOrDefaultAsync();
                user.Password = newPassword;
                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();

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

        public async Task<ApiResponse> ValidateEmail(string email)
        {
            try
            {
                string userName = (await _userRepository.Queryable.Where(user => user.Email == email).GetFirstOrDefaultAsync()).Account;
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

        public async Task<ApiResponse> ValidatePassword(int userId, string password)
        {
            try
            {
                var userModel = await _userRepository.Queryable.Where(user => user.Id == userId).GetFirstOrDefaultAsync();
                if (userModel.Password == password)
                {
                    return new ApiResponse()
                    {
                        Status = "success",
                        Message = "Successfully validate your password"
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Status = "fail",
                        Message = "Please input validate password"
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

        private string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}