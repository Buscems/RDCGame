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
    [HideInInspector]
    public bool highShield, lowShield;

    [Header("SpearAttack")]
    [Tooltip("How long to wait between attacks")]
    public Vector2 minMaxAttackDelay;
    public int damage;
    public float idealDistanceFromPlayer;
    [Tooltip("When the player is in the ideal distance for the enemy have the enemy recognize it and move slightly back and forth from the ideal distance based on this number")]
    public float distanceBuffer;


    // Start is called before the first frame update
    void Start()
    {
        bE = GetComponent<BaseEnemy>();
        rb = GetComponent<Rigidbody2D>();
        highShield = true;
        shieldTimer = Random.Range(minMaxStanceTimer.x, minMaxStanceTimer.y);
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
            if (Mathf.Abs((bE.target.position.x - transform.position.x)) > idealDistanceFromPlayer)
            {
                bE.velocity = new Vector2((bE.target.position.x - transform.position.x) * bE.moveSpeed, bE.velocity.y);
            }
            else //Have it do the shimmy sham
            {
                bE.velocity.x = 0;
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
        rb.MovePosition(rb.position + bE.velocity * Time.deltaTime);
    }

}
