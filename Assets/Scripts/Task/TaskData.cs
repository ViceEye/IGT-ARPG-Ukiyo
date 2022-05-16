using System;
using Ukiyo.Common;
using UnityEngine;

namespace Task
{
    [Serializable]
    public class TaskData
    {
        [SerializeField]
        protected int _id;
        public int ID { get => _id; set => _id = value; }

        [SerializeField]
        protected EnumTaskType _type;
        public EnumTaskType Type { get => _type; set => _type = value; }

        [SerializeField]
        protected int _progress;
        public int Progress { get => _progress; set => _progress = value; }
        
        [SerializeField]
        protected int _target;
        public int Target { get => _target; set => _target = value; }

        [SerializeField] 
        protected string _detail;
        public string Detail { get => _detail; set => _detail = value; }
    }
}