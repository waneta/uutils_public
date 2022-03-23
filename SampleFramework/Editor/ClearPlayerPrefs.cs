/************************************************************************************
 * @author   wangjian
 * 手动清除Player Preferences  ，在window上Preferences存储在注册表中
************************************************************************************/
using UnityEditor;
using UnityEngine;

namespace SampleFramework.UUtils.Editor
{
    public class ClearPlayerfres : ScriptableObject
    {

        [MenuItem("开发库/UUtils/SampleFramework/Clear All Player Preferences")]
        public static void OnClearButton()
        {
            if (EditorUtility.DisplayDialog("Delete All Player Preferences?", " Are you sure you want to delete all player preferences?", "Yes", "No"))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
        }

    }
}