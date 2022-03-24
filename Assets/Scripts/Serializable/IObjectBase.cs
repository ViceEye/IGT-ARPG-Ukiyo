using UnityEngine;

namespace Ukiyo.Serializable
{
    public interface IObjectBase
    {
        string ID { get; set; }
        string DisplayName { get; set; }
        Sprite Icon { get; set; }
        GameObject Prefab { get; set; }
    }
}
