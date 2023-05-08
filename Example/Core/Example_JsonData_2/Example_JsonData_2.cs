/*
 *	MiniJSON 基本使用2
 */
using System.IO;
using UnityEngine;
using JObject = System.Collections.Generic.Dictionary<string, object>;


namespace Client.UUtils.Example
{

    public class Ex1_UserInfo:Json_BasicInfo{

		public float head=18;
        public int userid = 0;
		public Ex1_UserInfo()
       	{

		}
		public Ex1_UserInfo(JObject json, int vercode){
			Deserialize(json, vercode);
		}
		public override JObject Serialize (int vercode)
		{
			JObject json = new JObject();
			JsonHelper.SetFloat(json,"head",head);
            JsonHelper.SetInteger(json, "userid", userid);
			return  json;
		}

		public override void Deserialize (JObject json,int vercode)
		{
            if(json==null) return;
			head = JsonHelper.GetFloat(json,"head");
            userid = JsonHelper.GetInteger(json, "userid");
		}

	}

    
    public class Example_JsonData_2 : MonoBehaviour
    {
        public Ex1_UserInfo usersData;
		const string FOLDERNAME = "Example_JsonData_2";
        public const string KEY_DATA_VERSION_CODE = "DataVersionCode";
        public const string KEY_DATA_VERSION_NAME = "DataVersionName";

        public const string KEY_SKINCOLOR_VALUE = "SkinColorValue";


        public const string VERSION_NAME_INIT = "0.0.0000";
        int DataVersionCode = 1;
        void OnGUI() {
            if (GUILayout.Button("保存json数据到文件")) {
                usersData = new Ex1_UserInfo();
				usersData.head = float.NaN;
                usersData.userid = 3;
				JObject json_data = usersData.Serialize(DataVersionCode);
				var str = Json.Serialize(json_data);
				Debug.LogError("str: "+str);

                SaveUserData();

            }
			if(GUILayout.Button("读取json文件")){
				Ex1_UserInfo usersData = ReadUsersData();
				Debug.LogError ("userid: " + usersData.userid+"head:"+ usersData.head);
			}

			if(GUILayout.Button("异步读取json")){
				AsyncReadUsersData(OnUsersDataReaderEventStatus);

			}


        }

        public bool WriteUsersData(Ex1_UserInfo data, string fileName)
        {
			string encryptFile = string.Format("{0}/{1}.ts", Application.dataPath+"/Example/UUtils/Core/"+FOLDERNAME, fileName);
            JObject json_data = data.Serialize(DataVersionCode);
            FileManager.Instance.WriteEncryptZipJsonObject(encryptFile, json_data);
            return true;
        }


        public void SaveUserData()
        {
            WriteUsersData(usersData, "userinfo");
        }

		public Ex1_UserInfo ReadUsersData()
		{
			JObject json_data = null;

			//检查基本数据文件是否存在，并尝试解析
			string normalFile = string.Format("{0}/{1}.txt", Application.dataPath+"/Example/UUtils/Core/"+FOLDERNAME, "userinfo");
			string encryptFile = string.Format("{0}/{1}.ts", Application.dataPath+"/Example/UUtils/Core/"+FOLDERNAME, "userinfo");
			#if !CLIENT_WEB
			if (File.Exists(normalFile))
			{
				json_data = FileManager.Instance.ReadJsonObject(normalFile);
			}
			else
			{
				//不存在基本数据文件，则尝试以加密方式解析
				json_data = FileManager.Instance.ReadEncryptZipJsonObject(encryptFile);
			}
			#endif
			if (json_data != null)
			{
				//检查数据文件版本号，如果低于当前系统版本，则升级重写
				int vercode = JsonHelper.GetInteger(json_data, KEY_DATA_VERSION_CODE);

				if (vercode !=DataVersionCode)
				{
					if (json_data.ContainsKey(KEY_DATA_VERSION_CODE))
					{
						json_data.Remove(KEY_DATA_VERSION_CODE);
					}
					json_data.Add(KEY_DATA_VERSION_CODE, DataVersionCode);
					#if !CLIENT_WEB
					FileManager.Instance.WriteEncryptZipJsonObject(encryptFile, json_data);

					if (File.Exists(normalFile))
					{
						File.Delete(normalFile);
					}
					#endif
				}
				return new Ex1_UserInfo(json_data, vercode);
			}
			return null;
		}

		/// <summary>
		/// 异步读取文件
		/// </summary>
		/// <param name="configData"></param>
		/// <returns></returns>
		public void AsyncReadUsersData(FileReadTaskEventHandler handler)
		{
			//检查基本数据文件是否存在，并尝试解析
			string normalFile = string.Format("{0}/{1}.txt", Application.dataPath+"/Example/UUtils/Core/"+FOLDERNAME, "userinfo");
			string encryptFile = string.Format("{0}/{1}.ts", Application.dataPath+"/Example/UUtils/Core/"+FOLDERNAME, "userinfo");
			if (File.Exists(normalFile))
			{
				//检查普通格式文件是否存在，并进行解码
				FileManager.Instance.AsyncReadJsonObject(normalFile, handler);
			}
			else
			{
				//检查是否存在加密数据文件，并进行解码
				FileManager.Instance.AsyncReadEncryptZipJsonObject(encryptFile, handler);
			}
		}

		public void OnUsersDataReaderEventStatus(string fname, FileDataType dtype, FileTaskStatus status, object data)
		{
			//检查是否当前文件

			{
				switch (status)
				{
				case FileTaskStatus.Progressing:
					{
						break;
					}
				case FileTaskStatus.Success:
					{
						
						{
							JObject jdata = data as JObject;
							int vercode = JsonHelper.GetInteger(jdata, KEY_DATA_VERSION_CODE);	
							Ex1_UserInfo _usersData = new Ex1_UserInfo(jdata, vercode);

							//对旧数据整体迁移加密之后回存；
							if (vercode != DataVersionCode)
							{

								fname = string.Format("{0}/{1}.txt", Application.dataPath+"/Example/UUtils/Core/"+FOLDERNAME, "userinfo");
								if (File.Exists(fname))
								{
									File.Delete(fname);
								}

								fname  = string.Format("{0}/{1}.ts", Application.dataPath+"/Example/UUtils/Core/"+FOLDERNAME, "userinfo");


								FileManager.Instance.AsyncWriteEncryptZipJsonObject(fname, _usersData.Serialize(DataVersionCode), null);
							}
							Debug.LogError("_usersData:"+_usersData.userid);
							//ApplicationUI.Instance.closeWindowObject(WINDOWOBJECT_TYPE.PROGRESSWINDOW, SHOW_EFFECT.NONE, true);


						}

						break;
					}
				case FileTaskStatus.Failed:
					{
						//*_*ApplicationUI.Instance.closeWindowObject(WINDOWOBJECT_TYPE.PROGRESSWINDOW, SHOW_EFFECT.NONE, true);
						//*_*PromptManager.Instance.ShowPrompt(5019, null, null, null);
						break;
					}
				}
			}
		}


		/*
		#region save Mapping
		public HitConfigData ReadHitHistoryConfig(string dirname)
		{
			JObject json_data = null;

			//检查基本数据文件是否存在，并尝试解析
			string normalFile = string.Format("{0}/{1}/{2}.txt", CommonVariable.PersistentHitDataPath, dirname, CommonVariable.SwingMappingFileName);
			string encryptFile = string.Format("{0}/{1}/{2}.ts", CommonVariable.PersistentHitDataPath, dirname, CommonVariable.SwingMappingFileName);
			if (File.Exists(normalFile))
			{
				json_data = FileManager.Instance.ReadJsonObject(normalFile);
			}
			else
			{
				//不存在基本数据文件，则尝试以加密方式解析
				json_data = FileManager.Instance.ReadEncryptZipJsonObject(encryptFile);
			}

			if (json_data != null)
			{
				//检查数据文件版本号，如果低于当前系统版本，则升级重写
				int vercode = JsonHelper.GetInteger(json_data, ClientConfig.KEY_DATA_VERSION_CODE);

				if (vercode != ClientConfig.DataVersionCode)
				{
					if (json_data.ContainsKey(ClientConfig.KEY_DATA_VERSION_CODE))
					{
						json_data.Remove(ClientConfig.KEY_DATA_VERSION_CODE);
					}
					json_data.Add(ClientConfig.KEY_DATA_VERSION_CODE, ClientConfig.DataVersionCode);

					FileManager.Instance.WriteEncryptZipJsonObject(encryptFile, json_data);

					if (File.Exists(normalFile))
					{
						File.Delete(normalFile);
					}
				}
				return new HitConfigData(json_data, vercode);
			}

			return null;

		}

		public HitConfigData ReadAllHitHistoryConfigByDir(string dirname)
		{
			JObject json_data = null;

			//检查基本数据文件是否存在，并尝试解析
			string normalFile = string.Format("{0}/{1}.txt", dirname, CommonVariable.SwingMappingFileName);
			string encryptFile = string.Format("{0}/{1}.ts", dirname, CommonVariable.SwingMappingFileName);
			if (File.Exists(normalFile))
			{
				json_data = FileManager.Instance.ReadJsonObject(normalFile);
			}
			else
			{
				//不存在基本数据文件，则尝试以加密方式解析
				json_data = FileManager.Instance.ReadEncryptZipJsonObject(encryptFile);
			}

			if (json_data != null)
			{
				//检查数据文件版本号，如果低于当前系统版本，则升级重写
				int vercode = JsonHelper.GetInteger(json_data, ClientConfig.KEY_DATA_VERSION_CODE);

				if (vercode != ClientConfig.DataVersionCode)
				{
					if (json_data.ContainsKey(ClientConfig.KEY_DATA_VERSION_CODE))
					{
						json_data.Remove(ClientConfig.KEY_DATA_VERSION_CODE);
					}
					json_data.Add(ClientConfig.KEY_DATA_VERSION_CODE, ClientConfig.DataVersionCode);

					FileManager.Instance.WriteEncryptZipJsonObject(encryptFile, json_data);

					if (File.Exists(normalFile))
					{
						File.Delete(normalFile);
					}
				}
				return new HitConfigData(json_data, vercode);
			}

			return null;

		}

		public HitConfigData ReadBlankHistoryConfig(string dirname)
		{
			JObject json_data = null;

			//检查基本数据文件是否存在，并尝试解析
			string normalFile = string.Format("{0}/{1}/{2}.txt", CommonVariable.PersistentBlankDataPath, dirname, CommonVariable.BlankMappingFileName);
			string encryptFile = string.Format("{0}/{1}/{2}.ts", CommonVariable.PersistentBlankDataPath, dirname, CommonVariable.BlankMappingFileName);
			if (File.Exists(normalFile))
			{
				json_data = FileManager.Instance.ReadJsonObject(normalFile);
			}
			else
			{
				//不存在基本数据文件，则尝试以加密方式解析
				json_data = FileManager.Instance.ReadEncryptZipJsonObject(encryptFile);
			}

			if (json_data != null)
			{
				//检查数据文件版本号，如果低于当前系统版本，则升级重写
				int vercode = JsonHelper.GetInteger(json_data, ClientConfig.KEY_DATA_VERSION_CODE);

				if (vercode != ClientConfig.DataVersionCode)
				{
					if (json_data.ContainsKey(ClientConfig.KEY_DATA_VERSION_CODE))
					{
						json_data.Remove(ClientConfig.KEY_DATA_VERSION_CODE);
					}
					json_data.Add(ClientConfig.KEY_DATA_VERSION_CODE, ClientConfig.DataVersionCode);

					FileManager.Instance.WriteEncryptZipJsonObject(encryptFile, json_data);

					if (File.Exists(normalFile))
					{
						File.Delete(normalFile);
					}
				}
				return new HitConfigData(json_data, vercode);
			}

			return null;

		}

		public bool WriteSwingMapping(HitConfigData data)
		{
			string encryptFile = data.getEncryptSwingMappingFileName();
			JObject json_data = data.Serialize(ClientConfig.DataVersionCode);
			bool result = FileManager.Instance.WriteEncryptZipJsonObject(encryptFile, json_data);
			if (result)
			{
				UpdateHitConfigData(data);
			}
			return result;
		}
		public bool WriteBlankSwingMapping(HitConfigData data)
		{
			string encryptFile = data.getEncryptBlankMappingFileName();
			JObject json_data = data.Serialize(ClientConfig.DataVersionCode);
			bool result = FileManager.Instance.WriteEncryptZipJsonObject(encryptFile, json_data);
			if (result)
			{

				Blank_UpdateHitConfigData(data);
			}
			return result;
		}

		#endregion

		#region SwingClub Data
		//one club data
		public SwingClubData ReadSwingClubData(HitConfigData configData)
		{
			JObject json_data = null;

			//检查基本数据文件是否存在，并尝试解析
			string normalFile = configData.GetSwingDataFileName();
			string encryptFile = configData.GetEncryptSwingDataFileName();
			if (File.Exists(normalFile))
			{
				json_data = FileManager.Instance.ReadJsonObject(normalFile);
			}
			else
			{
				//不存在基本数据文件，则尝试以加密方式解析
				json_data = FileManager.Instance.ReadEncryptZipJsonObject(encryptFile);
			}

			if (json_data != null)
			{
				//检查数据文件版本号，如果低于当前系统版本，则升级重写
				int vercode = JsonHelper.GetInteger(json_data, ClientConfig.KEY_DATA_VERSION_CODE);

				if (vercode != ClientConfig.DataVersionCode)
				{
					if (json_data.ContainsKey(ClientConfig.KEY_DATA_VERSION_CODE))
					{
						json_data.Remove(ClientConfig.KEY_DATA_VERSION_CODE);
					}
					json_data.Add(ClientConfig.KEY_DATA_VERSION_CODE, ClientConfig.DataVersionCode);

					FileManager.Instance.WriteEncryptZipJsonObject(encryptFile, json_data);

					if (File.Exists(normalFile))
					{
						File.Delete(normalFile);
					}
				}
				return new SwingClubData(json_data, vercode);
			}

			return null;
		}

		/// <summary>
		/// 异步读取文件
		/// </summary>
		/// <param name="configData"></param>
		/// <returns></returns>
		public void AsyncReadSwingClubData(HitConfigData configData, FileReadTaskEventHandler handler)
		{
			string filename = configData.GetSwingDataFileName();
			if (File.Exists(filename))
			{
				//检查普通格式文件是否存在，并进行解码
				FileManager.Instance.AsyncReadJsonObject(filename, handler);
			}
			else
			{
				//检查是否存在加密数据文件，并进行解码
				filename = configData.GetEncryptSwingDataFileName();
				FileManager.Instance.AsyncReadEncryptZipJsonObject(filename, handler);
			}
		}

		public void AsyncReadBlankData(HitConfigData configData, FileReadTaskEventHandler handler)
		{
			string filename = configData.GetBlankDataFileName();
			if (File.Exists(filename))
			{
				//检查普通格式文件是否存在，并进行解码
				FileManager.Instance.AsyncReadJsonObject(filename, handler);
			}
			else
			{
				//检查是否存在加密数据文件，并进行解码
				filename = configData.GetEncryptBlankDataFileName();
				FileManager.Instance.AsyncReadEncryptZipJsonObject(filename, handler);
			}
		}

		/// <summary>
		/// 该函数创建于20150525 1546 fzl
		/// 函数主体复制于  ReadSwingClubData(string fileName)，主要功能是为了从Hd文件读取字符串数据，
		/// 然后把数据传给上传服务器
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public string ReadSwingClubDataFromFile(HitConfigData configData)
		{
			SwingClubData data = ReadSwingClubData(configData);

			if (data != null)
			{
				return Json.Serialize(data);
			}

			return null;
		}

		public bool WriteSwingClubData(SwingClubData data, HitConfigData _hitConfigData)
		{
			string encryptFile = _hitConfigData.GetEncryptSwingDataFileName();

			JObject json_data = data.Serialize(ClientConfig.DataVersionCode);

			bool result = FileManager.Instance.WriteEncryptZipJsonObject(encryptFile, json_data);

			return result;
		}

		/// <summary>
		/// 异步保存数据文件
		/// </summary>
		public HitConfigData AsyncSaveSwingClubData(List<HitData> hitBufferDatas, Dictionary<HitState, int> hitStates, UserInfo userInfo, string title, string desc, string max_club_angle_speed, string max_thorax_turn_angle, string max_club_head_speed, FileWriteTaskEventHandler handler)
		{
			string fileName;

			//创建m数据文件对象
			SwingClubData _mClubData = new SwingClubData();

			//保存hitdata数据
			_mClubData.hitData = hitBufferDatas;

			//保存peaks数据
			_mClubData.peakData.Clear();
			for (int s = 0; s < Enum.GetValues(typeof(HitState)).Length; s++)
			{
				if (hitStates.ContainsKey((HitState)s))
				{
					_mClubData.peakData.Add(hitStates[(HitState)s]);
				}
				else
				{
					_mClubData.peakData.Add(-1);
				}
			}

			//保存用户信息
			UserInfo _cloneUser = FileSave.Copy<UserInfo>(userInfo);//深度复制，该方法未经过测试，录制数据时进行测试。
			_cloneUser.bagInfo.DeleteUnengagedClubs();
			_mClubData.userInfo = _cloneUser;

			//创建hitconfigdata
			HitConfigData _hitConfigData = new HitConfigData();
			_hitConfigData.currentSwingType =UI.Pad.OpenDataType.Swing;
			_hitConfigData.time = DateTime.Now;
			_hitConfigData.mDataCreateTime = Utils.DateTimeToUnixTimestamp(_hitConfigData.time.ToUniversalTime()); //新加字段，记录Java格式是标准时间戳，仅用于网络同步
			_hitConfigData.userInfo = _cloneUser;

			//保存mData 数据时，防止出现coachID 为空的现象，进行再次
			if (string.IsNullOrEmpty(_hitConfigData.userInfo.userBaseInfo.coachID)) {
				_hitConfigData.userInfo.userBaseInfo.coachID = DataManager.Instance.CurrentUser.userBaseInfo.email;
			}
			_hitConfigData.time_zone = (float)System.TimeZone.CurrentTimeZone.GetUtcOffset(System.DateTime.Now).TotalMinutes/ 60 ;
			//SwingData 描述
			_hitConfigData.mDescTitle = title;
			_hitConfigData.mDesc = desc;

			//保存m数据version；
			_hitConfigData.mVersion = _mClubData.syncVersion;

			//保存当前使用的雷达类型， ///-----------------
			if (RadarMonitor.Instance.radarRealData != null&&RadarMonitor.Instance.isGetRealData)
			{
				_hitConfigData.radarType = RadarMonitor.Instance.radarRealData.radarType;
			}


			//异步保存m数据文件；
			fileName = _hitConfigData.GetEncryptSwingDataFileName();
			FileManager.Instance.AsyncWriteEncryptZipJsonObject(fileName, _mClubData.Serialize(ClientConfig.DataVersionCode), handler);

			//[{"name":"angle_speed", "value":"180" ,"unit":"deg/s" },{"name":"turn_angle", "value":"50" ,"unit":"°" },{"name":"speed", "value":"1190" ,"unit":"" }]
			//Swing 评级指标
			SwingGrade _mGrade = new SwingGrade();
			SwingGradeObject _mGradeObject = new SwingGradeObject("angle_speed", max_club_angle_speed, "deg/s");
			_mGrade.gradeList.Add(_mGradeObject);
			SwingGradeObject _mGradeObject1 = new SwingGradeObject("turn_angle", max_thorax_turn_angle, "deg");
			_mGrade.gradeList.Add(_mGradeObject1);
			SwingGradeObject _mGradeObject2 = new SwingGradeObject("speed", max_club_head_speed, "km/hr");
			_mGrade.gradeList.Add(_mGradeObject2);

			_hitConfigData.mGrade = _mGrade;

			//默认无需生成新的SwingComment,第一次使用时，创建
			SwingComment _mComment = new SwingComment();
			_hitConfigData.mCommentVersion = _mComment.syncVersion;

			//异步保存标注文件
			fileName = _hitConfigData.GetEncryptCommentFileName();
			FileManager.Instance.AsyncWriteEncryptZipJsonObject(fileName, _mComment.Serialize(ClientConfig.DataVersionCode), handler);

			//大图快照内容
			SnapShotData _snapShotData = new SnapShotData();
			_hitConfigData.snapShotVersion = _snapShotData.syncVersion;
			_hitConfigData.snapShotVersionList = _snapShotData.snapShotList.Select(f => f.syncVersion).ToList();

			//异步保存快照文件
			WriteSnapShotData(_snapShotData, _hitConfigData);
			fileName = _hitConfigData.GetEncryptSnapshotFileName();
			FileManager.Instance.AsyncWriteEncryptZipJsonObject(fileName, _snapShotData.Serialize(ClientConfig.DataVersionCode), handler);

			//异步保存m的综述文件
			fileName = _hitConfigData.getEncryptSwingMappingFileName();
			FileManager.Instance.AsyncWriteEncryptZipJsonObject(fileName, _hitConfigData.Serialize(ClientConfig.DataVersionCode), handler);

			UpdateHitConfigData(_hitConfigData);
			return _hitConfigData;
		}


		#endregion*/



		/*public void OnSwingClubDataReaderEventStatus(string fname, FileDataType dtype, FileTaskStatus status, object data)
		{
			//检查是否当前文件
			if (currentPeolpe.CurrentHitConfigData != null)
			{
				switch (status)
				{
				case FileTaskStatus.Progressing:
					{
						break;
					}
				case FileTaskStatus.Success:
					{
						if (fname.Equals(currentPeolpe.CurrentHitConfigData.GetSwingDataFileName()) || fname.Equals(currentPeolpe.CurrentHitConfigData.GetEncryptSwingDataFileName()))
						{
							JObject jdata = data as JObject;
							int vercode = JsonHelper.GetInteger(jdata, ClientConfig.KEY_DATA_VERSION_CODE);
							SwingClubData mClubData = new SwingClubData(jdata, vercode);

							//对旧数据整体迁移加密之后回存；
							if (vercode != ClientConfig.DataVersionCode)
							{

								fname = currentPeolpe.CurrentHitConfigData.GetSwingDataFileName();
								if (File.Exists(fname))
								{
									File.Delete(fname);
								}

								fname = currentPeolpe.CurrentHitConfigData.GetEncryptSwingDataFileName();


								FileManager.Instance.AsyncWriteEncryptZipJsonObject(fname, mClubData.Serialize(ClientConfig.DataVersionCode), null);
							}

							ApplicationUI.Instance.closeWindowObject(WINDOWOBJECT_TYPE.PROGRESSWINDOW, SHOW_EFFECT.NONE, true);

							if (Base.It.ConfigData.enableAdjustImpactHandData)
							{
								try
								{
									mClubData.AdjustImpactHandData();
								}
								catch (System.Exception e1)
								{
								}

							}
							//将hitdata数据下标映射成index和time
							for (int s = 0; s < mClubData.hitData.Count; s++)
							{
								mClubData.hitData[s].totalIndex = s;
								mClubData.hitData[s].time = s * 1.0f / (Base.It.ConfigData.BaseFrameCountPerSecond * Base.It.ConfigData.insertFrameCount);
							}
							//foreach (var hd in mClubData.hitData)
							//{
							//    Debug.LogErrorFormat("{0},{1},{2}"
							//        , hd.skeletonsData[(int)SkeletonType.Club].rotation.x
							//        , hd.skeletonsData[(int)SkeletonType.Club].rotation.y
							//        , hd.skeletonsData[(int)SkeletonType.Club].rotation.z);
							//}
							PlaySwingClubData(mClubData);
							currentPeolpe.PlayerTimePoint = PlayTimePoint.LoadDataSuccess;
							MainUIContext.Instance.LastOpenHitConfigData = currentPeolpe.CurrentHitConfigData;
							//*_*在review，compare 界面，当读取一个新的数据时，需要根据当前播放数据中左右手握杆方式，进行相应的调整
							SettingControlProcess.Instance.RefreshSettingOptionByHistoryData(mClubData.userInfo);
							//if (isSaveAndReview) //如果是从save and Review 跳转的，直接播放
							//{
							//    PeopleMag.CurrentPeolpe.SetHeadTransparentCube();
							//    PeopleMag.CurrentPeolpe.SetBodyPlanesTransform();
							//    PeopleMag.Instance.Play(0);  // 20161107 新需求，录制完成后，跳转到Review页面，新的不再进行自动播放：
							//    isSaveAndReview = false;
							//}
						}

						break;
					}
				case FileTaskStatus.Failed:
					{
						ApplicationUI.Instance.closeWindowObject(WINDOWOBJECT_TYPE.PROGRESSWINDOW, SHOW_EFFECT.NONE, true);
						PromptManager.Instance.ShowPrompt(5019, null, null, null);
						break;
					}
				}
			}
		}*/

    }
}
