using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Chess.Pieces
{
    public class Pawn : Piece
    {
        public override List<Vector2Int> GetPossibleMoves(Vector2Int currentPosition, Piece[,] board)
        {
            List<Vector2Int> moves = new List<Vector2Int>();
            int direction = isWhite ? 1 : -1;

            Vector2Int forwardMove = currentPosition + new Vector2Int(0, direction);
            if (IsWithinBoard(forwardMove) && board[forwardMove.x, forwardMove.y] == null)
            {
                moves.Add(forwardMove);

                // Double move on first turn
                if (!hasMoved)
                {
                    Vector2Int doubleMove = currentPosition + new Vector2Int(0, 2 * direction);
                    if (IsWithinBoard(doubleMove) && board[doubleMove.x, doubleMove.y] == null)
                    {
                        moves.Add(doubleMove);
                    }
                }
            }

            Vector2Int[] captureMoves = new Vector2Int[]
            {
                currentPosition + new Vector2Int(-1, direction),
                currentPosition + new Vector2Int(1, direction)
            };

            foreach (Vector2Int captureMove in captureMoves)
            {
                if (IsWithinBoard(captureMove) && board[captureMove.x, captureMove.y] != null && 
                    board[captureMove.x, captureMove.y].isWhite != isWhite)
                {
                    moves.Add(captureMove);
                }
            }

            return moves;
        }
    }
}