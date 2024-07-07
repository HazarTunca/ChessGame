using Game.Scripts.Chess;
using Game.Scripts.Chess.Pieces;
using UnityEngine;

namespace Game.Scripts
{
    public class InputManager : MonoBehaviour
    {
        public ChessBoard chessBoard;
        Vector2Int? selectedPosition;

        void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            
            var mousePosScreen = Input.mousePosition;
            mousePosScreen.z = 10;

            Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);
            Vector2Int boardPosition = new Vector2Int(Mathf.RoundToInt(mousePositionWorld.x), Mathf.RoundToInt(mousePositionWorld.y));

            if (!Piece.IsWithinBoard(boardPosition)) return;
            if (selectedPosition.HasValue)
            {
                chessBoard.MovePiece(selectedPosition.Value, boardPosition);
                selectedPosition = null;
                chessBoard.ClearHighlights();
                return;
            }

            Piece selectedPiece = chessBoard.board[boardPosition.x, boardPosition.y];
            if (selectedPiece == null || selectedPiece.isWhite != chessBoard.isWhiteTurn) return;
                    
            selectedPosition = boardPosition;
            chessBoard.HighlightPossibleMoves(boardPosition);
        }
    }
}