using System;
using Ukiyo.Common;
using UnityEditor;
using UnityEngine;

namespace Ukiyo.Serializable
{
    /// <summary>
    /// Item detail for editor
    /// </summary>
    [Serializable]
    public class ObjectData
    {
        [SerializeField]
        protected int _id;
        public int ID { get => _id; set => _id = value; }
        
        [SerializeField]
        protected string _displayName;
        public string DisplayName { get => _displayName; set => _displayName = value; }
        
        [SerializeField]
        protected Sprite _icon;
        public Sprite Icon { get => _icon; set => _icon = value; }
        
        [SerializeField]
        protected int _capacity;
        public int Capacity { get => _capacity; set => _capacity = value; }

        [SerializeField]
        protected string _description;
        public string Description { get => _description; set => _description = value; }

        [SerializeField]
        protected EnumInventoryItemType _type;
        public EnumInventoryItemType Type { get => _type; set => _type = value; }

        public void Init(ObjectData data)
        {
            ID = data._id;
            DisplayName = data._displayName;
            Icon = data._icon;
            Capacity = data._capacity;
            Description = data._description;
            Type = data._type;
        }
    }
}