using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpecial : MonoBehaviour
{
    [SerializeField]
    private float speedMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        float lifetime = ps.main.duration + 0.5f;
        StartCoroutine(Lifetime(lifetime));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnParticleCollision(GameObject other)
    {
        other.GetComponent<Enemy>().ApplySpeedEffect(speedMultiplier, false);
        other.GetComponent<SpriteRenderer>().color = Element.getElementColor("Water");
    }

    private IEnumerator Lifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
