using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPieceMovement : Piece
{
    public RotationCollider[] colliders = new RotationCollider[8];
    public RotationCollider[] movementColliders = new RotationCollider[10];


    private void Awake()
    {
        rotationState = 0;
    }
 
    public override void RotationCheckerRight()
    {
        if (!colliders[0].IsCollidingWithMesh && !colliders[1].IsCollidingWithMesh && !colliders[2].IsCollidingWithMesh)
        {
            canRotateRight = true;
        }
        else
        {
            canRotateRight = false;
        }
    }

    public override void RotationCheckerLeft()
    {
        if (!colliders[3].IsCollidingWithMesh && !colliders[4].IsCollidingWithMesh && !colliders[5].IsCollidingWithMesh)
        {
            canRotateLeft = true;
        }
        else
        {
            canRotateLeft = false;
        }
    }
    public override void MovementCheckerRight()
    {
        if (rotationState == 0 && !movementColliders[4].IsCollidingWithMesh)
        {
            canMoveRight = true;
            return;
        }
        else
        {
            canMoveRight = false;
        }
        if (rotationState == 1 && !movementColliders[0].IsCollidingWithMesh && !movementColliders[1].IsCollidingWithMesh && !movementColliders[2].IsCollidingWithMesh && !movementColliders[3].IsCollidingWithMesh)
        {
            canMoveRight = true;
            return;
        }
        else
        {
            canMoveRight = false;
        }
        if (rotationState == 2 && !movementColliders[9].IsCollidingWithMesh)
        {
            canMoveRight = true;
            return;
        }
        else
        {
            canMoveRight = false;
        }
        if (rotationState == 3 && !movementColliders[5].IsCollidingWithMesh && !movementColliders[6].IsCollidingWithMesh && !movementColliders[7].IsCollidingWithMesh && !movementColliders[8].IsCollidingWithMesh)
        {
            canMoveRight = true;
            return;
        }
        else
        {
            canMoveRight = false;
        }

    }
    public override void MovementCheckerLeft()
    {

        if (rotationState == 0 && !movementColliders[9].IsCollidingWithMesh)
        {
            canMoveLeft = true;
            return;
        }
        else
        {
            canMoveLeft = false;
        }
        if (rotationState == 1 && !movementColliders[5].IsCollidingWithMesh && !movementColliders[6].IsCollidingWithMesh && !movementColliders[7].IsCollidingWithMesh && !movementColliders[8].IsCollidingWithMesh)
        {
            canMoveLeft = true;
            return;
        }
        else
        {
            canMoveLeft = false;
        }
        if (rotationState == 2 && !movementColliders[4].IsCollidingWithMesh)
        {
            canMoveLeft = true;
            return;
        }
        else
        {
            canMoveLeft = false;
        }
        if (rotationState == 3 && !movementColliders[0].IsCollidingWithMesh && !movementColliders[1].IsCollidingWithMesh && !movementColliders[2].IsCollidingWithMesh && !movementColliders[3].IsCollidingWithMesh)
        {
            canMoveLeft = true;
            return;
        }
        else
        {
            canMoveLeft = false;
        }
    }
    public override void MovementCheckerDown()
    {
        if (rotationState == 0 && !movementColliders[5].IsCollidingWithMesh && !movementColliders[6].IsCollidingWithMesh && !movementColliders[7].IsCollidingWithMesh && !movementColliders[8].IsCollidingWithMesh)
        {
            canMoveDown = true;
            return;
        }
        else
        {
            canMoveDown = false;
        }
        if (rotationState == 1 && !movementColliders[4].IsCollidingWithMesh)
        {
            canMoveDown = true;
            return;
        }
        else
        {
            canMoveDown = false;
        }
        if (rotationState == 2 && !movementColliders[0].IsCollidingWithMesh && !movementColliders[1].IsCollidingWithMesh && !movementColliders[2].IsCollidingWithMesh && !movementColliders[3].IsCollidingWithMesh)
        {
            canMoveDown = true;
            return;
        }
        else
        {
            canMoveDown = false;
        }
        if (rotationState == 3 && !movementColliders[9].IsCollidingWithMesh)
        {
            canMoveDown = true;
            return;
        }
        else
        {
            canMoveDown = false;
        }
    }
}
