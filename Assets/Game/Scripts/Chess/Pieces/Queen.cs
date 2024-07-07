using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Chess.Pieces
{
    public class Queen : Piece
    {
        public override List<Vector2Int> GetPossibleMoves(Vector2Int currentPosition, Piece[,] board)
        {
            List<Vector2Int> moves = new List<Vector2Int>();

            // vertical and horizontal
            Vector2Int[] rookDirections = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            foreach (Vector2Int direction in rookDirections)
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

            // diagonals
            Vector2Int[] bishopDirections =
                { Vector2Int.up + Vector2Int.left, Vector2Int.up + Vector2Int.right, Vector2Int.down + Vector2Int.left, Vector2Int.down + Vector2Int.right };
            foreach (Vector2Int direction in bishopDirections)
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