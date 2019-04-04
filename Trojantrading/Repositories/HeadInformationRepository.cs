using System.Linq;
using Trojantrading.Models;

namespace Trojantrading.Repositories
{
    public interface IHeadInformationRepository
    {
        HeadInformation Get(int id);
        void Delete(int id);
        void Update(HeadInformation headInformation);
    }
    
    public class HeadInformationRepository:IHeadInformationRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;
        
        public HeadInformationRepository(TrojantradingDbContext trojantradingDbContext)
        {
            this.trojantradingDbContext = trojantradingDbContext;
        }


        public HeadInformation Get(int id)
        {
            var headInformation = trojantradingDbContext.HeadInformations
                .Where(h => h.Id == id)
                .FirstOrDefault();
            return headInformation;
        }

        public void Delete(int id)
        {
            var headInformation = Get(id);
            trojantradingDbContext.HeadInformations.Remove(headInformation);
            trojantradingDbContext.SaveChanges();
        }

        public void Update(HeadInformation headInformation)
        {
            trojantradingDbContext.HeadInformations.Update(headInformation);
            trojantradingDbContext.SaveChanges();
        }
        
        
    }
}