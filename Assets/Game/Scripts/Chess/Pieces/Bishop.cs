using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Chess.Pieces
{
    public class Bishop : Piece
    {
        public override List<Vector2Int> GetPossibleMoves(Vector2Int currentPosition, Piece[,] board)
        {
            List<Vector2Int> moves = new List<Vector2Int>();
            Vector2Int[] directions = { 
                new Vector2Int(1, 1),   // up right
                new Vector2Int(1, -1),  // down right
                new Vector2Int(-1, 1),  // up left
                new Vector2Int(-1, -1)  // down left
            };

            foreach (Vector2Int direction in directions)
            {
                for (int i = 1; i < 8; i++)
                {
                    Vector2Int targetPosition = currentPosition + direction * i;
                    if (!IsWithinBoard(targetPosition)) break;

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