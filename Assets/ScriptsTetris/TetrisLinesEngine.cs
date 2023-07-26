using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisLinesEngine : MonoBehaviour
{
    public CreateBoardScript board;
    public TetrisScoreEngine scoreBoard;

    private void Start()
    {
        board = this.GetComponent<CreateBoardScript>();
        scoreBoard = this.GetComponent<TetrisScoreEngine>();
    }
    public void ClearFullLines()
    {
        int linesRemoved = 0;
        for (int i = 19; i > 0; i--)
        {
            int blocksActive = 0;
            for (int j = 0; j < 10; j++)
            {
                if (board.Board[i, j].isPartOfBoard)
                {
                    blocksActive++;
                }
            }
            if (blocksActive == 10)
            {
                for (int j = 0; j < 10; j++)
                {
                    board.Board[i, j].ClearCube();
                }
                MoveLinesDown(i);
                i++;
                linesRemoved++;
            }
        }
        if (linesRemoved != 0)
        {
            scoreBoard.CalculateScore(linesRemoved);
        }
    }
    private bool CheckIfLineIsEmpty(int i)
    {
        return true;
    }
    private void MoveLinesDown(int row)
    {
        for (int i = row; i > 0; i--)
        {
            for (int j = 0; j < 10; j++)
            {
                board.Board[i, j].InheritExistingCube(board.Board[i - 1, j]);
            }
        }
    }
}
