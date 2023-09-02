using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using UnityEngine;

namespace BallsAndBubble
{
    public class GridSystem : MonoBehaviour
    {
        public static GridSystem Instance { get; private set; }

        public enum PieceType
        {
            EMPTY,
            NORMAL,
            COUNT
        }

        [System.Serializable]
        public struct PiecePrefab
        {
            public PieceType Type;
            public GameObject Prefab;
        };

        public int xDim;
        public int yDim;

        public PiecePrefab[] PiecePrefabs;
        public GameObject BackgroundPrefab;

        private Dictionary<PieceType, GameObject> piecePrefabDict;
        private GamePiece[,] pieces;

        private float fillTime = 0.02f;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            piecePrefabDict = new Dictionary<PieceType, GameObject>();

            for (int i = 0; i < PiecePrefabs.Length; i++)
            {
                if (piecePrefabDict.ContainsKey(PiecePrefabs[i].Type) == false)
                {
                    piecePrefabDict.Add(PiecePrefabs[i].Type, PiecePrefabs[i].Prefab);
                }
            }

            //for(int x = 0; x < xDim; x++)
            //{
            //    for(int y = 0; y < yDim; y++)
            //    {
            //        GameObject background = Instantiate(BackgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
            //        background.transform.parent = this.transform;
            //    }
            //}


            pieces = new GamePiece[xDim, yDim];
            for (int x = 0; x < xDim; x++)
            {
                for (int y = 0; y < yDim; y++)
                {

                    SpawnNewPiece(x, y, PieceType.EMPTY);


                }
            }

            //Fill();
            StartCoroutine(PerformFill(0.02f));
        }


        public Vector2 GetWorldPosition(int x, int y)
        {
            return new Vector2(transform.position.x - xDim / 2f + x, transform.position.y + yDim / 2f - y);
        }


        public GamePiece SpawnNewPiece(int x, int y, PieceType type)
        {
            var newPiece = Instantiate(piecePrefabDict[type]);
            newPiece.transform.parent = this.transform;

            pieces[x, y] = newPiece.GetComponent<GamePiece>();
            pieces[x, y].Init(x, y, type);

            return pieces[x, y];
        }


        public void Fill()
        {
            while (true)
            {
                if (FillStep() == false)
                    break;
            }
        }

        public IEnumerator PerformFill(float time)
        {
            while (true)
            {
                if (FillStep() == false)
                    yield break;

                yield return new WaitForSeconds(fillTime);
            }
        }

        public IEnumerator PerformFill2(float time)
        {
            while (true)
            {
                if (FillStep2() == false)
                    yield break;

                yield return new WaitForSeconds(fillTime);
            }
        }

        public bool FillStep2()
        {
            bool movedPiece = false;

            for (int y = yDim - 2; y >= 0; y--)
            {
                for (int x = 0; x < xDim; x++)
                {
                    GamePiece piece = pieces[x, y];

                    if (piece.IsMoveable())
                    {
                        GamePiece nbBelow = pieces[x, y + 1];
                        if (nbBelow.Type == PieceType.EMPTY)
                        {
                            Destroy(nbBelow.gameObject);

                            piece.MoveablePiece.Move(x, y + 1, fillTime);
                            pieces[x, y + 1] = piece;
                            SpawnNewPiece(x, y, PieceType.EMPTY);
                            movedPiece = true;
                        }
                    }
                }

            }
            for (int x = 0; x < xDim; x++)
            {
                GamePiece nbBelow = pieces[x, 0];

                if (nbBelow.Type == PieceType.EMPTY)
                {
                    Destroy(nbBelow.gameObject);

                    GamePiece newPiece = Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity).GetComponent<GamePiece>();
                    newPiece.transform.parent = this.transform;

                    pieces[x, 0] = newPiece;
                    pieces[x, 0].Init(x, -1, PieceType.NORMAL);
                    pieces[x, 0].MoveablePiece.Move(x, 0, fillTime);
                    pieces[x, 0].ColorPiece.SetColor(Utilities.GetRandomEnum<ColorPiece.ColorType>());

                    movedPiece = true;
                }
            }

            return movedPiece;
        }

        public bool FillStep()
        {
            bool movedPiece = false;

            for (int y = yDim - 2; y >= 0; y--)
            {
                for (int x = 0; x < xDim; x++)
                {
                    GamePiece piece = pieces[x, y];

                    if (piece.IsMoveable())
                    {
                        GamePiece nbBelow = pieces[x, y + 1];
                        if (nbBelow.Type == PieceType.EMPTY)
                        {
                            Destroy(nbBelow.gameObject);

                            piece.MoveablePiece.Move(x, y + 1, fillTime);
                            pieces[x, y + 1] = piece;
                            SpawnNewPiece(x, y, PieceType.EMPTY);
                            movedPiece = true;
                        }
                    }
                }
            }

            for (int x = 0; x < xDim; x++)
            {
                GamePiece nbBelow = pieces[x, 0];

                if (nbBelow.Type == PieceType.EMPTY)
                {
                    Destroy(nbBelow.gameObject);

                    GamePiece newPiece = Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity).GetComponent<GamePiece>();
                    newPiece.transform.parent = this.transform;

                    pieces[x, 0] = newPiece;
                    pieces[x, 0].Init(x, -1, PieceType.NORMAL);
                    pieces[x, 0].MoveablePiece.Move(x, 0, fillTime);
                    pieces[x, 0].ColorPiece.SetColor(Utilities.GetRandomEnum<ColorPiece.ColorType>());

                    movedPiece = true;
                }
            }

            return movedPiece;
        }


        public bool IsAdjacent(GamePiece piece1, GamePiece piece2)
        {
            return (piece1.X == piece2.X && (int)Mathf.Abs(piece1.Y - piece2.Y) == 1 ||
                    piece1.Y == piece2.Y && (int)Mathf.Abs(piece1.X - piece2.X) == 1);
        }

        public List<GamePiece> GetMatch(GamePiece piece, int newX, int newY)
        {
            if(piece.IsColored())
            {
                ColorPiece.ColorType color = piece.ColorPiece.Color;
                List<GamePiece> horizontalPieces = new List<GamePiece>();
                List<GamePiece> verticalPieces = new List<GamePiece>();
                List<GamePiece> matchingPieces = new List<GamePiece>();

                // First check horizontal
                horizontalPieces.Add(piece);
                for(int dir = 0; dir <= 1; dir++)
                {
                    for(int xOffset = 1; xOffset < xDim; xOffset++)
                    {
                        int x;

                        if (dir == 0)   // Left
                        {
                            x = newX - xOffset;
                        }
                        else // Right
                        {
                            x = newX + xOffset;
                        }

                        if (x < 0 || x >= xDim)
                            break;

                        if (pieces[x, newY].IsColored() && pieces[x, newY].ColorPiece.Color == color)
                        {
                            horizontalPieces.Add(pieces[x, newY]);
                        }
                        else
                        {
                            break;
                        }

                    }
                }

                if(horizontalPieces.Count >= 2)
                {
                    for(int i = 0; i < horizontalPieces.Count; i++)
                    {
                        matchingPieces.Add(horizontalPieces[i]);
                    }
                }

                if(matchingPieces.Count >= 2)
                {
                    return matchingPieces;
                }


                // Check vertically
                horizontalPieces.Add(piece);
                for (int dir = 0; dir <= 1; dir++)
                {
                    for (int yOffset = 1; yOffset < yDim; yOffset++)
                    {
                        int y;

                        if (dir == 0)   // Up
                        {
                            y = newY - yOffset;
                        }
                        else // Down
                        {
                            y = newY + yOffset;
                        }

                        if (y < 0 || y >= yDim)
                            break;

                        if (pieces[newX, y].IsColored() && pieces[newX, y].ColorPiece.Color == color)
                        {
                            verticalPieces.Add(pieces[newX, y]);
                        }
                        else
                        {
                            break;
                        }

                    }
                }

                if (verticalPieces.Count >= 2)
                {
                    for (int i = 0; i < verticalPieces.Count; i++)
                    {
                        matchingPieces.Add(verticalPieces[i]);
                    }
                }

                if (verticalPieces.Count >= 2)
                {
                    return matchingPieces;
                }
            }

            return null;
        }


        public bool ClearAllValidMatches()
        {
            bool needsRefill = false;
            for(int y = 0; y < yDim; y++)
            {
                for(int x = 0; x < xDim; x++)
                {
                    if (pieces[x, y].IsClearable())
                    {
                        List<GamePiece> matches = GetMatch(pieces[x, y], x, y);

                        if (matches != null)
                        {
                            for(int i = 0; i < matches.Count; i++)
                            {
                                if(ClearPiece(matches[i].X, matches[i].Y))
                                {
                                    needsRefill = true;
                                }
                            }
                        }
                    }
                }
            }

            return needsRefill;
        }

        public bool ClearPiece(int x, int y)
        {
            if (pieces[x,y].IsClearable() && pieces[x,y].ClearablePiece.IsBeingCleared == false)
            {
                pieces[x, y].ClearablePiece.Clear();
                SpawnNewPiece(x, y, PieceType.EMPTY);

                return true;
            }

            return false;
        }
    }
}

