using UnityEngine;

namespace Enemy
{
    public class Attack : MonoBehaviour
    {

        public int damage;

        public Vector2 knockbackForce;
        [HideInInspector]
        public bool lookingLeft;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            var player = collision.GetComponent<Player.Movement>();
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
