namespace QFramework
{
    public partial class ConfigManager : ViewController, ISingleton
    {

        public static ConfigManager Default => MonoSingletonProperty<ConfigManager>.Instance; // 获取 ConfigManager 的单例实例

        public void OnSingletonInit()
        {
        }
    }
}
