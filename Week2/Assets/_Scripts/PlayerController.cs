using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX, movementZ;
    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] private TextMeshProUGUI countTxt;
    private int count;
    [SerializeField] private GameObject winTxtGo;

    [SerializeField] private GameObject enemy;

    [SerializeField] private AudioClip hitWallClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip pickupClip;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTxtGo.SetActive(false);
        winTxtGo.GetComponent<TextMeshProUGUI>().text = "You Win!!!";
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0f, movementZ);
        rb.AddForce(movement * moveSpeed);
    }

    private void OnMove(InputValue _moveValue)
    {
        Vector2 moveVec = _moveValue.Get<Vector2>();
        movementX = moveVec.x;
        movementZ = moveVec.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            source.PlayOneShot(pickupClip);
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
        if (count >= 12)
        {
            source.PlayOneShot(winClip);
            winTxtGo.SetActive(true);
            Destroy(enemy);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            source.PlayOneShot(hitWallClip);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.transform.parent.GetComponent<EnemyController>().PlayGameLose();
            winTxtGo.SetActive(true);
            winTxtGo.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            Destroy(gameObject);
        }
    }

    private void SetCountText()
    {
        countTxt.text = "Count: " + count.ToString();
    }
}