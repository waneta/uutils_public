/************************************************************************************
 * @author   wangjian
 * UpdateComponentManager使用例子
************************************************************************************/
using System.Collections;
using UnityEngine;

namespace Client.UUtils.Example
{
    public class Example_Timer : MonoBehaviour
    {

        void Start()
        {
            UDebug.SetLogLevel(UDebug.LOGLEVEL.INFO);
        }

        //要想UpdateComponentManager有效必须实现这一部分代码，全局只需要一个Update（其他MonoBehaviour不必实现这部分代码）
        float realTime;
        void Update()
        {
            float deltaTime = Time.deltaTime;
            float fixedDeltaTime = Time.fixedDeltaTime;
            float nowTime = Time.realtimeSinceStartup;
            float realDeltaTime = nowTime - realTime;
            realTime = nowTime;
            UpdateComponentManager.Instance.update(deltaTime, fixedDeltaTime, realDeltaTime);
        }

        public GameObject cube1;
        public Cube_Timer2 cube_timer2;

        public Cube_Timer3 cube_timer3;
        public Cube_Timer4 cube_timer4;
        public GameObject cube5;

        void OnGUI()
        {
            if (GUILayout.Button("Destroy Cube1"))
            {
                TimeCallManager.Instance.addGameTimeCall(3, new TimeCall.OnTimeCallOver(OnDestroyCube1CallBack), cube1);
            }

            if (GUILayout.Button("Destroy Cube2"))
            {
                TimeCallManager.Instance.addGameTimeCall(5, new TimeCall.OnTimeCallOver(OnDestroyCube2CallBack), cube_timer2);
            }

            if (GUILayout.Button("Destroy Cube3,4,5"))
            {
                object[] _array = new object[3];// list also
                _array[0] = cube_timer3 as object;
                _array[1] = cube_timer4 as object;
                _array[2] = cube5;

                TimeCallManager.Instance.addTimeCall(3, new TimeCall.OnTimeCallOver(OnDestroyMoreCallBack), _array);
            }

        }
        IEnumerator OnDestroyCube1CallBack(object o)
        {
            GameObject _cube = o as GameObject;
            Destroy(_cube.gameObject);
            UDebug.Log("OnDestroyCube1CallBack");
            yield break;
        }

        IEnumerator OnDestroyCube2CallBack(object o)
        {
            Cube_Timer2 _cube_timer2 = o as Cube_Timer2;
            Destroy(_cube_timer2.gameObject);
            UDebug.Log("OnDestroyCube2CallBack");
            yield break;
        }

        IEnumerator OnDestroyMoreCallBack(object o)
        {
            object[] _array = o as object[];
            Destroy((_array[0] as Cube_Timer3).gameObject);
            Destroy((_array[1] as Cube_Timer4).gameObject);
            Destroy(_array[2] as GameObject);

            UDebug.Log("OnDestroyMoreCallBack");
            yield break;
        }



    }
}
