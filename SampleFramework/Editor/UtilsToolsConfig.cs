using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Client.UUtils.Editor
{
    public class UtilsToolsConfig
    {
        private static string ProductName = "SampleFramework";                //产品名称
        public const string CodeListName = "codes.txt";
        public const string AssemblyFileName = "Assembly.xml";
        public static void SetProductName(string _productName)
        {
            ProductName = _productName;
        }
        public static string AssemblyInfoFilePath
        {
            get
            {
                return Application.dataPath + "/Addons/UUtils/Assembly.xml";
            }

        }
        //UUtils 拷贝代码路径
        private static string _sourceCodePath;
        public static string SourceCodePath
        {
            get
            {
                return Application.dataPath + "/Addons/UUtils/SampleFramework";
            }

        }
        //UUtils 拷贝代码目标路径
        private static string _destCodePath;
        public static string DestCodePath
        {
            get
            {
                _destCodePath = string.Format("{0}/Scripts/{1}/{2}", Application.dataPath, ProductName, "UUtils");
                return _destCodePath;
            }
        }

        //产品对应streamingAssetsPath
        private static string _streamingAssetsPath;
        public static string StreamingAssetsPath
        {
            get
            {

                _streamingAssetsPath = FileHelper.FormatPath(string.Format("{0}/{1}", Application.streamingAssetsPath, ProductName));

                return _streamingAssetsPath;
            }
        }

        //资源路径
        private static string _resoucesConfigPath;
        public static string ResoucesConfigPath
        {
            get
            {
                _resoucesConfigPath = FileHelper.FormatPath(string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, ProductName, CommonVariable.ConfigDirectory));

                return _resoucesConfigPath;
            }
        }

        private static string _resoucesConfigFile;
        public static string ResoucesConfigFile
        {
            get
            {
                _resoucesConfigFile = FileHelper.FormatPath(string.Format("{0}/{1}", ResoucesConfigPath, CommonVariable.ResourcesPathConfigFileName));

                return _resoucesConfigFile;
            }
        }

        private static string _clientInfoFile;
        public static string ClientInfoFile
        {
            get
            {
                _clientInfoFile = FileHelper.FormatPath(string.Format("{0}/{1}", Application.streamingAssetsPath, CommonVariable.ClientInfoFileName));
                return _clientInfoFile;
            }
        }


        //包含需要复制的数据文件列表的配置文件路径
        public static string PersistensListFile
        {
            get
            {
                return FileHelper.FormatPath(string.Format("{0}/{1}", Application.streamingAssetsPath, CommonVariable.persistensListFileName));
                
            }
        }
        //包含需要复制的配置文件列表 的配置文件路径
        public static string ConfigListFilePath
        {
            get
            {
                return FileHelper.FormatPath(string.Format("{0}/{1}", Application.streamingAssetsPath, CommonVariable.configListFileName));

            }
        }
        //包含需要复制的私密文件列表的配置文件路径
        public static string PrivateListFilePath
        {
            get
            {
                return FileHelper.FormatPath(string.Format("{0}/{1}", Application.streamingAssetsPath, CommonVariable.privateListFileName));
            }
        }
    }
}
