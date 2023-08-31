using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class BlockController : PoolAwareBehaviour
{
    #region BLOCK_TYPE_CHANGE_EVENT
    [HideInInspector] private BlockTypeEnum _type;
    [SerializeField] public BlockTypeEnum type
    {
        get { return _type; }
        set {
            _type = value;
            SetType();
        }
    }

    private void SetType()
    {
        if (ThemeController.instance.theme == null)
            return;

        ThemeBlocks blockTheme = ThemeController.instance.theme.blocks.Find(val => val.blockType.Equals(type));
        if (blockTheme != null)
        {
            spriteRenderer.sprite = blockTheme.blockSprite;
        }
        else
        {
            spriteRenderer.sprite = null;
        }
    }
    #endregion

#if UNITY_EDITOR
    //Shouldnt be use in runtime scripts it only works in editor
    [SerializeField] private BlockTypeEnum blockType;
#endif
    [SerializeField] public SpriteRenderer spriteRenderer;

    [HideInInspector] private Coroutine animCoroutine;
    [SerializeField] private float animationTimeInSec;
    [HideInInspector] public Vector2 posInGrid
    {
        get;
        private set;
    }
    [HideInInspector] private GridController gridController;

    public void SetValues(Vector2 posInGrid, GridController gridController)
    {
        this.posInGrid = posInGrid;
        this.gridController = gridController;
    }

    public new void OnKill()
    {
        base.OnKill();

        posInGrid = Vector2.zero;
        gridController = null;
    }


    public void StartMove(Vector3 pos)
    {
        if (animCoroutine != null)
        {
            StopCoroutine(animCoroutine);
            AnimationController.instance.DecBlockAnimCount();
        }

        animCoroutine = StartCoroutine(moveAnim(pos));
    }

    private IEnumerator moveAnim(Vector3 pos)
    {
        AnimationController.instance.IncBlockAnimCount();
        Vector3 startPos = this.transform.position;
        float sec = 0;

        while(sec < animationTimeInSec)
        {
            sec += Time.deltaTime * Time.timeScale;
            this.transform.position = Vector3.Lerp(startPos, pos, sec / animationTimeInSec);
            yield return new WaitForEndOfFrame();
        }

        this.transform.position = pos;
        animCoroutine = null;
        AnimationController.instance.DecBlockAnimCount();

        yield return null;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
            return;

        if(blockType != type)
            type = blockType;
    }
#endif
}
