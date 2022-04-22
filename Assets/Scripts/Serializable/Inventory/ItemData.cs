using System;

namespace Ukiyo.Serializable
{
    /// <summary>
    /// Item detail for inventory
    /// </summary>
    [Serializable]
    public class ItemData : ObjectData
    {
        public int _stack { get; protected set; }

        public ItemData(ObjectData source)
        {
            Init(source);
            SetStack(1);
        }
        
        public ItemData(ObjectData source, int stack)
        {
            Init(source);
            _stack = stack;
        }
        

        public void SetStack(int stack)
        {
            _stack = stack;
        }
    }
}