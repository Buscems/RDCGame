using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
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
}
