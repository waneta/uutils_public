/************************************************************************************
 * @author   wangjian
 * 程序入口脚本,会进行各种配置
************************************************************************************/
using UnityEngine;
using Client.UUtils;

namespace SampleFramework.UUtils
{
    public class Entry : MonoBehaviour
    {

        private Client _client;

        public EnumClientType clientType = EnumClientType.WINPAD;
        //*_*public EnumSkinSetting skinType = EnumSkinSetting.Normal;
        public UDebug.LOGLEVEL logLevel = UDebug.LOGLEVEL.INFO;

        //*_*public EnumLanguage language = EnumLanguage.English;

        public string Version = "1.0.20160801";
        [Range(0, 100)]
        public int NetVersion = 0;  //网络版本号：网络同步时使用：

     
        void Awake()
        {
            Common.AddComponent<DontDestroyer>(this);
            //u_utils初始化
            CommonVariable.ShowAllUnityPath();
            ClientInfo _clientInfo = ClientInfo.Deserialize(CommonVariable.ClientInfoFile);
            UDebug.SetLogLevel(_clientInfo.logLevel);
            CommonVariable.SetCompanyProductName(_clientInfo.CompanyName,_clientInfo.ProductName);

            ClientConfig.clientType = clientType;
            ClientConfig.NetVesrion = NetVersion;
            ClientConfig.Version = Version;
            
            _client = Client.Instance;

            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                int Xscreen = (int)WindowMod.GetSystemMetrics(0);
                int Yscreen = (int)WindowMod.GetSystemMetrics(1);
                Screen.SetResolution(Xscreen, Yscreen, true);
                Screen.fullScreen = true;
            }
        }

        void Start()
        {
            //*_*ApplicationUI.Instance.init();
            //*_*ApplicationUI.Instance.openWindowObject(WINDOWOBJECT_TYPE.PROGRESSWINDOW_LOADASSET, SHOW_EFFECT.NONE, false, false);
            Common.StartCoroutine(_client.StartUp(clientType));
        }

        void Update()
        {
            _client.update();
        }

        void OnDestroy()
        {
            if(_client != null)
                _client.Destroy();
        }

        void OnLevelWasLoaded(int level)
        {
            Debug.Log("OnLevelWasLoaded：" + Time.time);
            World.Instance.SetLoadEnd(level);
        }
    }
}
