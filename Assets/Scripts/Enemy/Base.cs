using UnityEngine;

namespace Enemy
{
    public class Base : MonoBehaviour
    {

        Rigidbody2D rb;

        [Header("EnemyStats")]
        public float maxHealth;
        [HideInInspector]
        public float health;
        public float moveSpeed;
        [HideInInspector]
        public Vector2 velocity;
        public bool isMovingEnemy;
        [HideInInspector]
        public bool isDead;
        string lastDirection;
        [HideInInspector]
        public bool noTurn;

        public GameObject hitEffect;
        public GameObject deathEffect;

        public GameObject[] weapons;

        [Header("Aggro")]
        //[HideInInspector]
        public bool isAggro;
        public BoxCollider2D aggroChecker;
        public Vector2 aggroRange;
        public Transform target;

        [Header("Animations")]
        [HideInInspector]
        public Animator enemyAnim;

        // Start is called before the first frame update
        void Start()
        {

            rb = GetComponent<Rigidbody2D>();
            enemyAnim = GetComponent<Animator>();

            health = maxHealth;

            //Setting up aggro variables
            aggroChecker.size = aggroRange;
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Update is called once per frame
        void Update()
        {
            //set up some animator variables
            if (isMovingEnemy)
            {
                enemyAnim.SetFloat("speed", Mathf.Abs(velocity.x));
            }

            //Make sure that the enemy is in check with the aggro checker while the enemy is alive
            if (!isDead)
            {
                this.isAggro = aggroChecker.GetComponent<AggroChecker>().isAggro;
            }
            else 
            { 
                isAggro = false; 
            }

            //check if the enemy is dead
            if(health <= 0 && !isDead)
            {
                Death();
            }

            if(!noTurn)
            {
                if (velocity.x < 0)
                {
                    lastDirection = "left";
                }
                else if (velocity.x > 0)
                {
                    lastDirection = "right";
                }

                if (lastDirection == "left")
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                if (lastDirection == "right")
                {
                    transform.localScale = new Vector3(1, 1);
                }
            }

            //check if the enemy is aggroed 
            if (isAggro)
            {
                //if the enemy is aggroed and is an enemy that moves move towards the player
                if (isMovingEnemy)
                {
                    velocity = new Vector2((target.position.x - transform.position.x) * moveSpeed, velocity.y);
                }
            }

        }

        private void FixedUpdate()
        {
            if (isMovingEnemy && !isDead)
            {
                rb.MovePosition(rb.position + velocity * Time.deltaTime);
            }
        }

        void Death()
        {

            isDead = true;
            velocity.x = 0;
            //change this to use animations later

            enemyAnim.SetTrigger("Death");
            Instantiate(deathEffect, this.transform.position, Quaternion.identity);
            if(weapons.Length > 0)
            {
                for(int i = 0; i < weapons.Length; i++)
                {
                    Destroy(weapons[i]);
                } 
            }
        
        }

    }
}
