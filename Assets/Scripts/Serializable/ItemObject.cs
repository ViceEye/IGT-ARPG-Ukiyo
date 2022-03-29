using UnityEngine;

namespace Ukiyo.Serializable
{
    /// <summary>
    /// Item for in game uses
    /// </summary>
    public class ItemObject : MonoBehaviour
    {
        public ItemData referenceData;

        public void onInteract()
        {
            Debug.Log("do interact");
        }
    }
}