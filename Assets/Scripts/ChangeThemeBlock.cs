using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeThemeBlock : ThemeDependedBehaviour
{
    [SerializeField] private BlockController controller;

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
        if (theme == null)
            return;
        ThemeBlocks blockTheme = theme.blocks.Find(val => val.blockType.Equals(controller.type));
        if (blockTheme != null) {
            controller.spriteRenderer.sprite = blockTheme.blockSprite;
        }
        else
        {
            controller.spriteRenderer.sprite = null;
        }
    }
}
