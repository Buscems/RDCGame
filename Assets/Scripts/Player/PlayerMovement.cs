using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class PlayerMovement : MonoBehaviour
{

    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    [HideInInspector]
    public Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    [Header("Platforming Movement")]
    public float speed;
    public float crouchSpeed;
    bool crouch;
    [SerializeField]
    Vector2 velocity;
    Rigidbody2D rb;
    float acceleration;
    public float inAirAccelerationRate;
    public float accelerationRate;
    public float decelerationRate;
    string lastDirection;
    bool cannotMove;

    [Header("Attack")]
    bool isAttacking;
    public int startingDamage;
    int currentDamage;
    public GameObject sword;

    [Header("Gravity Variables")]
    public float gravityUp;
    public float gravityDown;
    public float jumpVel;
    public float movingJumpVel;
    public float jumpTimerMax;
    bool isJumping;
    float jumpTimer;
    public float maxDownVel;
    public float onPlatformTimer;
    public float onPlatformTimerMax;
    public bool onTopOfPlatform;

    [Header("Animator")]
    Animator playerAnimator;
    public SpriteRenderer playerSprite;

    private void Awake()
    {
        //Rewired Code
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        CheckController(myPlayer);
    }

    // Start is called before the first frame update
    void Start()
    {
        //This needs to be changed if saving gets implemented as well as if the player loads into other levels. Should load the previous damage value if it ever gets upgraded.
        currentDamage = startingDamage;

        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (myPlayer.GetButtonDown("Attack"))
        {
            SwordAttack();
        }

        //make sure sword damage is what the current damage of the player has
        if(sword.GetComponent<PlayerAttack>().damage != currentDamage)
        {
            sword.GetComponent<PlayerAttack>().damage = currentDamage;
        }

    }

    private void FixedUpdate()
    {

        Gravity();

        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    void Movement()
    {

        //animation logic
        playerAnimator.SetFloat("speed", Mathf.Abs(velocity.x));
        playerAnimator.SetBool("onGround", onTopOfPlatform);
        playerAnimator.SetBool("crouch", crouch);

        if (velocity.x < 0)
        {
            lastDirection = "left";
        }
        else if(velocity.x > 0)
        {
            lastDirection = "right";
        }

        if (lastDirection == "left")
        {
            transform.localScale = new Vector3(-1, 1);
        }
        if(lastDirection == "right")
        {
            transform.localScale = new Vector3(1, 1);
        }

        if (!cannotMove)
        {

            //crouch logic
            if (onTopOfPlatform)
            {
                if (myPlayer.GetAxis("MoveVertical") < -.7)
                {
                    crouch = true;
                    acceleration = 0;
                }
                else
                {
                    crouch = false;
                }
            }

            //Acceleration logic on the ground
            if (onTopOfPlatform && !crouch)
            {
                //Moving Forward
                if (myPlayer.GetAxisRaw("MoveHorizontal") > 0)
                {
                    if (acceleration <= 0)
                    {
                        acceleration = 0;
                    }
                    if (acceleration < 1)
                    {
                        acceleration += accelerationRate;
                    }
                    else if (acceleration > 1)
                    {
                        acceleration = 1;
                    }
                }

                //Moving Backward
                if (myPlayer.GetAxisRaw("MoveHorizontal") < 0)
                {
                    if (acceleration >= 0)
                    {
                        acceleration = 0;
                    }
                    if (acceleration > -1)
                    {
                        acceleration -= accelerationRate;
                    }
                    else if (acceleration < -1)
                    {
                        acceleration = -1;
                    }
                }

                //Slowing Down
                if (myPlayer.GetAxisRaw("MoveHorizontal") == 0)
                {
                    if (acceleration > 0)
                    {
                        acceleration -= decelerationRate;
                    }
                    else if (acceleration < 0)
                    {
                        acceleration += decelerationRate;
                    }

                    if (Mathf.Abs(acceleration) <= .01f)
                    {
                        acceleration = 0;
                    }

                }
            }

            //Acceleration logic in the air
            if (!onTopOfPlatform)
            {
                //Moving Forward
                if (myPlayer.GetAxisRaw("MoveHorizontal") > 0)
                {
                    if (acceleration < 1)
                    {
                        acceleration += inAirAccelerationRate;
                    }
                    else if (acceleration > 1)
                    {
                        acceleration = 1;
                    }
                }

                //Moving Backward
                if (myPlayer.GetAxisRaw("MoveHorizontal") < 0)
                {
                    if (acceleration > -1)
                    {
                        acceleration -= inAirAccelerationRate;
                    }
                    else if (acceleration < -1)
                    {
                        acceleration = -1;
                    }
                }
            }

            velocity.x = speed * acceleration;

            //jump logic
            if (Mathf.Abs(velocity.x) > 0)
            {
                if (onPlatformTimer > 0)
                {
                    if (myPlayer.GetButtonDown("Jump"))
                    {
                        velocity.y = movingJumpVel;
                        jumpTimer = jumpTimerMax;
                        isJumping = true;
                        onTopOfPlatform = false;
                        playerAnimator.ResetTrigger("land");
                        playerAnimator.SetTrigger("jump");
                        onPlatformTimer = 0;
                    }
                }
                if (myPlayer.GetButton("Jump") && isJumping)
                {
                    velocity.y = movingJumpVel;
                    jumpTimer -= Time.deltaTime;
                }

                if (myPlayer.GetButtonUp("Jump") || jumpTimer <= 0)
                {
                    isJumping = false;
                }
            }
            else if (Mathf.Abs(velocity.x) == 0)
            {
                if (onPlatformTimer > 0)
                {
                    if (myPlayer.GetButtonDown("Jump"))
                    {
                        velocity.y = jumpVel;
                        jumpTimer = jumpTimerMax;
                        playerAnimator.ResetTrigger("jump");
                        isJumping = true;
                        playerAnimator.ResetTrigger("land");
                        playerAnimator.SetTrigger("jump");
                    }
                }
                if (myPlayer.GetButton("Jump") && isJumping)
                {
                    velocity.y = jumpVel;
                    jumpTimer -= Time.deltaTime;
                }

                if (myPlayer.GetButtonUp("Jump") || jumpTimer <= 0)
                {
                    isJumping = false;
                }
            }
        }


        //set timer that will let the player jump slightly off the platform
        if (onTopOfPlatform)
        {
            onPlatformTimer = onPlatformTimerMax;
        }
        else
        {
            onPlatformTimer -= Time.deltaTime;
        }
    }

    void SwordAttack()
    {
        if (!isAttacking)
        {
            if (onTopOfPlatform)
            {
                cannotMove = true;
                acceleration = 0;
                velocity = new Vector2(0, 0);
                if (crouch)
                {
                    sword.GetComponent<PlayerAttack>().crouchAttack = true;
                }
                else
                {
                    sword.GetComponent<PlayerAttack>().crouchAttack = false;
                }
                playerAnimator.SetTrigger("attack");
            }
            else
            {
                cannotMove = true;
                isJumping = false;
                playerAnimator.SetTrigger("attack");
            }
        }
    }

    public void EndAttack()
    {
        cannotMove = false;
    }

    void Gravity()
    {
        //gravity logic
        if (velocity.y > -maxDownVel)
        { //if we haven't reached maxDownVel
            if (velocity.y > 0)
            { //if player is moving up
                velocity.y -= gravityUp * Time.fixedDeltaTime;
            }
            else
            { //if player is moving down
                velocity.y -= gravityDown * Time.fixedDeltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        foreach (ContactPoint2D contact in collisionInfo.contacts)
        {
            //am I coming from the top/bottom?
            if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
            {
                velocity.y = 0; //stop vertical velocity
                if (contact.normal.y >= 0)
                { //am I hitting the top of the platform?

                    onTopOfPlatform = true;
                    playerAnimator.SetTrigger("land");
                }
                //am I hitting the bottom of a platform?
                if (contact.normal.y < 0)
                {
                    //hitHead = true;
                    velocity.y = 0;
                    //gotHitTimer = 0;
                    //maxKnockbackTime = 0;

                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collisionInfo)
    {
        foreach (ContactPoint2D contact in collisionInfo.contacts)
        {
            //am I coming from the top/bottom?
            if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
            {
                velocity.y = 0; //stop vertical velocity
                if (contact.normal.y >= 0)
                { //am I hitting the top of the platform?

                    onTopOfPlatform = true;
                }
                //am I hitting the bottom of a platform?
                if (contact.normal.y < 0)
                {
                    //hitHead = true;
                    velocity.y = 0;
                    //gotHitTimer = 0;
                    //maxKnockbackTime = 0;

                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collisionInfo)
    {
        onTopOfPlatform = false;
    }

    //[REWIRED METHODS]
    //these two methods are for ReWired, if any of you guys have any questions about it I can answer them, but you don't need to worry about this for working on the game - Buscemi
    void OnControllerConnected(ControllerStatusChangedEventArgs arg)
    {
        CheckController(myPlayer);
    }

    void CheckController(Player player)
    {
        foreach (Joystick joyStick in player.controllers.Joysticks)
        {
            var ds4 = joyStick.GetExtension<DualShock4Extension>();
            if (ds4 == null) continue;//skip this if not DualShock4
            switch (playerNum)
            {
                case 4:
                    ds4.SetLightColor(Color.yellow);
                    break;
                case 3:
                    ds4.SetLightColor(Color.green);
                    break;
                case 2:
                    ds4.SetLightColor(Color.blue);
                    break;
                case 1:
                    ds4.SetLightColor(Color.red);
                    break;
                default:
                    ds4.SetLightColor(Color.white);
                    Debug.LogError("Player Num is 0, please change to a number > 0");
                    break;
            }
        }
    }

}
