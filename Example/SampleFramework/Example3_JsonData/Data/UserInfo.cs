using System;
using JObject = System.Collections.Generic.Dictionary<string, object>;
using Client.UUtils;

namespace SampleFramework.UUtils.Example
{
    [Serializable()]
	public class UserInfo:Json_BasicInfo,ICloneable
   {
        
		public SyncVersion syncVersion;
		public UserBaseInfo userBaseInfo;
		public BodyInfo bodyInfo;
		public object Clone() 
		{ 
			return this.MemberwiseClone(); 
		}
         
      
		public UserInfo()
       {
			syncVersion = new SyncVersion ();
			userBaseInfo = new UserBaseInfo();
			bodyInfo = new BodyInfo();
         
		}
		public UserInfo(JObject json, int vercode)
        {
			Deserialize(json, vercode);
		}



		public override JObject Serialize(int vercode)
       {
			JObject json = new JObject();
			JsonHelper.Set (json, "syncVersion", syncVersion.Serialize(vercode)); 

			JObject _userBaseInfo = userBaseInfo.Serialize(vercode);
			JsonHelper.Set(json,"userBaseInfo",_userBaseInfo);

			JObject _bodyInfo = bodyInfo.Serialize(vercode);
			JsonHelper.Set(json,"bodyInfo",_bodyInfo);
			return json;
		}

		public override void Deserialize (JObject json,int vercode)
		{
            if(json==null) return;
	        syncVersion = new SyncVersion (JsonHelper.Get<JObject>(json,"syncVersion"),vercode);
            
			userBaseInfo = new UserBaseInfo(JsonHelper.Get<JObject>(json,"userBaseInfo"),vercode);

			bodyInfo = new BodyInfo(JsonHelper.Get<JObject>(json,"bodyInfo"),vercode);
		}

		 

	}
}
