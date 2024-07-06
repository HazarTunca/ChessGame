using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Chess.Pieces
{
    public class Knight : Piece
    {
        public override List<Vector2Int> GetPossibleMoves(Vector2Int currentPosition, Piece[,] board)
        {
            List<Vector2Int> moves = new List<Vector2Int>();
            Vector2Int[] knightMoves = {
                new Vector2Int(2, 1),
                new Vector2Int(2, -1),
                new Vector2Int(-2, 1),
                new Vector2Int(-2, -1),
                new Vector2Int(1, 2),
                new Vector2Int(1, -2),
                new Vector2Int(-1, 2),
                new Vector2Int(-1, -2)
            };

            foreach (Vector2Int move in knightMoves)
            {
                Vector2Int targetPosition = currentPosition + move;
                if (IsWithinBoard(targetPosition))
                {
                    Piece targetPiece = board[targetPosition.x, targetPosition.y];
                    if (targetPiece == null || targetPiece.isWhite != isWhite)
                    {
                        moves.Add(targetPosition);
                    }
                }
            }

            return moves;
        }
    }
}