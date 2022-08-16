using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public int damage;

    public Vector2 knockbackForce;
    [HideInInspector]
    public bool lookingLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            var player = collision.GetComponent<PlayerMovement>();
            player.currentHealth -= damage;

            if (lookingLeft)
            {
                var reverseKnockback = new Vector2(-knockbackForce.x, knockbackForce.y);
                player.Knockback(reverseKnockback);
            }
            else
            {
                player.Knockback(knockbackForce);
            }
        }
    }

}
