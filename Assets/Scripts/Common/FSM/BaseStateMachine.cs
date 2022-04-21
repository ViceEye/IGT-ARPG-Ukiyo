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
        
        protected virtual void Awake()
        {
            Debug.Log("Awake");
            CurrentState = _initialState;
            _cachedComponents = new Dictionary<Type, Component>();
        }

        protected virtual void Start()
        {
            Debug.Log("Start");
            CurrentState.BeforeExecute(this);
        }

        public BaseState CurrentState { get; set; }

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
            {
                _cachedComponents.Add(typeof(T), component);
            }
            return component;
        }

    }
}