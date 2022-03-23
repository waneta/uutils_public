using Client.UUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleFramework.UUtils
{

    public enum ASSET_STATE
    {
        NONE,
        LOADING,
        SHOWING,                //正在移动至正中显示的过度
        SHOW,                   //在屏幕正中显示的状态

        HIDE,                   //other 隐藏
        HIDING,                 //正在移动至隐藏的过度
    }
    public enum ASSET_TYPE
    {
        NONE,

        Struc_MainUI,                   //Main UI的主结构

        GraphItemUI,                    //单个图表
        SimpleGraph_VerticelGraph,
        SensorUI,                       //传感器UI
        ScreenUI,                       //
        TrainUI,
        GraphCoordinate,
    }
    public class AssetObject
    {
        public ASSET_STATE state = ASSET_STATE.NONE;
        public GameObject assetObject;
        public ASSET_TYPE type;

        public virtual void LoadAsset()
        {
            state = ASSET_STATE.LOADING;
        }

        public virtual void Show()
        {
            state = ASSET_STATE.SHOW;
        }

        public virtual void Hide()
        {
            state = ASSET_STATE.HIDE;
        }

    }

    public class AssetsCacheMananager : MonoSingleton<AssetsCacheMananager>
    {
        public delegate IEnumerator OnLoadCallBack();
        public OnLoadCallBack callBackMethod;

        public Dictionary<ASSET_TYPE, AssetObject> assetDic = new Dictionary<ASSET_TYPE, AssetObject>();
        public override void initialize()
        {
            base.initialize();
            assetDic.Add(ASSET_TYPE.Struc_MainUI, new MainUIStruc());
            //*_*assetDic.Add(ASSET_TYPE.SensorUI, new SensorStatusUIAsset());
            //*_*assetDic.Add(ASSET_TYPE.ScreenUI, new ScreenUIAsset());
            //*_*assetDic.Add(ASSET_TYPE.TrainUI, new TrainUIAsset());
            //*_*assetDic.Add(ASSET_TYPE.GraphCoordinate, new GraphCoordinateAsset());
            //*_*assetDic.Add(ASSET_TYPE.SimpleGraph_VerticelGraph, new SimpleVerticalGraphAsset());

        }

        int frameMax = 5;
        int loadingLimit = 1;
        int loadingCount = 0;
        int loadedCount = 0;
        public bool isAllLoad = false;

        public void Update()
        {

            frameMax--;
            if (frameMax < 0)
            {
                frameMax = 5;
                loadingCount = 0;
                loadedCount = 0;
                if (isAllLoad) return;
                foreach (KeyValuePair<ASSET_TYPE, AssetObject> pair in assetDic)
                {
                    if (pair.Value.state == ASSET_STATE.NONE)
                    {
                        pair.Value.LoadAsset();
                    }
                    if (pair.Value.state == ASSET_STATE.LOADING)
                    {
                        loadingCount++;
                    }
                    if (pair.Value.state == ASSET_STATE.HIDE)
                    {
                        loadedCount++;
                    }
                    if (loadingCount >= loadingLimit) continue;


                }
                if (loadedCount == assetDic.Count)
                {
                    isAllLoad = true;
                    //this.callBackMethod();
                    Common.StartCoroutine(this.callBackMethod());
                }
            }

        }

        public AssetObject getAssetObject(ASSET_TYPE type)
        {
            if (assetDic.ContainsKey(type))
            {
                return assetDic[type];
            }
            return null;
        }

        public T getAssetObject<T>(ASSET_TYPE type) where T : AssetObject
        {
            if (assetDic.ContainsKey(type))
            {
                if (assetDic[type] is T)
                {
                    return assetDic[type] as T;
                }

            }
            return null;
        }
    }

}