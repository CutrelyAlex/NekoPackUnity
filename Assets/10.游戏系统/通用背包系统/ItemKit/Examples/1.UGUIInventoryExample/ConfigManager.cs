

using System;
using UnityEngine;

namespace QFramework.Example
{
    public class ConfigManager
    {
        public static Lazy<IItem> Iron = new(() => Resources.Load<ItemConfig>("Iron"));
        public static Lazy<IItem> Powder = new(() => Resources.Load<ItemConfig>("Powder"));
    }
}