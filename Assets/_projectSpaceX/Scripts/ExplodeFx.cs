using System.Collections;
using UnityEngine;

namespace _projectSpaceX.Scripts
{
    public class ExplodeFx : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(ClearExplosion());
        }
        private IEnumerator ClearExplosion()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}
