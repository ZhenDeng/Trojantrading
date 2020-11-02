
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Trojantrading.Models;
using Trojantrading.Repositories.Generic;
using Trojantrading.Utilities;

namespace Trojantrading.Repositories
{
    public interface IHeaderInfomationRepository
    {
        Task<ApiResponse> AddHeader(HeadInformation headInformation);
        Task<List<HeadInformation>> GetHeadInformation();
        Task<ApiResponse> UpdateHeadInfomation(HeadInformation headInformation);
        Task<ApiResponse> DeleteHeadInfomation(HeadInformation headInformation);
    }

    public class HeaderInfomationRepository : IHeaderInfomationRepository
    {
        private readonly IRepositoryV2<HeadInformation> _headInformationDataRepository;

        public HeaderInfomationRepository(IRepositoryV2<HeadInformation> headInformationDataRepository)
        {
            _headInformationDataRepository = headInformationDataRepository;
        }

        public async Task<ApiResponse> AddHeader(HeadInformation headInformation)
        {
            try
            {
                _headInformationDataRepository.Create(headInformation);
                await _headInformationDataRepository.SaveChangesAsync();
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully Add Head Information"
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

        public async Task<List<HeadInformation>> GetHeadInformation()
        {
            var head = await _headInformationDataRepository.Queryable.GetListAsync();
            return head;
        }

        public async Task<ApiResponse> UpdateHeadInfomation(HeadInformation headInformation)
        {
            try
            {
                _headInformationDataRepository.Update(headInformation);
                await _headInformationDataRepository.SaveChangesAsync();
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully update Head Infomation"
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

        public async Task<ApiResponse> DeleteHeadInfomation(HeadInformation headInformation)
        {
            try
            {
                _headInformationDataRepository.Delete(headInformation);
                await _headInformationDataRepository.SaveChangesAsync();

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", headInformation.ImagePath);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully delete Head Infomation"
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
    }
}
