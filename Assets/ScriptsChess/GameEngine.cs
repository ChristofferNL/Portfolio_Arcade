using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEngine : MonoBehaviour
{
    #region Variables

    public static GameEngine Instance;
    public Board Board;
    public Button WhiteButton;
    public Button BlackButton;
    public enum ActivePlayer { None, White, Black }
    public ActivePlayer Player;
    public ChessPiece ActivePiece;
    public bool WhiteKingAttacked;
    public bool BlackKingAttacked;
    public bool BlackKingChecked;
    public bool WhiteKingChecked;
    public SpriteState ActiveSprite;
    public SpriteState InactiveSprite;
    public float WhitePlayerTime;
    public float BlackPlayerTime;
    public int TimeIncrement;
    public TextMeshProUGUI WhiteTimerText;
    public TextMeshProUGUI BlackTimerText;
    public TextMeshProUGUI EndgameText;
    public Image EndGameImage;
    public AudioClip MoveSound;
    public AudioClip ButtonSound;
    public UIManagerChess UIManager;
    public ChessAI ChessAI;
    public Image[] WhiteCapturedPieces;
    public Image[] BlackCapturedPieces;
    public TextMeshProUGUI WhiteScore;
    public TextMeshProUGUI BlackScore;
    private int _whiteCapturedPiecesAmount;
    private int _blackCapturedPiecesAmount;
    private int _score;

    #endregion

    #region Initialize

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Board = FindObjectOfType<Board>();
        BlackButton.onClick.AddListener(StartGame);
        UIManager = GetComponent<UIManagerChess>();
        ChessAI = GetComponent<ChessAI>();
    }

    public void StartGame()
    {
        Player = ActivePlayer.White;
        GetWhiteMoves();
        BlackButton.interactable = false;
        WhiteButton.interactable = false;
        BlackButton.spriteState = InactiveSprite;
        WhiteButton.spriteState = ActiveSprite;
        BlackButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = false;
        PlaySoundEffect(ButtonSound);
    }

    #endregion

    #region Runtime

    private void Update()
    {
        TimerController();
        if (ActivePiece != null)
        {
            var v3 = Input.mousePosition;
            v3.z = 7.5f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            ActivePiece.transform.position = new Vector3(v3.x, 0.61f, v3.z);
        }
    }

    public void TimerController()
    {
        if (Player == ActivePlayer.White && WhitePlayerTime > 0)
        {
            WhitePlayerTime -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(WhitePlayerTime / 60);
            float seconds = Mathf.FloorToInt(WhitePlayerTime % 60);
            WhiteTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else if (Player == ActivePlayer.White && WhitePlayerTime <= 0)
        {
            WhiteTimerText.text = "0:00";
            Debug.Log("White out of time");
            CheckVictory();
        }
        if (Player == ActivePlayer.Black && BlackPlayerTime > 0)
        {
            BlackPlayerTime -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(BlackPlayerTime / 60);
            float seconds = Mathf.FloorToInt(BlackPlayerTime % 60);
            BlackTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else if (Player == ActivePlayer.Black && BlackPlayerTime <= 0)
        {
            BlackTimerText.text = "0:00";
            Debug.Log("Black out of time");
            CheckVictory();
        }

    }

    public void PlaySoundEffect(AudioClip clip)
    {
        AudioSource source = Board.GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
    }

    #endregion

    #region Main

    /// <summary>
    /// Sets the Player variable to the other player, increments time for the former active player, gets all moves for the new ActivePlayer and calls the CheckVictory method
    /// </summary>
    public void ChangeActivePlayer()
    {
        foreach (BoardSquare square in Board.BoardSquares)
        {
            square.RemovedPiece = null;
        }
        if (Player == ActivePlayer.White)
        {
            WhitePlayerTime += TimeIncrement;
            TimerController();
            Player = ActivePlayer.Black;
            BlackButton.spriteState = ActiveSprite;
            WhiteButton.spriteState = InactiveSprite;
            if (IsKingAttacked(false))
            {
                BlackKingChecked = true;
            }
            else
            {
                BlackKingChecked = false;
            }
            ResetPieceMovesWhite();
            ResetPieceMovesBlack();
            GetBlackMoves();
            CheckVictory();
            ChessAI.GetPiecesWithMoves(Board);
        }
        else if (Player == ActivePlayer.Black)
        {
            BlackPlayerTime += TimeIncrement;
            TimerController();
            Player = ActivePlayer.White;
            BlackButton.spriteState = InactiveSprite;
            WhiteButton.spriteState = ActiveSprite;
            if (IsKingAttacked(false))
            {
                WhiteKingChecked = true;
            }
            else
            {
                WhiteKingChecked = false;
            }
            ResetPieceMovesWhite();
            ResetPieceMovesBlack();
            GetWhiteMoves();
            CheckVictory();
        }
    }

    public void CheckVictory() // Checks all victory conditions
    {
        if (Player == ActivePlayer.Black)
        {
            if (CalculateBlackMoves() == 0 && IsKingAttacked(false))
            {
                Player = ActivePlayer.None;
                EndGameImage.gameObject.SetActive(true);
                EndgameText.text = "White Wins!";
                UIManager.GameFinished();
            }
            else if (CalculateBlackMoves() == 0 && !IsKingAttacked(false))
            {
                Player = ActivePlayer.None;
                EndGameImage.gameObject.SetActive(true);
                EndgameText.text = "Game ends in a draw!";
                UIManager.GameFinished();
            }
            else if (BlackPlayerTime <= 0)
            {
                Player = ActivePlayer.None;
                EndGameImage.gameObject.SetActive(true);
                EndgameText.text = "White Wins!";
                UIManager.GameFinished();
            }
        }
        else if (Player == ActivePlayer.White)
        {
            if (CalculateWhiteMoves() == 0 && IsKingAttacked(false))
            {
                Player = ActivePlayer.None;
                EndGameImage.gameObject.SetActive(true);
                EndgameText.text = "Black Wins!";
                UIManager.GameFinished();
            }
            else if (CalculateWhiteMoves() == 0 && !IsKingAttacked(false))
            {
                Player = ActivePlayer.None;
                EndGameImage.gameObject.SetActive(true);
                EndgameText.text = "Game ends in a draw!";
                UIManager.GameFinished();
            }
            else if (WhitePlayerTime <= 0)
            {
                Player = ActivePlayer.None;
                EndGameImage.gameObject.SetActive(true);
                EndgameText.text = "Black Wins!";
                UIManager.GameFinished();
            }
        }
    }

    public int CalculateWhiteMoves() // Returns the number of possible moves for the black pieces
    {
        int result = 0;
        foreach (ChessPiece piece in Board.Pieces)
        {
            if (piece.IsWhite)
            {
                result += piece.MovableSquares.Count;
            }
        }
        return result;
    }

    public int CalculateBlackMoves() // Returns the number of possible moves for the black pieces
    {
        int result = 0;
        foreach (ChessPiece piece in Board.Pieces)
        {
            if (!piece.IsWhite)
            {
                result += piece.MovableSquares.Count;
            }
        }
        return result;
    }

    public void GetActivePiece(ChessPiece piece)
    {
        ActivePiece = piece;
    }

    /// <summary>
    /// Drops the ActivePiece at the squareToDrop BoardSquare, if the squareToDrop is not in the ActivePiece.MovableSquares 
    /// list of BoardSquares the ActivePiece will be returned to it's origin BoardSquare, in both cases the PopulateSquare() is called to set the new position of the ActivePiece
    /// </summary>
    /// <param name="squareToDrop"></param>
    public void DropActivePiece(BoardSquare squareToDrop)
    {
        if (ActivePiece != null)
        {
            if (TryCastling(squareToDrop))
            {
                return;
            }
            foreach (BoardSquare square in ActivePiece.MovableSquares)
            {
                if (square == squareToDrop)
                {
                    BoardSquare formerSquare = ActivePiece.CurrentSquare;
                    ResetSquareColors(formerSquare, square);
                    if (square.CurrentPiece != null)
                    {
                        ManageScoreBoard(square.CurrentPiece);
                    }
                    square.PopulateSquare(ActivePiece);
                    ActivePiece.HasMoved = true;
                    Promotion(ActivePiece, ActivePiece.CurrentSquare);
                    ActivePiece.GetMovableSquares(Board);
                    ActivePiece = null;
                    PlaySoundEffect(MoveSound);
                    ChangeActivePlayer();
                    return;
                }
            }
            ActivePiece.CurrentSquare.PopulateSquare(ActivePiece);
            ActivePiece = null;
        }
    }

    public void ManageScoreBoard(ChessPiece piece)
    {
        int value = 0;
        switch (piece.Type)
        {
            case ChessPiece.PieceType.Pawn:
                value = 1;
                break;
            case ChessPiece.PieceType.Knight:
                value = 3;
                break;
            case ChessPiece.PieceType.Bishop:
                value = 3;
                break;
            case ChessPiece.PieceType.Rook:
                value = 5;
                break;
            case ChessPiece.PieceType.Queen:
                value = 9;
                break;
            default:
                break;
        }
        if (piece.IsWhite)
        {
            _score -= value;
            BlackCapturedPieces[_blackCapturedPiecesAmount].sprite = piece.GetComponent<SpriteRenderer>().sprite;
            BlackCapturedPieces[_blackCapturedPiecesAmount].gameObject.SetActive(true);
            _blackCapturedPiecesAmount++;
        }
        else
        {
            _score += value;
            WhiteCapturedPieces[_whiteCapturedPiecesAmount].sprite = piece.GetComponent<SpriteRenderer>().sprite;
            WhiteCapturedPieces[_whiteCapturedPiecesAmount].gameObject.SetActive(true);
            _whiteCapturedPiecesAmount++;
        }
        if (_score > 0 || _score < 0)
        {
            WhiteScore.text = $"{+_score}";
            BlackScore.text = $"{-_score}";
        }
        else if (_score == 0)
        {
            WhiteScore.text = "0";
            BlackScore.text = "0";
        }

    }

    public void Promotion(ChessPiece piece, BoardSquare square)
    {
        if (piece.Type == ChessPiece.PieceType.Pawn && piece.IsWhite && square.Row == 7)
        {
            piece.SetInitValues(ChessPiece.PieceType.Queen, square, piece.IsWhite, 90);
        }
        else if (piece.Type == ChessPiece.PieceType.Pawn && !piece.IsWhite && square.Row == 0)
        {
            piece.SetInitValues(ChessPiece.PieceType.Queen, square, piece.IsWhite, 90);
        }
    }

    public bool TryCastling(BoardSquare square)
    {
        if (ActivePiece.Type == ChessPiece.PieceType.King && ActivePiece.CanCastleRight && ActivePiece.IsWhite && square == Board.BoardSquares[0, 6])
        {
            ResetSquareColors(ActivePiece.CurrentSquare, square);
            ActivePiece.CurrentSquare.State = BoardSquare.SquareState.Empty;
            ActivePiece.CurrentSquare.CurrentPiece = null;
            Board.BoardSquares[0, 6].PopulateSquare(ActivePiece);
            ActivePiece.HasMoved = true;
            ActivePiece = Board.BoardSquares[0, 7].CurrentPiece;
            ActivePiece.CurrentSquare.State = BoardSquare.SquareState.Empty;
            ActivePiece.CurrentSquare.CurrentPiece = null;
            Board.BoardSquares[0, 5].PopulateSquare(ActivePiece);
            ActivePiece.HasMoved = true;
            ActivePiece = null;
            PlaySoundEffect(MoveSound);
            ChangeActivePlayer();
            return true;
        }
        if (ActivePiece.Type == ChessPiece.PieceType.King && ActivePiece.CanCastleLeft && ActivePiece.IsWhite && square == Board.BoardSquares[0, 2])
        {
            ResetSquareColors(ActivePiece.CurrentSquare, square);
            ActivePiece.CurrentSquare.State = BoardSquare.SquareState.Empty;
            ActivePiece.CurrentSquare.CurrentPiece = null;
            Board.BoardSquares[0, 2].PopulateSquare(ActivePiece);
            ActivePiece.HasMoved = true;
            ActivePiece = Board.BoardSquares[0, 0].CurrentPiece;
            ActivePiece.CurrentSquare.State = BoardSquare.SquareState.Empty;
            ActivePiece.CurrentSquare.CurrentPiece = null;
            Board.BoardSquares[0, 3].PopulateSquare(ActivePiece);
            ActivePiece.HasMoved = true;
            ActivePiece = null;
            PlaySoundEffect(MoveSound);
            ChangeActivePlayer();
            return true;
        }
        if (ActivePiece.Type == ChessPiece.PieceType.King && ActivePiece.CanCastleRight && !ActivePiece.IsWhite && square == Board.BoardSquares[7, 6])
        {
            ResetSquareColors(ActivePiece.CurrentSquare, square);
            ActivePiece.CurrentSquare.State = BoardSquare.SquareState.Empty;
            ActivePiece.CurrentSquare.CurrentPiece = null;
            Board.BoardSquares[7, 6].PopulateSquare(ActivePiece);
            ActivePiece.HasMoved = true;
            ActivePiece = Board.BoardSquares[7, 7].CurrentPiece;
            ActivePiece.CurrentSquare.State = BoardSquare.SquareState.Empty;
            ActivePiece.CurrentSquare.CurrentPiece = null;
            Board.BoardSquares[7, 5].PopulateSquare(ActivePiece);
            ActivePiece.HasMoved = true;
            ActivePiece = null;
            PlaySoundEffect(MoveSound);
            ChangeActivePlayer();
            return true;
        }
        if (ActivePiece.Type == ChessPiece.PieceType.King && ActivePiece.CanCastleLeft && !ActivePiece.IsWhite && square == Board.BoardSquares[7, 2])
        {
            ResetSquareColors(ActivePiece.CurrentSquare, square);
            ActivePiece.CurrentSquare.State = BoardSquare.SquareState.Empty;
            ActivePiece.CurrentSquare.CurrentPiece = null;
            Board.BoardSquares[7, 2].PopulateSquare(ActivePiece);
            ActivePiece.HasMoved = true;
            ActivePiece = Board.BoardSquares[7, 0].CurrentPiece;
            ActivePiece.CurrentSquare.State = BoardSquare.SquareState.Empty;
            ActivePiece.CurrentSquare.CurrentPiece = null;
            Board.BoardSquares[7, 3].PopulateSquare(ActivePiece);
            ActivePiece.HasMoved = true;
            ActivePiece = null;
            PlaySoundEffect(MoveSound);
            ChangeActivePlayer();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Managing the color of all Boardsquares, setting the currentcolor of the formerSquare the ActivePiece has moved from and the newSquare it moves to. 
    /// </summary>
    /// <param name="formerSquare"></param>
    /// <param name="newSquare"></param>
    public void ResetSquareColors(BoardSquare formerSquare, BoardSquare newSquare)
    {
        foreach (BoardSquare square in Board.BoardSquares) // Resetting all squares to original color
        {
            if (square.IsWhiteColor)
            {
                square.CurrentColor = BoardSquare.SquareColor.White;
                square.SetSquareColor();
            }
            else
            {
                square.CurrentColor = BoardSquare.SquareColor.Black;
                square.SetSquareColor();
            }
        }
        if (formerSquare.IsWhiteColor)
        {
            formerSquare.CurrentColor = BoardSquare.SquareColor.ActivatedWhite;
            formerSquare.SetSquareColor();
        }
        else
        {
            formerSquare.CurrentColor = BoardSquare.SquareColor.ActivatedBlack;
            formerSquare.SetSquareColor();
        }
        if (newSquare.IsWhiteColor)
        {
            newSquare.CurrentColor = BoardSquare.SquareColor.ActivatedWhite;
            newSquare.SetSquareColor();
        }
        else
        {
            newSquare.CurrentColor = BoardSquare.SquareColor.ActivatedBlack;
            newSquare.SetSquareColor();
        }
    }

    public bool TestDropPiece(BoardSquare squareToTest, ChessPiece piece)
    {
        BoardSquare formerSquare = piece.CurrentSquare;
        formerSquare.State = BoardSquare.SquareState.Empty;
        formerSquare.CurrentPiece = null;
        squareToTest.PopulateSquare(piece);
        if (IsKingAttacked(false))
        {
            formerSquare.PopulateSquare(piece);
            squareToTest.ReturnCapturedPiece();

            return false;
        }
        formerSquare.PopulateSquare(piece);
        squareToTest.ReturnCapturedPiece();
        return true;
    }

    public bool IsKingAttacked(bool isDropped)
    {
        if (Player == ActivePlayer.White)
        {
            ResetPieceMovesBlack();
            GetBlackMoves();
            if (WhiteKingAttacked && isDropped)
            {
                WhiteKingAttacked = true;
                return true;
            }
            else if (WhiteKingAttacked && !isDropped)
            {
                WhiteKingAttacked = false;
                return true;
            }
        }
        else
        {
            ResetPieceMovesWhite();
            GetWhiteMoves();
            if (BlackKingAttacked && isDropped)
            {
                BlackKingAttacked = true;
                return true;
            }
            else if (BlackKingAttacked && !isDropped)
            {
                BlackKingAttacked = false;
                return true;
            }
        }
        return false;
    }

    public void ResetPieceMovesWhite()
    {
        WhiteKingAttacked = false;

        foreach (ChessPiece piece in Board.Pieces)
        {
            if (piece.IsWhite)
            {
                piece.MovableSquares.Clear();
            }
        }
    }
    public void ResetPieceMovesBlack()
    {

        BlackKingAttacked = false;
        foreach (ChessPiece piece in Board.Pieces)
        {
            if (!piece.IsWhite)
            {
                piece.MovableSquares.Clear();
            }
        }
    }

    public void GetWhiteMoves()
    {
        foreach (ChessPiece piece in Board.Pieces)
        {
            if (piece.IsWhite && piece.isActiveAndEnabled)
            {
                piece.GetMovableSquares(Board);
            }
        }
    }

    public void GetBlackMoves()
    {
        foreach (ChessPiece piece in Board.Pieces)
        {
            if (!piece.IsWhite && piece.isActiveAndEnabled)
            {
                piece.GetMovableSquares(Board);
            }
        }
    }

    #endregion
}
