using UnityEngine;
using System.Collections;

namespace Client.UUtils.Example
{

    public class Example_CRLF : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            string crlf_file = "file:///"+ Application.dataPath + "/Example/UUtils/Example_CRLF/" + "persistens_CRLF.txt";
            //string lf_file = "file:///"+Application.dataPath + "/Example/UUtils/Example_CRLF/" + "persistens_LF.txt";

            StartCoroutine(loadfile(crlf_file));
            Debug.LogError("============================");

            //StartCoroutine(loadfile(lf_file));
            //Debug.LogError("============================");

        }

        IEnumerator loadfile(string configfile)
        {
            Debug.LogError("configfile " + configfile);
            using (WWW w = new WWW(configfile))
            {
                yield return w;
                Debug.LogError("error:"+ w.error);
                if (string.IsNullOrEmpty(w.error))
                {

                    string str = w.text;
                    string[] array = str.Split(new char[] { '\r','\n' },System.StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < array.Length; i++)
                    {
                        string s = array[i];
                        Debug.LogError(s);
                    }
                }
            }
        }
    }
}
