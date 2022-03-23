/************************************************************************************
 * @author   wangjian
 * ResourceManager的使用 资源加载例子
************************************************************************************/
using UnityEngine;
using Client.UUtils;

namespace SampleFramework.UUtils.Example
{
    public class Example_Resources : MonoBehaviour
    {

        void Start()
        {
            UDebug.SetLogLevel(UDebug.LOGLEVEL.INFO);
            ClientInfo _clientInfo = ClientInfo.Deserialize(Application.streamingAssetsPath + "/info.ini");
            //注意调用数据读取和资源加载接口需要设置公司名称和产品名称，因为路径配置包含两个目录结构
            CommonVariable.SetCompanyProductName(_clientInfo.CompanyName, _clientInfo.ProductName);
        }

        void OnGUI()
        {
            if (GUILayout.Button("Instantiate Cube"))
            {
                GameObject cubeobject = ResourceManager.Instance.InstantiateGameObjectFromResourceByType("demo1_cube");
                if (cubeobject != null)
                {
                    cubeobject.transform.position = Vector3.zero;
                }
            }
        }


    }
}
