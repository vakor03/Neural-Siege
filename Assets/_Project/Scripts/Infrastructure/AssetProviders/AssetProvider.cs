using UnityEngine;

namespace _Project.Scripts.Infrastructure.AssetProviders
{
    public class AssetProvider : IAssetProvider
    {
        public TAsset Load<TAsset>(string key) where TAsset : Object
        {
            return Resources.Load<TAsset>(key);
        }

        public TAsset[] LoadAll<TAsset>(string key) where TAsset : Object
        {
            return Resources.LoadAll<TAsset>(key);
        }
    }
}