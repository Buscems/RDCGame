using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScript : MonoBehaviour
{
    [HideInInspector]
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGameplay()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
