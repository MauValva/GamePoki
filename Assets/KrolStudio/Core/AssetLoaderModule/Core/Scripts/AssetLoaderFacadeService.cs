using System;
using System.Threading.Tasks;
using Zenject;
using Object = UnityEngine.Object;

namespace KrolStudio
{
    public class AssetLoaderFacadeService : IAssetLoaderFacadeService 
    {
        private readonly IAssetLoaderService _resourceAssetLoaderService;
        private readonly IAssetLoaderService _addressablesAssetLoaderService;

        [Inject]
        public AssetLoaderFacadeService(IResourceAssetLoaderService resourceAssetLoaderService, 
                                        IAddressablesAssetLoaderService addressablesAssetLoaderFacadeService) 
        {
            _resourceAssetLoaderService = resourceAssetLoaderService ?? throw new ArgumentNullException(nameof(resourceAssetLoaderService));
            _addressablesAssetLoaderService = addressablesAssetLoaderFacadeService ?? throw new ArgumentNullException(nameof(addressablesAssetLoaderFacadeService));
        }

        public async Task<TAsset> LoadAssetAsync<TAsset>(string key, AssetLoadSource assetLoadSource, string groupName = "Default") where TAsset : Object =>
            await GetAssetLoaderService(assetLoadSource)
                .LoadAssetAsync<TAsset>(key, groupName);

        public TAsset LoadAsset<TAsset>(string key, AssetLoadSource assetLoadSource, string groupName = "Default") where TAsset : Object =>
            GetAssetLoaderService(assetLoadSource)
                .LoadAsset<TAsset>(key, groupName);

        public void ReleaseAssetsInGroup(AssetLoadSource assetLoadSource, string groupName = "Default") =>
            GetAssetLoaderService(assetLoadSource).ReleaseAssetsInGroup(groupName);

        public void ReleaseAllAssets(AssetLoadSource assetLoadSource) =>
            GetAssetLoaderService(assetLoadSource).ReleaseAllAssets();

        public bool HasLoadedAsset(string key, AssetLoadSource assetLoadSource, string groupName = "Default") =>
            GetAssetLoaderService(assetLoadSource)
                .HasLoadedAsset(key, groupName);

        public IAssetLoaderService GetAssetLoaderService(AssetLoadSource assetLoadSource)
        {
            return assetLoadSource switch
            {
                AssetLoadSource.Addressables => _addressablesAssetLoaderService,
                AssetLoadSource.Resources => _resourceAssetLoaderService,
                _ => throw new ArgumentOutOfRangeException(nameof(assetLoadSource), assetLoadSource, null)
            };
        }
    }
}