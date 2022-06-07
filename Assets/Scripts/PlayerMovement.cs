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
    [SerializeField]
    Vector2 velocity;
    Rigidbody2D rb;
    float acceleration;
    public float inAirAccelerationRate;
    public float accelerationRate;
    public float decelerationRate;

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
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {

        Gravity();

        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    void Movement()
    {

        //Acceleration logic on the ground
        if (onTopOfPlatform)
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
                    isJumping = true;
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
