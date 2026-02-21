using UnityEngine;

namespace Misc
{
    public class HiddenPaths : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            gameObject.SetActive(false);

        }
    }
}
