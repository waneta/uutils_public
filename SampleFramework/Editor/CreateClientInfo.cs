/************************************************************************************
 * @author   wangjian
 * 
************************************************************************************/
using Client.UUtils.Editor;
using UnityEditor;
using UnityEngine;

namespace SampleFramework.UUtils.Editor
{
    public class CreateClientInfo : ScriptableObject
    {

        [MenuItem("开发库/UUtils/SampleFramework/1.Create Client Info")]
        public static void OnClearButton()
        {
            if (EditorUtility.DisplayDialog("Create Client Info?", " Create Client Info?", "Yes", "No"))
            {
                ClientInfo config = new ClientInfo();

                string fileName = UtilsToolsConfig.ClientInfoFile;
;
                config.Serialize(fileName);
            }
        }

    }
}