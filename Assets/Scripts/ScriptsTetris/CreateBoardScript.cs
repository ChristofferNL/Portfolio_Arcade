using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoardScript : MonoBehaviour
{
    public GameObject holderParent;
    public GameObject BuildingBlock;
    public BoardBlock BoardBlock;
    public BoardBlock[,] Board = new BoardBlock[20, 10];
    void Start()
    {
        CreateBoard();  
    }

    void CreateBoard()
    {
        // create outerbounds blocks
        for (int i = 0; i < 26; i++)
        {
            Instantiate(BuildingBlock, new Vector3(-6, 0.5f, 15 - i), Quaternion.identity);
        }
        for (int i = 0; i < 26; i++)
        {
            Instantiate(BuildingBlock, new Vector3(5, 0.5f, 15 - i), Quaternion.identity);
        }
        for (int i = 0; i < 10; i++)
        {
            Instantiate(BuildingBlock, new Vector3(-5 + i, 0.5f, -10), Quaternion.identity);
        }
        // create multiarray of the boardblocks
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Board[i, j] = Instantiate(BoardBlock, new Vector3(-5 + j, 0.5f, 10 - i), Quaternion.identity);
            }
        }
        // move the board objects to Worldbuilder in hierarchy 
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("OuterBoundsBlock"))
        {
            gameObject.transform.SetParent(holderParent.transform);
        }
        foreach (BoardBlock block in Board)
        {
            block.transform.SetParent(holderParent.transform);
        }
    }
}
