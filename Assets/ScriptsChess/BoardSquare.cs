using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class BoardSquare : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Variables
    public Material Material { get; private set; }
    public int Row;
    public int Col;
    public Material WhiteMaterial;
    public Material BlackMaterial;
    public Material ActivatedWhiteMaterial;
    public Material ActivatedBlackMaterial;
    public ChessPiece CurrentPiece;
    public ChessPiece RemovedPiece;
    public TextMeshProUGUI LetterBox;
    public TextMeshProUGUI NumberBox;
    public char[] ColumnChar = new char[8] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
    public enum SquareAttackState { Clear, AttackedByWhite, AttackedByBlack }
    public SquareAttackState AttackState;
    public enum SquareState { Empty, OccupiedWhite, OccupiedBlack }
    public SquareState State;
    public SquareState TempState;
    public enum SquareColor { White, Black, ActivatedWhite, ActivatedBlack }
    public SquareColor CurrentColor { set; get; }
    public bool IsWhiteColor;
    public bool ContainsWhiteKing;
    public bool ContainsBlackKing;
    private MeshRenderer meshRenderer;

    #endregion

    #region Initialize

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        LetterBox.text = "";
        NumberBox.text = "";

    }

    public void SetInitValues(int row, int col, bool isWhite)
    {
        Row = row;
        Col = col;
        IsWhiteColor = isWhite;
        if (isWhite)
        {
            CurrentColor = SquareColor.White;
            LetterBox.color = BlackMaterial.color;
            NumberBox.color = BlackMaterial.color;
        }
        else
        {
            CurrentColor = SquareColor.Black;
            LetterBox.color = WhiteMaterial.color;
            NumberBox.color = WhiteMaterial.color;
        }
        SetSquareColor();
        SetSquareText(row, col);
    }

    public void SetSquareColor()
    {
        switch (CurrentColor)
        {
            case SquareColor.White:
                meshRenderer.material = WhiteMaterial;
                break;
            case SquareColor.Black:
                meshRenderer.material = BlackMaterial;
                break;
            case SquareColor.ActivatedWhite:
                meshRenderer.material = ActivatedWhiteMaterial;
                break;
            case SquareColor.ActivatedBlack:
                meshRenderer.material = ActivatedBlackMaterial;
                break;
            default:
                break;
        }
    }

    public void SetSquareText(int row, int col)
    {
        if (row == 0)
        {
            LetterBox.text = ColumnChar[col].ToString();
        }
        if (col == 0)
        {
            NumberBox.text = (row + 1).ToString();
        }
        this.name = ColumnChar[col].ToString() + (row + 1).ToString();
    }

    #endregion

    #region Main

    public void PopulateSquare(ChessPiece piece)
    {
        if (CurrentPiece == null)
        {
            CurrentPiece = piece;
        }
        else
        {
            if (CurrentPiece != piece)
            {
                CapturePiece(CurrentPiece);
                CurrentPiece = piece;
            }
        }
        if (piece.IsWhite)
        {
            State = SquareState.OccupiedWhite;
            if (piece.Type == ChessPiece.PieceType.King)
            {
                ContainsWhiteKing = true;
            }
            else
            {
                ContainsWhiteKing = false;
                ContainsBlackKing = false;
            }
        }
        else
        {
            State = SquareState.OccupiedBlack;
            if (piece.Type == ChessPiece.PieceType.King)
            {
                ContainsBlackKing = true;
            }
            else
            {
                ContainsWhiteKing = false;
                ContainsBlackKing = false;
            }
        }

        piece.CurrentSquare = this;
        piece.transform.position = new Vector3(piece.CurrentSquare.transform.position.x,
                                                piece.CurrentSquare.transform.position.y + 0.51f,
                                                piece.CurrentSquare.transform.position.z);
    }

    public void CapturePiece(ChessPiece piece)
    {
        RemovedPiece = piece;
        piece.gameObject.SetActive(false);
        //Destroy(piece.gameObject);
        //Debug.Log(piece.ToString() + " Destroyed");
    }

    public void ReturnCapturedPiece()
    {
        if (RemovedPiece != null)
        {
            CurrentPiece = RemovedPiece;
            RemovedPiece.gameObject.SetActive(true);
            if (RemovedPiece.IsWhite)
            {
                State = SquareState.OccupiedWhite;
            }
            else
            {
                State = SquareState.OccupiedBlack;
            }
        }
        else
        {
            State = SquareState.Empty;
            CurrentPiece = null;
            ContainsWhiteKing = false;
            ContainsBlackKing = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CurrentPiece != null && CurrentPiece.MovableSquares.Count > 0 && GameEngine.Instance.Player == GameEngine.ActivePlayer.White)
        {
            GameEngine.Instance.GetActivePiece(CurrentPiece);
            State = SquareState.Empty;
            CurrentPiece = null;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 999))
        {
            if (hit.collider.gameObject.TryGetComponent<BoardSquare>(out BoardSquare square))
            {
                GameEngine.Instance.DropActivePiece(square);
                return;
            }
        }
        else
        {
            GameEngine.Instance.DropActivePiece(this);
        }
    }

    #endregion
}
