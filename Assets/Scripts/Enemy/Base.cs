using UnityEngine;
using Utility;
using static Utility.AnimatorConstants;

namespace Enemy
{
    public class Base : MonoBehaviour
    {
        private Rigidbody2D _rb;

        [Header("EnemyStats")] 
        public float maxHealth;

        public Health health { get; set; }

        [HideInInspector]
        public float moveSpeed;
        [HideInInspector]
        public Vector2 velocity;
        public bool isMovingEnemy;
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

        private AggroChecker _aggroChecker;

        // Start is called before the first frame update
        void Start()
        {
            _aggroChecker = aggroChecker.GetComponent<AggroChecker>();

            _rb = GetComponent<Rigidbody2D>();
            enemyAnim = GetComponent<Animator>();

            health = new Health(maxHealth, Death);

            //Setting up aggro variables
            aggroChecker.size = aggroRange;
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Update is called once per frame
        private void Update()
        {
            //set up some animator variables
            if (isMovingEnemy)
            {
                enemyAnim.SetFloat(Speed, Mathf.Abs(velocity.x));
            }

            //Make sure that the enemy is in check with the aggro checker while the enemy is alive
            isAggro = !health.IsDead() && _aggroChecker.isAggro;

            if(!noTurn)
            {
                var direction = velocity.x > 0 ? 1 : -1;
                transform.localScale = new Vector3(direction, 1);
            }

            //check if the enemy is aggroed and is an enemy that moves move towards the player
            if (!isAggro || !isMovingEnemy) return;
            velocity = new Vector2((target.position.x - transform.position.x) * moveSpeed, velocity.y);
        }

        private void FixedUpdate()
        {
            if (!isMovingEnemy || health.IsDead()) return;
            _rb.MovePosition(_rb.position + velocity * Time.deltaTime);
        }

        private void Death()
        {
            velocity.x = 0;
            //change this to use animations later

            enemyAnim.SetTrigger(AnimatorConstants.Death);
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            if (weapons.Length <= 0) return;
            
            foreach (var t in weapons) Destroy(t);
        }

        public void Damage(int damage)
        {
            health.Damage(damage);
        }
    }
}
