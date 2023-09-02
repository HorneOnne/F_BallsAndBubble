using UnityEngine;

namespace BallsAndBubble
{
    public class ClearablePiece : MonoBehaviour
    {
        protected GamePiece piece;


        public bool IsBeingCleared { get; set; } = false;
        private void Awake()
        {
            piece = GetComponent<GamePiece>();
        }

        public void Clear()
        {
            IsBeingCleared = true;
            Destroy(this.gameObject);
            Debug.Log("clear");
        }
    }
}

