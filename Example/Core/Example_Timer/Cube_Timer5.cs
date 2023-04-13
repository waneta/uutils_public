using UnityEngine;
using System.Collections;
namespace Client.UUtils.Example
{
    public class Cube_Timer5 : MonoBehaviour, UpdateComponent
    {

        // Use this for initialization
        void Start()
        {
            UpdateComponentManager.Instance.addUpdateComponent(this, UPDATE_SPACE.MINUTE);
        }
        public void update(float deltaTime, float fixedDeltaTime, float realDeltaTime)
        {
            UDebug.Log("{0}:deltaTime:{1} fixedDeltaTime:{2} realDeltaTime:{3}", gameObject.name, deltaTime, fixedDeltaTime, realDeltaTime);
        }
        void OnDestroy()
        {

        }

    }
}
