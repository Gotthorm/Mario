using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
	// Use this for initialization
	void Start()
    {
        // Load preferences
        m_DesiredWindowWidth = PlayerPrefs.GetInt(m_kSettingPrefix + m_kSettingWindowWidth, m_DefaultWindowWidth);
        m_DesiredWindowHeight = PlayerPrefs.GetInt(m_kSettingPrefix + m_kSettingWindowHeight, m_DefaultWindowHeight);
        m_FullScreen = (PlayerPrefs.GetInt(m_kSettingPrefix + m_kSettingFullScreen, m_DefaultFullScreen ? 1 : 0) == 1);

        Debug.Log("Loaded preference: desiredWidth(" + m_DesiredWindowWidth.ToString() + ")");
        Debug.Log("Loaded preference: desiredHeight(" + m_DesiredWindowHeight.ToString() + ")");
        Debug.Log("Loaded preference: FullScreen(" + m_FullScreen.ToString() + ")");

        m_CurrentWindowWidth = (m_DesiredWindowWidth < 0) ? Screen.currentResolution.width : m_DesiredWindowWidth;
        m_CurrentWindowHeight = (m_DesiredWindowHeight < 0) ? Screen.currentResolution.height : m_DesiredWindowHeight;

        Debug.Log("Current target window dimensions: " + m_CurrentWindowWidth.ToString() + "X" + m_CurrentWindowHeight.ToString());

        // The returned resolutions are sorted by width, lower resolutions come first
        Resolution[] resolutionList = Screen.resolutions;

        bool isCurrentResolutionValid = false;
        List<string> ResolutionStringList = new List<string>();
        foreach (Resolution resolution in resolutionList)
        {
            Debug.Log(resolution.ToString());
            ResolutionStringList.Add(resolution.width.ToString() + "X" + resolution.height.ToString());

            if (resolution.width == m_CurrentWindowWidth && resolution.height == m_CurrentWindowHeight)
            {
                isCurrentResolutionValid = true;

                // When logging the entries, always finish iterating the list
                //break;
            }
        }

        if(isCurrentResolutionValid == false)
        {
            // If this happens the saved preferences might be invalid.
            // Use the default settings and reset the p[references.
            m_CurrentWindowWidth = Screen.width;
            m_CurrentWindowHeight = Screen.height;
            m_FullScreen = m_DefaultFullScreen;
            m_DesiredWindowWidth = m_DefaultWindowWidth;
            m_DesiredWindowHeight = m_DefaultWindowHeight;
        }

        Debug.Log("Attempting window dimensions: " + m_CurrentWindowWidth.ToString() + "X" + m_CurrentWindowHeight.ToString());

        Screen.SetResolution(m_CurrentWindowWidth, m_CurrentWindowHeight, m_FullScreen);

        m_ResolutionDropdown = GameObject.Find("Dropdown_ScreenResolution").GetComponent<Dropdown>();

        if(m_ResolutionDropdown)
        {
            m_ResolutionDropdown.ClearOptions();
            m_ResolutionDropdown.AddOptions(ResolutionStringList);
            //m_ResolutionDropdown.options.Add(new Dropdown.OptionData("FartyPants"));
        }
    }

    // Update is called once per frame
    void Update()
    {
	
	}

    // Use this to clean up
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(m_kSettingPrefix + m_kSettingWindowWidth, m_DesiredWindowWidth);
        PlayerPrefs.SetInt(m_kSettingPrefix + m_kSettingWindowHeight, m_DesiredWindowHeight);
        PlayerPrefs.SetInt(m_kSettingPrefix + m_kSettingFullScreen, m_FullScreen ? 1 : 0);
    }

    // Settings
    private int m_DesiredWindowWidth = -1;
    private int m_DesiredWindowHeight = -1;
    private int m_CurrentWindowWidth = 0;
    private int m_CurrentWindowHeight = 0;
    private bool m_FullScreen = false;

    // Just some name to ensure we do not conflict with system settings
    private const string m_kSettingPrefix = "Mario_";

    private const string m_kSettingWindowWidth = "WindowWidth";
    private const string m_kSettingWindowHeight = "WindowHeight";
    private const string m_kSettingFullScreen = "FullScreen";

    // Window width and height use the value of -1 to indicate "current"
    private const int m_DefaultWindowWidth = -1;
    private const int m_DefaultWindowHeight = -1;
    private const bool m_DefaultFullScreen = true;

    private Dropdown m_ResolutionDropdown = null;
}
