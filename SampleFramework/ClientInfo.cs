/************************************************************************************
 * @author   wangjian
 * 客户端信息 eg 软件版本信息，数据版本信息，保存在info.ini中,禁止在运行时修改保存
 * 
 * 命名空间的格式  Client+Utils+产品名称， 比如  Client.UUtils.SampleFramework  产品名称就是SampleFramework
 ************************************************************************************/
using Client.UUtils;
namespace SampleFramework.UUtils
{

    public enum SoftwareMode
    {
        None,
        STANDARD,  //标准产品
    }

    public class ClientInfo
    {

        public string CompanyName = "waneta";					//公司名称
        public string ShowProductName = "SampleFramework";      //对外展示产品名称，比如 安装包名称，桌面图标名称等
        public string ProductName = "SampleFramework";          //产品名称&程序内部使用，多用于路径相关

        public string AppId = "";
        public SoftwareMode SoftwareMode = SoftwareMode.STANDARD;

        //数据版本号，“主版本.子版本.发布日期”
        public string DataVersion = ClientConfig.DataVersion;

        //数据版本号，初始为0，仅当数据结构发生变化时才升级
        public int DataVersionCode = ClientConfig.DataVersionCode;

        public UDebug.LOGLEVEL logLevel = UDebug.LOGLEVEL.INFO;

        public int MajorVersion = 3;        //主版本号，当 API 的兼容性变化时，X 需递增。
        public int MinorVersion = 0;        //次版本号，当增加功能时(不影响 API 的兼容性)，Y 需递增。
        public int Revision = 0;            //修订号，当做Bug修复时(不影响 API 的兼容性)，Z 需递增。

        public string Build_Number = "";    //编译版本号，一般是编译器在编译过程中自动生成的，我们只定义其格式，并不进行人为控制。按照git的commitID前6位，如：8e45cc6f
        public int BatchID = 0;            //执行bat的次数，次数递增
        public bool ReleaseFlag = false;    //是否正式版本

        public void Serialize(string fileName)
        {
            FileHelper.ObjectToXML<ClientInfo>(this, fileName);
        }
        public static ClientInfo Deserialize(string fileName)
        {
            return FileHelper.XMLToObject<ClientInfo>(fileName);
        }
    }
}
