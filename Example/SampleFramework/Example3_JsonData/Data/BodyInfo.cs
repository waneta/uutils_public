using Client.UUtils;
using System;
using JObject = System.Collections.Generic.Dictionary<string, object>;

namespace SampleFramework.UUtils.Example
{

    [Serializable()]
    public class BodyInfo : Json_BasicInfo
    {
        public SyncVersion syncVersion;
        public float head = 18;
        public float neck = 9f;
        public float shoulderWidth = 35;
        public float body = 65;

        public float hipWidth = 22f;
        public float foreArm = 28f;
        public float upperArm = 29f;
        public float palm = 19f;

        public float upperLeg = 45;
        public float lowerLeg = 42;
        public float heelHeight = 7.5f;
        public float footLength = 22;

        public float boydHeight = 0.0f;

        public float bodyHeightSkeleton = 0.0f;
        public float bodySwingspanSkeleton = 0.0f;

        public BodyInfo()
        {
            syncVersion = new SyncVersion();
            boydHeight = BodyInfoHeight;
        }
        public float BodyInfoHeight
        {
            get
            {
                return (head + neck + body + upperLeg + lowerLeg + heelHeight + hipWidth / 2);
            }
        }
        public BodyInfo(JObject json, int vercode)
        {
            Deserialize(json, vercode);
        }
        public override JObject Serialize(int vercode)
        {
            JObject json = new JObject();
            if (syncVersion != null)
                JsonHelper.Set(json, "syncVersion", syncVersion.Serialize(vercode));
            JsonHelper.SetFloat(json, "head", head);
            JsonHelper.SetFloat(json, "neck", neck);
            JsonHelper.SetFloat(json, "shoulderWidth", shoulderWidth);
            JsonHelper.SetFloat(json, "body", body);

            JsonHelper.SetFloat(json, "hipWidth", hipWidth);
            JsonHelper.SetFloat(json, "foreArm", foreArm);
            JsonHelper.SetFloat(json, "upperArm", upperArm);
            JsonHelper.SetFloat(json, "palm", palm);

            JsonHelper.SetFloat(json, "upperLeg", upperLeg);
            JsonHelper.SetFloat(json, "lowerLeg", lowerLeg);
            JsonHelper.SetFloat(json, "heelHeight", heelHeight);
            JsonHelper.SetFloat(json, "footLength", footLength);
            JsonHelper.SetFloat(json, "boydHeight", boydHeight);

            JsonHelper.SetFloat(json, "bodyHeightSkeleton", bodyHeightSkeleton);
            JsonHelper.SetFloat(json, "bodySwingspanSkeleton", bodySwingspanSkeleton);
            return json;
        }

        public override void Deserialize(JObject json, int vercode)
        {

            if (json == null) return;
            syncVersion = new SyncVersion(JsonHelper.Get<JObject>(json, "syncVersion"), vercode);
            head = JsonHelper.GetFloat(json, "head");
            neck = JsonHelper.GetFloat(json, "neck");
            shoulderWidth = JsonHelper.GetFloat(json, "shoulderWidth");
            body = JsonHelper.GetFloat(json, "body");

            hipWidth = JsonHelper.GetFloat(json, "hipWidth");
            foreArm = JsonHelper.GetFloat(json, "foreArm");
            upperArm = JsonHelper.GetFloat(json, "upperArm");
            palm = JsonHelper.GetFloat(json, "palm");

            upperLeg = JsonHelper.GetFloat(json, "upperLeg");
            lowerLeg = JsonHelper.GetFloat(json, "lowerLeg");
            heelHeight = JsonHelper.GetFloat(json, "heelHeight");
            footLength = JsonHelper.GetFloat(json, "footLength");

            boydHeight = JsonHelper.GetFloat(json, "boydHeight");

            bodyHeightSkeleton = JsonHelper.GetFloat(json, "bodyHeightSkeleton");
            bodySwingspanSkeleton = JsonHelper.GetFloat(json, "bodySwingspanSkeleton");

        }

    }

}
