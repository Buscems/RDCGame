using Rewired;
using Rewired.ControllerExtensions;
using UnityEngine;
using Utility;
using static Utility.AnimatorConstants;

namespace Player
{
    public class Overworld : MonoBehaviour
    {

        //the following is in order to use rewired
        [Tooltip("Reference for using rewired")]
        [HideInInspector]
        public Rewired.Player myPlayer;
        [Header("Rewired")]
        [Tooltip("Number identifier for each player, must be above 0")]
        public int playerNum;

        [Header("Movement")]
        public float speed;
        [SerializeField]
        Vector2 velocity;
        Rigidbody2D rb;
        int direction;

        Animator anim;

        public FadeScript fade;

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

            anim = GetComponent<Animator>();

            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {

            anim.SetFloat(Direction, direction);

            velocity = new Vector2(myPlayer.GetAxisRaw("MoveHorizontal"), myPlayer.GetAxisRaw("MoveVertical")) * speed;
            if(myPlayer.GetAxis("MoveHorizontal") < .2f && myPlayer.GetAxis("MoveVertical") < .3f && myPlayer.GetAxis("MoveVertical") > -.3f)
            {
                direction = 0;
            }
            else if(myPlayer.GetAxis("MoveVertical") > .2f && myPlayer.GetAxis("MoveHorizontal") < .3f && myPlayer.GetAxis("MoveHorizontal") > -.3f)
            {
                direction = 1;
            }
            else if(myPlayer.GetAxis("MoveHorizontal") > .2f && myPlayer.GetAxis("MoveVertical") < .3f && myPlayer.GetAxis("MoveVertical") > -.3f)
            {
                direction = 2;
            }
            else if(myPlayer.GetAxis("MoveVertical") < .2f && myPlayer.GetAxis("MoveHorizontal") < .3f && myPlayer.GetAxis("MoveHorizontal") > -.3f)
            {
                direction = 3;
            }
        
            if(Mathf.Abs(myPlayer.GetAxisRaw("MoveHorizontal")) > 0 || Mathf.Abs(myPlayer.GetAxisRaw("MoveVertical")) > 0)
            {
                anim.SetFloat(Walk, 1);
            }
            else
            {
                anim.SetFloat(Walk, 0);
            }

        }

        void FixedUpdate()
        {
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Area 1"))
            {
                fade.anim.SetTrigger(FadeIn);
            }
        }

        //[REWIRED METHODS]
        //these two methods are for ReWired, if any of you guys have any questions about it I can answer them, but you don't need to worry about this for working on the game - Buscemi
        void OnControllerConnected(ControllerStatusChangedEventArgs arg)
        {
            CheckController(myPlayer);
        }

        void CheckController(Rewired.Player player)
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
}
