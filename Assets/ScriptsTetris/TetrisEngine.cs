using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TetrisEngine : MonoBehaviour
{
    public CreateBoardScript myBoard;
    public TetrisLinesEngine myLineEngine;
    public BlockSpawner spawner;
    public Piece activePiece;
    public Scene gameScene;
    public float moveCooldown;
    public float rotateCooldown;
    public float moveCooldownSeconds;
    public float rotateCooldownSeconds;
    public float moveDownCooldown;
    public float moveDownCooldownSeconds;
    public float moveDownManuallyCooldown;
    public float moveDownManuallyCooldownSeconds;
    bool nextActionMoveLeft;
    bool nextActionMoveRight;
    bool nextActionRotateLeft;
    bool nextActionRotateRight;
    bool nextActionMoveDown;
    bool nextActionSlamDown;
    bool hasActivePiece;

    private void Start()
    {
        spawner = this.gameObject.GetComponent<BlockSpawner>();
        myBoard = this.gameObject.GetComponent<CreateBoardScript>();
        myLineEngine = this.gameObject.GetComponent<TetrisLinesEngine>();
        moveCooldown = 0.07f;
        moveCooldownSeconds = 0.07f;
        rotateCooldown = 0.07f;
        rotateCooldownSeconds = 0.07f;
        moveDownCooldown = 0.4f;
        moveDownCooldownSeconds = 0.4f;
        moveDownManuallyCooldown = 0.08f;
        moveDownManuallyCooldownSeconds = 0.08f;
    }

    void Update()
    {
        if (!hasActivePiece)
        {
            SetActivePiece();
            hasActivePiece = true;
            foreach (BoardBlock block in myBoard.Board)
            {
                block.containsActiveCube = false;
            }
        }

        moveDownCooldown -= Time.deltaTime;
        if (moveDownCooldown <= 0.0f)
        {
            nextActionMoveDown = true;
            
        }

        activePiece.CheckMovement();

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            ResetNextMove();
            if (activePiece.canMoveRight && !activePiece.hasMoved)
            {
                nextActionMoveRight = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            ResetNextMove();
            if (activePiece.canMoveLeft && !activePiece.hasMoved)
            {
                nextActionMoveLeft = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ResetNextRotation();
            if (activePiece.canRotateRight && !activePiece.hasRotated)
            {
                nextActionRotateRight = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ResetNextRotation();
            if (activePiece.canRotateLeft && !activePiece.hasRotated)
            {
                nextActionRotateLeft = true;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (activePiece.canMoveDown && !activePiece.hasMovedDownManually)
            {
                nextActionMoveDown = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (activePiece.transform.position.z < 10.5f)
            {
                nextActionSlamDown = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            ResetNextMove();
        }
        if (activePiece.hasMoved)
        {
            moveCooldown -= Time.deltaTime;
            if (moveCooldown <= 0.0f)
            {
                activePiece.hasMoved = false;
                moveCooldown = moveCooldownSeconds;
            }
        }
        if (activePiece.hasRotated)
        {
            rotateCooldown -= Time.deltaTime;
            if (rotateCooldown <= 0.0f)
            {
                activePiece.hasRotated = false;
                rotateCooldown = rotateCooldownSeconds;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(0);
        }
    }
    private void FixedUpdate()
    {
        /*activePiece.CheckMovement();*/ // denna behövs inte
        if (nextActionSlamDown)
        {
            MoveDownSlam();
            moveDownCooldown = 0.0f;
            return;
        }
        if (nextActionMoveDown && activePiece.canMoveDown) // funktion som returnerar boolen direkt
        {
            MoveDown();
            return;
        }
        else if (nextActionMoveDown && !activePiece.canMoveDown) //Piece gets placed here
        {
            PlacePiece();
            return;
        }
        if (nextActionMoveLeft && activePiece.canMoveLeft)
        {
            MoveLeft();
            return;
        }
        if (nextActionMoveRight && activePiece.canMoveRight)
        {
            MoveRight();
            return;
        }
        if (nextActionRotateLeft && activePiece.canRotateLeft)
        {
            RotateLeft();
            return;
        }
        if (nextActionRotateRight && activePiece.canRotateRight)
        {
            RotateRight();
            return;
        }
    }
    void ResetNextMove()
    {
        nextActionMoveLeft = false;
        nextActionMoveRight = false;

    }
    void ResetNextRotation()
    {
        nextActionRotateLeft = false;
        nextActionRotateRight = false;
    }
    void MoveLeft()
    {
        activePiece.transform.position = new Vector3(activePiece.transform.position.x - 1, activePiece.transform.position.y, activePiece.transform.position.z);
        activePiece.hasMoved = true;
        nextActionMoveLeft = false;
    }
    void MoveRight()
    {
        activePiece.transform.position = new Vector3(activePiece.transform.position.x + 1, activePiece.transform.position.y, activePiece.transform.position.z);
        activePiece.hasMoved = true;
        nextActionMoveRight = false;
    }
    void RotateLeft()
    {
        activePiece.transform.Rotate(activePiece.transform.rotation.x, -90, activePiece.transform.rotation.z, Space.World);
        activePiece.rotationState--;
        activePiece.hasRotated = true;
        nextActionRotateLeft = false;
        if (activePiece.rotationState < 0)
        {
            activePiece.rotationState = 3;
        }
    }
    void RotateRight()
    {
        activePiece.transform.Rotate(activePiece.transform.rotation.x, 90, activePiece.transform.rotation.z, Space.World);
        activePiece.rotationState++;
        activePiece.hasRotated = true;
        nextActionRotateRight = false;
        if (activePiece.rotationState > 3)
        {
            activePiece.rotationState = 0;
        }
    }
    void MoveDown()
    {
        activePiece.transform.position = new Vector3(activePiece.transform.position.x, activePiece.transform.position.y, activePiece.transform.position.z - 1);
        nextActionMoveDown = false;
        moveDownCooldown = moveDownCooldownSeconds;
    }
    void MoveDownSlam()
    {
        int FinalPathDown = 21;
        int currentPathDown = 0;
        nextActionSlamDown = false;
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (myBoard.Board[i, j].containsActiveCube)
                {
                    currentPathDown = CalculatePathDown(i +1, j);
                    if (currentPathDown < FinalPathDown)
                    {
                        FinalPathDown = currentPathDown;
                    }
                }
            }
        }
        for (int i = 0; i < FinalPathDown; i++)
        {
            MoveDown();
        }
    }
    private int CalculatePathDown(int indexI, int indexJ)
    {
        int pathCounter = 0;
        for (int i = indexI; i < 20; i++)
        {
            if (myBoard.Board[i, indexJ].isPartOfBoard)
            {
                return pathCounter;
            }
            else
            {
                pathCounter++;
            }
        }
        return pathCounter;
    }
    public void PlacePiece()
    {
        foreach (BoardBlock block in myBoard.Board)
        {
            if (block.containsActiveCube)
            {
                block.InheritActiveCube();
            }
        }
        if (activePiece.transform.position.z > 9f)
        {
            Debug.Log("Game ended");
            gameScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(gameScene.name);
        }
        Destroy(activePiece.gameObject);
        hasActivePiece = false;
        myLineEngine.ClearFullLines();
        moveDownCooldown = moveDownCooldownSeconds;
    }
    public void SetActivePiece()
    {
        activePiece = spawner.SpawnActiveBlock();
    }
    public void ChangeGameSpeed()
    {
        moveDownCooldownSeconds -= 0.02f;
    }
}
