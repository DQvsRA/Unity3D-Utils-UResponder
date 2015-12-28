using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/**
 * UResponder class
 * 
 * @version UResponder v0.2
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
        private GameObject _target;

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
                uint replies = 0
            )
        {
            // Setting the parameters.
            _action = action;
            _listener = listener;
            this.replies = replies;
            _hash = ++_index << 0x08;

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
            uint replies = 0
            )
        {
            return new UResponder(action, listener, replies);
        }
        
        public static bool remove(string action = null)
        {
            return CORE.remove(action);
        }

        public static bool has(string action = null)
        {
            return CORE.has(action);
        }

        public static bool dispatch(string action = null, ArrayList args = null)
        {
            return CORE.dispatch(action, args);
        }
        
        public void perform(ArrayList withParams = null)
        {
            // Call a listener in this UResponder.
            if (withParams != null)
            {
                if(_listener.Method.GetParameters().Length == 1)
                _listener.DynamicInvoke(withParams);
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
		public GameObject target { get { return _target; } }
		
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
        private Dictionary<string, List<uint>> _hashByAction = new Dictionary<string, List<uint>>();

        public void register(UResponder responder)
        {
            _responderByHash.Add(responder.hash, responder);
            if(_hashByAction.ContainsKey(responder.action))
            {
                _hashByAction[responder.action].Add(responder.hash);
            } else _hashByAction.Add(responder.action, new List<uint>() { responder.hash });
        }

        public bool remove(string action)
        {
            return false;
        }

        public bool has(string action)
        {
            return false;
        }

        public bool dispatch(string action, ArrayList args)
        {
            if (_hashByAction.ContainsKey(action))
            {
                List<uint> _hashes = _hashByAction[action];
                uint[] hashes = _hashes.ToArray();
                UResponder responder;
                bool perform = hashes.Length > 0;
                if (perform)
                    foreach (uint hash in hashes)
                    {
                        responder = _responderByHash[hash];
                        uint replies = responder.replies;
                        if (replies > 0)
                        {
                            if (--replies == 0) ExcludeResponder(responder);
                            responder.replies = replies;
                        }
                        responder.perform(args);
                    }
                else _hashByAction.Remove(action);
                return perform;
            }
            return false;
        }

        /**
        * Exclude a UResponder based on a listener and a target.
        * 
        * @param	index		The index of the UResponder to be removed.
        */
        private void ExcludeResponder(UResponder resp)
        {
            uint hash = resp.hash;
            List<uint> _hashesForAction = _hashByAction[resp.action];
            _hashesForAction.RemoveAt(_hashesForAction.IndexOf(hash));
            _responderByHash.Remove(hash);

        }
    }
}

