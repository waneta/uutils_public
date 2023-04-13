/************************************************************************************
 * @author   wangjian
 * 场景切换管理
************************************************************************************/
using System.Collections;
using Client.UUtils;

namespace SampleFramework.UUtils
{

    public class World : Singleton<World>, UpdateComponent
    {

        public enum CHANGE_TYPE
        {
            NONE,
            ENTERPC,
            MAIN,
        }
        public enum SceneType
        {
            None,
            Start,
            Main,
        }

        public World()
        {
            UpdateComponentManager.Instance.addAlwayUpdateComponet(this, UPDATE_SPACE.NONE);
        }

        ~World()
        {
        }

        public void update(float deltaTime, float fixedDeltaTime, float realDeltaTime)
        {

            if (isLoadEnd)
            {
                isLoadEnd = false;
                loadMapEnd();
            }
        }

        private int level;
        private string levelName;
        private bool isLoadEnd = false;

        public void SetLoadEnd(int _level)
        {
            level = _level;
            isLoadEnd = true;
        }

        public void startChangeMap(string _levelName)
        {
            levelName = _levelName;
            UDebug.Log("startChangeMap {0}", _levelName);

            if (_levelName == "Start")
            {
                currentScene = SceneType.None;

            }
            else if (_levelName == "Main")
            {
                currentScene = SceneType.Main;
            }
            UDebug.Log("startChangeMap levelName:{0} SceneType :{0}", _levelName, currentScene);
            LoadNorMalSence(levelName);
        }

        void loadMapEnd()
        {
            Common.StartCoroutine(LoadMapEnd_imp());
        }


        SceneType currentScene = SceneType.None;

        //资源参加完成
        IEnumerator AssetsLoadOver()
        {
            UDebug.Log("Assets Load Over");
            //*_*ApplicationUI.Instance.openWindowObject(WINDOWOBJECT_TYPE.IP_MAINUI, SHOW_EFFECT.NONE);
            /*if(ClientConfig.skinType == EnumSkinSetting.Sunshine)
            {
                Debug.LogError(" AssetsLoadOver 2");
                yield return new WaitForSeconds(0.3f);//等待皮肤切换
			}*/

            //*_*ApplicationUI.Instance.closeWindowObject(WINDOWOBJECT_TYPE.PROGRESSWINDOW_LOADASSET, SHOW_EFFECT.NONE,true);
            //ApplicationUI.Instance.init();


            yield break;
        }
        IEnumerator LoadMapEnd_imp()
        {

            if (level == 0)
            {

            }
            else if (level == 1)
            {
#if !CLIENT_WEB

#else
                	//*_*ApplicationUI.Instance.init();
                	//*_*ApplicationUI.Instance.openWindowObject(WINDOWOBJECT_TYPE.IP_MAINUI, SHOW_EFFECT.NONE);
                	//*_*ApplicationUI.Instance.openWindowObject(WINDOWOBJECT_TYPE.PROGRESSWINDOW_START, SHOW_EFFECT.NONE, false, false);

                	WebPlayerPlugin.Instance.WebPlayerInitOver();//通知web js 此场景初始化完成
#endif
            }

            yield break;
        }



        void LoadNorMalSence(string name)
        {
            //show load UI
            //...
            //Test
            //load scene
            Common.StartCoroutine(ResourceManager.Instance.LoadSceneResource(name));
        }



    }
}
