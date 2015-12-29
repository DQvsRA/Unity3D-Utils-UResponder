using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/**
 * UResponder class
 * 
 * @version UResponder v0.3
 * @author Vladimir Minkin
 */

namespace uresponder
{
    public class UResponder
    {
        private static uint _index = 0;
        
        //**************************************************
        //  Constants
        //**************************************************

        /**
        * The instance to the singleton UResponderCore.
        * 
        * @private
        */
        private static readonly UResponderCore CORE = new UResponderCore();

        //**************************************************
        //  Public Properties
        //**************************************************

        /**
        * The number of times that this item will respond.
        * Default is 0 that means infinity times.
        * 
        */
        public uint replies;

        //**************************************************
        //  Protected / Private Properties
        //**************************************************

        /**
		 * The UResponder Action associated with this UResponder.
		 * 
		 * @private
		 */
        private string _action;
        
        /**
        * The closure method that this item was associated.
        * 
        * @private
        */
        private Delegate _listener;

        /**
		 * An Object that can be associated with this item.
		 * This is optional.
		 * 
		 * @private
		 */
        private object _target;

        /**
		 * An unique hash.
		 * 
		 * @private
		 */
        private uint _hash;

        //**************************************************
        //  Initialization
        //**************************************************
        
        public UResponder(
                string action,
                Delegate listener,
                uint replies = 0,
                object useTarget = null
            )
        {
            // Setting the parameters.
            _action = action;
            _listener = listener;
            this.replies = replies;
            _hash = ++_index << 0x08;
            _target = useTarget;
            // Register the UResponder instance.
            CORE.register(this);
        }

        //**********************************************************************************************************
        //
        //  Public Methods
        //
        //**********************************************************************************************************
        public static UResponder add(
            string action,
            Delegate listener,
            uint replies = 0,
            object useTarget = null
            ) { return new UResponder(action, listener, replies, useTarget); }
        
        public static bool remove(string action = null) { return CORE.remove(action); }
        public static bool has(string action = null) {  return CORE.has(action); }
        public static bool dispatch(string action, ArrayList args = null) { return CORE.dispatch(action, args); }
        //public static bool dispatch(string action, object target, ArrayList args = null) { return CORE.dispatch(action, target, args); }

        public void perform(ArrayList withParams = null) {
            // Call a listener in this UResponder.
            //Debug.Log("PERFORM: " + action + " with params:" + (withParams != null) + " | " + _listener.Method.Name);
            if (withParams != null) {
                if (_listener.Method.GetParameters().Length == 1)
                    _listener.DynamicInvoke(withParams);
                else _listener.DynamicInvoke();
            }
            else _listener.DynamicInvoke();
        }

        //**********************************************************************************************************
        //
        //  Getters / Setters
        //
        //**********************************************************************************************************
        
		/**
		 * [read-only] The UResponder Action associated with this UResponder.
		 */
		public string action { get { return _action; } }
		
		/**
		 * [read-only] The target defined to this UResponder. Return null if no target was defined.
		 */
		public object target { get { return _target; } }
		
		/**
		 * [read-only] The listener defined to this UResponder.
		 */
		public Delegate listener { get { return _listener; } }

		/**
		 * [read-only] [internal use] Unique identification for this instance based on its attributes.
		 */
		public uint hash { get { return _hash; } }
	}

    /**
	 * The core processor for UResponders
	 */
    internal sealed class UResponderCore
    {
        private Dictionary<uint, UResponder> _responderByHash = new Dictionary<uint, UResponder>();
        private Dictionary<string, List<uint>> _hashsByAction = new Dictionary<string, List<uint>>();
        private Dictionary<object, List<string>> _actionsByTarget = new Dictionary<object, List<string>>();

        public void register(UResponder responder) {
            uint hash = responder.hash;
            string action = responder.action;
            _responderByHash.Add(hash, responder);

            if (_hashsByAction.ContainsKey(action)) 
                    _hashsByAction[action].Add(hash);
            else    _hashsByAction.Add(action, new List<uint>() { hash });

            if (responder.target != null) {
                if (_actionsByTarget.ContainsKey(responder.target))
                        _actionsByTarget[responder.target].Add(responder.action);
                else    _actionsByTarget.Add(responder.target, new List<string>() { responder.action });
            }
        }

        public bool remove(string action) {
            if (_hashsByAction.ContainsKey(action)) {
                var hashes = _hashsByAction[action].ToArray();
                foreach (uint hash in hashes) ExcludeByAction(_responderByHash[hash]);
                return true;
            }
            else return false;
        }

        public bool remove(object target) {
            return false;
        }

        public bool has(string action) {
            return false;
        }

        public bool dispatch(string action, ArrayList args) {
            bool result = true;
            if (_hashsByAction.ContainsKey(action))
            {
                UResponder responder;
                List<uint> _hashes = _hashsByAction[action];
                uint[] hashes = _hashes.ToArray();
                uint replies = 0;
                foreach (uint hash in hashes) {
                    responder = _responderByHash[hash];
                    replies = responder.replies;
                    if (replies > 0) {
                        if (--replies == 0 && ExcludeByAction(responder)) result = false;
                        responder.replies = replies;
                    }
                    responder.perform(args);
                }
            }
            return result;
        }

        public bool dispatch(string action, object target, ArrayList args)
        {
            bool result = true;
            if (_actionsByTarget.ContainsKey(target))
            {
                List<string> actionsForTarget = _actionsByTarget[target];
                if (actionsForTarget.Contains(action))
                {
                    
                }
                else result = false;
            }
            return result;
        }

        /**
        * Exclude a UResponder based on an action.
        * 
        * @param	The UResponder to remove.
        * @return   If there is no hashes (no UResponders) for that action stop future perform
        */
        private bool ExcludeByAction(UResponder resp) {
            uint hash = resp.hash;
            string action = resp.action;
            object target = resp.target;
            bool noMoreActions = false;

            // Remove UResponder by hash
            _responderByHash.Remove(hash);

            // Remove action from UResponder target
            if (target != null && _actionsByTarget.ContainsKey(target))
            {
                List<string> actionsForTarget = _actionsByTarget[target];
                actionsForTarget.RemoveAt(actionsForTarget.IndexOf(action));
                if(actionsForTarget.Count == 0) {
                    _actionsByTarget.Remove(target);
                }
            }

            // Remove hash for UResponder action
            List<uint> hashesForAction = _hashsByAction[action];
            hashesForAction.RemoveAt(hashesForAction.IndexOf(hash));
            if (hashesForAction.Count == 0) {
                _hashsByAction.Remove(action);
                noMoreActions = true;
            }
            
            return noMoreActions;
        }
    }
}

