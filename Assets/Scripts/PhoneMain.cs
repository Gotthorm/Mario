using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// iPhone Physical Pixels
// iPhone 4:   640 x 960	
// iPhone 5:   640 x 1136	
// iPhone 6:   750 x 1334	
// iPhone 6+: 1080 x 1920
// iPhone 7:   750 x 1334	
// iPhone 7+: 1080 x 1920

public class PhoneMain : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        Debug.Log("PhoneMain.Start");

        m_TimeElapsedText = GameObject.Find("Text_TimeRunning").GetComponent<Text>();

        m_RectTransform = this.transform.parent.GetComponent<RectTransform>();

        Text screenWidth = GameObject.Find("Text_WindowWidth").GetComponent<Text>();
        Text screenHeight = GameObject.Find("Text_WindowHeight").GetComponent<Text>();
        Text aspectRatio = GameObject.Find("Text_WindowAspectRatio").GetComponent<Text>();
        Text pixelScale = GameObject.Find("Text_WindowPixelScale").GetComponent<Text>();

        screenWidth.text = m_RectTransform.rect.width.ToString("0");
        screenHeight.text = m_RectTransform.rect.height.ToString("0");
        //Debug.Log("Parent width: " + parentTransform.rect.width);
    }

    // Update is called once per frame
    void Update ()
    {
        m_ElapsedTime += Time.deltaTime;

        if(m_TimeElapsedText)
        {
            float seconds = Mathf.Floor(m_ElapsedTime % 60);
            float minutes = Mathf.Floor(m_ElapsedTime / 60) % 60;
            float hours = Mathf.Floor(m_ElapsedTime / 3600);
            m_TimeElapsedText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    // Use this to clean up
    private void OnDestroy()
    {
        Debug.Log("PhoneMain.OnDestroy");
    }

    float m_ElapsedTime = 0.0f;
    Text m_TimeElapsedText = null;
    RectTransform m_RectTransform;
}
