using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector]
    public int damage;

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
            collision.GetComponent<BaseEnemy>().health -= damage;
            collision.GetComponent<BaseEnemy>().hitEffect.transform.position = collision.transform.position;
            collision.GetComponent<BaseEnemy>().hitEffect.gameObject.SetActive(true);
        }
    }

}
