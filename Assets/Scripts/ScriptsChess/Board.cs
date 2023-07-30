using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    #region Variables

    public BoardSquare[,] BoardSquares = new BoardSquare[8, 8];
    public BoardSquare SquarePrefab;
    public ChessPiece PiecePrefab;
    public int Row;
    public int Col;
    public Transform StartingPosition;
    public List<ChessPiece> Pieces = new();

    #endregion

    #region Initialize

    private void Awake()
    {
        CreateBoard();
    }

    public void CreateBoard()
    {
        int squaresCreated = 1;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                BoardSquare clone = Instantiate(SquarePrefab, new Vector3(StartingPosition.position.x - i,
                                                        StartingPosition.position.y,
                                                        StartingPosition.position.z + j),
                                                        Quaternion.identity);
                clone.SetInitValues(i, j, IsWhite(squaresCreated));
                BoardSquares[i, j] = clone;
                squaresCreated++;
                if (i == 0 || i == 1 || i == 6 || i == 7)
                {
                    CreatePiece(i, j);
                }

            }
            squaresCreated++;
        }
    }

    public void CreatePiece(int i, int j)
    {
        ChessPiece clone = Instantiate(PiecePrefab, new Vector3(StartingPosition.position.x - i,
                                                        StartingPosition.position.y + 0.51f,
                                                        StartingPosition.position.z + j),
                                                        Quaternion.identity);
        clone.transform.Rotate(90, -90, 0, Space.Self);
        if (i == 1)
        {
            clone.SetInitValues(ChessPiece.PieceType.Pawn, BoardSquares[i, j], true, 10);
        }
        if (i == 6)
        {
            clone.SetInitValues(ChessPiece.PieceType.Pawn, BoardSquares[i, j], false, 10);
        }
        if (i == 0)
        {
            if (j == 0 || j == 7)
            {
                clone.SetInitValues(ChessPiece.PieceType.Rook, BoardSquares[i, j], true, 50);
            }
            if (j == 1 || j == 6)
            {
                clone.SetInitValues(ChessPiece.PieceType.Knight, BoardSquares[i, j], true, 30);
            }
            if (j == 2 || j == 5)
            {
                clone.SetInitValues(ChessPiece.PieceType.Bishop, BoardSquares[i, j], true, 30);
            }
            if (j == 3)
            {
                clone.SetInitValues(ChessPiece.PieceType.Queen, BoardSquares[i, j], true, 90);
            }
            if (j == 4)
            {
                clone.SetInitValues(ChessPiece.PieceType.King, BoardSquares[i, j], true, 900);
            }
        }
        if (i == 7)
        {
            if (j == 0 || j == 7)
            {
                clone.SetInitValues(ChessPiece.PieceType.Rook, BoardSquares[i, j], false, 50);
            }
            if (j == 1 || j == 6)
            {
                clone.SetInitValues(ChessPiece.PieceType.Knight, BoardSquares[i, j], false, 30);
            }
            if (j == 2 || j == 5)
            {
                clone.SetInitValues(ChessPiece.PieceType.Bishop, BoardSquares[i, j], false, 30);
            }
            if (j == 3)
            {
                clone.SetInitValues(ChessPiece.PieceType.Queen, BoardSquares[i, j], false, 90);
            }
            if (j == 4)
            {
                clone.SetInitValues(ChessPiece.PieceType.King, BoardSquares[i, j], false, 900);
            }
        }
        Pieces.Add(clone);
        BoardSquares[i, j].PopulateSquare(clone);
    }

    private bool IsWhite(int squareIndex)
    {
        int remains = squareIndex % 2;
        if (remains == 0)
        {
            return true;
        }
        return false;
    }

    #endregion
}
