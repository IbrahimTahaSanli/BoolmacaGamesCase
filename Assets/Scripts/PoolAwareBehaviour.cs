using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolAwareBehaviour : MonoBehaviour
{
    [HideInInspector] public bool isLive = false;
    public void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public virtual void OnLive() { 
    }

    public virtual void OnKill() { 
    }

    public void Kill() { 
        isLive = false;
        this.gameObject.SetActive(false);
    }

    public void Alive()
    {
        isLive=true;
        this.gameObject.SetActive(true);
    }
}
