using Client.UUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using JObject = System.Collections.Generic.Dictionary<string, object>;

namespace SampleFramework.UUtils.Example
{
    [Serializable()]
	public class UsersData :Json_BasicInfo
    {

		private string currentUserID;
        public bool syncUserData = false;  //新加字段，标识该用户数据是否已经与网络同步
		public string CurrentUserID
      	{
			set
            {
				currentUserID= value;
                _currentUser = GetUserInfoByID(currentUserID);
			}
			get{
				return currentUserID;
			}
		}

		private string defalutUserID;
		public string DefalutUserID
       {
			set
           {
				defalutUserID = value;
			}
			get
           {
				return defalutUserID;
			}
		}
		private string currentApplyUserID;
		public string CurrentApplyUserID{
			set{
				currentApplyUserID = value;
                _currentApplyUser = GetUserInfoByID(currentApplyUserID);
			}
			get{
				return currentApplyUserID;
			}
		}
  
    public List<UserInfo> userList;

		public UsersData()
        {

			userList = new List<UserInfo>();
		}

		public UsersData(JObject json, int vercode){

			Deserialize(json,vercode);
		}

        public override JObject Serialize(int vercode)
      {
			JObject json = new JObject();
			JsonHelper.SetString(json,"currentUserID",currentUserID);
			//JsonHelper.SetString(json,"currentApplyUserID",currentUserID);
            JsonHelper.SetString(json, "currentApplyUserID", currentApplyUserID);
			List<JObject> userlist = userList.Select(c => c.Serialize(vercode)).ToList();
			JsonHelper.Set(json, "userList", userlist);
            JsonHelper.SetBoolean(json, "syncUserData", syncUserData);
			return json;
		}

        public override void Deserialize(JObject json, int vercode)
		{
            if(json==null) return;
			List<object> _List = JsonHelper.Get<List<object>>(json, "userList");
			userList  = _List.Select(o => new UserInfo(o as JObject,vercode)).ToList();

            CurrentUserID = JsonHelper.GetString(json, "currentUserID");
            CurrentApplyUserID = JsonHelper.GetString(json, "currentApplyUserID");
            syncUserData = JsonHelper.GetBoolean(json, "syncUserData");
		}


		#region add ,del, edit ... method

		public bool isExitUser(string key){
		      for(int i=0; i<userList.Count; i++){
					if(userList[i].userBaseInfo.email == key){
						return true;
		        }
		      }
			return false;
	    }
        public bool isExitActiveUser(string key)
        {
            for (int i = 0; i < userList.Count; i++)
            {
                if (userList[i].userBaseInfo.email == key && userList[i].syncVersion.state!=SyncDataProcesState.DELETE)
                {
                    return true;
                }
            }
            return false;
        }
		public void AddUser(UserInfo info){
			if(!isExitUser(info.userBaseInfo.email)){
				userList.Add(info);
			}else{
				EidtUser(info);
			}

		}
        public void AddDemoUser(UserInfo info)
        {
            if (!isExitUser(info.userBaseInfo.email))
            {
                userList.Add(info);
            }
        }



		public void DelUser(UserInfo info){
			for(int i=0; i<userList.Count; i++){
				if(userList[i].userBaseInfo.email == info.userBaseInfo.email){
					userList[i] = info;
					userList[i].syncVersion.state = SyncDataProcesState.DELETE;
					break;
				}
			}
           // userList.Remove(info);//原先的逻辑，直接删除用户列表，由于增加了网络功能，20150528 1025 此处逻辑更换为把该条信息置换为 Delete
         //info.userBaseInfo.userInfoState=UserInfoState.Delete;  

           //学生列表的学生被标记，删除，那么在历数据面板中的历史数据也应该作相应的标记。
           /* List<HitConfigData> hitConfigDatas = DataManager.Instance.hitHistoryConfig.historyData;
            for(int i=0;i<hitConfigDatas.Count;i++)
            {
               if(hitConfigDatas[i].dataInfoState!=DataInfoState.Detele&&hitConfigDatas[i].userInfo.userBaseInfo.coachID==DataManager.Instance.usersData.CurrentUser.userBaseInfo.email)
               {
                    if(hitConfigDatas[i].userInfo.userBaseInfo.email==info.userBaseInfo.email)
                    {
                        hitConfigDatas[i].dataInfoState=DataInfoState.Detele;
                    }
               }
            }*////
            ////GameObject.Find("LoginUploadScripts").GetComponent<LoginUploadScripts>().UpdateUI();
           //// DataManager.Instance.SaveSwingHistoryConfig();
            
		}
        
		public void EidtUser(UserInfo info)
        {

			for(int i=0; i<userList.Count; i++){
				if(userList[i].userBaseInfo.email == info.userBaseInfo.email){
					userList[i] = info;
					break;
				}
			}
		}

		public UserInfo GetUserInfoByID(string email){
			for(int i=0; i<userList.Count; i++){
				if(userList[i].userBaseInfo.email.Equals(email)){
					return userList[i];
				}
			}
			return null;
		}

        /// <summary>
        /// 缓存当前用户，提高性能；
        /// </summary>
        private UserInfo _currentUser;

		public UserInfo CurrentUser
        {
			get
            {
                return _currentUser;
			}
		}

        /// <summary>
        /// 缓存当前用户，提高性能；
        /// </summary>
        private UserInfo _currentApplyUser;

		public UserInfo CurrentApplyUser{
			get{

                if (_currentApplyUser!=null){
                    return _currentApplyUser;
                }
                else{
                    return _currentUser;
                }
                
			}
            set{_currentApplyUser=value;}
		}
		#endregion



	}

    
}
