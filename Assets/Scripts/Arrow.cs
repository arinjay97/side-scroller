using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed = 10f;
    [SerializeField] int pointsForEnemy = 50;
    [SerializeField] AudioClip deathSFX;

    Rigidbody2D arrow;
    Player player;
    CapsuleCollider2D arrowCollider;
    float arrowVelocity;
    float playerDirection;

    void Start()
    {
        arrow = GetComponent<Rigidbody2D>();
        arrowCollider = GetComponent<CapsuleCollider2D>();
        player = FindObjectOfType<Player>();
        playerDirection = player.transform.localScale.x;
        arrowVelocity = arrowSpeed * playerDirection;
    }

    void Update()
    {
        if(playerDirection > 0)
        {
            arrow.velocity = new Vector2(arrowVelocity, 0);
        }
        if(playerDirection < 0)
        {
            arrow.velocity = new Vector2(arrowVelocity, 0);
            transform.localScale = new Vector2(-5, -5);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            FindObjectOfType<GameSession>().AddToScore(pointsForEnemy);
            AudioSource.PlayClipAtPoint(deathSFX, gameObject.transform.position);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
