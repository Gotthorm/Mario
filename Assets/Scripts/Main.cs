﻿using UnityEngine;
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
        List<string> resolutionStringList = new List<string>();

        // The default option is for whatever the current screen resolution is set to
        resolutionStringList.Add("Default");

        // Iterate through all of the supported resolutions
        // Save the index of the preferred resolution
        int defaultResolutionIndex = -1;
        int preferredResolutionIndex = -1;
        int currentResolutionIndex = 0;
        foreach (Resolution resolution in resolutionList)
        {
            Debug.Log(resolution.ToString());
            resolutionStringList.Add(resolution.width.ToString() + "x" + resolution.height.ToString());

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

        m_CurrentResolution = Screen.currentResolution;

        Debug.Log("Achieved window dimensions: " + m_CurrentResolution.width.ToString() + "x" + m_CurrentResolution.height.ToString());

        m_ResolutionDropdown = GameObject.Find("Dropdown_ScreenResolution").GetComponent<Dropdown>();

        if (m_ResolutionDropdown)
        {
            m_ResolutionDropdown.ClearOptions();
            m_ResolutionDropdown.AddOptions(resolutionStringList);
            m_ResolutionDropdown.value = preferredResolutionIndex + 1;
            Debug.Log("Set resolution drop down to index " + m_ResolutionDropdown.value);
        }

        m_WindowModeDropdown = GameObject.Find("Dropdown_ScreenMode").GetComponent<Dropdown>();

        if (m_WindowModeDropdown)
        {
            m_WindowModeDropdown.value = m_FullScreen ? 0 : 1;
            Debug.Log("Set window mode drop down to index " + m_WindowModeDropdown.value);
        }

        m_PhoneTypeDropdown = GameObject.Find("Dropdown_PhoneType").GetComponent<Dropdown>();

        if (m_PhoneTypeDropdown)
        {
            m_PhoneTypeDropdown.ClearOptions();
            m_PhoneTypeDropdown.AddOptions( PhoneMain.GetPhoneTypeList() );
            m_ResolutionDropdown.value = PhoneMain.GetPhoneTypeDefault();

            //
            m_PhoneTypeDropdown.enabled = false;
        }

        m_PhoneModelDropdown = GameObject.Find("Dropdown_PhoneModel").GetComponent<Dropdown>();

        if (m_PhoneModelDropdown)
        {
            m_PhoneModelDropdown.ClearOptions();
            m_PhoneModelDropdown.AddOptions(PhoneMain.GetPhoneModelList(PhoneMain.GetPhoneModelDefault(PhoneMain.GetPhoneTypeDefault())));
            m_PhoneModelDropdown.value = PhoneMain.GetPhoneModelDefault(PhoneMain.GetPhoneTypeDefault());
            //
            m_PhoneModelDropdown.enabled = false;
        }

        ResetPhoneApplication();
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

        Debug.Log("Saved preference: desiredWidth(" + m_PreferredResolution.width.ToString() + ")");
        Debug.Log("Saved preference: desiredHeight(" + m_PreferredResolution.height.ToString() + ")");
        Debug.Log("Saved preference: FullScreen(" + m_FullScreen.ToString() + ")");
    }

    public void PreferredResolutionDropdownChangedHandler(int index)
    {
        Debug.Log("resolution selected: " + index);

        // The returned resolutions are sorted by width, lower resolutions come first
        // I believe this list can be empty on some platforms like Android?
        Resolution[] resolutionList = Screen.resolutions;

        if(index <= resolutionList.Length && index > 0)
        {
            Resolution resolution = resolutionList[index - 1];

            Debug.Log("Screen.SetResolution(" + resolution.width + ", " + resolution.height + ", " + m_FullScreen + ")");

            Screen.SetResolution(resolution.width, resolution.height, m_FullScreen);

            m_CurrentResolution = resolution;
            m_PreferredResolution = resolution;
            StartCoroutine(DelayedNotifyResized());
        }
    }

    public void PreferredWindowModeDropdownChangedHandler(int index)
    {
        Debug.Log("window mode selected: " + index);
        m_FullScreen = (index == 0);
        Screen.SetResolution(m_CurrentResolution.width, m_CurrentResolution.height, m_FullScreen);
        StartCoroutine(DelayedNotifyResized());
    }

    public void PhoneTypeDropdownChangedHandler(int index)
    {

    }

    public void PhoneModelDropdownChangedHandler(int index)
    {

    }

    public void ResetPhoneApplication()
    {
        Debug.Log("ResetPhoneApplication");

        GameObject viewPort = GameObject.Find("Panel_PhoneViewport");

        if (viewPort)
        {
            Debug.Log("Found the view port");

            if(m_PhoneMain)
            {
                Debug.Log("Destroying view port");
                m_PhoneMain = null;
                Destroy(viewPort.GetComponent<PhoneMain>());
            }

            m_PhoneMain = viewPort.AddComponent<PhoneMain>();

            if (m_PhoneMain && m_PhoneTypeDropdown && m_PhoneModelDropdown)
            {
                m_PhoneMain.Initialize(m_PhoneTypeDropdown.value, m_PhoneModelDropdown.value);
            }
        }
    }

    // When you change the resolution, it doesnt actually happen until the next frame.
    // Waiting until the end of the "next" frame is long enough to ensure it has changed.
    private IEnumerator DelayedNotifyResized()
    {
        if (m_PhoneMain)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            m_PhoneMain.Resized();
        }
    }

    // Settings
    private Resolution m_PreferredResolution;
    private Resolution m_CurrentResolution;
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
    private Dropdown m_WindowModeDropdown = null;
    private Dropdown m_PhoneTypeDropdown = null;
    private Dropdown m_PhoneModelDropdown = null;

    private bool m_Dirty = false;

    private PhoneMain m_PhoneMain = null;
}
