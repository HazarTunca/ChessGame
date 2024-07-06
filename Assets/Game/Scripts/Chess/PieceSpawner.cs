using Game.Scripts.Chess.Pieces;
using UnityEngine;

namespace Game.Scripts.Chess
{
    public class PieceSpawner : MonoBehaviour
    {
        public ChessBoard chessBoard;
        
        [Space(10)]
        public GameObject whitePawnPrefab;
        public GameObject blackPawnPrefab;

        public GameObject whiteRookPrefab;
        public GameObject blackRookPrefab;

        public GameObject whiteKnightPrefab;
        public GameObject blackKnightPrefab;

        public GameObject whiteBishopPrefab;
        public GameObject blackBishopPrefab;

        public GameObject whiteQueenPrefab;
        public GameObject blackQueenPrefab;

        public GameObject whiteKingPrefab;
        public GameObject blackKingPrefab;
        
        void Awake()
        {
            // pawn
            for (int i = 0; i < 8; i++)
            {
                CreatePiece(chessBoard, whitePawnPrefab, new Vector3(i, 1, 0), true);
                CreatePiece(chessBoard, blackPawnPrefab, new Vector3(i, 6, 0), false);
            }

            // rook
            CreatePiece(chessBoard, whiteRookPrefab, new Vector3(0, 0, 0), true);
            CreatePiece(chessBoard, whiteRookPrefab, new Vector3(7, 0, 0), true);
            CreatePiece(chessBoard, blackRookPrefab, new Vector3(0, 7, 0), false);
            CreatePiece(chessBoard, blackRookPrefab, new Vector3(7, 7, 0), false);

            // knight
            CreatePiece(chessBoard, whiteKnightPrefab, new Vector3(1, 0, 0), true);
            CreatePiece(chessBoard, whiteKnightPrefab, new Vector3(6, 0, 0), true);
            CreatePiece(chessBoard, blackKnightPrefab, new Vector3(1, 7, 0), false);
            CreatePiece(chessBoard, blackKnightPrefab, new Vector3(6, 7, 0), false);

            // bishop
            CreatePiece(chessBoard, whiteBishopPrefab, new Vector3(2, 0, 0), true);
            CreatePiece(chessBoard, whiteBishopPrefab, new Vector3(5, 0, 0), true);
            CreatePiece(chessBoard, blackBishopPrefab, new Vector3(2, 7, 0), false);
            CreatePiece(chessBoard, blackBishopPrefab, new Vector3(5, 7, 0), false);

            // queen
            CreatePiece(chessBoard, whiteQueenPrefab, new Vector3(3, 0, 0), true);
            CreatePiece(chessBoard, blackQueenPrefab, new Vector3(3, 7, 0), false);

            // king
            chessBoard.whiteKing = CreatePiece(chessBoard, whiteKingPrefab, new Vector3(4, 0, 0), true);
            chessBoard.blackKing = CreatePiece(chessBoard, blackKingPrefab, new Vector3(4, 7, 0), false);
        }
        
        public static GameObject CreatePiece(ChessBoard chessBoard, GameObject prefab, Vector3 position, bool isWhite)
        {
            GameObject piece = Instantiate(prefab, position, Quaternion.identity);
            piece.GetComponent<Piece>().isWhite = isWhite;

            int x = (int)position.x;
            int y = (int)position.y;

            chessBoard.board[x, y] = piece.GetComponent<Piece>();
            piece.transform.position = position;

            return piece;
        }
    }
}