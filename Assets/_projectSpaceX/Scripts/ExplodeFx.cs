using System.Collections;
using UnityEngine;

public class ExplodeFx : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ClearExplosion());
    }
    private IEnumerator ClearExplosion()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
