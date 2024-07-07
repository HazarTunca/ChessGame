using System;
using System.Collections.Generic;
using Game.Scripts.Chess.Pieces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Chess
{
    public class ChessBoard : MonoBehaviour
    {
        public PieceSpawner pieceSpawner;

        public Piece[,] board = new Piece[8, 8];
        public GameObject highlightPrefab;
        public bool isWhiteTurn = true;

        public GameObject whiteKing;
        public GameObject blackKing;

        readonly List<GameObject> highlights = new List<GameObject>();

        public void HighlightPossibleMoves(Vector2Int position)
        {
            ClearHighlights();

            Piece selectedPiece = board[position.x, position.y];
            if (selectedPiece == null || selectedPiece.isWhite != isWhiteTurn) return;

            List<Vector2Int> possibleMoves = selectedPiece.GetPossibleMoves(position, board);
            for (int i_move = 0; i_move < possibleMoves.Count; i_move++)
            {
                var move = possibleMoves[i_move];
                GameObject highlight = Instantiate(highlightPrefab, new Vector3(move.x, move.y, -0.1f), Quaternion.identity);
                highlights.Add(highlight);
            }
        }

        public void ClearHighlights()
        {
            int highlightCount = highlights.Count;
            for (int i_highlight = 0; i_highlight < highlightCount; i_highlight++)
            {
                Destroy(highlights[i_highlight]);
            }

            highlights.Clear();
        }

        public void MovePiece(Vector2Int from, Vector2Int to)
        {
            Debug.Assert(Piece.IsWithinBoard(from) && Piece.IsWithinBoard(to));

            Piece piece = board[from.x, from.y];
            if (piece == null || !piece.CanMoveTo(to, board)) return;

            Piece capturedPiece = board[to.x, to.y];
            if (capturedPiece != null) Destroy(capturedPiece.gameObject);

            // Move the piece
            board[to.x, to.y] = piece;
            board[from.x, from.y] = null;
            piece.Move(to);
            isWhiteTurn = !isWhiteTurn;

            // check if a king is captured
            if (capturedPiece is King)
            {
                if (capturedPiece.isWhite)
                {
                    whiteKing = null;
                }
                else
                {
                    blackKing = null;
                }
            }

            // promote pawn
            if (piece is Pawn && (to.y == 0 || to.y == 7))
            {
                Destroy(piece.gameObject);

                PieceSpawner.CreatePiece(this, piece.isWhite ? pieceSpawner.whiteQueenPrefab : pieceSpawner.blackQueenPrefab,
                    new Vector3(to.x, to.y, 0), piece.isWhite);
            }

            if (CheckGameEnd()) return;

            // let the AI move
            if (!isWhiteTurn)
            {
                Vector2Int bestMove = CalculateBestMove(3);
                MovePiece(new Vector2Int(bestMove.x / 8, bestMove.x % 8), new Vector2Int(bestMove.y / 8, bestMove.y % 8));
            }
        }

        Vector2Int CalculateBestMove(int depth)
        {
            int bestScore = int.MinValue;
            List<Vector2Int> bestMoves = new List<Vector2Int>();

            for (int fromX = 0; fromX < 8; fromX++)
            {
                for (int fromY = 0; fromY < 8; fromY++)
                {
                    Piece piece = board[fromX, fromY];
                    if (piece == null || piece.isWhite != isWhiteTurn) continue;

                    List<Vector2Int> moves = piece.GetPossibleMoves(new Vector2Int(fromX, fromY), board);
                    for (int i_move = 0; i_move < moves.Count; i_move++)
                    {
                        var to = moves[i_move];

                        Piece capturedPiece = board[to.x, to.y];
                        board[to.x, to.y] = piece;
                        board[fromX, fromY] = null;

                        int score = Minimax(depth - 1, false, int.MinValue, int.MaxValue);

                        board[fromX, fromY] = piece;
                        board[to.x, to.y] = capturedPiece;

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMoves.Clear();
                            bestMoves.Add(new Vector2Int(fromX * 8 + fromY, to.x * 8 + to.y));
                        }
                        else if (score == bestScore)
                        {
                            bestMoves.Add(new Vector2Int(fromX * 8 + fromY, to.x * 8 + to.y));
                        }
                    }
                }
            }

            if (bestMoves.Count <= 0) return new Vector2Int(-1, -1);

            List<Vector2Int> captureMoves = bestMoves.FindAll(move => board[(move.y / 8), (move.y % 8)] != null);
            return captureMoves.Count > 0 ? captureMoves[Random.Range(0, captureMoves.Count)] : bestMoves[Random.Range(0, bestMoves.Count)];
        }

        int Minimax(int depth, bool isMaximizingPlayer, int alpha, int beta)
        {
            if (depth == 0)
            {
                return EvaluateBoard();
            }

            if (!isMaximizingPlayer)
            {
                int maxEval = int.MinValue;
                for (int fromX = 0; fromX < 8; fromX++)
                {
                    for (int fromY = 0; fromY < 8; fromY++)
                    {
                        Piece piece = board[fromX, fromY];
                        if (piece != null && piece.isWhite == isWhiteTurn)
                        {
                            List<Vector2Int> moves = piece.GetPossibleMoves(new Vector2Int(fromX, fromY), board);
                            for (int i_move = 0; i_move < moves.Count; i_move++)
                            {
                                var to = moves[i_move];

                                Piece capturedPiece = board[to.x, to.y];
                                board[to.x, to.y] = piece;
                                board[fromX, fromY] = null;

                                int eval = Minimax(depth - 1, false, alpha, beta);
                                maxEval = Mathf.Max(maxEval, eval);
                                alpha = Mathf.Max(alpha, eval);

                                board[fromX, fromY] = piece;
                                board[to.x, to.y] = capturedPiece;

                                if (beta <= alpha) break;
                            }
                        }
                    }
                }

                return maxEval;
            }

            int minEval = int.MaxValue;
            for (int fromX = 0; fromX < 8; fromX++)
            {
                for (int fromY = 0; fromY < 8; fromY++)
                {
                    Piece piece = board[fromX, fromY];
                    if (piece != null && piece.isWhite != isWhiteTurn)
                    {
                        List<Vector2Int> moves = piece.GetPossibleMoves(new Vector2Int(fromX, fromY), board);
                        for (int i_move = 0; i_move < moves.Count; i_move++)
                        {
                            var to = moves[i_move];

                            Piece capturedPiece = board[to.x, to.y];
                            board[to.x, to.y] = piece;
                            board[fromX, fromY] = null;

                            int eval = Minimax(depth - 1, true, alpha, beta);
                            minEval = Mathf.Min(minEval, eval);
                            beta = Mathf.Min(beta, eval);

                            board[fromX, fromY] = piece;
                            board[to.x, to.y] = capturedPiece;

                            if (beta <= alpha) break;
                        }
                    }
                }
            }

            return minEval;
        }

        int EvaluateBoard()
        {
            int score = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = board[x, y];
                    if (piece == null) continue;
                    
                    int pieceValue = GetPieceValue(piece);
                    if (piece.isWhite) score += pieceValue;
                    else
                    {
                        score -= pieceValue;
                        // Bonus for attacking enemy pieces (for the being aggressive)
                        score -= GetAttackBonus(new Vector2Int(x, y));
                    }
                }
            }

            return score;
        }

        int GetPieceValue(Piece piece)
        {
            switch (piece)
            {
                case Pawn: return 10;
                case Rook: return 50;
                case Knight:
                case Bishop: return 30;
                case Queen: return 90;
                case King: return 900;
                default: return 0;
            }
        }

        int GetAttackBonus(Vector2Int position)
        {
            int bonus = 0;
            List<Vector2Int> attackedSquares = new List<Vector2Int>();

            Piece attacker = board[position.x, position.y];
            if (attacker != null)
            {
                attackedSquares = attacker.GetPossibleMoves(position, board);
            }

            // Check if any enemy pieces are under attack
            for (int i_square = 0; i_square < attackedSquares.Count; i_square++)
            {
                var square = attackedSquares[i_square];
                
                Piece target = board[square.x, square.y];
                if (target != null && target.isWhite)
                {
                    bonus += GetPieceValue(target) * 1000;
                }
            }

            return bonus;
        }

        bool CheckGameEnd()
        {
            // Check if either king is missing
            if (whiteKing == null || blackKing == null)
            {
                Debug.Log(whiteKing == null ? "Black wins!" : "White wins!");
                return true;
            }

            // Check if there are any possible moves left for the current player
            bool hasMoves = false;

            for (int fromX = 0; fromX < 8; fromX++)
            {
                for (int fromY = 0; fromY < 8; fromY++)
                {
                    Piece piece = board[fromX, fromY];
                    if (piece != null && piece.isWhite == isWhiteTurn)
                    {
                        List<Vector2Int> moves = piece.GetPossibleMoves(new Vector2Int(fromX, fromY), board);
                        if (moves.Count > 0)
                        {
                            hasMoves = true;
                            break;
                        }
                    }
                }

                if (hasMoves)
                    break;
            }

            if (!hasMoves)
            {
                Debug.Log(isWhiteTurn ? "Black wins by checkmate/stalemate!" : "White wins by checkmate/stalemate!");
                return true;
            }

            return false;
        }

        void OnDrawGizmos()  
        {
            Gizmos.color = Color.red;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Gizmos.DrawWireCube(new Vector3(x, y, 0), Vector3.one);
                }
            }
        }
    }
}