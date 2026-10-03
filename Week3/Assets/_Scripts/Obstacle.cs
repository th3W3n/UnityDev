using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float minSize = 0.5f;
    [SerializeField] private float maxSize = 2.0f;
    [SerializeField] private float minSpeed = 50.0f;
    [SerializeField] private float maxSpeed = 150.0f;
    [SerializeField] private float maxSpinSpeed = 10.0f;
    [SerializeField] private GameObject collisionParticle;

    private void Start()
    {
        float randSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randSize, randSize, 1);
        float randSpeed = Random.Range(minSpeed, maxSpeed) / randSize;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 randDir = Random.insideUnitCircle;
        rb.AddForce(randDir * randSpeed);
        float randTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randTorque);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (collisionParticle != null)
            {
                Vector2 hitSpot = collision.GetContact(0).point;
                Instantiate(collisionParticle, hitSpot, Quaternion.identity);
            }
        }
    }
}