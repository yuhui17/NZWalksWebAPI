
using NZWalks.Model.Models.Domain;

namespace NZWalks.NZWalksDataAccess.Repositories
{

    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
