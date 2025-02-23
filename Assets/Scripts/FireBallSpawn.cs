using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpawn : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Bounds colliderBounds;
    private Vector2 colliderCenter;
    
    private float rotationAngle = 0;
    private Vector2 fbDirection= Vector2.zero;

    void Start()
    {
        boxCollider= GetComponent<BoxCollider2D>();
        colliderBounds = boxCollider.bounds;
        colliderCenter = colliderBounds.center;
    }

    private void Update()
    {
        if(Random.Range(0f,100f) < GameManager.Instance.spawnRatio)
        {
            //Get the bounds of the collider to define the spawn are of Fireballs
            float randomX = Random.Range(colliderCenter.x - colliderBounds.extents.x, colliderCenter.x + colliderBounds.extents.x);
            float randomY = Random.Range(colliderCenter.y - colliderBounds.extents.y, colliderCenter.y + colliderBounds.extents.y);
            Vector2 spawnPosition = new Vector2 (randomX, randomY);
            GameObject fireBall = Instantiate(GameManager.Instance.FireBallPrefab, spawnPosition, Quaternion.identity);

            //Set Fireballs rotation (not that useful XD)
            if(this.gameObject.transform.position.x == 0)
            {
                if (this.gameObject.transform.position.y > 0)
                {
                    rotationAngle = 270;
                    fbDirection = new Vector2(0,-GameManager.Instance.fireBallVelocity);
                }
                else
                {
                    rotationAngle = 90;
                    fbDirection = new Vector2(0, GameManager.Instance.fireBallVelocity);
                }
            }
            else
            {
                if(this.gameObject.transform.position.x > 0)
                {
                    fbDirection = new Vector2(-GameManager.Instance.fireBallVelocity, 0);
                }
                else
                {
                    rotationAngle = 180;
                    fbDirection = new Vector2(GameManager.Instance.fireBallVelocity, 0);
                }
                
                
            }
            fireBall.transform.eulerAngles= Vector3.forward * rotationAngle;
            fireBall.GetComponent<Rigidbody2D>().velocity = fbDirection;
        }
    }

    //If a Fireball get into this collider and enough time has passed since spawned, then it get destroyed and increase player's score (Fireball count)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "Fireball" && collision.gameObject.GetComponent<FireBall>().canBeDestroyed)
        {
            GameManager.Instance.IncreaseScore();
            Destroy(collision.gameObject);
        }
    }
}
