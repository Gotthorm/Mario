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
    }

    // Update is called once per frame
    void Update()
    {
	
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
}
