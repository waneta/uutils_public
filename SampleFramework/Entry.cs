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
        public UDebug.LOGLEVEL logLevel = UDebug.LOGLEVEL.INFO;

     
        void Awake()
        {
            Common.AddComponent<DontDestroyer>(this);
            //u_utils初始化
            CommonVariable.ShowAllUnityPath();
            ClientInfo _clientInfo = ClientInfo.Deserialize(CommonVariable.ClientInfoFile);
            UDebug.SetLogLevel(_clientInfo.logLevel);
            CommonVariable.SetCompanyProductName(_clientInfo.CompanyName,_clientInfo.ProductName);

            ClientConfig.clientType = clientType;
            ClientConfig.Version = string.Format("{0}{1}{2}", _clientInfo.MajorVersion, _clientInfo.MinorVersion,_clientInfo.Revision) ;
            
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
            World.Instance.SetLoadEnd(level);
        }
    }
}
