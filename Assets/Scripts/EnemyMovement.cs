using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float enemySpeed = 1f;
    [SerializeField] AudioClip deathAudio;

    Rigidbody2D enemy;
    BoxCollider2D reversePeriscope;
    Player player;
    Animator playerAnimator;
    
    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
        reversePeriscope = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<Player>();
        playerAnimator = player.GetComponent<Animator>();
        
    }

    void Update()
    {
        Run();
        
    }

    void Run()
    {
        enemy.velocity = new Vector2(enemySpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        enemySpeed = -enemySpeed;
        EnemyFlip();
    }

    void EnemyFlip()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemy.velocity.x)), 1f);
    }

    //void EnemyFlip()
    //{
    //    if (!reversePeriscope.IsTouchingLayers(LayerMask.GetMask("Ground")))
    //    {
    //        transform.localScale = new Vector2(Mathf.Sign(enemy.velocity.x), 1f);
    //        enemySpeed = -enemySpeed;
    //    }
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(deathAudio, gameObject.transform.position);
            player.isAlive = false;
            playerAnimator.SetTrigger("Dead");
            FindObjectOfType<GameSession>().playerDeath();
        }
    }

}
