using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GM : MonoBehaviour
{
    static public GM Instance { get; private set; }
    [SerializeField] private GameObject mainPnl;
    [SerializeField] private GameObject settingsPnl;
    [SerializeField] private GameObject instructionPnl;
    [SerializeField] private GameObject gamePnl;

    [SerializeField] private Toggle musicTgl;
    [SerializeField] private Slider volumeSld;
    [SerializeField] private TMP_Dropdown enemySpdDpd;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private NavMeshAgent enemyAgent;
    [SerializeField] private float[] enemySpeeds = { 2f, 3.5f, 5f };

    public TextMeshProUGUI countTxt;
    public GameObject winTxtGo;
    [SerializeField] private GameObject MainMenuBtn;
    [SerializeField] private GameObject QuitBtn;

    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyController enemy;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        TurnOffAllPanels();
        mainPnl.SetActive(true);
        Time.timeScale = 0f;

        bgmSource.mute = false;
        OnVolumeSlider();
        OnEnemySpeedDropdownSelected();
    }

    public void OnPlayBtnClicked()
    {
        TurnOffAllPanels();
        gamePnl.SetActive(true);
        Time.timeScale = 1f;
        MainMenuBtn.SetActive(false);
        QuitBtn.SetActive(false);
        winTxtGo.SetActive(false);

        player.ResetState();
        enemy.ResetState();
    }
    public void OnSettingsBtnClicked()
    {
        TurnOffAllPanels();
        settingsPnl.SetActive(true);
    }
    public void OnInstructionBtnClicked()
    {
        TurnOffAllPanels();
        instructionPnl.SetActive(true);
    }
    public void OnQuitBtnClicked() => Application.Quit();
    public void OnBackBtnClicked()
    {
        TurnOffAllPanels();
        mainPnl.SetActive(true);
    }
    public void OnMusicToggle() => bgmSource.mute = !musicTgl.isOn;
    public void OnVolumeSlider() => AudioListener.volume = volumeSld.value;
    public void OnEnemySpeedDropdownSelected()
    {
        if (enemyAgent != null)
            enemyAgent.speed = enemySpeeds[enemySpdDpd.value];
    }
    public void EndGameUI(bool _isWin = true)
    {
        winTxtGo.SetActive(true);
        winTxtGo.GetComponent<TextMeshProUGUI>().text = _isWin ? "You Win!!!" : "You Lose!";
        MainMenuBtn.SetActive(true);
        QuitBtn.SetActive(true);
        Time.timeScale = 0f;
    }

    private void TurnOffAllPanels()
    {
        mainPnl.SetActive(false);
        settingsPnl.SetActive(false);
        instructionPnl.SetActive(false);
        gamePnl.SetActive(false);
    }
}
