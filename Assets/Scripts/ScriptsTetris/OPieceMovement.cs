using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPieceMovement : Piece
{
    public RotationCollider[] movementColliders = new RotationCollider[6];

    private void Update()
    {
        rotationState = 0;
    }
    private void Awake()
    {
        rotationState = 0;
    }

    public override void RotationCheckerRight()
    {

        canRotateRight = false;

    }

    public override void RotationCheckerLeft()
    {

        canRotateLeft = false;

    }
    public override void MovementCheckerRight()
    {
        if (rotationState == 0 && !movementColliders[4].IsCollidingWithMesh && !movementColliders[5].IsCollidingWithMesh)
        {
            canMoveRight = true;
            return;
        }
        else
        {
            canMoveRight = false;
        }
        if (rotationState == 1 && !movementColliders[0].IsCollidingWithMesh && !movementColliders[1].IsCollidingWithMesh && !movementColliders[4].IsCollidingWithMesh)
        {
            canMoveRight = true;
            return;
        }
        else
        {
            canMoveRight = false;
        }
        if (rotationState == 2 && !movementColliders[1].IsCollidingWithMesh && !movementColliders[6].IsCollidingWithMesh)
        {
            canMoveRight = true;
            return;
        }
        else
        {
            canMoveRight = false;
        }
        if (rotationState == 3 && !movementColliders[2].IsCollidingWithMesh && !movementColliders[3].IsCollidingWithMesh && !movementColliders[5].IsCollidingWithMesh)
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

        if (rotationState == 0 && !movementColliders[0].IsCollidingWithMesh && !movementColliders[1].IsCollidingWithMesh)
        {
            canMoveLeft = true;
            return;
        }
        else
        {
            canMoveLeft = false;
        }
        if (rotationState == 1 && !movementColliders[5].IsCollidingWithMesh && !movementColliders[3].IsCollidingWithMesh && !movementColliders[2].IsCollidingWithMesh)
        {
            canMoveLeft = true;
            return;
        }
        else
        {
            canMoveLeft = false;
        }
        if (rotationState == 2 && !movementColliders[7].IsCollidingWithMesh && !movementColliders[8].IsCollidingWithMesh)
        {
            canMoveLeft = true;
            return;
        }
        else
        {
            canMoveLeft = false;
        }
        if (rotationState == 3 && !movementColliders[0].IsCollidingWithMesh && !movementColliders[1].IsCollidingWithMesh && !movementColliders[4].IsCollidingWithMesh)
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
        if (rotationState == 0 && !movementColliders[2].IsCollidingWithMesh && !movementColliders[3].IsCollidingWithMesh)
        {
            canMoveDown = true;
            return;
        }
        else
        {
            canMoveDown = false;
        }
        if (rotationState == 1 && !movementColliders[7].IsCollidingWithMesh && !movementColliders[8].IsCollidingWithMesh)
        {
            canMoveDown = true;
            return;
        }
        else
        {
            canMoveDown = false;
        }
        if (rotationState == 2 && !movementColliders[4].IsCollidingWithMesh && !movementColliders[1].IsCollidingWithMesh && !movementColliders[0].IsCollidingWithMesh)
        {
            canMoveDown = true;
            return;
        }
        else
        {
            canMoveDown = false;
        }
        if (rotationState == 3 && !movementColliders[1].IsCollidingWithMesh && !movementColliders[6].IsCollidingWithMesh)
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
