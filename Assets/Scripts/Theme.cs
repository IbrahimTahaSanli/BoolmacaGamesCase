using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeDefaut", menuName = "ScriptableObjects/Theme scriptable object", order = 1)]
public class Theme : ScriptableObject
{
    [SerializeField] public List<ThemeBlocks> blocks;
    [SerializeField] public Sprite cartSprite;
    [SerializeField] public Sprite themeToggleSprite;
    [SerializeField] public Sprite adBlockSprite;
    [SerializeField] public Sprite rankingButtonSprite;
    [SerializeField] public Sprite rewindButtonSprite;
    [SerializeField] public Sprite restartButtonSprite;
    [SerializeField] public Sprite settingsButtonSprite;
    [SerializeField] public Sprite soundButtonSprite;
    [SerializeField] public Sprite vibrationButtonSprite;

}


[Serializable]
public class ThemeBlocks
{
    public BlockTypeEnum blockType;
    public Sprite blockSprite;
}
