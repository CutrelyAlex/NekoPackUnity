using UnityEngine;


namespace QFramework
{
    [CreateAssetMenu(menuName = "@System/ItemKit/ItemConfig")]
    public class ItemConfig : ScriptableObject, IItem
    {
        public string Key;
        public string Name;
        public Sprite Icon;

        public string GetKey => Key;

        public string GetName => Name;

        public Sprite GetIcon => Icon;
    }
}
