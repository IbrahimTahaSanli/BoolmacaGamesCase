using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    #region GRID_X_CHANGE_EVENT
    [HideInInspector] private uint _row;
    [HideInInspector] public uint row { 
        get { return _row; }
        set { 
            _row = value; 
            UpdateGrid();
        }
    }

    #endregion
    #region GRID_Y_CHANGE_EVENT
    [HideInInspector] private uint _column;
    [HideInInspector] public uint column
    {
        get { return _column; }
        set { 
            _column = value;
            UpdateGrid();
        }
    }
    #endregion

    #if UNITY_EDITOR
    [SerializeField][Min(0)] private uint gridRow;
    [SerializeField][Min(0)] private uint gridColumn;
    #endif

    [HideInInspector] private List<List<BlockController>> grid = new List<List<BlockController>>();
    [SerializeField] private BlockPool blockPool;
    [SerializeField] private Vector2 blockOffset;


    public void UpdateGrid()
    {
        CreateGrid();
        PlaceGrid();
    }

    public void CreateGrid()
    {
        if(this.row < this.grid.Count)
        {
            for (int y = 0; y < this.row; y++)
            {
                int beforeColumn = this.grid[y].Count;
                if (this.column < beforeColumn)
                {
                    this.grid[y].GetRange((int)this.column, this.grid[y].Count - (int)this.column).ForEach(field => field.Kill());
                    this.grid[y] = this.grid[y].GetRange(0, (int)this.column);
                }
                else
                {
                    for (int x = 0; x < this.column - beforeColumn; x++)
                    {
                        BlockController block = blockPool.GetBlock<BlockController>();
                        block.type = BlockTypeEnum.B0;
                        block.SetValues(new Vector2(y, x), this);
                        this.grid[y].Add(block);
                    }
                }
            }

            this.grid.GetRange((int)this.row, this.grid.Count - (int)this.row).ForEach(col => col.ForEach(block => block.Kill()));
            this.grid = this.grid.GetRange(0, (int)this.row);
        }
        else
        {
            for (int y = 0; y < this.grid.Count; y++)
            {
                int beforeColumn = this.grid[y].Count;

                if(this.column < beforeColumn)
                {
                    this.grid[y].GetRange((int)this.column, this.grid[y].Count - (int)this.column).ForEach(field => field.Kill());
                    this.grid[y] = this.grid[y].GetRange(0, (int)this.column);
                }
                else
                {
                    for(int x = 0; x < this.column - beforeColumn; x++)
                    {
                        BlockController block = blockPool.GetBlock<BlockController>();
                        block.type = BlockTypeEnum.B0;
                        block.SetValues(new Vector2(y, x), this);
                        this.grid[y].Add(block);
                    }
                }
            }

            for(int y = this.grid.Count; y < this.row; y++)
            {
                List<BlockController> currentRow = new List<BlockController>();
                this.grid.Add(currentRow);
                for(int x = 0; x < this.column; x++)
                {
                    BlockController block = blockPool.GetBlock<BlockController>();
                    block.type = BlockTypeEnum.B0;
                    block.SetValues(new Vector2(y, x), this);
                    this.grid[y].Add(block);
                }
            }
        }
    }

    public void PlaceGrid()
    {
        for (int x = 0; x < this.row; x++)
            for (int y = 0; y < this.column; y++)
                this.grid[x][y].StartMove(
                    new Vector3(
                        (x - this.row / 2) * blockOffset.x,
                        (y - this.column / 2) * blockOffset.y,
                        0));
    }

    public void Blast(Vector2 lastPos)
    {
        List<Vector2> extractNeighbors(Vector2 vec)
        {
            List<Vector2> checkBlocks = new List<Vector2>();
            checkBlocks.Add(vec);
            List<Vector2> sameType = new List<Vector2>();
            sameType.Add(vec);

            if (vec.x - 1 >= 0)
                checkBlocks.Add(new Vector2(vec.x - 1, vec.y));

            if (vec.x + 1 < this.row)
                checkBlocks.Add(new Vector2(vec.x + 1, vec.y));

            if (vec.y - 1 >= 0)
                checkBlocks.Add(new Vector2(vec.x, vec.y - 1));

            if (vec.y + 1 < this.column)
                checkBlocks.Add(new Vector2(vec.x, vec.y + 1));


            BlockTypeEnum type = this.grid[(int)vec.x][(int)vec.y].type;

            for (int i = 1; i < checkBlocks.Count; i++)
            {
                Vector2 currentVec = checkBlocks[i];
                if (this.grid[(int)currentVec.x][(int)currentVec.y].type == type)
                {
                    sameType.Add(currentVec);

                    if (currentVec.x - 1 >= 0 && !checkBlocks.Contains(new Vector2(currentVec.x-1, currentVec.y)))
                        checkBlocks.Add(new Vector2(currentVec.x - 1, currentVec.y));

                    if (currentVec.x + 1 < this.row && !checkBlocks.Contains(new Vector2(currentVec.x + 1, currentVec.y)))
                        checkBlocks.Add(new Vector2(currentVec.x + 1, currentVec.y));

                    if (currentVec.y - 1 >= 0 && !checkBlocks.Contains(new Vector2(currentVec.x, currentVec.y - 1)))
                        checkBlocks.Add(new Vector2(currentVec.x, currentVec.y - 1));

                    if (currentVec.y + 1 < this.column && !checkBlocks.Contains(new Vector2(currentVec.x, currentVec.y + 1)))
                        checkBlocks.Add(new Vector2(currentVec.x, currentVec.y + 1));
                }
            }

            return sameType;
        }

        List<Vector2> firstBlast = extractNeighbors(lastPos);
        if (firstBlast.Count >= 3) { 
            for (int i = 1; i < firstBlast.Count; i++)
                this.grid[(int)firstBlast[i].x][(int)firstBlast[i].y].type = BlockTypeEnum.B0;

            this.grid[(int)lastPos.x][(int)lastPos.y].type = (BlockTypeEnum)((int)this.grid[(int)lastPos.x][(int)lastPos.y].type << 1);
        }

        bool isDone = false;
        while (!isDone)
        {
            isDone = true;
            
            for (int x = 0; x < this.row; x++)
                for(int y = 0; y < this.row; y++)
                {
                    if (this.grid[x][y].type == BlockTypeEnum.B0)
                        continue;

                    List<Vector2> neighbors = extractNeighbors(new Vector2(x, y));

                    if (neighbors.Count < 3)
                        continue;

                    for (int i = 1; i < neighbors.Count; i++)
                        this.grid[(int)neighbors[i].x][(int)neighbors[i].y].type = BlockTypeEnum.B0;

                    this.grid[(int)neighbors[0].x][(int)neighbors[0].y].type = (BlockTypeEnum)((int)this.grid[(int)neighbors[0].x][(int)neighbors[0].y].type << 1);

                    isDone = false;

                }
        }

        UpdateGrid();
    }


    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
            return;

        if (gridRow != row)
            row = gridRow;

        if (gridColumn != column)
            column = gridColumn;
    }
    #endif
}
