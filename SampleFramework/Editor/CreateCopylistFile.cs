/************************************************************************************
 * @author   wangjian
 * 数据文件根目录下的所有文件相对streamingAssetsPath的路径保存在persistens.txt
 * 配置文件根目录下的所有文件相对streamingAssetsPath的路径保存在config.txt
 * 私密文件根目录下的所有文件相对streamingAssetsPath的路径保存在private.txt
 * 保存的格式是 \产品名称\目录\文件名 eg: \u_utils\Private\SecurityKey.sk  注意：EntryFileHandle.cs 会用此处的产品名称修改目标路径
************************************************************************************/
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Client.UUtils;

namespace SampleFramework.UUtils.Editor
{
    public class CreateCopylistFile : ScriptableObject
    {


        [MenuItem("开发库/UUtils/SampleFramework/2.生成程序启动需要复制文件列表")]
        public static void OnClearButton()
        {
            if (EditorUtility.DisplayDialog("生成程序启动所需要复制数据和配置文件列表?", " 生成 streaming 文件列表?", "是", "否"))
            {
                StartConfigProjectStreamFile();


            }
        }

        public static void StartConfigProjectStreamFile()
        {
            ClientInfo _clientInfo = ClientInfo.Deserialize(CommonVariable.ClientInfoFile);
            CommonVariable.SetCompanyProductName(_clientInfo.CompanyName, _clientInfo.ProductName);

            string ProductName = _clientInfo.ProductName;

            //数据文件列表
            List<string> persistentsList = new List<string>();
            GetFilePath(Application.streamingAssetsPath + "/" + ProductName + "/" + CommonVariable.UserDataDirectory, "*.*", ref persistentsList);
            GetFilePath(Application.streamingAssetsPath + "/" + ProductName + "/" + "ActionData", "*.*", ref persistentsList);

            OutFile(persistentsList, CommonVariable.persistensListFileName);

            //配置文件列表
            List<string> configList = new List<string>();
            GetFilePath(Application.streamingAssetsPath + "/" + ProductName + "/" + CommonVariable.ConfigDirectory, "*.*", ref configList);
            OutFile(configList, CommonVariable.configListFileName);

            //私密文件列表
            List<string> privateList = new List<string>();
            GetFilePath(Application.streamingAssetsPath + "/" + ProductName + "/" + CommonVariable.PrivateDirectory, "*.*", ref privateList);
            OutFile(privateList, CommonVariable.privateListFileName);
        }

        public static void OutFile(List<string> fileList, string fileName)
        {
            StringBuilder _stringBuilder = new StringBuilder();
            for (int i = 0; i < fileList.Count; i++)
            {
                string filename = fileList[i].Replace('/', '\\');
                if (i == fileList.Count - 1)
                    _stringBuilder.Append(filename);
                else
                    _stringBuilder.Append(filename + "\n");
            }

            string str = _stringBuilder.ToString();
            StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/" + fileName);
            writer.Write(str);
            writer.Close();
        }


        public static void GetFilePath(string folderName, string type, ref List<string> fileList)
        {

            //Debug.Log("folderName==" + folderName);
            if (!Directory.Exists(folderName)) return;
            string[] files = Directory.GetFiles(folderName, type);
            foreach (string str in files)
            {
                if (str.EndsWith(".txt") || str.EndsWith(".sk") || str.EndsWith(".ts") || str.EndsWith(".hs") || str.EndsWith(".hd") || str.EndsWith(".xml") || str.EndsWith(".jpg") || str.EndsWith(".html") || str.EndsWith(".js") || str.EndsWith(".css") || str.EndsWith(".png")
                    || str.EndsWith(".TTF"))
                {

                    fileList.Add(str.Replace(Application.streamingAssetsPath, ""));
                }
            }
            string[] dirs = Directory.GetDirectories(folderName);

            foreach (string str in dirs)
            {
                //string dirName = Path.GetDirectoryName(str);
                //string dest = destFolder + str.Replace(dirName, "");
                GetFilePath(str, type, ref fileList);
            }

        }


    }
}