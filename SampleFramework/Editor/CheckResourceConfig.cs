/************************************************************************************
 * @author   wangjian
 * 检测资源路径配置是否正确
************************************************************************************/
using System.IO;
using UnityEditor;
using UnityEngine;
using Client.UUtils;
using Client.UUtils.Editor;

namespace SampleFramework.UUtils.Editor
{
    public class CheckResourceConfig : ScriptableObject
    {

        [MenuItem("开发库/UUtils/SampleFramework/检查prefab路径配置")]
        public static void OnReadConfig()
        {
            ClientInfo _clientInfo = ClientInfo.Deserialize(CommonVariable.ClientInfoFile);
            CommonVariable.SetCompanyProductName(_clientInfo.CompanyName, _clientInfo.ProductName);

            string fileName = CommonVariable.ResoucesConfigFile;
            Debug.LogError("ResoucesConfigFile:" + fileName);
            if (!File.Exists(fileName))
            {
                Debug.LogError("File not Exist :" + fileName);
                return;
            }

            //检查每个路径下是否存在
            FileStream aFile = File.OpenRead(fileName);
            StreamReader sr = new StreamReader(aFile);
            string strLine = sr.ReadLine();
            while (strLine != null)
            {
                //Debug.LogError("strLine " + strLine);
                string[] strs = strLine.Split('=');
                if (strs != null && strs.Length == 2)
                {

                    string resfile = Application.dataPath + "/Resources/" + strs[1].Trim() + ".prefab";
                    if (!File.Exists(resfile))
                    {
                        Debug.LogErrorFormat("prefab is not Exist:{0}", resfile);
                    }
                }
                strLine = sr.ReadLine();
            }
            sr.Close();

        }

        public static  void CreateResourceFile()
        {
            string fileName = UtilsToolsConfig.ResoucesConfigFile;
            //Debug.LogError("ResoucesConfigFile:" + fileName);
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
        }


    }


}
