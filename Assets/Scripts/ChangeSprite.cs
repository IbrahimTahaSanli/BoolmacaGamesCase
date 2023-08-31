using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : ThemeDependedBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnThemeChange(Theme theme)
    {
        this.spriteRenderer.sprite = theme.cartSprite;
    }
}
