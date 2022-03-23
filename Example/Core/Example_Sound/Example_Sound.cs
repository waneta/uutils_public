using UnityEngine;

namespace Client.UUtils.Example
{

    public class Example_Sound : MonoBehaviour
    {
        void OnGUI() {
            if (GUILayout.Button("播放")) {
                
				SoundManager.Instance.playSound("UUtils/Sound/cheer",Camera.main.gameObject);
            }
        }
    }
}
