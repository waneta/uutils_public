using UnityEngine;

namespace Client.UUtils.Example
{

    public class MessageType
    {
        public const string M_EVENT_MESSENGER_TYPE1 = "m_event_message_type1";
        public const string M_EVENT_MESSENGER_UISTATUS_CHANGE = "m_event_message_uistatus_change";
        public const string M_EVENT_MESSENGER_TYPE3 = "m_event_message_type3";
    }

    public class Message3Object
    {
        public int type = 0;
        public string content = "";
    }

    public class Example_Messager : MonoBehaviour
    {
        private int _uiStatus = 0;
        public int UIStatus
        {
            get
            {
                return _uiStatus;
            }
            set
            {
                if (_uiStatus != value)
                {
                    _uiStatus = value;
                    Messenger<int>.Broadcast(MessageType.M_EVENT_MESSENGER_UISTATUS_CHANGE, _uiStatus);
                }
            }
        }



        void OnGUI()
        {
            //只是广播事件，不带参
            if (GUILayout.Button("Send Message1"))
            {
                Messenger.Broadcast(MessageType.M_EVENT_MESSENGER_TYPE1);
            }

            //广播基本数据类型
            if (GUILayout.Button("Send Message2"))
            {
                UIStatus = Random.Range(0, 100);

            }

            //广播对象
            if (GUILayout.Button("Send Message3"))
            {
                Message3Object o = new Message3Object();
                o.type = 1;
                o.content = "Hello World";
                Messenger<Message3Object>.Broadcast(MessageType.M_EVENT_MESSENGER_TYPE3, o);
            }


        }


    }
}
