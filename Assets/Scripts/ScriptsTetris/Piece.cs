using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public bool canRotateRight; // bör returneras via rotationcheckern
    public bool canRotateLeft;
    public bool canMoveRight;
    public bool canMoveLeft;
    public bool canMoveDown;
    public bool hasMoved;
    public bool hasMovedDownManually;
    public bool hasRotated;
    public bool isActivePiece;
    public int rotationState;
    public void CheckMovement()
    {
        RotationCheckerRight();
        RotationCheckerLeft();
        MovementCheckerRight();
        MovementCheckerLeft();
        MovementCheckerDown();
    }
    public abstract void RotationCheckerRight();
    public abstract void RotationCheckerLeft();
    public abstract void MovementCheckerRight();
    public abstract void MovementCheckerLeft();
    public abstract void MovementCheckerDown();
}
