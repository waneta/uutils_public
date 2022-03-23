using UnityEngine;
using System.Collections;
namespace Client.UUtils.Example
{

	public class Cube_MessengerObject2 : MonoBehaviour {


		void Start () {
			Messenger<int>.AddListener(MessageType.M_EVENT_MESSENGER_UISTATUS_CHANGE,OnMessageType2Change);
		}

		void OnDestroy(){
			Messenger<int>.RemoveListener(MessageType.M_EVENT_MESSENGER_UISTATUS_CHANGE,OnMessageType2Change);
		}

		void OnMessageType2Change(int _status){
			Debug.LogError (gameObject.name + ": OnMessageType1Change status "+_status);
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
