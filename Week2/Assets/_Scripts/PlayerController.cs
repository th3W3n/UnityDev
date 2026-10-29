using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX, movementZ;
    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] private GameObject enemy;

    [SerializeField] private AudioClip hitWallClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip pickupClip;
    private AudioSource source;

    [SerializeField] private VisualEffect explosion;
    [SerializeField] private VisualEffect pickupFX;
    [SerializeField] private VisualEffect winFX;
    [SerializeField] private ParticleSystem trailPS;

    [SerializeField] private Transform pickupsParent;

    private Vector3 initPos;

    private int count;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();

        initPos = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0f, movementZ);
        rb.AddForce(movement * moveSpeed);
    }

    public void OnMove(InputValue _moveValue)
    {
        Vector2 moveVec = _moveValue.Get<Vector2>();
        movementX = moveVec.x;
        movementZ = moveVec.y;

        bool isMoving = moveVec != Vector2.zero;
        if (isMoving && !trailPS.isEmitting) trailPS.Play();
        else if (!isMoving && trailPS.isEmitting) trailPS.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            source.PlayOneShot(pickupClip);
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
            pickupFX.transform.position = other.transform.position;
            pickupFX.Play();
        }
        if (count >= 12)
        {
            source.PlayOneShot(winClip);
            GM.Instance.EndGameUI(true);
            enemy.SetActive(false);
            winFX.Play();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            source.PlayOneShot(hitWallClip);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.transform.parent.GetComponent<EnemyController>().PlayGameLose();
            GM.Instance.EndGameUI(false);
            explosion.transform.position = transform.position;
            explosion.Play();
            gameObject.SetActive(false);
        }
    }

    private void SetCountText() => GM.Instance.countTxt.text = "Count: " + count.ToString();
    public void ResetState()
    {
        gameObject.SetActive(true);
        rb.position = initPos;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        count = 0;
        SetCountText();
        foreach (Transform pickup in pickupsParent.transform)
            pickup.gameObject.SetActive(true);
    }
}