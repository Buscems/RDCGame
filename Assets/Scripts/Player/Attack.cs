using Enemy;
using Enemy.Spearman;
using UnityEngine;

namespace Player
{
    public class Attack : MonoBehaviour
    {
        [HideInInspector]
        public int damage;
        [HideInInspector]
        public bool crouchAttack;

        // Start is called before the first frame update
        void Start()
        {
            crouchAttack = false;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //check if the attack hits anything that it should be checking and do damage
            if (!collision.CompareTag("Enemy")) return;
            //This should only apply to the spearman enemy and any others that may be able to block an attack from a high or low stance
            var enemy = collision.GetComponent<Base>();
            var damageFunction = enemy.damage;
            damageFunction(damage, crouchAttack, collision.transform.position);
        }

    }
}
