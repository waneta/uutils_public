/*
 *	MiniJSON 基本使用1
 */

using System.Collections.Generic;
using UnityEngine;

namespace Client.UUtils.Example
{

    public class Example_JsonData_1 : MonoBehaviour
    {

		const string FOLDERNAME = "Example_JsonData_1";
		const string FILENAME = "userinfo";
        void OnGUI() {
            if (GUILayout.Button("保存json数据到文件")) {
                Dictionary<string, object> json_data = new Dictionary<string, object>();
				json_data.Add("headLength",float.NaN);

				var str = Json.Serialize(json_data);
				Debug.LogError("str: "+str);

                WriteJsonData(FileDataType.DataJsonObject,json_data,FILENAME);

            }
			if(GUILayout.Button("读取json文件")){
				ReadJsonData(FileDataType.DataJsonObject,FILENAME);
			}
        }

        public bool WriteJsonData(FileDataType fileDataType, Dictionary<string, object> json_data, string fileName)
        {
			//json对象文件（字符串，utf8）
			if(fileDataType == FileDataType.DataJsonObject){
				string filepath = string.Format("{0}/{1}_Normal.txt", Application.dataPath+ "/Example/UUtils/Core/" + FOLDERNAME, fileName);
            	
				string jstr = Json.Serialize(json_data);
				Debug.LogError("WriteJsonData:"+jstr);
				if (jstr != null)
				{
					return FileManager.Instance.WriteString(filepath, jstr);
				}
				else
				{
					return false;
				}

			}
			//压缩json文件（字符串，utf8）
			else if(fileDataType == FileDataType.DataZipJsonObject){
				string filepath = string.Format("{0}/{1}_Zip.text", Application.dataPath+ "/Example/UUtils/Core/" + FOLDERNAME, fileName);
            	FileManager.Instance.WriteJsonObject(filepath, json_data);

			}
			//加密并压缩json文件（字符串，utf8）
			else if(fileDataType == FileDataType.DataEncryptZipJsonObject){
				string encryptFile = string.Format("{0}/{1}_EncryptZip.ts", Application.dataPath+ "/Example/UUtils/Core/" + FOLDERNAME, fileName);
            	FileManager.Instance.WriteEncryptZipJsonObject(encryptFile, json_data);
			}

            return true;
        }



		public Dictionary<string,object> ReadJsonData(FileDataType fileDataType,string fileName)
		{
			Dictionary<string,object> json_data = null;

			//json对象文件（字符串，utf8）
			if(fileDataType == FileDataType.DataJsonObject){
				string filepath = string.Format("{0}/{1}_Normal.txt", Application.dataPath+ "/Example/UUtils/Core/" + FOLDERNAME, fileName);

				try
				{
					string jstr =  FileManager.Instance.ReadString(filepath);
					Debug.LogError("ReadJsonData:"+jstr);
					if (jstr != null)
					{
						object result = Json.Deserialize(jstr);
						return result as Dictionary<string,object>;
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogError("FileManager ReadJsonObject :" + ex.ToString());
				}
            	

			}
			//压缩json文件（字符串，utf8）
			else if(fileDataType == FileDataType.DataZipJsonObject){
				string filepath = string.Format("{0}/{1}_Zip.text", Application.dataPath+ "/Example/UUtils/Core/" + FOLDERNAME, fileName);
            	json_data = FileManager.Instance.ReadZipJsonObject(filepath);

			}
			//加密并压缩json文件（字符串，utf8）
			else if(fileDataType == FileDataType.DataEncryptZipJsonObject){
				string encryptFile = string.Format("{0}/{1}_EncryptZip.ts", Application.dataPath+ "/Example/UUtils/Core/" + FOLDERNAME, fileName);
            	json_data = FileManager.Instance.ReadEncryptZipJsonObject(encryptFile);
			}

			
			return json_data;
		}

	

    }
}
