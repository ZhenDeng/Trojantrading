
using System;
using System.Collections.Generic;
using System.Linq;
using Trojantrading.Models;

namespace Trojantrading.Repositories
{
    public interface IHeaderInfomationRepository
    {
        ApiResponse AddHeader(HeadInformation headInformation);
        List<HeadInformation> GetHeadInformation();
        ApiResponse UpdateHeadInfomation(HeadInformation headInformation);
        ApiResponse DeleteHeadInfomation(HeadInformation headInformation);
    }

    public class HeaderInfomationRepository : IHeaderInfomationRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;

        public HeaderInfomationRepository(
            TrojantradingDbContext trojantradingDbContext)
        {
            this.trojantradingDbContext = trojantradingDbContext;
        }

        public ApiResponse AddHeader(HeadInformation headInformation)
        {
            try
            {
                trojantradingDbContext.HeadInformations.Add(headInformation);
                trojantradingDbContext.SaveChanges();
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

        public List<HeadInformation> GetHeadInformation()
        {
            var head = trojantradingDbContext.HeadInformations.ToList();
            return head;
        }

        public ApiResponse UpdateHeadInfomation(HeadInformation headInformation)
        {
            try
            {
                trojantradingDbContext.HeadInformations.Update(headInformation);
                trojantradingDbContext.SaveChanges();
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

        public ApiResponse DeleteHeadInfomation(HeadInformation headInformation)
        {
            try
            {
                trojantradingDbContext.HeadInformations.Remove(headInformation);
                trojantradingDbContext.SaveChanges();
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
