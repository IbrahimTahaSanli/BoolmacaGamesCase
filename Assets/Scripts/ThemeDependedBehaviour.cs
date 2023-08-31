using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeDependedBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        ThemeController.instance.AddThemeDependedBehaviour(this); 
    }

    private void OnDestroy()
    {
        ThemeController.instance.RemoveThemeDependedBehaviour(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnThemeChange(Theme theme)
    {

    }
}
