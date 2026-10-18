using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
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
    [SerializeField] private ParticleSystem trailPS;

    [Header("Beep Sounds")]
    [SerializeField] private float farDetectDist = 5f;
    [SerializeField] private float nearDetectDist = 0.5f;
    [SerializeField] private float farDistInterval = 1.0f;
    [SerializeField] private float nearDistInterval = 0.1f;
    [SerializeField] private float farDistPitch = 0.8f;
    [SerializeField] private float nearDistPitch = 2.0f;
    private float beepTimer = 0f;

    private Collider2D col;



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
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
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

        bool isMoving = Mouse.current.leftButton.isPressed;
        if (isMoving && !trailPS.isEmitting) trailPS.Play();
        else if (!isMoving && trailPS.isEmitting) trailPS.Stop();

        HandleBeep();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            GM.Instance.PlaySFX_HitWall();

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GM.Instance.PlaySFX_GameLose();
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
    private void HandleBeep()
    {
        float nearest = float.MaxValue;
        foreach (Obstacle o in Obstacle.All)
        {
            //edge-to-edge measure of distance, not center-to-center
            float d = Physics2D.Distance(col, o.Col).distance;
            if (d < nearest) nearest = d;
        }

        beepTimer -= Time.deltaTime;
        if (nearest > farDetectDist || beepTimer > 0f) return;

        // Lerp(0, 10, 0.25) = 2.5, get the value
        // InverseLerp(0, 10, 2.5) = 0.25, get the ratio
        // "1 - " part flips the result, so 0 = far, 1 = close
        float danger = 1f - Mathf.InverseLerp(nearDetectDist, farDetectDist, nearest);
        GM.Instance.PlaySFX_Beep(Mathf.Lerp(farDistPitch, nearDistPitch, danger));
        beepTimer = Mathf.Lerp(farDistInterval, nearDistInterval, danger);
    }
}