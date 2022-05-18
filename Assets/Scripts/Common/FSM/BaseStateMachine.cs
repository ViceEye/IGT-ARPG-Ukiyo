using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ukiyo.Common.FSM
{
    public class BaseStateMachine : MonoBehaviour
    {
        [SerializeField] 
        private BaseState _initialState;
        [SerializeField] 
        private BaseState _currentState;
        private Dictionary<Type, Component> _cachedComponents;
        
        // Scripting uses of FSM
        // Caching actions, states, decisions prevents duplication and allows reuse
        private Dictionary<Type, FSMAction> _cachedActions;
        private Dictionary<Type, BaseState> _cachedStates;
        private Dictionary<Type, Decision> _cachedDecisions;
        private Dictionary<Type, Transition> _cachedTransitions;
        
        public BaseState CurrentState { get; set; }
        
        // Init Cache Components
        protected virtual void Awake()
        {
            CurrentState = _initialState;
            _cachedComponents = new Dictionary<Type, Component>();
            
            _cachedActions = new Dictionary<Type, FSMAction>();
            _cachedStates = new Dictionary<Type, BaseState>();
            _cachedDecisions = new Dictionary<Type, Decision>();
            _cachedTransitions = new Dictionary<Type, Transition>();
        }

        protected virtual void Start()
        {
            CurrentState.BeforeExecute(this);
        }

        protected virtual void Update()
        {
            _currentState = CurrentState;
            CurrentState.Execute(this);
        }

        protected virtual void FixedUpdate()
        {
            CurrentState.FixedExecute(this);
        }

        /// <summary>
        /// Get and cache a component for FSM
        /// </summary>
        /// <typeparam name="T">Type of Component</typeparam>
        /// <returns>Component</returns>
        public new T GetComponent<T>() where T : Component
        {
            // Check cache for component
            if(_cachedComponents.ContainsKey(typeof(T)))
                return _cachedComponents[typeof(T)] as T;

            // Default GetComponent()
            var component = base.GetComponent<T>();
            if(component != null)
                _cachedComponents.Add(typeof(T), component);
            
            return component;
        }

        /// <summary>
        /// Get and cache a action for FSM
        /// </summary>
        /// <typeparam name="T">Type of Action</typeparam>
        /// <returns>Component</returns>
        public FSMAction GetAction<T>() where T : FSMAction
        {
            // Check cache for component
            if(_cachedActions.ContainsKey(typeof(T)))
                return _cachedActions[typeof(T)];

            // Create a instance of action
            var action = ScriptableObject.CreateInstance<T>();
            if(action != null)
                _cachedActions.Add(typeof(T), action);
                
            return action;
        }

        /// <summary>
        /// Get and cache a state for FSM
        /// </summary>
        /// <typeparam name="T">Type of State</typeparam>
        /// <returns>Component</returns>
        public BaseState GetState<T>() where T : BaseState
        {
            // Check cache for component
            if(_cachedStates.ContainsKey(typeof(T)))
                return _cachedStates[typeof(T)];

            // Create a instance of state
            var state = ScriptableObject.CreateInstance<T>();
            if(state != null)
                _cachedStates.Add(typeof(T), state);
                
            return state;
        }

        /// <summary>
        /// Get and cache a decision for FSM
        /// </summary>
        /// <typeparam name="T">Type of Decision</typeparam>
        /// <returns>Component</returns>
        public Decision GetDecision<T>() where T : Decision
        {
            // Check cache for decision
            if(_cachedDecisions.ContainsKey(typeof(T)))
                return _cachedDecisions[typeof(T)];

            // Create a instance of decision
            var decision = ScriptableObject.CreateInstance<T>();
            if(decision != null)
                _cachedDecisions.Add(typeof(T), decision);
                
            return decision;
        }

        /// <summary>
        /// Get and cache a transition for FSM
        /// </summary>
        /// <typeparam name="T">Type of Transition</typeparam>
        /// <returns>Component</returns>
        public Transition GetTransition<T>() where T : Transition
        {
            // Check cache for transition
            if(_cachedTransitions.ContainsKey(typeof(T)))
                return _cachedTransitions[typeof(T)];

            // Create a instance of transition
            var decision = ScriptableObject.CreateInstance<T>();
            if(decision != null)
                _cachedTransitions.Add(typeof(T), decision);
            
            return decision;
        }
    }
}