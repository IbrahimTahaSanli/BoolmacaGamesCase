using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomController : MonoBehaviour
{
    [HideInInspector] public static RandomController instance;

    [SerializeField] public RandomizerStatus status;
    [SerializeField] private Vector3 secondBlockOffset;

    [SerializeField] private BlockPool blockPool;

    [HideInInspector] public BlockController currentBlock;
    [HideInInspector] public BlockController nextBlock;

    private RandomController()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);
    }

    public void Start()
    {
        currentBlock = blockPool.GetBlock<BlockController>();
        currentBlock.type = randomBlockType();
        currentBlock.transform.position = this.transform.position;
        
        nextBlock = blockPool.GetBlock<BlockController>();
        nextBlock.type = randomBlockType();
        nextBlock.transform.position = this.transform.position + secondBlockOffset;
    }

    public BlockTypeEnum getCurrentBlock()
    {
        BlockTypeEnum returnType = currentBlock.type;

        currentBlock.Kill();
        currentBlock = nextBlock;
        currentBlock.StartMove(this.transform.position);

        nextBlock = blockPool.GetBlock<BlockController>();
        nextBlock.type = randomBlockType();
        nextBlock.transform.position = this.transform.position + secondBlockOffset;

        return returnType;
    }

    public BlockTypeEnum randomBlockType()
    {
        float ran = Random.Range(0.0f, 1.0f);
        return status.blockChances.Find(elem => ((ran -= elem.chance) < 0.0f)).type;
    }
}
