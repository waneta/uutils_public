using Client.UUtils;
using System;
using System.Linq;
using JObject = System.Collections.Generic.Dictionary<string, object>;

namespace SampleFramework.UUtils.Example
{
    public enum GenderEnum
    {
        Male,
        Female,
    }
    public enum UserType
    {
        None,
        Coach,
        Student,
        Demo,
    }

    [Serializable()]
	public class UserBaseInfo:Json_BasicInfo{
        //public string id ="";
        private string _email = "";
        public string email {
            get { return _email.ToLower(); }
            set { _email = value.ToLower(); }
            //get { return _email; }
            //set { _email = value; }
        }

        public string _firstName="";
		public string _lastName="";
        public string firstName
        {
            set
            {
                _firstName = value;
            }
            get
            {
                return _firstName;
            }
        }
        public string lastName
        {
            set
            {
                _lastName = value;
            }
            get
            {
                //return Utils.ProcessStrIndexUpper(_lastName);
                return _lastName;
            }
        }

		public GenderEnum gender=GenderEnum.Male;

		public int firstCode=-1;
		public string country="";
		public int secondCode=-1;
		public string province="";
		public int thirdCode=-1;
		public string city="";

		public string detailAddress="";
		public string postCode="";
		public string phone="";
		public string facebook="";
		public string twitter="";



        private string clubType = "Iron";
        private bool graspClubStyle = true; //true  （右撇子）   false （左撇子）
        private bool unitForm = true;  //身体数据采用的单位，公制或英制，Metric Or Britich, true is Metric false is britich

        //20150525 1758 ADD fzl

        private string _coachID = "";
        public string coachID {
            //set { _coachID = value.ToLower(); }
            //get { return _coachID.ToLower(); }
            set { _coachID = value; }
            get { return _coachID; }
        }


        public UserType userType = UserType.None;
        
      

        public string userAge="";
        public string userHeight="";
        public string userWeight="";
        public string userHDCP="";

        public int userAgeEnum = 0;
        public int userHeightEnum = 0;
        public int userWeightEnum = 0;
        public int userHDCPEnum = 0;


        //2015 1230 1720 在基本用户中添加用户名和密码输入框
        public string userName="";
        public string passWord="";

        //20160912新增字段，Biom 选项，
        public bool isUseBiom = false;//仅对教练有意义
        public int PelvicPivotAxisEnum = 0;
        public int TrailArmActionEnum = 0;
        public int PosturalReleaseEnum = 0;
        public int LeverDeliveryActionEnum = 0;
        public int DownmPlaneEnum = 0;
        public int HipDifferentialEnum = 0;



        /// <summary>
        /// 返回用户的名字,首先LastName, firstName,email
        /// </summary>
        /// <returns></returns>
        public string UserSimpleName{ 
            get{
                if(lastName!=""){
                   return lastName;
                }
                else if(firstName!=""){
                   return firstName;
                }
                else if(email!=""){
                    if(email.Contains('@')){
                       return email.Split('@')[0];
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 返回用户的名字,首先 firstName,lastName,email
        /// </summary>
        /// <returns></returns>
        public string UserSimpleName_FirstName
        {
            get
            {
                string _f = ""; string _s = "";
                getCurrentLanguageNameOrder(ref _f, ref _s);
                if (_f != "")
                {
                    return _f;
                }
                else if (_s!= "")
                {
                    return _s;
                }
                else if (email != "")
                {
                    if (email.Contains('@'))
                    {
                        return email.Split('@')[0];
                    }
                }
                return "";
            }
        }
        /// <summary>
        /// 获取名字的首字母，兼容中英文
        /// </summary>
        /// <returns></returns>
        public string GetNameType() {
            //return Utils.GetSpellCode(UserSimpleName_FirstName);
            return UserSimpleName_FirstName;
        }

        private void getCurrentLanguageNameOrder(ref string _f,ref string _s) {
            /*_if (LocalLanguage.instance.CurrentLanguage == EnumLanguage.Chinese)
            {
                _f = lastName;
                _s = firstName;
            }
            else if (LocalLanguage.instance.CurrentLanguage == EnumLanguage.English)
            {
                _f = firstName;
                _s = lastName;
            }*/
        }


        /// <summary>
        /// 不同的语言环境，名字排序不同，此处作统一处理，原则上以前显示的名字应该从此处获取
        /// 存储含义：firstname 名; lastName 姓氏
        /// </summary>
        /// <returns></returns>
        public string getCurrentLanguageShowName() {
            string _f=""; string _s="";
            getCurrentLanguageNameOrder(ref _f, ref _s);
            if (string.IsNullOrEmpty(_f))
            {
                return _s;
            }
            else
            {
                return _f + " " + _s;
            }
        }



        public string club_Type
        {
            set
            {
                clubType = value;
            }
            get
            {
                return clubType;
            }
        }

        public bool grasp_ClubeStyle
        {
            set
            {
                graspClubStyle = value;
            }
            get
            {
                return graspClubStyle;
            }
        }
        public bool unit_Form
        {
            set
            {
                unitForm = value;
            }
            get
            {
                return unitForm;
            }
        }


		public SyncVersion syncVersion;
		public UserBaseInfo(){
			//id =  Utils.GenerateId();
			syncVersion = new SyncVersion ();
		}
		public UserBaseInfo(JObject json, int vercode){
			Deserialize(json, vercode);
		}

		public override JObject Serialize (int vercode)
		{
			JObject json = new JObject();
			JsonHelper.Set (json, "syncVersion", syncVersion.Serialize(vercode));
			//JsonHelper.SetString(json,"id",id);

			JsonHelper.SetString(json,"email",email);
			JsonHelper.SetString(json,"firstName",firstName);
			JsonHelper.SetString(json,"lastName",lastName);
			JsonHelper.SetInteger(json,"gender",(int)gender);


			JsonHelper.SetInteger(json,"firstCode",firstCode);
			JsonHelper.SetString(json,"country",country);
			JsonHelper.SetInteger(json,"secondCode",secondCode);
			JsonHelper.SetString(json,"province",province);
			JsonHelper.SetInteger(json,"thirdCode",thirdCode);
			JsonHelper.SetString(json,"city",city);

			JsonHelper.SetString(json,"detailAddress",detailAddress);
			JsonHelper.SetString(json,"postCode",postCode);
			JsonHelper.SetString(json,"phone",phone);
			JsonHelper.SetString(json,"facebook",facebook);
			JsonHelper.SetString(json,"twitter",twitter);

            JsonHelper.SetString(json, "clubType", clubType);
            JsonHelper.SetBoolean(json, "graspHandStyle", graspClubStyle);
			JsonHelper.SetBoolean(json, "unitForm", unitForm);

            //20150525 1758 ADD fzl
            JsonHelper.SetInteger(json, "userType", (int)userType);
            JsonHelper.SetString(json, "coachID", coachID);
            
           //201505280903 ADD
           //JsonHelper.SetInteger(json,"userInfoState",(int)userInfoState); 
           //JsonHelper.SetInteger(json, "userAgeGroup", (int)userAgeGroup);
           //JsonHelper.SetInteger(json, "userHeightGroup", (int)userHeightGroup);
           //JsonHelper.SetInteger(json, "userweightGroup", (int)userWeightGroup);
           //JsonHelper.SetInteger(json, "userHDCPGroup", (int)userHDCPGroup);

            JsonHelper.SetString(json, "userAge", userAge);
            JsonHelper.SetString(json, "userHeight", userHeight);
            JsonHelper.SetString(json, "userWeight", userWeight);
            JsonHelper.SetString(json, "userHDCP", userHDCP);

            JsonHelper.SetString(json,"userName",userName);
            JsonHelper.SetString(json, "passWord", passWord);

            JsonHelper.SetInteger(json, "userAgeEnum", userAgeEnum);
            JsonHelper.SetInteger(json, "userHeightEnum", userHeightEnum);
            JsonHelper.SetInteger(json, "userWeightEnum", userWeightEnum);
            JsonHelper.SetInteger(json, "userHDCPEnum", userHDCPEnum);

            JsonHelper.SetBoolean(json, "isUseBiom", isUseBiom);

            JsonHelper.SetInteger(json, "TrailArmActionEnum", TrailArmActionEnum);
            JsonHelper.SetInteger(json, "PelvicPivotAxisEnum", PelvicPivotAxisEnum);
            JsonHelper.SetInteger(json, "PosturalReleaseEnum", PosturalReleaseEnum);
            JsonHelper.SetInteger(json, "LeverDeliveryActionEnum", LeverDeliveryActionEnum);
            JsonHelper.SetInteger(json, "DownmPlaneEnum", DownmPlaneEnum);
            JsonHelper.SetInteger(json, "HipDifferentialEnum", HipDifferentialEnum);

			return json;
		}
		
		public override void Deserialize (JObject json,int vercode)
		{

            if(json==null) return;
			syncVersion = new SyncVersion (JsonHelper.Get<JObject>(json,"syncVersion"), vercode); 
//			id = JsonHelper.GetString(json,"id");
			email = JsonHelper.GetString(json,"email");
			firstName = JsonHelper.GetString(json,"firstName");
			lastName = JsonHelper.GetString(json,"lastName");
			gender = (GenderEnum)JsonHelper.GetInteger(json,"gender");

			firstCode = JsonHelper.GetInteger(json,"firstCode");
			country = JsonHelper.GetString(json,"country");
			secondCode = JsonHelper.GetInteger(json,"secondCode");
			province = JsonHelper.GetString(json,"province");
			thirdCode = JsonHelper.GetInteger(json,"thirdCode");
			city = JsonHelper.GetString(json,"city");

			detailAddress = JsonHelper.GetString(json,"detailAddress");
			postCode = JsonHelper.GetString(json,"postCode");
			phone = JsonHelper.GetString(json,"phone");
			facebook = JsonHelper.GetString(json,"facebook");
			twitter = JsonHelper.GetString(json,"twitter");


            clubType = JsonHelper.GetString(json, "clubType");
            graspClubStyle = JsonHelper.GetBoolean(json, "graspHandStyle");
            unitForm = JsonHelper.GetBoolean(json, "unitForm");

            userType = (UserType)JsonHelper.GetInteger(json, "userType");
            coachID = JsonHelper.GetString(json, "coachID");

            //userAgeGroup=(UserAgeGroup)JsonHelper.GetInteger(json,"userAgeGroup");
            //userHeightGroup = (UserHeightGroup)JsonHelper.GetInteger(json, "userHeightGroup");
            //userWeightGroup = (UserWeightGroup)JsonHelper.GetInteger(json, "userWeightGroup");
            //userHDCPGroup = (UserHDCPGroup)JsonHelper.GetInteger(json, "userHDCPGroup");


            userAge =JsonHelper.GetString(json,"userAge");
            userHeight = JsonHelper.GetString(json, "userHeight");
            userWeight = JsonHelper.GetString(json, "userWeight");
            userHDCP = JsonHelper.GetString(json, "userHDCP");


 
            //201505280903 ADD
           // userInfoState=(UserInfoState)JsonHelper.GetInteger(json,"userInfoState");


           userName=JsonHelper.GetString(json,"userName");
           passWord=JsonHelper.GetString(json,"passWord");


           userAgeEnum = JsonHelper.GetInteger(json, "userAgeEnum");
           userHeightEnum = JsonHelper.GetInteger(json, "userHeightEnum");
           userWeightEnum = JsonHelper.GetInteger(json, "userWeightEnum");
           userHDCPEnum = JsonHelper.GetInteger(json, "userHDCPEnum");

            //20160912
            isUseBiom = JsonHelper.GetBoolean(json, "isUseBiom");
            PelvicPivotAxisEnum = JsonHelper.GetInteger(json, "PelvicPivotAxisEnum");
            TrailArmActionEnum = JsonHelper.GetInteger(json, "TrailArmActionEnum");
            PosturalReleaseEnum = JsonHelper.GetInteger(json, "PosturalReleaseEnum");
            LeverDeliveryActionEnum = JsonHelper.GetInteger(json, "LeverDeliveryActionEnum");
            DownmPlaneEnum = JsonHelper.GetInteger(json, "DownmPlaneEnum");
            HipDifferentialEnum = JsonHelper.GetInteger(json, "HipDifferentialEnum");
		}

	}


}
