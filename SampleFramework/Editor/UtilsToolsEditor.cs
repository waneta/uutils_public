/************************************************************************************
 * @author   wangjian
 * 创建新项目  UUtils 扩展代码文件
************************************************************************************/
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Client.UUtils;
using System.Text.RegularExpressions;
using System;
using SampleFramework.UUtils;
using SampleFramework.UUtils.Editor;

namespace Client.UUtils.Editor
{
    
    
    public class UtilsToolsEditor : EditorWindow
    {
        [MenuItem("开发库/UUtils/UtilsTools")]
        static void OpenUUtilsToolWindow()
        {
            //创建窗口
            Rect wr = new Rect(0, 0, 500, 550);
            UtilsToolsEditor window = (UtilsToolsEditor)EditorWindow.GetWindowWithRect(typeof(UtilsToolsEditor), wr, true, "UUtils Tools");
            window.Show();
        }

        static string projectName = "";
        

        //[MenuItem("开发库/PS/UUtils/生成新项目框架代码")]
        public static void OnClickNewProjectFileButton()
        {
            if (!File.Exists(CommonVariable.ClientInfoFile))
            {
                if (EditorUtility.DisplayDialog("配置新项目", "确定生成新项目[ " + projectName + " ]所需文件?", "是", "否"))
                {
                    AssemblyProject();
                    AssetDatabase.Refresh();
                }
                return;
            }
            ClientInfo _clientInfo = ClientInfo.Deserialize(CommonVariable.ClientInfoFile);
            if (projectName.Equals(_clientInfo.ProductName))
            {
                if (EditorUtility.DisplayDialog("配置项目", "当前项目[ " + projectName + " ]已经配置！！！是否重新导入配置?", "是", "否"))
                {
                    AssemblyProject();
                    AssetDatabase.Refresh();
                }
            }
            else
            {
                if (EditorUtility.DisplayDialog("配置新项目", "确定生成新项目[ " + projectName + " ]所需文件?", "是", "否"))
                {
                    AssemblyProject();
                    AssetDatabase.Refresh();
                }
            }



        }
        public static void OnClickClearProjectFileButton()
        {
            ClientInfo _clientInfo = ClientInfo.Deserialize(CommonVariable.ClientInfoFile);
            if (EditorUtility.DisplayDialog("清除当前项目的配置", "确定清除当前项目[ " + _clientInfo.ProductName + " ]所有文件?", "是", "否"))
            {
                
                string path = GetCodeDestPath(_clientInfo.ProductName);
                Debug.LogError("path "+path);
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (dirInfo.Exists)
                {
                    Debug.LogError("path2 " + path);
                    dirInfo.Delete(true);
                }
                    
                AssetDatabase.Refresh();
            }
        }
        //配置（装配）项目
        static void AssemblyProject()
        {
            //Debug.LogError("AssemblyProject "+ projectName);
            //初始化路径
            UtilsToolsConfig.SetProductName(projectName);
            //1
            StartCopyCodeFile();
            //2
            CreateClientInfo();
            //3
            DirectoryInfo directory = new DirectoryInfo(UtilsToolsConfig.ResoucesConfigPath);
            if (!directory.Exists)
                directory.Create();
            CheckResourceConfig.CreateResourceFile();
            CreateCopylistFile.StartConfigProjectStreamFile();
            

        }
        static void CreateClientInfo()
        {
            ClientInfo config = new ClientInfo();
            config.ProductName = projectName;
            config.ShowProductName = projectName;
            config.AppId = Utils.GenerateId();
            string fileName = UtilsToolsConfig.ClientInfoFile;
            config.Serialize(fileName);
        }

        static void StartCopyCodeFile()
        {
            string destDir = GetCodeDestPath();
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);
            //1.组件信息
            File.Copy(UtilsToolsConfig.AssemblyInfoFilePath, FileHelper.FormatPath(UtilsToolsConfig.DestCodePath+"/"+ UtilsToolsConfig.AssemblyFileName));
            //2.代码文件列表
            List<string> persistentsList = GetCopyFile(UtilsToolsConfig.SourceCodePath);
            //OutFile(persistentsList, UtilsToolsConfig.codeListName);

            for (int i = 0; i < persistentsList.Count; i++)
            {
                string filepath = FileHelper.FormatPath(UtilsToolsConfig.SourceCodePath + persistentsList[i]);

                byte[] buffer = FileHelper.GetNormalBuffer(filepath);
                string str = System.Text.Encoding.UTF8.GetString(buffer);
                string result = "";
                result = str.Replace("namespace Client.UUtils.Editor", string.Format("namespace {0}.UUtils.Editor", projectName));

                result = result.Replace("namespace SampleFramework.UUtils", string.Format("namespace {0}.UUtils", projectName));
                result = result.Replace("[MenuItem(\"开发库/UUtils/SampleFramework", string.Format("[MenuItem(\"开发库/{0}/UUtils", projectName));
                FileHelper.CreateFile(FileHelper.FormatPath(UtilsToolsConfig.DestCodePath + persistentsList[i]), result);

            }
        }
        static string GetCodeDestPath(string _projectName)
        {
            return Application.dataPath + "/Scripts/" + _projectName + "/UUtils";

        }
        static string GetCodeDestPath()
        {
            //destPath = Application.dataPath + "/Scripts/" + projectName + "/UUtils";
            return UtilsToolsConfig.DestCodePath;
        }
        static string GetSourcePath()
        {
            return UtilsToolsConfig.SourceCodePath;
        }



        private string detailsContent;
        bool projectname_valid = false;
        void OnGUI()
        {
            projectName = EditorGUILayout.TextField("项目名：", projectName);
            EditorGUILayout.LabelField("命名规则：大小写字母，下划线，数字 例如：GAR、hub_2、hello");
            

            if (GUILayout.Button("预览详情", GUILayout.Width(200)))
            {
                NameValidCheckType reuslt = CheckProjectNameValid(projectName);
                UtilsToolsConfig.SetProductName(projectName);
                if (reuslt == NameValidCheckType.Valid)
                {
                    
                    detailsContent = "==============1. 复制项目UUtils Code================\n";
                    detailsContent += "源文件列表:\n";
                    detailsContent += GetCopyFileOutString(UtilsToolsConfig.SourceCodePath);
                    detailsContent += "目标路径:\n";
                    string[] splitstrarry = GetCodeDestPath().Split(new string[] { Application.dataPath }, StringSplitOptions.RemoveEmptyEntries);
                    detailsContent += splitstrarry[0];


                    detailsContent += "\n==============2.  创建客户端版本信息配置文件================\n";

                    splitstrarry = UtilsToolsConfig.ClientInfoFile.Split(new string[] { Application.dataPath }, StringSplitOptions.RemoveEmptyEntries);
                    if(splitstrarry!=null && splitstrarry.Length>0)
                        detailsContent += "路径:" + splitstrarry[0];

                    detailsContent += "\n==============3.  创建客户端streamingAssets资源目录=========\n";
                    
                    splitstrarry = UtilsToolsConfig.StreamingAssetsPath.Split(new string[] { Application.dataPath }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitstrarry != null && splitstrarry.Length > 0)
                        detailsContent += "路径:" + splitstrarry[0];

                    splitstrarry = UtilsToolsConfig.ResoucesConfigFile.Split(new string[] { Application.dataPath }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitstrarry != null && splitstrarry.Length > 0)
                        detailsContent += "\n路径:" + splitstrarry[0];

                    splitstrarry = UtilsToolsConfig.PersistensListFile.Split(new string[] { Application.dataPath }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitstrarry != null && splitstrarry.Length > 0)
                        detailsContent += "\n路径:" + splitstrarry[0];

                    splitstrarry = UtilsToolsConfig.ConfigListFilePath.Split(new string[] { Application.dataPath }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitstrarry != null && splitstrarry.Length > 0)
                        detailsContent += "\n路径:" + splitstrarry[0];

                    splitstrarry = UtilsToolsConfig.PrivateListFilePath.Split(new string[] { Application.dataPath }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitstrarry != null && splitstrarry.Length > 0)
                        detailsContent += "\n路径:" + splitstrarry[0];



                    projectname_valid = true;
                }
                else
                {
                    this.ShowNotification(new GUIContent("项目名为空或项目名格式不对"));
                    projectname_valid = false;
                }
            }

            if(projectname_valid)
                EditorGUILayout.TextArea(detailsContent, GUILayout.Width(490), GUILayout.Height(390));

            
            if (GUILayout.Button("配置项目", GUILayout.Width(200)))
            {
                NameValidCheckType reuslt = CheckProjectNameValid(projectName);
                if (reuslt == NameValidCheckType.None)
                {
                    //打开一个通知栏
                    this.ShowNotification(new GUIContent("项目名不能为空"));
                }
                else if(reuslt == NameValidCheckType.Invalid)
                {
                    //打开一个通知栏
                    this.ShowNotification(new GUIContent("项目名格式不对"));
                }else if(reuslt == NameValidCheckType.Valid)
                {
                    //打开一个通知栏
                    this.ShowNotification(new GUIContent("项目名格式正确"));
                    projectname_valid = true;
                }

                if (projectname_valid)
                {
                    OnClickNewProjectFileButton();
                }
                
            }

            if (GUILayout.Button("清除当前项目配置", GUILayout.Width(200)))
            {
                if (File.Exists(CommonVariable.ClientInfoFile))
                {
                    OnClickClearProjectFileButton();
                }
                else
                {
                    this.ShowNotification(new GUIContent("info.ini 不存在"));
                }
                
            }

             if (GUILayout.Button("关闭窗口", GUILayout.Width(200)))
            {
                //关闭窗口
                this.Close();
            }
        }
        enum NameValidCheckType
        {
            None,//空
            Invalid,//无效
            Valid,//符合要求，有效
        }
        static NameValidCheckType CheckProjectNameValid(string _projectName)
        {
            NameValidCheckType result = NameValidCheckType.None;
            if (string.IsNullOrEmpty(_projectName))
            {
                return NameValidCheckType.None;
            }
            else
            {

                string pattern = @"[a-zA-Z]{1,}[0-9_]{0,}";
                Regex r = new Regex(pattern); // 定义一个Regex对象实例

                Match m = r.Match(_projectName);
                if (m.Success)
                {
                    result = NameValidCheckType.Valid;
                }
                else
                {
                    result = NameValidCheckType.Invalid;
                }
            }
            return result;
        }
        static string GetCopyFileOutString(string _codePath)
        {
            List<string> list = GetCopyFile(_codePath);
            string str = "";
            for(int i = 0; i < list.Count; i++)
            {
                string filepath = FileHelper.FormatPath(UtilsToolsConfig.SourceCodePath + list[i]);
                string[] sp = filepath.Split(new string[] { Application.dataPath }, StringSplitOptions.RemoveEmptyEntries);
                if(sp!=null && sp.Length > 0)
                {
                    str += sp[0] + "\n";
                }
                
            }
            return str;
        }
        static List<string> GetCopyFile(string codePath)
        {
            //数据文件列表
            List<string> persistentsList = new List<string>();
            GetFilePath(codePath, "*.*", ref persistentsList);
            //OutFile(persistentsList, UtilsToolsConfig.codeListName);

            List<string> _list = new List<string>();
            for (int i = 0; i < persistentsList.Count; i++)
            {
                if (persistentsList[i].Contains("UtilsToolsEditor"))
                {
                    continue;
                };
                if (persistentsList[i].Contains("UtilsToolsConfig"))
                {
                    continue;
                };

                _list.Add(persistentsList[i]);
            }

            return _list;
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
            StreamWriter writer = new StreamWriter(UtilsToolsConfig.SourceCodePath + "/" + fileName);
            writer.Write(str);
            writer.Close();
        }


        public static void GetFilePath(string folderName, string type, ref List<string> fileList)
        {
            folderName = FileHelper.FormatPath(folderName);
            //Debug.Log("folderName==" + folderName);

            if (!Directory.Exists(folderName)) return;
            string[] files = Directory.GetFiles(folderName, type);
            foreach (string str in files)
            {
                if (str.EndsWith(".txt") || str.EndsWith(".sk") || str.EndsWith(".ts") || str.EndsWith(".hs") || str.EndsWith(".hd") || str.EndsWith(".xml") || str.EndsWith(".jpg") || str.EndsWith(".html") || str.EndsWith(".js") || str.EndsWith(".css") || str.EndsWith(".png")
                    || str.EndsWith(".TTF") || str.EndsWith(".cs"))
                {

                    fileList.Add(str.Replace(UtilsToolsConfig.SourceCodePath, ""));
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