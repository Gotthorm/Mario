using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        // Load preferences
        m_PreferredResolution.width = PlayerPrefs.GetInt(m_kSettingPrefix + m_kSettingWindowWidth, m_DefaultWindowWidth);
        m_PreferredResolution.height = PlayerPrefs.GetInt(m_kSettingPrefix + m_kSettingWindowHeight, m_DefaultWindowHeight);
        m_FullScreen = (PlayerPrefs.GetInt(m_kSettingPrefix + m_kSettingFullScreen, m_DefaultFullScreen ? 1 : 0) == 1);

        Debug.Log("Loaded preference: desiredWidth(" + m_PreferredResolution.width.ToString() + ")");
        Debug.Log("Loaded preference: desiredHeight(" + m_PreferredResolution.height.ToString() + ")");
        Debug.Log("Loaded preference: FullScreen(" + m_FullScreen.ToString() + ")");

        // The returned resolutions are sorted by width, lower resolutions come first
        // I believe this list can be empty on some platforms like Android?
        Resolution[] resolutionList = Screen.resolutions;

        // Create the string list that will be used to populate the UI control for the screen resolution
        List<string> ResolutionStringList = new List<string>();

        // The default option is for whatever the current screen resolution is set to
        ResolutionStringList.Add("Default");

        // Iterate through all of the supported resolutions
        // Save the index of the preferred resolution
        int defaultResolutionIndex = -1;
        int preferredResolutionIndex = -1;
        int currentResolutionIndex = 0;
        foreach (Resolution resolution in resolutionList)
        {
            Debug.Log(resolution.ToString());
            ResolutionStringList.Add(resolution.width.ToString() + "x" + resolution.height.ToString());

            // A match here will only occur if the preferred resolution is not "default"
            if (resolution.width == m_PreferredResolution.width && resolution.height == m_PreferredResolution.height)
            {
                preferredResolutionIndex = currentResolutionIndex;
                Debug.Log("System supports preferred resolution: " + m_PreferredResolution.width.ToString() + "x" + m_PreferredResolution.height.ToString());
            }

            // When we encounter the current screen resolution in the list of options, we save its index
            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            {
                defaultResolutionIndex = currentResolutionIndex;
            }

            ++currentResolutionIndex;
        }

        Debug.Break();

        // If we found the preferred resolution, use that
        // else, if we found the default resolution, use that
        // else, if the resolution list has at least one entry, use that
        // else, fail
        Resolution targetResolution;

        if (preferredResolutionIndex >= 0)
        {
            // The user had a valid resolution saved in the preferences
            targetResolution = resolutionList[preferredResolutionIndex];
            // Set the control to display the resolution option as being selected
        }
        else if(defaultResolutionIndex > 0)
        {
            // Either the user selected "default" or the selected resolution is not supported
            // The default resolution is valid, so use it.
            targetResolution = Screen.currentResolution;

            // Revert the saved preferences to default
            m_PreferredResolution.width = m_DefaultWindowWidth;
            m_PreferredResolution.height = m_DefaultWindowHeight;
        }
        else if (resolutionList.Length > 0)
        {
            // Nothing seem to work, so just use the lowest supported resolution
            targetResolution = resolutionList[0];
            preferredResolutionIndex = 0;

            // Revert the saved preferences to default
            m_PreferredResolution.width = m_DefaultWindowWidth;
            m_PreferredResolution.height = m_DefaultWindowHeight;
        }
        else
        {
            Debug.Log("Failed to find any supported screen resolution");
            Application.Quit();
            return;
        }

        Debug.Log("Attempting window dimensions: " + targetResolution.width.ToString() + "x" + targetResolution.height.ToString());

        Screen.SetResolution(targetResolution.width, targetResolution.height, m_FullScreen);

        m_ResolutionDropdown = GameObject.Find("Dropdown_ScreenResolution").GetComponent<Dropdown>();

        if(m_ResolutionDropdown)
        {
            m_ResolutionDropdown.ClearOptions();
            m_ResolutionDropdown.AddOptions(ResolutionStringList);
            m_ResolutionDropdown.value = preferredResolutionIndex + 1;
        }
    }

    // Update is called once per frame
    private void Update()
    {
	
	}

    // Use this to clean up
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(m_kSettingPrefix + m_kSettingWindowWidth, m_PreferredResolution.width);
        PlayerPrefs.SetInt(m_kSettingPrefix + m_kSettingWindowHeight, m_PreferredResolution.height);
        PlayerPrefs.SetInt(m_kSettingPrefix + m_kSettingFullScreen, m_FullScreen ? 1 : 0);
    }

    public void DropdownValueChangedHandler(int index)
    {
        Debug.Log("selected: " + index);
    }

    // Settings
    private Resolution m_PreferredResolution;
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
