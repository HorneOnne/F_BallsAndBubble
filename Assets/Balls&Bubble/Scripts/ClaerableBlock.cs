using UnityEngine;

namespace BallsAndBubble
{
    public class ClaerableBlock : MonoBehaviour
    {
        protected Block piece;


        public bool IsBeingCleared { get; set; } = false;
        private void Awake()
        {
            piece = GetComponent<Block>();
        }

        public void Clear()
        {
            IsBeingCleared = true;         
            Destroy(this.gameObject);
        }
    }
}

