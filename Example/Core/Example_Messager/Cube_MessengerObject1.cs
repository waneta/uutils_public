using UnityEngine;
using System.Collections;
namespace Client.UUtils.Example
{

    public class Cube_MessengerObject1 : MonoBehaviour
    {


        void Start()
        {
            Messenger.AddListener(MessageType.M_EVENT_MESSENGER_TYPE1, OnMessageType1Change);
        }

        void OnDestroy()
        {
            Messenger.RemoveListener(MessageType.M_EVENT_MESSENGER_TYPE1, OnMessageType1Change);
        }

        void OnMessageType1Change()
        {
            Debug.LogError(gameObject.name + ": OnMessageType1Change");
            Common.StartCoroutine(SetCubeColor());
        }
        IEnumerator SetCubeColor()
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponent<MeshRenderer>().material.color = Color.gray;
            yield break;

        }

    }
}
