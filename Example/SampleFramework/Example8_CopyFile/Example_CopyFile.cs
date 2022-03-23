using System.Collections;
using System.IO;
using UnityEngine;
using JObject = System.Collections.Generic.Dictionary<string, object>;
using Client.UUtils;

namespace SampleFramework.UUtils.Example
{

    public class Example_CopyFile : MonoBehaviour
    {

        void Start()
        {
            ClientInfo _clientInfo = ClientInfo.Deserialize(CommonVariable.ClientInfoFile);

            UDebug.SetLogLevel(_clientInfo.logLevel);
            CommonVariable.SetCompanyProductName(_clientInfo.CompanyName, _clientInfo.ProductName);//涉及数据读取，和资源加载必须设置公司名和产品名

        }
        void OnGUI()
        {
            if (GUILayout.Button("开始从StreamingAssets目录复制文件"))
            {
                StartCoroutine(CopyFile());
            }

            if (GUILayout.Button("读取u_utils示例数据"))
            {
                UsersData usersData = ReadUsersData();
                Debug.LogError("usersData.CurrentUserID: " + usersData.CurrentUserID);
            }


        }
        IEnumerator CopyFile()
        {
            yield return EntryFileHandle.CopyPersistentsData();//复制数据文件，eg 挥杆数据，体侧数据
            yield return EntryFileHandle.CopyPrivateData();	   //复制私密数据，eg Security key
            yield return EntryFileHandle.CopyConfigData(true, new EntryFileHandle.OnCopyCallOver(OnCopyConfigDataFinished));
        }
        void OnCopyConfigDataFinished()
        {
            Debug.LogError("copy finished.");

        }

        public UsersData ReadUsersData()
        {
            JObject json_data = null;

            //检查基本数据文件是否存在，并尝试解析
            string normalFile = string.Format("{0}/{1}.txt", CommonVariable.PersistentUserDataPath, "userinfo");
            string encryptFile = string.Format("{0}/{1}.ts", CommonVariable.PersistentUserDataPath, "userinfo");
#if !CLIENT_WEB
            if (File.Exists(normalFile))
            {
                json_data = FileManager.Instance.ReadJsonObject(normalFile);
            }
            else
            {
                //不存在基本数据文件，则尝试以加密方式解析
                json_data = FileManager.Instance.ReadEncryptZipJsonObject(encryptFile);
            }
#endif
            if (json_data != null)
            {
                //检查数据文件版本号，如果低于当前系统版本，则升级重写
                int vercode = JsonHelper.GetInteger(json_data, ClientConfig.KEY_DATA_VERSION_CODE);

                if (vercode != ClientConfig.DataVersionCode)
                {
                    if (json_data.ContainsKey(ClientConfig.KEY_DATA_VERSION_CODE))
                    {
                        json_data.Remove(ClientConfig.KEY_DATA_VERSION_CODE);
                    }
                    json_data.Add(ClientConfig.KEY_DATA_VERSION_CODE, ClientConfig.DataVersionCode);
#if !CLIENT_WEB
                    FileManager.Instance.WriteEncryptZipJsonObject(encryptFile, json_data);

                    if (File.Exists(normalFile))
                    {
                        File.Delete(normalFile);
                    }
#endif
                }
                return new UsersData(json_data, vercode);
            }
            return null;
        }
    }


}
