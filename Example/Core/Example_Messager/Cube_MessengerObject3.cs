using UnityEngine;
using System.Collections;
namespace Client.UUtils.Example
{

	public class Cube_MessengerObject3 : MonoBehaviour {


		void Start () {
			Messenger<Message3Object>.AddListener(MessageType.M_EVENT_MESSENGER_TYPE3,OnMessageType3Change);
		}

		void OnDestroy(){
			Messenger<Message3Object>.RemoveListener(MessageType.M_EVENT_MESSENGER_TYPE3,OnMessageType3Change);
		}

		void OnMessageType3Change(Message3Object _message3Object){
			Debug.LogError (gameObject.name + ": OnMessageType3Change  "+ _message3Object.content);
			Common.StartCoroutine(SetCubeColor());
		}

		IEnumerator SetCubeColor(){
			gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
			yield return new WaitForSeconds(0.5f);
			gameObject.GetComponent<MeshRenderer>().material.color = Color.gray;
			yield break;

		}

	}
}
