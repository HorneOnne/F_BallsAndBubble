using System.Collections;
using UnityEngine;

namespace BallsAndBubble
{
    public class MoveablePiece : MonoBehaviour
    {
        private GamePiece piece;
        private IEnumerator moveCoroutine;

        private void Awake()
        {
            piece = GetComponent<GamePiece>();
        }

        //public void Move(int newX, int newY)
        //{
        //    piece.X = newX;
        //    piece.Y = newY;

        //    piece.transform.localPosition = GridSystem.Instance.GetWorldPosition(newX, newY);
        //}

        public void Move(int newX, int newY,float moveTime)
        {
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = PerformMove(newX, newY, moveTime);
            StartCoroutine(moveCoroutine);
        }

        private IEnumerator PerformMove(int newX, int newY, float moveTime)
        {
            piece.X = newX;
            piece.Y = newY;

            Vector3 startPosition = transform.position;
            Vector3 endPosition = GridSystem.Instance.GetWorldPosition(newX, newY);

            for (float t = 0; t <= 1 * moveTime; t += Time.deltaTime)
            {
                piece.transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return 0;
            }

            piece.transform.position = endPosition;
        }
    }
}

