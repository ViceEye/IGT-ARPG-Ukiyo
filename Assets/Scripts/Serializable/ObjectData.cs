using System;
using UnityEngine;

namespace Ukiyo.Serializable
{
    [CreateAssetMenu(fileName = "Player", menuName = "Ukiyo/New Object Data")]
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
        
        public GameObject __prefab;
        [NonSerialized] protected GameObject _prefab;
        public GameObject Prefab { get => _prefab; set => _prefab = value; }
    }
}