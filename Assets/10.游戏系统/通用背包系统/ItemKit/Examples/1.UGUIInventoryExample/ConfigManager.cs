namespace QFramework
{
    public partial class ConfigManager : ViewController, ISingleton
    {

        public static ConfigManager Default => MonoSingletonProperty<ConfigManager>.Instance; // ��ȡ ConfigManager �ĵ���ʵ��

        public void OnSingletonInit()
        {
        }
    }
}
