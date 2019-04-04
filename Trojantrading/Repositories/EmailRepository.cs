using System.Linq;
using Trojantrading.Util;
namespace Trojantrading.Repositories
{
    public interface IEmailRepository
    {
        string GetAccount();
        string GetPassWord();
    }
    public class EmailRepository : IEmailRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;

        public EmailRepository(TrojantradingDbContext trojantradingDbContext){
            this.trojantradingDbContext = trojantradingDbContext;
        }

        public string GetAccount()
        {
            var companyInfo = trojantradingDbContext.CompanyInfos
                .Where(c=>c.Id == Constrants.ID_ONE)
                .FirstOrDefault();
            return companyInfo.Email;
        }

        public string GetPassWord()
        {
            var companyInfo = trojantradingDbContext.CompanyInfos
                .Where(c=>c.Id == Constrants.ID_ONE)
                .FirstOrDefault();
            return companyInfo.EmailPassWord;
        }
    }
}