using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    #region Variables

    public BoardSquare CurrentSquare;
    public bool IsAlive { get; private set; }
    public bool IsWhite;
    public bool HasMoved;
    public bool HasBeenChecked;
    public bool CanCastleRight;
    public bool CanCastleLeft;
    public enum PieceType { Pawn, King, Queen, Rook, Knight, Bishop }
    public PieceType Type;
    public List<Sprite> WhiteSprites = new();
    public List<Sprite> BlackSprites = new();
    public List<BoardSquare> MovableSquares = new();
    public List<BoardSquare> GuardedSquares = new();
    public int PieceValue;
    private SpriteRenderer spriteRenderer;

    #endregion

    #region Initialize

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetInitValues(PieceType type, BoardSquare currentSquare, bool isWhite, int value)
    {
        Type = type;
        CurrentSquare = currentSquare;
        IsWhite = isWhite;
        PieceValue = value;
        SetSprite();
        if (isWhite)
        {
            name = type.ToString() + " - White";
        }
        else
        {
            name = type.ToString() + " - Black";
        }
    }
    public void SetSprite()
    {
        if (IsWhite)
        {
            spriteRenderer.sprite = WhiteSprites[(int)Type];
        }
        else
        {
            spriteRenderer.sprite = BlackSprites[(int)Type];
        }
    }

    #endregion

    #region Movement

    public void GetMovableSquares(Board board)
    {
        MovableSquares.Clear();
        GuardedSquares.Clear();
        switch (Type)
        {
            case PieceType.Pawn:
                if (IsWhite)
                {
                    GetWhitePawnMoves(board);
                }
                else
                {
                    GetBlackPawnMoves(board);
                }
                break;
            case PieceType.King:
                GetStraightMoves(board);
                GetDiagonalMoves(board);
                GetCastleMoves(board);
                break;
            case PieceType.Queen:
                GetStraightMoves(board);
                GetDiagonalMoves(board);
                break;
            case PieceType.Rook:
                GetStraightMoves(board);
                break;
            case PieceType.Knight:
                GetKnightMoves(board);
                break;
            case PieceType.Bishop:
                GetDiagonalMoves(board);
                break;
            default:
                break;
        }
    }

    public bool SquareIsMovable(int i, int j, Board board, bool canAttack)
    {
        int row = CurrentSquare.Row + i;
        int col = CurrentSquare.Col + j;

        if (row < 0 || row > 7 || col < 0 || col > 7)
        {
            return false;
        }

        BoardSquare squareToCheck = board.BoardSquares[row, col];

        if (squareToCheck.State == BoardSquare.SquareState.Empty)
        {
            if (canAttack && Type == PieceType.Pawn) // Making sure a pawn cannot attack a empty square
            {
                GuardedSquares.Add(squareToCheck);
                return false;
            }
            if (IsWhite && GameEngine.Instance.Player == GameEngine.ActivePlayer.White || !IsWhite && GameEngine.Instance.Player == GameEngine.ActivePlayer.Black)
            {
                if (GameEngine.Instance.TestDropPiece(squareToCheck, this))
                {
                    MovableSquares.Add(squareToCheck);
                }

            }
            if (Type != PieceType.Pawn)
            {
                GuardedSquares.Add(squareToCheck);
            }
            if (Type != PieceType.King && Type != PieceType.Knight && Type != PieceType.Pawn) // keep looking at the next square in the same direction
            {
                if (i > 0)
                {
                    i++;
                }
                if (i < 0)
                {
                    i--;
                }
                if (j > 0)
                {
                    j++;
                }
                if (j < 0)
                {
                    j--;
                }
                SquareIsMovable(i, j, board, canAttack);
            }
            return true;
        }
        else if (squareToCheck.State == BoardSquare.SquareState.OccupiedBlack && IsWhite && canAttack)
        {
            GuardedSquares.Add(squareToCheck);
            if (IsWhite && GameEngine.Instance.Player == GameEngine.ActivePlayer.White || !IsWhite && GameEngine.Instance.Player == GameEngine.ActivePlayer.Black)
            {
                if (GameEngine.Instance.TestDropPiece(squareToCheck, this))
                {
                    MovableSquares.Add(squareToCheck);
                    GuardedSquares.Add(squareToCheck);
                }
                else
                {
                    return false;
                }
            }
            if (squareToCheck.CurrentPiece.Type == PieceType.King && IsWhite && squareToCheck.CurrentPiece != this)
            {
                GameEngine.Instance.BlackKingAttacked = true;
            }
            return false;
        }
        else if (squareToCheck.State == BoardSquare.SquareState.OccupiedWhite && !IsWhite && canAttack)
        {
            GuardedSquares.Add(squareToCheck);
            if (IsWhite && GameEngine.Instance.Player == GameEngine.ActivePlayer.White || !IsWhite && GameEngine.Instance.Player == GameEngine.ActivePlayer.Black)
            {
                if (GameEngine.Instance.TestDropPiece(squareToCheck, this))
                {
                    MovableSquares.Add(squareToCheck);
                    GuardedSquares.Add(squareToCheck);
                }
                else
                {
                    return false;
                }
            }
            if (squareToCheck.CurrentPiece.Type == PieceType.King && !IsWhite && squareToCheck.CurrentPiece != this)
            {
                GameEngine.Instance.WhiteKingAttacked = true;
            }
            return false;
        }
        else if (canAttack)
        {
            GuardedSquares.Add(squareToCheck);
        }
        return false;
    }

    public void GetKnightMoves(Board board)
    {
        SquareIsMovable(2, -1, board, true);
        SquareIsMovable(2, 1, board, true);
        SquareIsMovable(1, -2, board, true);
        SquareIsMovable(1, 2, board, true);
        SquareIsMovable(-1, -2, board, true);
        SquareIsMovable(-1, 2, board, true);
        SquareIsMovable(-2, -1, board, true);
        SquareIsMovable(-2, 1, board, true);
    }

    public void GetStraightMoves(Board board)
    {
        SquareIsMovable(1, 0, board, true);
        SquareIsMovable(-1, 0, board, true);
        SquareIsMovable(0, 1, board, true);
        SquareIsMovable(0, -1, board, true);
    }

    public void GetDiagonalMoves(Board board)
    {
        SquareIsMovable(1, -1, board, true);
        SquareIsMovable(1, 1, board, true);
        SquareIsMovable(-1, -1, board, true);
        SquareIsMovable(-1, 1, board, true);
    }

    public void GetWhitePawnMoves(Board board)
    {
        if (CurrentSquare.Col != 'A')
        {
            SquareIsMovable(1, -1, board, true);
        }
        if (CurrentSquare.Col != 'H')
        {
            SquareIsMovable(1, +1, board, true);
        }

        if (SquareIsMovable(1, 0, board, false))
        {
            if (!HasMoved)
            {
                SquareIsMovable(2, 0, board, false);
            }
        }
    }
    public void GetBlackPawnMoves(Board board)
    {
        if (CurrentSquare.Col != 'A')
        {
            SquareIsMovable(-1, -1, board, true);
        }
        if (CurrentSquare.Col != 'H')
        {
            SquareIsMovable(-1, +1, board, true);
        }

        if (SquareIsMovable(-1, 0, board, false))
        {
            if (!HasMoved)
            {
                SquareIsMovable(-2, 0, board, false);
            }
        }
    }

    #endregion

    #region Castling
    public void GetCastleMoves(Board board)
    {
        WhiteKingCheckRightCastle(board);
        WhiteKingCheckLeftCastle(board);
        BlackKingCheckRightCastle(board);
        BlackKingCheckLeftCastle(board);
    }

    public void WhiteKingCheckRightCastle(Board board)
    {
        if (board.BoardSquares[0, 7].CurrentPiece == null)
        {
            CanCastleRight = false;
            return;
        }
        if (board.BoardSquares[0, 7].CurrentPiece.Type != PieceType.Rook)
        {
            CanCastleRight = false;
            return;
        }
        if (IsWhite && GameEngine.Instance.Player == GameEngine.ActivePlayer.White
                    && !HasMoved
                    && board.BoardSquares[0, 5].CurrentPiece == null
                    && board.BoardSquares[0, 6].CurrentPiece == null
                    && board.BoardSquares[0, 7].CurrentPiece.Type == PieceType.Rook
                    && board.BoardSquares[0, 7].CurrentPiece.HasMoved == false
                    && GameEngine.Instance.TestDropPiece(board.BoardSquares[0, 5], this)
                    && GameEngine.Instance.TestDropPiece(board.BoardSquares[0, 6], this)
                    && !GameEngine.Instance.WhiteKingChecked)
        {
            CanCastleRight = true;
        }
        else if (IsWhite)
        {
            CanCastleRight = false;
        }
    }

    public void WhiteKingCheckLeftCastle(Board board)
    {
        if (board.BoardSquares[0, 0].CurrentPiece == null)
        {
            CanCastleLeft = false;
            return;
        }
        if (board.BoardSquares[0, 0].CurrentPiece.Type != PieceType.Rook)
        {
            CanCastleLeft = false;
            return;
        }
        if (IsWhite && GameEngine.Instance.Player == GameEngine.ActivePlayer.White
                    && !HasMoved
                    && board.BoardSquares[0, 1].CurrentPiece == null
                    && board.BoardSquares[0, 2].CurrentPiece == null
                    && board.BoardSquares[0, 3].CurrentPiece == null
                    && board.BoardSquares[0, 0].CurrentPiece.Type == PieceType.Rook
                    && board.BoardSquares[0, 0].CurrentPiece.HasMoved == false
                    && GameEngine.Instance.TestDropPiece(board.BoardSquares[0, 3], this)
                    && GameEngine.Instance.TestDropPiece(board.BoardSquares[0, 2], this)
                    && !GameEngine.Instance.WhiteKingChecked)
        {
            CanCastleLeft = true;
        }
        else if (IsWhite)
        {
            CanCastleLeft = false;
        }
    }

    public void BlackKingCheckRightCastle(Board board)
    {
        if (board.BoardSquares[7, 7].CurrentPiece == null)
        {
            CanCastleRight = false;
            return;
        }
        if (board.BoardSquares[7, 7].CurrentPiece.Type != PieceType.Rook)
        {
            CanCastleRight = false;
            return;
        }
        if (!IsWhite && GameEngine.Instance.Player == GameEngine.ActivePlayer.Black
                     && !HasMoved
                     && board.BoardSquares[7, 5].CurrentPiece == null
                     && board.BoardSquares[7, 6].CurrentPiece == null
                     && board.BoardSquares[7, 7].CurrentPiece.Type == PieceType.Rook
                     && board.BoardSquares[7, 7].CurrentPiece.HasMoved == false
                     && GameEngine.Instance.TestDropPiece(board.BoardSquares[7, 5], this)
                     && GameEngine.Instance.TestDropPiece(board.BoardSquares[7, 6], this)
                     && !GameEngine.Instance.BlackKingChecked)
        {
            CanCastleRight = true;
        }
        else if (!IsWhite)
        {
            CanCastleRight = false;
        }
    }

    public void BlackKingCheckLeftCastle(Board board)
    {
        if (board.BoardSquares[7, 0].CurrentPiece == null)
        {
            CanCastleLeft = false;
            return;
        }
        if (board.BoardSquares[7, 0].CurrentPiece.Type != PieceType.Rook)
        {
            CanCastleLeft = false;
            return;
        }
        if (!IsWhite && GameEngine.Instance.Player == GameEngine.ActivePlayer.Black
                    && !HasMoved
                    && board.BoardSquares[7, 1].CurrentPiece == null
                    && board.BoardSquares[7, 2].CurrentPiece == null
                    && board.BoardSquares[7, 3].CurrentPiece == null
                    && board.BoardSquares[7, 0].CurrentPiece.Type == PieceType.Rook
                    && board.BoardSquares[7, 0].CurrentPiece.HasMoved == false
                    && GameEngine.Instance.TestDropPiece(board.BoardSquares[7, 3], this)
                    && GameEngine.Instance.TestDropPiece(board.BoardSquares[7, 2], this)
                    && !GameEngine.Instance.BlackKingChecked)
        {
            CanCastleLeft = true;
        }
        else if (!IsWhite)
        {
            CanCastleLeft = false;
        }
    }
    #endregion
}
