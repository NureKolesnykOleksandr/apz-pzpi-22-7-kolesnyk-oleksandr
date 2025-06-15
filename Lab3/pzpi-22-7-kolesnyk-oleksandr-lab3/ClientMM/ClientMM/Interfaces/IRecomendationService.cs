using ClientMM.Models;

namespace ClientMM.Interfaces
{
    public interface IRecomendationService
    {
        Task<IEnumerable<Recommendation>> GetAllRecommendationsForUser(int userId);
        Task<Recommendation> GenerateRecommendationForUser(int userId);
    }
}
