using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject boosterFlame;
    [SerializeField] private float thrustForce = 1f;
    [SerializeField] private float maxSpeed = 1f;
    private Rigidbody2D rb;

    [SerializeField] private float scoreMultiplier = 10.0f;
    private float elapsedTime = 0f;
    private float score = 0f;

    [SerializeField] private PanelRenderer panel;
    [SerializeField] private VisualElementReference labelRef;
    private Label scoreText;

    [SerializeField] private GameObject explosion;

    private void Awake()
    {
        panel.RegisterUIReloadCallback(OnUIReload);
    }
    private void OnDestroy()
    {
        panel.UnregisterUIReloadCallback(OnUIReload);
    }
    private void Start()
    {
        boosterFlame.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.Floor(elapsedTime * scoreMultiplier);
        if (scoreText != null)
            scoreText.text = $"Score: {score}";

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 dir = (mousePos - transform.position).normalized;
            transform.up = dir;
            rb.AddForce(dir * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
            boosterFlame.SetActive(true);
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
            boosterFlame.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Destroyed");
            Destroy(gameObject);
            if (explosion != null)
                Instantiate(explosion, transform.position, Quaternion.identity);
            if (GM.Instance != null)
                GM.Instance.GameOver((int)score);
        }
    }

    private void OnUIReload(PanelRenderer _panel, VisualElement _root, int _version)
    {
        labelRef.RegisterReferenceResolvedCallback(HandleLabelRefCallBack);
    }
    private void HandleLabelRefCallBack(VisualElement _element)
    {
        if (_element is Label lbl)
            scoreText = lbl;
    }
}