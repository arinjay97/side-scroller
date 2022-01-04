using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForCoin = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("I collected a coin");
            FindObjectOfType<GameSession>().AddToScore(pointsForCoin);
            AudioSource.PlayClipAtPoint(coinPickupSFX, gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}
