using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThemeController : MonoBehaviour
{
    [HideInInspector] public static ThemeController instance;

    [HideInInspector] private Theme _theme;
    [HideInInspector] public Theme theme
    {
        get { return _theme; }
        set {
            _theme = value;
            invokeThemeChangeEvents(value);
        }
    }

#if UNITY_EDITOR
    //Only works in editor so it shouldnt be used in runtime scripts
    [SerializeField] private Theme themeObject;
#endif

    #region THEME_CHANGE_EVENT
    [HideInInspector] private List<ThemeDependedBehaviour> spriteDependedBehaviours = new List<ThemeDependedBehaviour>();

    public void AddThemeDependedBehaviour(ThemeDependedBehaviour theme)
    {
        if (!spriteDependedBehaviours.Contains(theme))
        {
            this.spriteDependedBehaviours.Add(theme);
            theme.OnThemeChange(this.theme);
        }
    }

    public void RemoveThemeDependedBehaviour(ThemeDependedBehaviour theme)
    {
        if (spriteDependedBehaviours.Contains(theme))
        {
            this.spriteDependedBehaviours.Remove(theme);
        }
    }

    private void invokeThemeChangeEvents(Theme theme)
    {
        foreach (ThemeDependedBehaviour behaviour in this.spriteDependedBehaviours)
            behaviour.OnThemeChange(theme);
    }
    #endregion

    public ThemeController()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(this.themeObject != this.theme)
        {
            this.theme = this.themeObject;
        }
    }
#endif
}
