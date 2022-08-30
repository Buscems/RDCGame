using UnityEngine;

namespace Enemy
{
    public class HitEffect : MonoBehaviour
    {

        public void DestroyThisObject()
        {
            Destroy(this.gameObject);
        }

        public void GoAway()
        {
            transform.position = new Vector2(-5000, 0);
            gameObject.SetActive(false);
        }

    }
}
