using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhoneMain : MonoBehaviour
{
#region Static Accessors
    //
    public static List<string> GetPhoneTypeList() { return m_PhoneTypes; }

    // iPhone is current default
    public static int GetPhoneTypeDefault() { return (int)PHONE_TYPE.DEFAULT; }

    // Only iPhone supported currently
    public static List<string> GetPhoneModelList(int phoneType) { return m_PhoneModelLists[0]; }

    // Only iPhone supported currently, which defaults to 6+ currently
    public static int GetPhoneModelDefault(int phoneType) { return (int)IPHONE_MODEL.DEFAULT; }
#endregion

    //
    public bool Initialize(int phoneType, int phoneModel)
    {
        bool results = false;

        if( phoneType == (int)PHONE_TYPE.IPHONE )
        {
            // iPhone Physical Pixels
            // iPhone 4:   640 x 960	
            // iPhone 5:   640 x 1136	
            // iPhone 6:   750 x 1334	
            // iPhone 6+: 1080 x 1920
            // iPhone 7:   750 x 1334	
            // iPhone 7+: 1080 x 1920

            IPHONE_MODEL model = (IPHONE_MODEL)phoneModel;

            switch (model)
            {
                case IPHONE_MODEL.IPHONE_4:
                    m_ScreenResolution.height = 960;
                    m_ScreenResolution.width = 640;
                    break;
                case IPHONE_MODEL.IPHONE_5:
                    m_ScreenResolution.height = 1136;
                    m_ScreenResolution.width = 640;
                    break;
                case IPHONE_MODEL.IPHONE_6:
                case IPHONE_MODEL.IPHONE_7:
                    m_ScreenResolution.height = 1334;
                    m_ScreenResolution.width = 750;
                    break;
                case IPHONE_MODEL.IPHONE_6_PLUS:
                case IPHONE_MODEL.IPHONE_7_PLUS:
                default:
                    m_ScreenResolution.height = 1920;
                    m_ScreenResolution.width = 1080;
                    break;
            }

            Resized();
            results = true;
        }

        m_TimeElapsedText = GameObject.Find("Text_TimeRunning").GetComponent<Text>();

        return results;
    }

    public void Resized()
    {
        Debug.Log("PhoneMain resolution set => width: " + m_ScreenResolution.width + " height: " + m_ScreenResolution.height);

        m_RectTransform = this.GetComponent<RectTransform>();

        Text emulatedScreenWidth = GameObject.Find("Text_EmulatedWindowWidth").GetComponent<Text>();
        Text emulatedScreenHeight = GameObject.Find("Text_EmulatedWindowHeight").GetComponent<Text>();
        Text aspectRatio = GameObject.Find("Text_WindowAspectRatio").GetComponent<Text>();
        Text pixelScale = GameObject.Find("Text_WindowPixelScale").GetComponent<Text>();
        Text targetScreenWidth = GameObject.Find("Text_TargetWindowWidth").GetComponent<Text>();
        Text targetScreenHeight = GameObject.Find("Text_TargetWindowHeight").GetComponent<Text>();

        emulatedScreenWidth.text = m_RectTransform.rect.width.ToString("0");
        emulatedScreenHeight.text = m_RectTransform.rect.height.ToString("0");
        aspectRatio.text = (m_RectTransform.rect.width / m_RectTransform.rect.height).ToString();
        pixelScale.text = "1 : " + (m_RectTransform.rect.width / m_ScreenResolution.width).ToString("0.00");
        targetScreenWidth.text = m_ScreenResolution.width.ToString("0");
        targetScreenHeight.text = m_ScreenResolution.height.ToString("0");
    }

    // Update is called once per frame
    private void Update ()
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

    private float m_ElapsedTime = 0.0f;
    private Text m_TimeElapsedText = null;
    private RectTransform m_RectTransform;
    private Resolution m_ScreenResolution;

    private enum PHONE_TYPE { IPHONE, DEFAULT = IPHONE };
    private enum IPHONE_MODEL { IPHONE_4, IPHONE_5, IPHONE_6, IPHONE_6_PLUS, IPHONE_7, IPHONE_7_PLUS, DEFAULT = IPHONE_6_PLUS };

    private static List<string> m_PhoneTypes = new List<string> { "Apple iPhone" };
    private static List<string> m_iPhoneModelStringList = new List<string> { "iPhone 4", "iPhone 5", "iPhone 6", "iPhone 6+", "iPhone 7", "iPhone 7+" };
    private static List<List<string>> m_PhoneModelLists = new List<List<string>> { m_iPhoneModelStringList };
}
