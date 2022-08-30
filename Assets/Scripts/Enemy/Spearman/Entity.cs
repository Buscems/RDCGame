using UnityEngine;
using Utility;

namespace Enemy.Spearman
{ 
    public class Entity : MonoBehaviour
    {
        private Base _bE;
        private Rigidbody2D _rb;
        private float _shieldTimer, _timerBetweenAttacks, _turnAroundTimer, _timerToChangeMovement;
        private bool _idealRange, _moveLeft, _lookingLeft, _firstAggro;


        [Header("ShieldPosition")]
        public Vector2 minMaxStanceTimer;
        public bool lowShield, canChangeShield;

        [Header("SpearAttack")]
        [Tooltip("How long to wait between attacks")]
        public Vector2 minMaxAttackDelay;
        public int damage;
        public float idealDistanceFromPlayer;
        [SerializeField]
        public Vector2 timerToChangeMovementMinMax;
        //[HideInInspector]
        public bool turnedAround;
        public Vector2 turnAroundTimerMinMax;
        public float slowSpeedMultiplier;
        // [Tooltip("When the player is in the ideal distance for the enemy have the enemy recognize it and move slightly back and forth from the ideal distance based on this number")] public float distanceBuffer;
        [HideInInspector]
        public bool isAttacking;
        public Attack spear;

        // Start is called before the first frame update
        void Start()
        {
            _bE = GetComponent<Base>();
            _rb = GetComponent<Rigidbody2D>();
            lowShield = false;
            _shieldTimer = Random.Range(minMaxStanceTimer.x, minMaxStanceTimer.y);
            _turnAroundTimer = Random.Range(turnAroundTimerMinMax.x, turnAroundTimerMinMax.y);
            _timerBetweenAttacks = Random.Range(minMaxAttackDelay.x, minMaxAttackDelay.y);
            turnedAround = false;
            _firstAggro = true;
            canChangeShield = true;
            _bE.isMovingEnemy = false;
            _bE.damage = Damage;
        }

        // Update is called once per frame
        void Update()
        {
            _bE.enemyAnim.SetFloat(AnimatorConstants.Speed, Mathf.Abs(_bE.velocity.x));
            var dx = _bE.target.position.x - transform.position.x;

            if (_bE.isAggro)
            {
                //Check if the spearman is in its ideal range of the player
                if (Mathf.Abs(dx) > idealDistanceFromPlayer && !_idealRange)
                {
                    _bE.velocity = new Vector2(dx * _bE.moveSpeed, _bE.velocity.y);
                }
                else {
                    _idealRange = true;
                }

                if (_idealRange)
                {

                    //make sure the spearman looks at the player
                    if (dx < 0)
                    {
                    
                        if (!_lookingLeft)
                        {
                            if (!_firstAggro)
                            {
                                _turnAroundTimer -= Time.deltaTime;
                                turnedAround = true;
                                if (_turnAroundTimer < 0)
                                {
                                    transform.localScale = new Vector3(-1, 1);
                                    _turnAroundTimer = Random.Range(turnAroundTimerMinMax.x, turnAroundTimerMinMax.y);
                                    turnedAround = false;
                                    _lookingLeft = true;
                                }
                            }
                            else
                            {
                                _lookingLeft = true;
                            }
                        }
                    }
                    if (dx > 0)
                    {
                        if (_lookingLeft)
                        {
                            if (!_firstAggro)
                            {
                                _turnAroundTimer -= Time.deltaTime;
                                turnedAround = true;
                                if (_turnAroundTimer < 0)
                                {
                                    transform.localScale = new Vector3(1, 1);
                                    _turnAroundTimer = Random.Range(turnAroundTimerMinMax.x, turnAroundTimerMinMax.y);
                                    turnedAround = false;
                                    _lookingLeft = false;
                                }
                            }
                            else
                            {
                                _lookingLeft = false;
                            }
                        }
                    }

                    if (Mathf.Abs(dx) > (idealDistanceFromPlayer + 2))
                    {
                        _bE.noTurn = false;
                        _idealRange = false;
                    }
                    _bE.noTurn = true;

                    _timerToChangeMovement -= Time.deltaTime;
                    if(_timerToChangeMovement < 0)
                    {
                        _moveLeft = !_moveLeft;
                        _timerToChangeMovement = Random.Range(timerToChangeMovementMinMax.x, timerToChangeMovementMinMax.y);
                    }

                    int direction = _moveLeft ? 1 : -1;

                    _bE.velocity = new Vector2(dx * direction * (_bE.moveSpeed * slowSpeedMultiplier), _bE.velocity.y);

                    attack();
                }

            }


            //Have the spearman switch between high and low shield stances to deflect attacks if they are not attacking
            if (canChangeShield)
            {
                _shieldTimer -= Time.deltaTime;
                if (_shieldTimer <= 0)
                {
                    lowShield = !lowShield;
                    _shieldTimer = Random.Range(minMaxStanceTimer.x, minMaxStanceTimer.y);

                }
            }
            _bE.enemyAnim.SetBool(AnimatorConstants.LowShield, lowShield);
            _bE.enemyAnim.SetBool(AnimatorConstants.HighShield, !lowShield);
        }

        private void attack()
        {
            if (!isAttacking)
            {
                _timerBetweenAttacks -= Time.deltaTime;
                if (_timerBetweenAttacks <= 0)
                {
                    isAttacking = true;
                    canChangeShield = false;
                    spear.lookingLeft = _lookingLeft;
                    _bE.enemyAnim.SetTrigger(AnimatorConstants.Attack1);
                    _timerBetweenAttacks = Random.Range(minMaxAttackDelay.x, minMaxAttackDelay.y);
                }
            }
        }



        private void FixedUpdate()
        {
            if (!_bE.health.IsDead() && !isAttacking)
            {
                _rb.MovePosition(_rb.position + _bE.velocity * Time.deltaTime);
            }
        }

        public void EndAttack()
        {
            isAttacking = false;
            canChangeShield = true;
        }

        private void Damage(int damageToTake, bool crouchAttack, Vector3 pos)
        {
            if (!turnedAround && crouchAttack == lowShield) return;
            _bE.health.Damage(damageToTake);
            _bE.hitEffect.transform.position = pos;
            _bE.hitEffect.gameObject.SetActive(true);
        }

    }
}
