using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Chess.Pieces
{
    public abstract class Piece : MonoBehaviour
    {
        public bool isWhite;
        public bool hasMoved;

        public abstract List<Vector2Int> GetPossibleMoves(Vector2Int currentPosition, Piece[,] board);
    
        public virtual bool CanMoveTo(Vector2Int targetPosition, Piece[,] board)
        {
            List<Vector2Int> possibleMoves = GetPossibleMoves(new Vector2Int((int)transform.position.x, (int)transform.position.y), board);
            return possibleMoves.Contains(targetPosition);
        }

        public virtual void Move(Vector2Int targetPosition)
        {
            transform.position = new Vector3(targetPosition.x, targetPosition.y, 0);
            hasMoved = true;
        }
        
        public static bool IsWithinBoard(Vector2Int position)
        {
            return position.x >= 0 && position.x < 8 && position.y >= 0 && position.y < 8;
        }
    }
}