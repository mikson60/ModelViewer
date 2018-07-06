using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    // Very simple state machine for switching between 2 states.

    enum UIState { Menu, ModelView };
    UIState m_state;
    UIState State
    {
        get { return m_state; }
        set
        {
            m_state = value;
            OnEnterState(m_state);
        }
    }

    // UI Elements
    [SerializeField] GameObject m_menuPanel;
    [SerializeField] GameObject m_modelViewPanel;

    [SerializeField] InputField m_nameInputField;

    [SerializeField] Text m_greetingText;

    // Dropdown
    [SerializeField] GameObject m_dropdownHider;

    [SerializeField] Text m_downloadProgressText;

    // Constants
    const string PREFS_PLAYER_NAME = "PlayerName";

    private void Awake()
    {
        SetState(UIState.Menu);
    }

    private void OnEnable()
    {
        DownloadEvents.OnDownloadStart += DownloadEvents_OnDownloadStart;
        DownloadEvents.OnDownloadEnd += DownloadEvents_OnDownloadEnd;
        DownloadEvents.OnDownloadProgressUpdate += DownloadEvents_OnDownloadProgressUpdate;
    }

    private void OnDisable()
    {
        DownloadEvents.OnDownloadStart -= DownloadEvents_OnDownloadStart;
        DownloadEvents.OnDownloadEnd -= DownloadEvents_OnDownloadEnd;
        DownloadEvents.OnDownloadProgressUpdate -= DownloadEvents_OnDownloadProgressUpdate;
    }

    private void DownloadEvents_OnDownloadStart()
    {
        if (m_dropdownHider) { m_dropdownHider.SetActive(true); }

        if (m_downloadProgressText) {
            m_downloadProgressText.gameObject.SetActive(true);
            m_downloadProgressText.text = "Download in progress...";
        }
    }

    private void DownloadEvents_OnDownloadEnd()
    {
        if (m_dropdownHider) { m_dropdownHider.SetActive(false); }

        if (m_downloadProgressText) { m_downloadProgressText.gameObject.SetActive(false); }
    }

    private void DownloadEvents_OnDownloadProgressUpdate(float progress)
    {
        if (m_downloadProgressText)
        {
            m_downloadProgressText.text = "Downloading..." + ((int)(progress * 100)).ToString() + "%";
        }
    }

    public void LaunchModelViewer()
    {
        if (m_nameInputField && !string.IsNullOrEmpty(m_nameInputField.text.Trim()))
        {
            PlayerPrefs.SetString(PREFS_PLAYER_NAME, m_nameInputField.text);
            PlayerPrefs.Save();
        }

        SetState(UIState.ModelView);
    }

    void TogglePanel(GameObject panel, bool value)
    {
        if (panel) { panel.SetActive(value); }
    }

    void SetState(UIState state) { State = state; }
    void OnEnterState(UIState state)
    {
        if (state == UIState.Menu)
        {
            if (!m_menuPanel) { Debug.LogError("Menu Panel not found"); return; }

            TogglePanel(m_modelViewPanel, false);
            TogglePanel(m_menuPanel, true);

            if (PlayerPrefs.HasKey(PREFS_PLAYER_NAME) && m_nameInputField)
            {
                m_nameInputField.text = PlayerPrefs.GetString(PREFS_PLAYER_NAME);
            }
        }
        else if (state == UIState.ModelView)
        {
            if (!m_modelViewPanel) { Debug.LogError("Model View Panel not found"); return; }

            TogglePanel(m_menuPanel, false);
            TogglePanel(m_modelViewPanel, true);

            if (m_greetingText)
            {
                m_greetingText.text = PlayerPrefs.HasKey(PREFS_PLAYER_NAME) ? 
                    string.Format("Greetings, {0}!", PlayerPrefs.GetString(PREFS_PLAYER_NAME)) 
                    : "Greetings!";
            }
        }
    }
}
