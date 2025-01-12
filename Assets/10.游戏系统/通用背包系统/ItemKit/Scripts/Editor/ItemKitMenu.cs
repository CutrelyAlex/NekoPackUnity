using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


namespace QFramework
{
    public class ItemKitMenu : MonoBehaviour
    {
        [MenuItem("NekoPack/ItemKit/Create Code")]
        public static void CreateCode()
        {
            ItemConfig[] itemConfigs = AssetDatabase.FindAssets($"t:{nameof(ItemConfig)}")
                                                    // ��guidת��Ϊ·��
                                                    .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                                                    // ��·��������Դ
                                                    .Select(assetPath => AssetDatabase.LoadAssetAtPath<ItemConfig>(assetPath))
                                                    .ToArray();

            string filePath = "Assets/10.��Ϸϵͳ/ͨ�ñ���ϵͳ/ItemKit/Examples/Items.cs";
            RootCode rootCode = new(); // Qframework�Ĵ������ɹ���

            rootCode.Using("UnityEngine")
                    .Using("Qframework")
                    .EmptyLine()
                    .Namespace("Qframework.Example",ns =>
                    {
                        ns.Class("Items", string.Empty, false, false, c =>
                        {
                            foreach(var itemConfig in itemConfigs)
                            {
                                c.Custom($"public static string {itemConfig.name} = \"{itemConfig.Key}\";");
                            }
                        });
                    });
            StringBuilder stringBudiler = new();
            StringCodeWriter writer = new(stringBudiler);
            rootCode.Gen(writer);

            System.IO.File.WriteAllText(filePath, stringBudiler.ToString());

            stringBudiler.ToString().LogInfo();
        }
        
    }
}
            
