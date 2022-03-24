using UnityEngine;

namespace Ukiyo.Serializable
{
    public class ItemObject : MonoBehaviour
    {
        public ItemData referenceData;

        public void onInteract()
        {
            Debug.Log("do interact");
        }
    }
}