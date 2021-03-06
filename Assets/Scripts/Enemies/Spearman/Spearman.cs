using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : MonoBehaviour
{

    BaseEnemy bE;
    Rigidbody2D rb;

    [Header("ShieldPosition")]
    public Vector2 minMaxStanceTimer;
    float shieldTimer;
    //[HideInInspector]
    public bool highShield, lowShield;

    [Header("SpearAttack")]
    [Tooltip("How long to wait between attacks")]
    public Vector2 minMaxAttackDelay;
    public int damage;
    public float idealDistanceFromPlayer;
    [SerializeField]
    bool idealRange;
    public Vector2 timerToChangeMovementMinMax;
    float timerToChangeMovement;
    bool moveLeft, lookingLeft;
    //[HideInInspector]
    public bool turnedAround;
    public Vector2 turnAroundTimerMinMax;
    float turnAroundTimer;
    public float slowSpeedMultiplier;
    [Tooltip("When the player is in the ideal distance for the enemy have the enemy recognize it and move slightly back and forth from the ideal distance based on this number")]
    public float distanceBuffer;


    // Start is called before the first frame update
    void Start()
    {
        bE = GetComponent<BaseEnemy>();
        rb = GetComponent<Rigidbody2D>();
        highShield = true;
        shieldTimer = Random.Range(minMaxStanceTimer.x, minMaxStanceTimer.y);
        turnAroundTimer = Random.Range(turnAroundTimerMinMax.x, turnAroundTimerMinMax.y);
        turnedAround = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if the base enemy script was taking care of enemy movement and if it is then override it. Might seem counterintuitive but it prevents issues and allows enemies with basic movement to be handled by the base script
        if (bE.isMovingEnemy)
        {
            bE.isMovingEnemy = false;
        }

        bE.enemyAnim.SetFloat("speed", Mathf.Abs(bE.velocity.x));

        if (bE.isAggro)
        {
            //Check if the spearman is in its ideal range of the player
            if (Mathf.Abs((bE.target.position.x - transform.position.x)) > idealDistanceFromPlayer && !idealRange)
            {
                bE.velocity = new Vector2((bE.target.position.x - transform.position.x) * bE.moveSpeed, bE.velocity.y);
            }
            else if(!idealRange) //Have it do the shimmy sham
            {
                idealRange = true;
            }

            if (idealRange)
            {

                //make sure the spearman looks at the player
                if (bE.target.position.x - transform.position.x < 0)
                {
                    //need to fix to make it so that when they first look at the player it recognizes that they are looking left and vice versa
                    if (!lookingLeft)
                    {
                        turnAroundTimer -= Time.deltaTime;
                        turnedAround = true;
                        if (turnAroundTimer < 0)
                        {
                            transform.localScale = new Vector3(-1, 1);
                            turnAroundTimer = Random.Range(turnAroundTimerMinMax.x, turnAroundTimerMinMax.y);
                            turnedAround = false;
                            lookingLeft = true;
                        }
                    }
                }
                if (bE.target.position.x - transform.position.x > 0)
                {
                    if (lookingLeft)
                    {
                        turnAroundTimer -= Time.deltaTime;
                        turnedAround = true;
                        if (turnAroundTimer < 0)
                        {
                            transform.localScale = new Vector3(1, 1);
                            turnAroundTimer = Random.Range(turnAroundTimerMinMax.x, turnAroundTimerMinMax.y);
                            turnedAround = false;
                            lookingLeft = false;
                        }
                    }
                }

                if (Mathf.Abs(bE.target.position.x - transform.position.x) > (idealDistanceFromPlayer + 2))
                {
                    bE.noTurn = false;
                    idealRange = false;
                }
                bE.noTurn = true;

                timerToChangeMovement -= Time.deltaTime;
                if(timerToChangeMovement < 0)
                {
                    if (moveLeft)
                    {
                        moveLeft = false;
                    }
                    else
                    {
                        moveLeft = true;
                    }
                    timerToChangeMovement = Random.Range(timerToChangeMovementMinMax.x, timerToChangeMovementMinMax.y);
                }

                if (moveLeft)
                {
                    bE.velocity = new Vector2((bE.target.position.x - transform.position.x) * (bE.moveSpeed * slowSpeedMultiplier), bE.velocity.y);
                }
                else
                {
                    bE.velocity = new Vector2((transform.position.x - bE.target.position.x) * (bE.moveSpeed * slowSpeedMultiplier), bE.velocity.y);
                }

            }

        }

        //Have the spearman switch between high and low shield stances to deflect attacks
        shieldTimer -= Time.deltaTime;
        if(shieldTimer <= 0)
        {
            if (highShield)
            {
                lowShield = true;
                highShield = false;
                shieldTimer = Random.Range(minMaxStanceTimer.x, minMaxStanceTimer.y);
            }
            else if (lowShield)
            {
                highShield = true;
                lowShield = false;
                shieldTimer = Random.Range(minMaxStanceTimer.x, minMaxStanceTimer.y);
            }
        }

        bE.enemyAnim.SetBool("lowShield", lowShield);
        bE.enemyAnim.SetBool("highShield", highShield);
    }



    private void FixedUpdate()
    {
        if (!bE.isDead)
        {
            rb.MovePosition(rb.position + bE.velocity * Time.deltaTime);
        }
    }

}
