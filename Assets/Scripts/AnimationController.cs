using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [HideInInspector] public static AnimationController instance;

    [HideInInspector] public uint blockAnimtaionCount
    {
        get;
        private set;
    } = 0;

    public AnimationController()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);
    }

    public void IncBlockAnimCount() => blockAnimtaionCount++;
    public void DecBlockAnimCount() => blockAnimtaionCount--;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
