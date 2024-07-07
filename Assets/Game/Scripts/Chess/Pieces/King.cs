using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Chess.Pieces
{
    public class King : Piece {
        public override List<Vector2Int> GetPossibleMoves(Vector2Int currentPosition, Piece[,] board) {
            List<Vector2Int> moves = new List<Vector2Int>();

            Vector2Int[] directions = {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right,
                Vector2Int.up + Vector2Int.left,
                Vector2Int.up + Vector2Int.right,
                Vector2Int.down + Vector2Int.left,
                Vector2Int.down + Vector2Int.right
            };

            foreach (Vector2Int direction in directions) {
                Vector2Int targetPosition = currentPosition + direction;
                if (!IsWithinBoard(targetPosition)) continue;
                Piece targetPiece = board[targetPosition.x, targetPosition.y];
                if (targetPiece == null || targetPiece.isWhite != isWhite) {
                    moves.Add(targetPosition);
                }
            }

            return moves;
        }
    }
}