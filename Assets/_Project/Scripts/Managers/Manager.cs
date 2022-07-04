using System;
using UnityEngine;

    public class Manager<T> : MonoBehaviour where T : Manager<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance==null)
            {
                Instance = (T) this;
                //Debug.Log(typeof(T).ToString()+" is initialized");
            }
            else
            {
                Destroy(gameObject);
                //Debug.LogError("Cannot initialize second instance");
            }
        }
        

        protected virtual void OnDestroy()
        {
            if (Instance==this)
            {
                Instance = null;
            }
        }
  
}