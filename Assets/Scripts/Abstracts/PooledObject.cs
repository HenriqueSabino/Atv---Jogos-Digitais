using System.Collections.Generic;
using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
    public List<IPooledObjectListener> Listeners { get; protected set; }

    protected virtual void Awake()
    {
        Listeners = new List<IPooledObjectListener>();
    }

    void OnDisable()
    {
        foreach (var listener in Listeners)
        {
            listener.OnObjectDisabled(this);
        }
    }
}