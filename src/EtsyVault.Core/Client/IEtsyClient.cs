using EtsyVault.Core.Models;

namespace EtsyVault.Core.Client;

public interface IEtsyClient
{
    Task<EtsyShopDto?> GetShopMeAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EtsyListingDto>> GetAllListingsAsync(long shopId, IProgress<string>? logProgress = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EtsyListingImageDto>> GetListingImagesAsync(long listingId, CancellationToken cancellationToken = default);
}
