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
                                                    // 将guid转换为路径
                                                    .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                                                    // 从路径加载资源
                                                    .Select(assetPath => AssetDatabase.LoadAssetAtPath<ItemConfig>(assetPath))
                                                    .ToArray();

            string filePath = "Assets/10.游戏系统/通用背包系统/ItemKit/Examples/Items.cs";
            RootCode rootCode = new(); // Qframework的代码生成功能

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
            
