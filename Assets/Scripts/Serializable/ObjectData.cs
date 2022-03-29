using System;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

namespace Ukiyo.Serializable
{
    /// <summary>
    /// Item detail for editor
    /// </summary>
    [CreateAssetMenu(fileName = "Item", menuName = "Ukiyo/New Object Data")]
    public class ObjectData : ScriptableObject, IObjectBase
    {
        public string __id;
        [NonSerialized] protected string _id;
        public string ID { get => _id; set => _id = value; }
        
        public string __displayName;
        [NonSerialized] protected string _displayName;
        public string DisplayName { get => _displayName; set => _displayName = value; }

        public Sprite __icon;
        [NonSerialized] protected Sprite _icon;
        public Sprite Icon { get => _icon; set => _icon = value; }

        public ObjectData()
        {
            ID = __id;
            DisplayName = __displayName;
            Icon = __icon;
        }
    }
}