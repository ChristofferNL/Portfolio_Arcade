using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessAI : MonoBehaviour
{
    public List<ChessPiece> PiecesWithMoves = new();

    public void GetPiecesWithMoves(Board board)
    {
        PiecesWithMoves.Clear();
        foreach (ChessPiece piece in board.Pieces)
        {
            if (piece.MovableSquares.Count > 0 && !piece.IsWhite)
            {
                PiecesWithMoves.Add(piece);
            }
        }
        if (PiecesWithMoves.Count > 0)
        {
            GetStrongestMove(board);
        }
    }

    public int SquareUnderAttack(BoardSquare square, Board board)
    {
        int lowestValueAttacker = 0;
        foreach (ChessPiece pieceToCheck in board.Pieces)
        {
            for (int i = 0; i < pieceToCheck.GuardedSquares.Count; i++)
            {
                if (pieceToCheck.GuardedSquares.Contains(square) && pieceToCheck.IsWhite)
                {
                    if (lowestValueAttacker == 0 || lowestValueAttacker != 0 && pieceToCheck.PieceValue < lowestValueAttacker)
                    {
                        lowestValueAttacker = pieceToCheck.PieceValue;
                    }
                }
            }
        }
        return lowestValueAttacker;
    }

    public bool SquareIsGuarded(BoardSquare square, Board board, ChessPiece piece)
    {
        foreach (ChessPiece pieceToCheck in board.Pieces)
        {
            for (int i = 0; i < pieceToCheck.GuardedSquares.Count; i++)
            {
                if (pieceToCheck.GuardedSquares.Contains(square) && !pieceToCheck.IsWhite && pieceToCheck != piece)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void GetStrongestMove(Board board)
    {
        BoardSquare squareToMove = null;
        ChessPiece pieceToMove = null;
        int currentHigestValue = 0;
        int startValue = 0;

        for (int i = 0; i < PiecesWithMoves.Count; i++)
        {
            if (SquareUnderAttack(PiecesWithMoves[i].CurrentSquare, board) > 0)
            {
                startValue = PiecesWithMoves[i].PieceValue;
            }

            for (int j = 0; j < PiecesWithMoves[i].MovableSquares.Count; j++)
            {
                int squareValue = startValue;
                if (SquareUnderAttack(PiecesWithMoves[i].MovableSquares[j], board) > 0)
                {
                    squareValue -= PiecesWithMoves[i].PieceValue;
                }
                if (PiecesWithMoves[i].MovableSquares[j].State == BoardSquare.SquareState.OccupiedWhite)
                {
                    switch (PiecesWithMoves[i].MovableSquares[j].CurrentPiece.Type)
                    {
                        case ChessPiece.PieceType.Pawn:
                            squareValue += 11;
                            if (squareValue > currentHigestValue)
                            {
                                currentHigestValue = squareValue;
                                squareToMove = PiecesWithMoves[i].MovableSquares[j];
                                pieceToMove = PiecesWithMoves[i];
                            }
                            break;
                        case ChessPiece.PieceType.Knight:
                            squareValue += 31;
                            if (squareValue > currentHigestValue)
                            {
                                currentHigestValue = squareValue;
                                squareToMove = PiecesWithMoves[i].MovableSquares[j];
                                pieceToMove = PiecesWithMoves[i];
                            }
                            break;
                        case ChessPiece.PieceType.Bishop:
                            squareValue += 31;
                            if (squareValue > currentHigestValue)
                            {
                                currentHigestValue = squareValue;
                                squareToMove = PiecesWithMoves[i].MovableSquares[j];
                                pieceToMove = PiecesWithMoves[i];
                            }
                            break;
                        case ChessPiece.PieceType.Rook:
                            squareValue += 51;
                            if (squareValue > currentHigestValue)
                            {
                                currentHigestValue = squareValue;
                                squareToMove = PiecesWithMoves[i].MovableSquares[j];
                                pieceToMove = PiecesWithMoves[i];
                            }
                            break;
                        case ChessPiece.PieceType.Queen:
                            squareValue += 91;
                            if (squareValue > currentHigestValue)
                            {
                                currentHigestValue = squareValue;
                                squareToMove = PiecesWithMoves[i].MovableSquares[j];
                                pieceToMove = PiecesWithMoves[i];
                            }
                            break;
                        case ChessPiece.PieceType.King:
                            squareValue += 900;
                            if (squareValue > currentHigestValue)
                            {
                                currentHigestValue = squareValue;
                                squareToMove = PiecesWithMoves[i].MovableSquares[j];
                                pieceToMove = PiecesWithMoves[i];
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (squareValue > currentHigestValue)
                    {
                        currentHigestValue = squareValue;
                        squareToMove = PiecesWithMoves[i].MovableSquares[j];
                        pieceToMove = PiecesWithMoves[i];
                    }
                }
            }
        }
        if (currentHigestValue == 0)
        {
            GetRandomMove();
            Debug.Log("Random Move");
        }
        else
        {
            Debug.Log(currentHigestValue);
            ExecuteMove(pieceToMove, squareToMove);
        }
    }

    public void GetRandomMove()
    {
        int randomNumber = Random.Range(0, PiecesWithMoves.Count);
        ChessPiece pieceToMove = PiecesWithMoves[randomNumber];
        int randomMoveNumber = Random.Range(0, pieceToMove.MovableSquares.Count);
        BoardSquare squareToMove = pieceToMove.MovableSquares[randomMoveNumber];
        ExecuteMove(pieceToMove, squareToMove);
    }

    public void ExecuteMove(ChessPiece piece, BoardSquare square)
    {
        GameEngine.Instance.GetActivePiece(piece);
        piece.CurrentSquare.State = BoardSquare.SquareState.Empty;
        piece.CurrentSquare.CurrentPiece = null;
        GameEngine.Instance.DropActivePiece(square);
    }

}
