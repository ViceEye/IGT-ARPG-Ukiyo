using UnityEngine;

namespace Ukiyo.Serializable
{
    /// <summary>
    /// Item Interface
    /// </summary>
    public interface IObjectBase
    {
        string ID { get; set; }
        string DisplayName { get; set; }
        Sprite Icon { get; set; }
    }
}
