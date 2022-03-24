using System;

namespace Ukiyo.Serializable
{
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
    }
}