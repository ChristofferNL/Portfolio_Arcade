using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Contains methods for raycasts to determine collisions with WorldMaterial tagged objects, transform containers for the rays starting positions, 
/// methods to handle when the player hits the ground or the roof, also handles OnTriggerStay and OnTriggerExit collision
/// </summary>
public class PlayerCollision : MonoBehaviour
{
    public Player Player;
    
    public Transform DownLeftRayPos, DownRightRayPos, UpLeftRayPos, UpRightRayPos, MidLeftRayPos, MidRightRayPos;
    private void Awake()
    {
        Player = GetComponent<Player>();
        
    }
    private void Update()
    {
        RayColliders();
    }
    private void RayColliders()
    {
        RayCollidersDown();
        RayCollidersLeft();
        RayCollidersRight();
        RayCollidersUp();
    }
    /// <summary>
    /// Makes a raycast from all 3 raypositionholders on the right side of the player object, if any ray hits a worldmaterial object the character can no longer move in the Right direction
    /// </summary>
    private void RayCollidersRight()
    {
        int right = 0;
        if (Physics.Raycast(UpRightRayPos.position, transform.TransformDirection(Vector3.right), out RaycastHit rightDownHit, 0.30f))
        {
            if (rightDownHit.transform.gameObject.CompareTag("WorldMaterial"))
            {
                right++;
            }
        }
        if (Physics.Raycast(DownRightRayPos.position, transform.TransformDirection(Vector3.right), out RaycastHit rightUpHit, 0.30f))
        {
            if (rightUpHit.transform.gameObject.CompareTag("WorldMaterial"))
            {
                right++;
            }
        }
        if (Physics.Raycast(MidRightRayPos.position, transform.TransformDirection(Vector3.right), out RaycastHit rightMidHit, 0.30f))
        {
            if (rightMidHit.transform.gameObject.CompareTag("WorldMaterial"))
            {
                right++;
            }
        }
        if (right > 0)
        {
            Player.PlayerAttributes.CollidingRight = true;
        }
        else
        {
            Player.PlayerAttributes.CollidingRight = false;
        }
    }
    /// <summary>
    /// Makes a raycast from all 3 raypositionholders on the left side of the player object, if any ray hits a worldmaterial object the character can no longer move in the Left direction
    /// </summary>
    private void RayCollidersLeft()
    {
        int left = 0;
        if (Physics.Raycast(DownLeftRayPos.position, transform.TransformDirection(Vector3.left), out RaycastHit leftDownHit, 0.30f))
        {
            if (leftDownHit.transform.gameObject.CompareTag("WorldMaterial"))
            {
                left++;
            }
        }
        if (Physics.Raycast(UpLeftRayPos.position, transform.TransformDirection(Vector3.left), out RaycastHit leftUpHit, 0.30f))
        {
            if (leftUpHit.transform.gameObject.CompareTag("WorldMaterial"))
            {
                left++;
            }
        }
        if (Physics.Raycast(MidLeftRayPos.position, transform.TransformDirection(Vector3.left), out RaycastHit leftMidHit, 0.30f))
        {
            if (leftMidHit.transform.gameObject.CompareTag("WorldMaterial"))
            {
                left++;
            }
        }
        if (left > 0)
        {
            Player.PlayerAttributes.CollidingLeft = true;
        }
        else
        {
            Player.PlayerAttributes.CollidingLeft = false;
        }
    }
    /// <summary>
    /// Makes a raycast from both raypositionholders on the down side of the player object, 
    /// if any ray hits a worldmaterial the HittingTheGround() sets the grounded variable to true and locks the players y-position, 
    /// if they are not hitting a WorldMaterial object grounded is set to false which enables gravity
    /// </summary>
    private void RayCollidersDown()
    {
        int down = 0;
        float yPos = transform.position.y;
        if (Physics.Raycast(DownLeftRayPos.position, transform.TransformDirection(Vector3.down), out RaycastHit downLeftHit, 0.60f))
        {
            if (downLeftHit.transform.gameObject.CompareTag("WorldMaterial") && Player.PlayerAttributes.CounterJumptime > 0.2f || downLeftHit.transform.gameObject.CompareTag("WorldMaterial") && !Player.PlayerAttributes.Jumping)
            {
                yPos = downLeftHit.point.y + 1.05f;
                down++;
            }
        }
        if (Physics.Raycast(DownRightRayPos.position, transform.TransformDirection(Vector3.down), out RaycastHit downRightHit, 0.60f))
        {
            if (downRightHit.transform.gameObject.CompareTag("WorldMaterial") && Player.PlayerAttributes.CounterJumptime > 0.2f || downRightHit.transform.gameObject.CompareTag("WorldMaterial") && !Player.PlayerAttributes.Jumping)
            {
                yPos = downRightHit.point.y + 1.05f;
                down++;
            }
        }
        if (down > 0)
        {
            HittingTheGround(true, yPos);
        }
        else
        {
            HittingTheGround(false, yPos);
        }
    }
    /// <summary>
    /// Makes a raycast from both raypositionholders on the up side of the player object, 
    /// if any ray hits a worldmaterial the HittingTheRoof() sets the y velocity of the player to 0 and removes the players ability to continue jumping
    /// </summary>
    private void RayCollidersUp()
    {
        int up = 0;
        if (Physics.Raycast(UpLeftRayPos.position, transform.TransformDirection(Vector3.up), out RaycastHit upLeftHit, 0.45f) && Player.PlayerAttributes.GravitationalForce > 0)
        {
            if (upLeftHit.transform.gameObject.CompareTag("WorldMaterial"))
            {
                up++;
            }
        }
        if (Physics.Raycast(UpRightRayPos.position, transform.TransformDirection(Vector3.up), out RaycastHit upRightHit, 0.45f) && Player.PlayerAttributes.GravitationalForce > 0)
        {
            if (upRightHit.transform.gameObject.CompareTag("WorldMaterial"))
            {
                up++;
            }
        }
        if (up > 0)
        {
            HittingTheRoof(true);
        }
        else
        {
            HittingTheRoof(false);
        }
    }
    private void HittingTheGround(bool hit, float yPos)
    {
        if (hit)
        {
            if (!Player.PlayerAttributes.Grounded)
            {
                Player.PlayerAttributes.PlayerVelocity /= 1.25f; // reduces the PlayerVelocity to create a slight feeling of slowing down as you hit the ground
            }
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
            Player.PlayerAttributes.Grounded = true;
            Player.PlayerAttributes.CanJump = true;
            Player.PlayerAttributes.CanWalljump = false;
            Player.PlayerAttributes.CounterJumptime = 0;
            Player.PlayerAttributes.GravitationalForce = 0;
            Player.PlayerAttributes.WalljumpCooldown = 0;
        }
        else
        {
            if (Player.PlayerAttributes.Grounded)
            {
                Player.PlayerAttributes.JumpStartingVelocity = Player.PlayerAttributes.PlayerVelocity; // The JumpStartVelocity increases the velocity of the jump and is only checked while Player is grounded
            }
            Player.PlayerAttributes.Grounded = false;
        }
    }
    private void HittingTheRoof(bool hit)
    {
        if (hit)
        {
            Player.PlayerAttributes.CollidingUp = true;
            Player.PlayerAttributes.CanJump = false;
            Player.PlayerAttributes.Jumping = false;
            Player.PlayerAttributes.GravitationalForce = 0;
        }
        else
        {
            Player.PlayerAttributes.CollidingUp = false;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!other.gameObject.GetComponent<Enemy>().Destroyed && Player.PlayerAttributes.CanTakeDamage)
            {
                Player.TakeDamage(other.GetComponent<Enemy>().DamageDealt);
                Player.PlayerAttributes.PlayerVelocity = 0;
                Player.PlayerAttributes.GravitationalForce = 0;
                Player.PlayerAttributes.CanJump = false;
            }
        }
        if (other.gameObject.tag == "Lava")
        {
            if (other.gameObject.GetComponent<Lava>() && Player.PlayerAttributes.CanTakeDamage)
            {
                Player.TakeDamage(other.GetComponent<Lava>().DamageDealt);
                Player.PlayerAttributes.PlayerVelocity = 0;
                Player.PlayerAttributes.GravitationalForce = 0;
                Player.PlayerAttributes.CanJump = false;
            }
        }
        if (other.gameObject.tag == "Teleporter")
        {
            Player.PlayerAttributes.CanActivateTeleporter = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Teleporter")
        {
            Player.PlayerAttributes.CanActivateTeleporter = false;
        }
        
    }
}
