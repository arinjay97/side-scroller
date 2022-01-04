using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float playerSpeed = 10;
    [SerializeField] float jumpStrength = 1;
    [SerializeField] float climbSpeed = 10;
    [SerializeField] float waterGravity = 1f;
    [SerializeField] float waterJumpStrength = 5;
    [SerializeField] Transform bow;
    [SerializeField] GameObject arrow;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] float volume = 3f;

    AudioSource audioSource;
    Vector2 moveInput;
    Rigidbody2D player;
    Animator playerAnimator;
    CapsuleCollider2D playerCollider;
    float gravityScaleAtStart;
    float jumpAtStart;
    BoxCollider2D feetCollider;

    public bool isAlive = true;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = player.gravityScale;
        audioSource = GetComponent<AudioSource>();
        jumpAtStart = jumpStrength;
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }

        Run();
        FlipPlayer();
        ClimbLadder();
        playerDeath();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * playerSpeed, player.velocity.y);
        player.velocity = playerVelocity;

        bool isPlayerMoving = Mathf.Abs(player.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", isPlayerMoving);

        //Originally used this logic
        //
        //if(player.velocity.x == 0 && player.velocity.y == 0)
        //{
        //    playerAnimator.SetBool("isRunning", false);
        //}
        //else
        //{
        //    playerAnimator.SetBool("isRunning", true);
        //}
    }

    void FlipPlayer()
    {
        bool isPlayerMoving = Mathf.Abs(player.velocity.x) > Mathf.Epsilon;

        if (isPlayerMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(player.velocity.x), 1f);
        }
    }

    void OnJump(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        //if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            player.velocity += new Vector2(0f, jumpStrength);
        }

        //if (value.isPressed && playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        //{
        //    player.velocity += new Vector2(0f, jumpStrength);
        //}
    }

    void ClimbLadder()
    {
        //if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        //{
        //    player.gravityScale = gravityScaleAtStart;
        //    playerAnimator.SetBool("isClimbing", false);
        //    return;
        //}
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            player.gravityScale = gravityScaleAtStart;
            playerAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(player.velocity.x, moveInput.y * climbSpeed);
        player.velocity = climbVelocity;
        player.gravityScale = 0;

        bool isPlayerClimbing = Mathf.Abs(player.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("isClimbing", isPlayerClimbing);

        //if (value.isPressed)
        //{
        //    transform.Translate(0, 1, 0);
        //    playerAnimator.SetBool("isClimbing", true);
        //}
    }

    //void OnRoll(InputValue value)
    //{

    //    if (value.isPressed)
    //    {
    //        bool isPressed = true;
    //    }
    //    playerAnimator.SetBool("isRolling", true);
    //}


    void playerDeath()
    {

        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            Debug.Log("Died to enemy");
            isAlive = false;
            playerAnimator.SetTrigger("Dead");
            FindObjectOfType<GameSession>().playerDeath();
        }
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Traps")))
        {
            Debug.Log("Died to traps");
            isAlive = false;
            playerAnimator.SetTrigger("Dead");
            FindObjectOfType<GameSession>().playerDeath();
            
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        Debug.Log("Shooting");
        Instantiate(arrow, bow.position, transform.rotation);
        playerAnimator.SetTrigger("Shoot");

        //Tried to use booleans for shooting as well
        //bool isShooting = false;
        //if(value.isPressed)
        //{
        //    isShooting = true;
        //}
        //playerAnimator.SetBool("isShooting", isShooting);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Logic for changing gravity in water. I was able to change the jump speed in water
        // to make it kind of like swimming in mario where you press space to keep afloat 
        if (other.tag == "Water")
        {
            player.gravityScale = 0.1f;
            return;
        }
        if (other.tag == "Enemy")
        {
            Debug.Log("Play");
            AudioSource.PlayClipAtPoint(deathAudio, Camera.main.transform.position);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Water")
        {
            jumpStrength = waterJumpStrength;
            return;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Water")
        {
            jumpStrength = jumpAtStart;
            player.gravityScale = gravityScaleAtStart;
            return;
        }
    }
}
