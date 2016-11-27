using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class iPhoneEmulator : MonoBehaviour
{

	// Use this for initialization
	void Start()
    {
        m_Button_7S = GameObject.Find("Button_iPhone7S").GetComponent<Button>();
        m_Button_7  = GameObject.Find("Button_iPhone7").GetComponent<Button>();
        m_Button_6S = GameObject.Find("Button_iPhone6S").GetComponent<Button>();
        m_Button_6  = GameObject.Find("Button_iPhone6").GetComponent<Button>();
        m_Button_5  = GameObject.Find("Button_iPhone5").GetComponent<Button>();
        m_Button_4  = GameObject.Find("Button_iPhone4").GetComponent<Button>();

        EnableButton(m_Button_7S);

        m_WindowWidth = GameObject.Find("Text_MainWindowWidth").GetComponent<Text>();
        m_WindowHeight = GameObject.Find("Text_MainWindowHeight").GetComponent<Text>();
        m_iPhoneWidth = GameObject.Find("Text_iPhoneWindowWidth").GetComponent<Text>();
        m_iPhoneHeight = GameObject.Find("Text_iPhoneWindowHeight").GetComponent<Text>();

        m_MainWindowRect = GameObject.Find("Canvas_Main").GetComponent<RectTransform>();

        if(m_MainWindowRect)
        {
            m_MainWindowSize = new Vector2(m_MainWindowRect.rect.width, m_MainWindowRect.rect.height);
            UpdateWindowSize(m_MainWindowRect.rect.width, m_MainWindowRect.rect.height);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_MainWindowRect)
        {
            if(m_MainWindowSize.x != m_MainWindowRect.rect.width || m_MainWindowSize.y != m_MainWindowRect.rect.height )
            {
                UpdateWindowSize(m_MainWindowRect.rect.width, m_MainWindowRect.rect.height);
            }
        }

        if (m_iPhoneWidth)
        {
            m_iPhoneWidth.text = "fart";
        }
        if (m_iPhoneHeight)
        {
            m_iPhoneHeight.text = "fart";
        }
    }

    public void OnClick_iPhone7S()
    {
        DisableCurrentButton();
        EnableButton(m_Button_7S);
    }

    public void OnClick_iPhone7()
    {
        DisableCurrentButton();
        EnableButton(m_Button_7);
    }

    public void OnClick_iPhone6S()
    {
        DisableCurrentButton();
        EnableButton(m_Button_6S);
    }

    public void OnClick_iPhone6()
    {
        DisableCurrentButton();
        EnableButton(m_Button_6);
    }

    public void OnClick_iPhone5()
    {
        DisableCurrentButton();
        EnableButton(m_Button_5);
    }

    public void OnClick_iPhone4()
    {
        DisableCurrentButton();
        EnableButton(m_Button_4);
    }

    private void UpdateWindowSize( float width, float height )
    {
        m_MainWindowSize = new Vector2(width, height);

        if (m_WindowWidth)
        {
            m_WindowWidth.text = width.ToString();
        }
        if (m_WindowHeight)
        {
            m_WindowHeight.text = height.ToString();
        }
    }

    // .5625 is 16:9

    private void DisableCurrentButton()
    {
        if (m_CurrentButton)
        {
            //m_CurrentButton.enabled = true;
            m_CurrentButton.interactable = true;
            m_CurrentButton = null;
        }
    }

    private void EnableButton(Button button)
    {
        m_CurrentButton = button;
        if (m_CurrentButton)
        {
            //m_CurrentButton.enabled = false;
            m_CurrentButton.interactable = false;
            m_CurrentButton = button;
        }
    }

    private Button m_CurrentButton = null;

    private Button m_Button_7S = null;
    private Button m_Button_7 = null;
    private Button m_Button_6S = null;
    private Button m_Button_6 = null;
    private Button m_Button_5 = null;
    private Button m_Button_4 = null;

    private Text m_WindowWidth = null;
    private Text m_WindowHeight = null;
    private Text m_iPhoneWidth = null;
    private Text m_iPhoneHeight = null;

    private RectTransform m_MainWindowRect = null;
    private Vector2 m_MainWindowSize;
}
