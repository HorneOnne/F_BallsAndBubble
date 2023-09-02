using UnityEngine;

namespace BallsAndBubble
{
    public class GamePiece : MonoBehaviour
    {
        private int x;
        private int y;
        private GridSystem.PieceType type;
        
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }  
        public GridSystem.PieceType Type { get => type; }
        public MoveablePiece MoveablePiece { get; private set; }
        public ColorPiece ColorPiece { get; private set; }
        public ClearablePiece ClearablePiece { get; private set; }


        private void Awake()
        {
            MoveablePiece = GetComponent<MoveablePiece>();
            ColorPiece = GetComponent<ColorPiece>();
            ClearablePiece = GetComponent<ClearablePiece>();
        }


        public void Init(int x, int y, GridSystem.PieceType type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }

        public bool IsMoveable()
        {
            return MoveablePiece != null;
        }

        public bool IsColored()
        {
            return ColorPiece != null;
        }

        public bool IsClearable()
        {
            return ClearablePiece != null;
        }

        private void OnMouseDown()
        {
            Debug.Log("Mouse down");
            GridSystem.Instance.ClearPiece(x,y);
            StartCoroutine(GridSystem.Instance.PerformFill2(0.1f));
        }
    }
}

