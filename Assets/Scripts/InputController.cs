using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [HideInInspector] private bool isHolding = false;
    [HideInInspector] private BlockTypeEnum currentBlock;
    [HideInInspector] private BlockController lastBlock;

    [SerializeField] private GridController gridController;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (!isHolding && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                BlockController block = hit.transform.GetComponent<BlockController>();
                if(block.type != BlockTypeEnum.B0)
                    return;
                
                isHolding = true;

                currentBlock = RandomController.instance.getCurrentBlock();

                block.type = currentBlock;
                lastBlock = block;
            }
        }
        else if (isHolding && Input.GetMouseButton(0)){
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                BlockController block = hit.transform.GetComponent<BlockController>();
                if (block.type != BlockTypeEnum.B0 || (int)currentBlock <= 2)
                    return;

                Vector2 pos = block.posInGrid - lastBlock.posInGrid;
                if (pos.magnitude < 1)
                    return;

                lastBlock.type = (BlockTypeEnum)((int)lastBlock.type >> 1);
                block.type = lastBlock.type;
                currentBlock = block.type;
                lastBlock = block;
            }
        }
        else if (isHolding && Input.GetMouseButtonUp(0))
        {
            isHolding=false;
            gridController.Blast(lastBlock.posInGrid);
        }


    }
}
