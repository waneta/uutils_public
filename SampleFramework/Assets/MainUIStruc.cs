namespace SampleFramework.UUtils
{
    public class MainUIStruc : AssetObject
    {
        public CacheMainUI mainstruc;
        public override void LoadAsset()
        {
            base.LoadAsset();
            //*_*mainstruc = ResourceManager.Instance.InstantiateGameObjectFromResource<CacheMainUI>(RESOURCE_ENUM.struc_MainUI);
            state = ASSET_STATE.HIDE;
        }
        
    }
}
