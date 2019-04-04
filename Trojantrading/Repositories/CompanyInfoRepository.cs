using System.Linq;
using Trojantrading.Models;

namespace Trojantrading.Repositories
{

    public interface ICompanyInfoRepository
    {
        void Update(CompanyInfo companyInfo);
        CompanyInfo Get(int id);
    }

    public class CompanyInfoRepository:ICompanyInfoRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;

        public CompanyInfoRepository(TrojantradingDbContext trojantradingDbContext)
        {
            this.trojantradingDbContext = trojantradingDbContext;
        }

        public CompanyInfo Get(int id)
        {
            var companyInfo = trojantradingDbContext.CompanyInfos
                .Where(h => h.Id == id)
                .FirstOrDefault();
            return companyInfo;
        }

        public void Update(CompanyInfo companyInfo)
        {
            trojantradingDbContext.CompanyInfos.Update(companyInfo);
            trojantradingDbContext.SaveChanges();
        }

    }
}