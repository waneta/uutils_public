/************************************************************************************
 * @author   wangjian
 * 客户端最上层控制，如启动，网络控制，退出等
************************************************************************************/
using Client.UUtils;
using System.Collections;
using UnityEngine;

namespace SampleFramework.UUtils
{

    public class Client : Singleton<Client>
    {

        public EnumClientType clientType;
        //启动程序
        public IEnumerator StartUp(EnumClientType type)
        {
#if !CLIENT_WEB
            //提取系统配置中保存的软件版本信息
            string lastSaveVersion = PlayerPrefs.GetString(ClientConfig.KEY_SOFTWARE_VERSION_NAME);

            clientType = type;
            UDebug.LogError("===========*************{0} client all path info *************==================\n{1}", CommonVariable.GetProuctName(), CommonVariable.ShowAllPath());

            //当前版本和系统配置中保存的上次运行版本不一致
            if (!lastSaveVersion.Equals(ClientConfig.Version))
            {
                UDebug.LogError("===========*************========== client first Lanuch ================*************==================");

                //#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN //打成dll,不能用unity的预编译

                yield return EntryFileHandle.CopyPersistentsData();
                yield return EntryFileHandle.CopyPrivateData();
                yield return EntryFileHandle.CopyConfigData(true, new EntryFileHandle.OnCopyCallOver(OnCopyConfigDataFinished));

            }
            else
            {
                ClientConfig.isFirst = false;
                yield return EntryFileHandle.CopyConfigData(false, new EntryFileHandle.OnCopyCallOver(OnCopyConfigDataFinished));
            }
            yield break;
#else
                clientType = type;
                ClientConfig.isFirst = false;
                CheckWebPlayerPlugin();
                OnCopyConfigDataFinished();
                yield break;
#endif
        }

        //文件复制完成回调
        public void OnCopyConfigDataFinished()
        {
            PlayerPrefs.SetString(ClientConfig.KEY_SOFTWARE_VERSION_NAME, ClientConfig.Version);
            PlayerPrefs.SetInt(ClientConfig.KEY_SOFTWARE_VERSION_CODE, ClientConfig.SoftwareVersionCode);

            ClientConfig.width = Screen.width;
            ClientConfig.height = Screen.height;

            //*_*MT_Table_Club tableClub = TableManager.Instance.TableClub;
            //*_*DataManager _dataManager = DataManager.Instance;

            //*_*_dataManager.SetUrlConfig();
            World.Instance.startChangeMap("Main");

        }

        //web js 与unity通信脚本
        private void CheckWebPlayerPlugin()
        {
            //在场景中寻找附加了WebPlayerPlugin组件的物体
            WebPlayerPlugin webPlayerPlugin = GameObject.FindObjectOfType<WebPlayerPlugin>();
            GameObject webPlayerPluginObj;
            if (webPlayerPlugin == null)
            {
                webPlayerPluginObj = new GameObject("WebPlayerPlugin");
                webPlayerPluginObj.AddComponent<WebPlayerPlugin>();
                webPlayerPluginObj.AddComponent<DontDestroyer>();
            }
            else
            {
                webPlayerPluginObj = webPlayerPlugin.gameObject;
            }

            if (!webPlayerPluginObj.activeSelf)
                webPlayerPluginObj.SetActive(true);

            DontDestroyer dontDestroyer = webPlayerPluginObj.GetComponent<DontDestroyer>();
            if (dontDestroyer == null)
                webPlayerPluginObj.AddComponent<DontDestroyer>();

        }

        public float realTime;
        public void update()
        {
            float deltaTime = Time.deltaTime;
            float fixedDeltaTime = Time.fixedDeltaTime;
            float nowTime = Time.realtimeSinceStartup;
            float realDeltaTime = nowTime - realTime;
            realTime = nowTime;
            UpdateComponentManager.Instance.update(deltaTime, fixedDeltaTime, realDeltaTime);
        }

        public void Destroy()
        {
        }
    }
}
