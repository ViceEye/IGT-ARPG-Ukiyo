using System;
using UnityEngine.Purchasing.MiniJSON;

namespace Ukiyo.Serializable
{
    /// <summary>
    /// Item detail for storage
    /// </summary>
    [Serializable]
    public class ItemData
    {
        public ObjectData data { get; private set; }
        public int stackSize { get; private set; }

        public ItemData(ObjectData source)
        {
            data = source;
            AddToStack();
        }

        public void AddToStack()
        {
            stackSize++;
        }

        public void ReduceFromStack()
        {
            stackSize--;
        }

        public void SetTheStack(int stack)
        {
            stackSize = stack;
        }
    }
}