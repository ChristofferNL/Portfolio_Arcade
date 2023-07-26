using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPieceMovement : Piece
{
    public RotationCollider[] colliders = new RotationCollider[1];
    public RotationCollider[] movementColliders = new RotationCollider[8];


    private void Awake()
    {
        rotationState = 0;
    }

    public override void RotationCheckerRight()
    {
        if (!colliders[0].IsCollidingWithMesh)
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
        if (!colliders[0].IsCollidingWithMesh)
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
        if (rotationState == 0 && !movementColliders[3].IsCollidingWithMesh && !movementColliders[4].IsCollidingWithMesh)
        {
            canMoveRight = true;
            return;
        }
        else
        {
            canMoveRight = false;
        }
        if (rotationState == 1 && !movementColliders[1].IsCollidingWithMesh && !movementColliders[2].IsCollidingWithMesh && !movementColliders[3].IsCollidingWithMesh)
        {
            canMoveRight = true;
            return;
        }
        else
        {
            canMoveRight = false;
        }
        if (rotationState == 2 && !movementColliders[7].IsCollidingWithMesh && !movementColliders[1].IsCollidingWithMesh)
        {
            canMoveRight = true;
            return;
        }
        else
        {
            canMoveRight = false;
        }
        if (rotationState == 3 && !movementColliders[5].IsCollidingWithMesh && !movementColliders[6].IsCollidingWithMesh && !movementColliders[0].IsCollidingWithMesh)
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

        if (rotationState == 0 && !movementColliders[7].IsCollidingWithMesh && !movementColliders[1].IsCollidingWithMesh)
        {
            canMoveLeft = true;
            return;
        }
        else
        {
            canMoveLeft = false;
        }
        if (rotationState == 1 && !movementColliders[5].IsCollidingWithMesh && !movementColliders[6].IsCollidingWithMesh && !movementColliders[0].IsCollidingWithMesh)
        {
            canMoveLeft = true;
            return;
        }
        else
        {
            canMoveLeft = false;
        }
        if (rotationState == 2 && !movementColliders[3].IsCollidingWithMesh && !movementColliders[4].IsCollidingWithMesh)
        {
            canMoveLeft = true;
            return;
        }
        else
        {
            canMoveLeft = false;
        }
        if (rotationState == 3 && !movementColliders[1].IsCollidingWithMesh && !movementColliders[2].IsCollidingWithMesh && !movementColliders[3].IsCollidingWithMesh)
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
        if (rotationState == 0 && !movementColliders[5].IsCollidingWithMesh && !movementColliders[6].IsCollidingWithMesh && !movementColliders[0].IsCollidingWithMesh)
        {
            canMoveDown = true;
            return;
        }
        else
        {
            canMoveDown = false;
        }
        if (rotationState == 1 && !movementColliders[3].IsCollidingWithMesh && !movementColliders[4].IsCollidingWithMesh)
        {
            canMoveDown = true;
            return;
        }
        else
        {
            canMoveDown = false;
        }
        if (rotationState == 2 && !movementColliders[1].IsCollidingWithMesh && !movementColliders[2].IsCollidingWithMesh && !movementColliders[3].IsCollidingWithMesh)
        {
            canMoveDown = true;
            return;
        }
        else
        {
            canMoveDown = false;
        }
        if (rotationState == 3 && !movementColliders[7].IsCollidingWithMesh && !movementColliders[1].IsCollidingWithMesh)
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
