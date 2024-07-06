using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Chess.Pieces
{
    public class Rook : Piece
    {
        public override List<Vector2Int> GetPossibleMoves(Vector2Int currentPosition, Piece[,] board)
        {
            List<Vector2Int> moves = new List<Vector2Int>();
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach (Vector2Int direction in directions)
            {
                for (int i = 1; i < 8; i++)
                {
                    Vector2Int targetPosition = currentPosition + direction * i;
                    if (targetPosition.x < 0 || targetPosition.x >= 8 || targetPosition.y < 0 || targetPosition.y >= 8)
                        break;
                    
                    Piece targetPiece = board[targetPosition.x, targetPosition.y];
                    if (targetPiece == null)
                    {
                        moves.Add(targetPosition);
                    }
                    else
                    {
                        if (targetPiece.isWhite != isWhite)
                        {
                            moves.Add(targetPosition);
                        }
                        break;
                    }
                }
            }

            return moves;
        }
    }
}