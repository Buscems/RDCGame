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
            if(collision.tag == "Enemy")
            {
                //This should only apply to the spearman enemy and any others that may be able to block an attack from a high or low stance
                if (collision.GetComponent<Entity>() != null)
                {
                    var spear = collision.GetComponent<Entity>();
                    //Check to see if the player is doing an attack that is opposite the position of the shield
                    if (!spear.turnedAround)
                    {
                        if (spear.highShield && crouchAttack)
                        {
                            collision.GetComponent<Base>().health -= damage;
                            collision.GetComponent<Base>().hitEffect.transform.position = collision.transform.position;
                            if (collision.GetComponent<Base>().hitEffect.gameObject.activeSelf == false)
                            {
                                collision.GetComponent<Base>().hitEffect.gameObject.SetActive(true);
                            }
                        }
                        if (spear.lowShield && !crouchAttack)
                        {
                            collision.GetComponent<Base>().health -= damage;
                            collision.GetComponent<Base>().hitEffect.transform.position = collision.transform.position;
                            if (collision.GetComponent<Base>().hitEffect.gameObject.activeSelf == false)
                            {
                                collision.GetComponent<Base>().hitEffect.gameObject.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        collision.GetComponent<Base>().health -= damage;
                        collision.GetComponent<Base>().hitEffect.transform.position = collision.transform.position;
                        if (collision.GetComponent<Base>().hitEffect.gameObject.activeSelf == false)
                        {
                            collision.GetComponent<Base>().hitEffect.gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    collision.GetComponent<Base>().health -= damage;
                    collision.GetComponent<Base>().hitEffect.transform.position = collision.transform.position;
                    if (collision.GetComponent<Base>().hitEffect.gameObject.activeSelf == false)
                    {
                        collision.GetComponent<Base>().hitEffect.gameObject.SetActive(true);
                    }
                }
            }
        }

    }
}
