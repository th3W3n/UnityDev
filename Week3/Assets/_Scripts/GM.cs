using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public static GM Instance { get; private set; }
    [SerializeField] private PanelRenderer panel;
    [SerializeField] private VisualElementReference restartBtnRef;
    [SerializeField] private VisualElementReference highScoreLabelRef;
    private Button restartBtn;
    private Label highScoreText;

    private const string HighScoreKey = "HighScore";
    private int highScore;

    [SerializeField] private AudioClip hitWallClip;
    [SerializeField] private AudioClip obstaclesCollideClip;
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioClip beepClip;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSource beepSource;

    private void Awake()
    {
        Instance = this;
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        panel.RegisterUIReloadCallback(OnUIReload);
    }
    private void OnDestroy()
    {
        if (Instance == this) Instance = null;

        panel.UnregisterUIReloadCallback(OnUIReload);
        if (restartBtn != null)
            restartBtn.clicked -= OnRestartClicked;
    }

    private void OnUIReload(PanelRenderer _panel, VisualElement _root, int _version)
    {
        restartBtnRef.RegisterReferenceResolvedCallback(HandleRestartBtnRefCallBack);
        highScoreLabelRef.RegisterReferenceResolvedCallback(HandleHighScoreLabelRefCallBack);
    }
    private void HandleRestartBtnRefCallBack(VisualElement _element)
    {
        if (_element is Button btn)
        {
            if (restartBtn != null)
                restartBtn.clicked -= OnRestartClicked;

            restartBtn = btn;
            restartBtn.clicked += OnRestartClicked;
        }
    }
    private void HandleHighScoreLabelRefCallBack(VisualElement _element)
    {
        if (_element is Label lbl)
        {
            highScoreText = lbl;
            UpdateHighScoreText();
        }
    }
    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
            highScoreText.text = $"High Score: {highScore}";
    }
    private void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver(int _score)
    {
        if (_score > highScore)
        {
            highScore = _score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
            UpdateHighScoreText();
        }
        if (restartBtn != null)
            restartBtn.style.display = DisplayStyle.Flex;
    }

    public void PlaySFX_HitWall() => source.PlayOneShot(hitWallClip);
    public void PlaySFX_ObstaclesCollide() => source.PlayOneShot(obstaclesCollideClip, 0.4f);
    public void PlaySFX_GameLose() => source.PlayOneShot(loseClip);
    public void PlaySFX_Beep(float _pitch)
    {
        beepSource.pitch = _pitch;
        beepSource.PlayOneShot(beepClip, 0.5f);
    }
}