using System;
using Ukiyo.Common;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

namespace Ukiyo.Serializable
{
    /// <summary>
    /// Item detail for editor
    /// </summary>
    [Serializable]
    public class ObjectData
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

        [TextArea]
        public string __description;
        [NonSerialized] protected string _description;
        public string Description { get => _description; set => _description = value; }

        public EnumInventoryItemType __type;
        [NonSerialized] protected EnumInventoryItemType _type;
        public EnumInventoryItemType Type { get => _type; set => _type = value; }
        
        public ObjectData()
        {
            ID = __id;
            DisplayName = __displayName;
            Icon = __icon;
            Description = __description;
            Type = __type;
        }
    }
}