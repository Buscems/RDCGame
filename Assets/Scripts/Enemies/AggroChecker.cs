using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroChecker : MonoBehaviour
{
    [HideInInspector]
    public bool isAggro;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isAggro = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isAggro = false;
        }
    }

}
