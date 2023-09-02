using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallsAndBubble
{
    public class GridSystem : MonoBehaviour
    {
        public static GridSystem Instance { get; private set; }

        public enum BlockType
        {
            EMPTY,
            NORMAL,
        }

        [System.Serializable]
        public struct BlockPrefab
        {
            public BlockType Type;
            public GameObject Prefab;
        };

        public int width;
        public int height;
        public bool IsSurvival { get; set; }

        public BlockPrefab[] BlocksPrefabs;

        private Dictionary<BlockType, GameObject> blockPrefabDict;
        private Block[,] blocks;

        private float fillTime = 0.02f;
        private WaitForSeconds waitForSeconds;


        private void Awake()
        {
            Instance = this;
            waitForSeconds = new WaitForSeconds(fillTime);
        }

        private void OnEnable()
        {
            Block.OnBlockClicked += BlockClickedLogicHandle;
        }

        private void OnDisable()
        {
            Block.OnBlockClicked -= BlockClickedLogicHandle;
        }

        private void Start()
        {
            blockPrefabDict = new Dictionary<BlockType, GameObject>();

            for (int i = 0; i < BlocksPrefabs.Length; i++)
            {
                if (blockPrefabDict.ContainsKey(BlocksPrefabs[i].Type) == false)
                {
                    blockPrefabDict.Add(BlocksPrefabs[i].Type, BlocksPrefabs[i].Prefab);
                }
            }



            blocks = new Block[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    SpawnNewBlock(x, y, BlockType.EMPTY);
                }
            }

            StartCoroutine(PerformFillHorizontal(fillTime));
        }


        private void BlockClickedLogicHandle(Block block)
        {
            var listNB = GridSystem.Instance.FindConnectedGroup(block.X, block.Y, block.ColorBlock.Color);
            if (listNB.Count > 1)
            {
                for (int i = 0; i < listNB.Count; i++)
                {
                    ClearPiece(listNB[i].X, listNB[i].Y);
                }
            }

            StartCoroutine(PerformMoveBlockDown(()=>
            {
                MoveAllEmptyColumnRight();

                if (IsSurvival)
                {
                    for (int i = 0; i < width - 1; i++)
                    {
                        if (IsColumnEmpty(i))
                        {
                            StartCoroutine(PerformFillColume(i));
                        }
                    }
                }
            }));

           

            StartCoroutine(Utilities.WaitAfter(0.5f, () =>
            {
                bool isGameOver = NoConnectedGroupsGreaterThanOne();
                Debug.Log($"Gameover: {isGameOver}");
            }));
        }

        private void MoveAllEmptyColumnRight()
        {
            for (int i = width - 1; i >= 0; i--)
            {
                if (IsColumnEmpty(i))
                {
                    for (int x = i; x >= 0; x--)
                    {
                        MoveColumnRight(x);
                    }
                }
            }
        }

        public bool NoConnectedGroupsGreaterThanOne()
        {
            // Create a dictionary to store the sizes of connected groups
            Dictionary<Block, int> groupSizes = new Dictionary<Block, int>();

            // Iterate through all blocks in the grid
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Block block = blocks[x, y];

                    if (!groupSizes.ContainsKey(block) && block.Type == BlockType.NORMAL)
                    {
                        // Find the connected group for the current block
                        List<Block> connectedGroup = FindConnectedGroup(x, y, block.ColorBlock.Color);

                        // Store the size of the connected group in the dictionary
                        groupSizes[block] = connectedGroup.Count;
                    }
                }
            }

            // Check if any connected group has a size greater than one
            foreach (var groupSize in groupSizes)
            {
                if (groupSize.Value > 1)
                {
                    // There is at least one connected group with more than one block
                    return false;
                }
            }

            // No connected groups with more than one block were found
            return true;
        }

        public bool IsColumnEmpty(int column)
        {
            // Check if the specified column is valid
            if (column >= 0 && column < width)
            {
                // Iterate through the column from top to bottom
                for (int y = 0; y < height; y++)
                {
                    if (blocks[column, y].Type != BlockType.EMPTY)
                    {
                        // If a non-empty block is found in the column, it's not empty
                        return false;
                    }
                }

                // If all blocks in the column are empty, it's considered empty
                return true;
            }

            // Invalid column, return false
            return false;
        }

        public bool FilColumn(int column)
        {
            bool movedPiece = false;

            // Iterate through the grid from top to bottom and left to right
            for (int y = height - 1; y >= 1; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    Block piece = blocks[x, y];

                    if (piece.IsMoveable())
                    {
                        Block nbAbove = blocks[x, y - 1];  // Check the block above

                        if (nbAbove.Type == BlockType.EMPTY)
                        {
                            Destroy(nbAbove.gameObject);

                            piece.MoveableBlock.Move(x, y - 1, fillTime);
                            blocks[x, y - 1] = piece;
                            SpawnNewBlock(x, y, BlockType.EMPTY);
                            movedPiece = true;
                        }
                    }
                }
            }


            Block nb = blocks[column, height - 1];  // Check the top row
            if (nb.Type == BlockType.EMPTY)
            {
                Destroy(nb.gameObject);

                Block newPiece = Instantiate(blockPrefabDict[BlockType.NORMAL], GetWorldPosition(column, height), Quaternion.identity).GetComponent<Block>();
                newPiece.transform.parent = this.transform;

                blocks[column, height - 1] = newPiece;
                blocks[column, height - 1].Init(column, height, BlockType.NORMAL);
                blocks[column, height - 1].MoveableBlock.Move(column, height - 1, fillTime);
                blocks[column, height - 1].ColorBlock.SetColor(Utilities.GetRandomEnum<ColorBlock.ColorType>());

                movedPiece = true;
            }
            return movedPiece;
        }
        private IEnumerator PerformFillColume(int column)
        {
            while (true)
            {
                if (FilColumn(column) == false)
                    yield break;

                yield return waitForSeconds;
            }
        }


        public bool MoveColumnLeft(int column)
        {
            bool movedPiece = false;

            // Check if the specified column is valid
            if (column >= 1 && column < width)
            {
                // Iterate through the column from top to bottom
                for (int y = height - 1; y >= 0; y--)
                {
                    Block currentBlock = blocks[column, y];

                    if (currentBlock.IsMoveable())
                    {
                        Block nbLeft = blocks[column - 1, y];  // Check the block to the left

                        if (nbLeft.Type == BlockType.EMPTY)
                        {
                            Destroy(nbLeft.gameObject);

                            currentBlock.MoveableBlock.Move(column - 1, y, fillTime);
                            blocks[column - 1, y] = currentBlock;
                            blocks[column, y] = SpawnNewBlock(column, y, BlockType.EMPTY); // Empty the current position
                            movedPiece = true;
                        }
                    }
                }
            }

            return movedPiece;
        }
        public bool MoveColumnRight(int column)
        {
            bool movedPiece = false;

            // Check if the specified column is valid
            if (column >= 0 && column < width - 1)
            {
                // Iterate through the column from top to bottom
                for (int y = height - 1; y >= 0; y--)
                {
                    Block currentBlock = blocks[column, y];

                    if (currentBlock.IsMoveable())
                    {
                        Block nbRight = blocks[column + 1, y];  // Check the block to the right

                        if (nbRight.Type == BlockType.EMPTY)
                        {
                            Destroy(nbRight.gameObject);

                            currentBlock.MoveableBlock.Move(column + 1, y, fillTime);
                            blocks[column + 1, y] = currentBlock;
                            blocks[column, y] = SpawnNewBlock(column, y, BlockType.EMPTY); // Empty the current position
                            movedPiece = true;
                        }
                    }
                }
            }

            return movedPiece;
        }

       

        
        private IEnumerator PerformFillHorizontal(float fillTime)
        {
            while (true)
            {
                if (FillHorizontal() == false)
                    yield break;

                yield return waitForSeconds;
            }
        }
        public bool FillHorizontal()
        {
            bool movedPiece = false;

            // Iterate through the grid from top to bottom and left to right
            for (int y = height - 1; y >= 1; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    Block piece = blocks[x, y];

                    if (piece.IsMoveable())
                    {
                        Block nbAbove = blocks[x, y - 1];  // Check the block above

                        if (nbAbove.Type == BlockType.EMPTY)
                        {
                            Destroy(nbAbove.gameObject);

                            piece.MoveableBlock.Move(x, y - 1, fillTime);
                            blocks[x, y - 1] = piece;
                            SpawnNewBlock(x, y, BlockType.EMPTY);
                            movedPiece = true;
                        }
                    }
                }
            }

            // Fill the top row with new blocks if it's empty
            for (int x = 0; x < width; x++)
            {
                Block nbAbove = blocks[x, height - 1];  // Check the top row

                if (nbAbove.Type == BlockType.EMPTY)
                {
                    Destroy(nbAbove.gameObject);

                    Block newPiece = Instantiate(blockPrefabDict[BlockType.NORMAL], GetWorldPosition(x, height), Quaternion.identity).GetComponent<Block>();
                    newPiece.transform.parent = this.transform;

                    blocks[x, height - 1] = newPiece;
                    blocks[x, height - 1].Init(x, height, BlockType.NORMAL);
                    blocks[x, height - 1].MoveableBlock.Move(x, height - 1, fillTime);
                    blocks[x, height - 1].ColorBlock.SetColor(Utilities.GetRandomEnum<ColorBlock.ColorType>());

                    movedPiece = true;
                }
            }

            return movedPiece;
        }


        public bool MoveBlocksDown()
        {
            bool movedPiece = false;

            // Iterate through the grid from top to bottom and left to right
            for (int y = 1; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Block piece = blocks[x, y];

                    if (piece.IsMoveable())
                    {
                        Block nbAbove = blocks[x, y - 1];  // Check the block above

                        if (nbAbove.Type == BlockType.EMPTY)
                        {
                            Destroy(nbAbove.gameObject);

                            piece.MoveableBlock.Move(x, y - 1, fillTime);
                            blocks[x, y - 1] = piece;
                            blocks[x, y] = SpawnNewBlock(x, y, BlockType.EMPTY); // Empty the current position
                            movedPiece = true;
                        }
                    }
                }
            }

            return movedPiece;
        }
        public IEnumerator PerformMoveBlockDown(System.Action OnFinished)
        {
            while (true)
            {
                if (MoveBlocksDown() == false)
                {
                    OnFinished?.Invoke();
                    yield break;
                }                 
                yield return waitForSeconds;
            }

            
        }


        public Vector2 GetWorldPosition(int x, int y)
        {
            return new Vector2(transform.position.x - width / 2f + x, transform.position.y - height / 2f + y);
        }


        public Block SpawnNewBlock(int x, int y, BlockType type)
        {
            var newPiece = Instantiate(blockPrefabDict[type]);
            newPiece.transform.parent = this.transform;

            blocks[x, y] = newPiece.GetComponent<Block>();
            blocks[x, y].Init(x, y, type);

            return blocks[x, y];
        }
 

        public bool ClearPiece(int x, int y)
        {
            if (blocks[x, y].IsClearable() && blocks[x, y].ClearableBlock.IsBeingCleared == false)
            {
                blocks[x, y].ClearableBlock.Clear();
                SpawnNewBlock(x, y, BlockType.EMPTY);

                //MoveBlocksDown();
                return true;
            }

            return false;
        }


        #region Utilities
        public List<Block> FindConnectedGroup(int x, int y, ColorBlock.ColorType targetColor)
        {
            List<Block> connectedGroup = new List<Block>();
            bool[,] visited = new bool[width, height];

            // Define the relative positions of neighboring blocks (up, down, left, right)
            int[][] directions = new int[][] {
            new int[] { 0, 1 },   // Down
            new int[] { 0, -1 },  // Up
            new int[] { -1, 0 },  // Left
            new int[] { 1, 0 }    // Right
                };

            // DFS function to recursively find connected blocks
            void DFS(int currX, int currY)
            {
                visited[currX, currY] = true;
                connectedGroup.Add(blocks[currX, currY]);

                foreach (var dir in directions)
                {
                    int newX = currX + dir[0];
                    int newY = currY + dir[1];

                    // Check if the new position is within the grid boundaries
                    if (newX >= 0 && newX < width && newY >= 0 && newY < height && blocks[newX, newY].Type == BlockType.NORMAL &&
                        !visited[newX, newY] && blocks[newX, newY].ColorBlock.Color == targetColor)
                    {
                        DFS(newX, newY);
                    }
                }
            }

            // Start DFS from the specified position (x, y)
            if (x >= 0 && x < width && y >= 0 && y < height && blocks[x, y].ColorBlock.Color == targetColor)
            {
                DFS(x, y);
            }

            return connectedGroup;
        }
        #endregion
    }
}

