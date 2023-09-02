using UnityEngine;

namespace BallsAndBubble
{
    public class Block : MonoBehaviour
    {
        public static event System.Action<Block> OnBlockClicked;

        private int x;
        private int y;
        private GridSystem.BlockType type;
        
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }  
        public GridSystem.BlockType Type { get => type; }
        public MoveableBlock MoveableBlock { get; private set; }
        public ColorBlock ColorBlock { get; private set; }
        public ClaerableBlock ClearableBlock { get; private set; }


        private void Awake()
        {
            MoveableBlock = GetComponent<MoveableBlock>();
            ColorBlock = GetComponent<ColorBlock>();
            ClearableBlock = GetComponent<ClaerableBlock>();
        }


        public void Init(int x, int y, GridSystem.BlockType type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }

        public bool IsMoveable()
        {
            return MoveableBlock != null;
        }

        public bool IsColored()
        {
            return ColorBlock != null;
        }

        public bool IsClearable()
        {
            return ClearableBlock != null;
        }

        private void OnMouseDown()
        {
            if(GameplayManager.Instance.CurrentState == GameplayManager.GameState.PLAYING)
            {
                OnBlockClicked?.Invoke(this);
            }
        }
    }
}

