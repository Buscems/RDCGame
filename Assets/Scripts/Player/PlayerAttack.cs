using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public bool crouchAttack;

    // Start is called before the first frame update
    void Start()
    {
        
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
            if (collision.GetComponent<Spearman>() != null)
            {
                var spear = collision.GetComponent<Spearman>();
                //Check to see if the player is doing an attack that is opposite the position of the shield
                if (spear.highShield && crouchAttack)
                {
                    collision.GetComponent<BaseEnemy>().health -= damage;
                    collision.GetComponent<BaseEnemy>().hitEffect.transform.position = collision.transform.position;
                    collision.GetComponent<BaseEnemy>().hitEffect.gameObject.SetActive(true);
                }
                if (spear.lowShield && !crouchAttack)
                {
                    collision.GetComponent<BaseEnemy>().health -= damage;
                    collision.GetComponent<BaseEnemy>().hitEffect.transform.position = collision.transform.position;
                    collision.GetComponent<BaseEnemy>().hitEffect.gameObject.SetActive(true);
                }
            }
            else
            {
                collision.GetComponent<BaseEnemy>().health -= damage;
                collision.GetComponent<BaseEnemy>().hitEffect.transform.position = collision.transform.position;
                collision.GetComponent<BaseEnemy>().hitEffect.gameObject.SetActive(true);
            }
        }
    }

}
