using Client.UUtils;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace SampleFramework.UUtils.Example
{
    public class Example_xml : MonoBehaviour
    {

        void OnGUI()
        {
            if (GUILayout.Button("保存Xml"))
            {
                ExWindowConfig config = new ExWindowConfig();
                config.Nodes.Add(new ExWindowNode("Main Window", "主要界面"));
                config.Nodes.Add(new ExWindowNode("Config Window", "配置界面"));

                ExWindowConfig.Save(config, Application.dataPath + "/Example/UUtils/Example2_XmlFile/Example2_XmlFile/exwindowconfig.xml");
            }

            if (GUILayout.Button("读取Xml"))
            {

                ExWindowConfig config = ExWindowConfig.Read(Application.dataPath + "/Example/UUtils/Example2_XmlFile/Example2_XmlFile/exwindowconfig.xml");
                if (config != null)
                {
                    foreach (ExWindowNode _node in config.Nodes)
                    {
                        Debug.LogErrorFormat(" type:{0} note:{1}", _node.type, _node.note);
                    }
                }
            }

            if (GUILayout.Button("保存文件"))
            {

                string fileName = Application.dataPath + "/Example/UUtils/Example2_XmlFile/Example2_XmlFile/testfile1.txt";
                FileHelper.WriteFile(fileName, System.Text.Encoding.Default.GetBytes("文件写入"));


            }
            if (GUILayout.Button("读取文件"))
            {

                string fileName = Application.dataPath + "/Example/UUtils/Example2_XmlFile/Example2_XmlFile/testfile1.txt";
                Byte[] buffer = FileHelper.GetNormalBuffer(fileName);
                if (buffer != null && buffer.Length > 0)
                {
                    Debug.LogError(" data :" + System.Text.Encoding.Default.GetString(buffer));
                }

            }

            if (GUILayout.Button("保存ClientConfig.Xml"))
            {
                ClientInfo config = new ClientInfo();

                string fileName = Application.dataPath + "/Example/UUtils/Example2_XmlFile/Example2_XmlFile/info.ini";
                config.Serialize(fileName);
            }

            if (GUILayout.Button("读取ClientConfig.Xml"))
            {
                ClientInfo config = new ClientInfo();
                string fileName = Application.dataPath + "/Example/UUtils/Example2_XmlFile/Example2_XmlFile/info.ini";
                config = ClientInfo.Deserialize(fileName);
                Debug.LogError("config: " + config.ProductName);
            }


        }
    }



    [Serializable()]
    public class ExWindowConfig
    {

        public List<ExWindowNode> Nodes = new List<ExWindowNode>();

        public static ExWindowConfig Read(string fileName)
        {
            return FileHelper.XMLToObject<ExWindowConfig>(fileName);
        }

        public static void Save(ExWindowConfig data, string fileName)
        {
            FileHelper.ObjectToXML<ExWindowConfig>(data, fileName);

        }
    }
    [Serializable()]
    public class ExWindowNode
    {
        public string note;         //注释
        public string type;         //窗口类型

        public ExWindowNode() { }
        public ExWindowNode(string _type, string _note = "")
        {
            type = _type;
            note = _note;
        }
    }
}
