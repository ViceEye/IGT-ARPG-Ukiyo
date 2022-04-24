using UnityEngine;

namespace Ukiyo.Framework
{public interface IAnimatorListener
    {
        /// <summary>
        /// Recieve messages from the Animator State Machine Behaviours
        /// </summary>
        /// <param name="message">The name of the method</param>
        /// <param name="value">the parameter</param>
        void OnAnimatorBehaviourMessage(string message, object value);

        /*
        public virtual void OnAnimatorBehaviourMessage(string message, object value)
        {
            this.InvokeWithParams(message, value);
        }
        */
    }
    
    public class MessagesBehavior : StateMachineBehaviour
    {
        public MessageItem[] onEnterMessage; //Store messages to send it when Enter the animation State
        public MessageItem[] onExitMessage; //Store messages to send it when Exit  the animation State
        public MessageItem[] onTimeMessage; //Store messages to send on a specific time  in the animation State

        IAnimatorListener[] listeners; //To all the MonoBehavious that Have this 


        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            listeners = animator.GetComponents<IAnimatorListener>();


            foreach (MessageItem onTimeM in onTimeMessage) //Set all the messages Ontime Sent = false when start
            {
                onTimeM.sent = false;
            }

            foreach (MessageItem onEnterM in onEnterMessage)
            {
                if (onEnterM.Active && onEnterM.message != string.Empty)
                {
                    foreach (var item in listeners)
                        DeliverListener(onEnterM, item);
                }
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (MessageItem onExitM in onExitMessage)
            {
                if (onExitM.Active && onExitM.message != string.Empty)
                {
                    foreach (var item in listeners)
                        DeliverListener(onExitM, item);
                }
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (MessageItem onTimeM in onTimeMessage)
            {
                if (onTimeM.Active && onTimeM.message != string.Empty)
                {
                    if (!onTimeM.sent && Mathf.Abs(stateInfo.normalizedTime % 1 - onTimeM.time) <= 0.1f)
                    {
                        onTimeM.sent = true;

                        foreach (var item in listeners)
                            DeliverListener(onTimeM, item);
                    }
                }
            }
        }

        /// <summary>
        /// Send messages to all scripts with IBehaviourListener to this animator 
        /// </summary>
        private void DeliverListener(MessageItem m, IAnimatorListener listener)
        {
            switch (m.typeM)
            {
                case TypeMessage.Bool:
                    listener.OnAnimatorBehaviourMessage(m.message, m.boolValue);
                    break;
                case TypeMessage.Int:
                    listener.OnAnimatorBehaviourMessage(m.message, m.intValue);
                    break;
                case TypeMessage.Float:
                    listener.OnAnimatorBehaviourMessage(m.message, m.floatValue);
                    break;
                case TypeMessage.String:
                    listener.OnAnimatorBehaviourMessage(m.message, m.stringValue);
                    break;
                case TypeMessage.Void:
                    listener.OnAnimatorBehaviourMessage(m.message, null);
                    break;
            }
        }
        
        [System.Serializable]
        public class MessageItem
        {
            public string message;
            public TypeMessage typeM;
            public bool boolValue;
            public int intValue;
            public float floatValue;
            public string stringValue;

            public float time;
            public bool sent;
            public bool Active;

            public MessageItem()
            {
                message = string.Empty;
                Active = true;
            }
        }

        public enum TypeMessage
        {
            Bool = 0,
            Int = 1,
            Float = 2,
            String = 3,
            Void = 4
        }
    }
}
