namespace Trojantrading.Repositories
{
    public interface IPdfBoardRepository
    {
        
    }
    public class PdfBoardRepository:IPdfBoardRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;
        
        public PdfBoardRepository(TrojantradingDbContext trojantradingDbContext)
        {
            this.trojantradingDbContext = trojantradingDbContext;
        }
        
        
    }
}