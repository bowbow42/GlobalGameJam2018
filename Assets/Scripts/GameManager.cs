using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int m_Tunnel1 = 5;

    // user interface parts
    public Text m_LevelText;
    public Text m_TimeText;
    public Text m_LevelStartText;
    public Slider m_LevelProgressSlider;

    // loaded prefab for autogeneration
    public GameObject m_Player;
    public GameObject m_TubePart;

    // game delays
    public float m_StartDelay = 3f;
    private WaitForSeconds m_StartWait;

    // collections for all levels
    public CableManager[] m_Level1Cables;


    // cable assets for random game design
    public PlatFormManager[] m_Platforms;


    public float m_CurrentTime = 120f;

    private int time, a;
    private float x;


    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);

        SpawnAllGameElements();

        //m_LevelStartText.text = "";
        m_LevelText.text = "Level 1 - Dei Mudda's Lan";
        m_TimeText.text = "TTL: " + m_CurrentTime;
        m_LevelProgressSlider.value = 100;

        StartCoroutine(GameLoop());
    }


    private void SpawnAllGameElements()
    {
        for (int i = 0; i < m_Level1Cables.Length; i++)
        {
            int selectedObj = Random.Range(0, m_Platforms.Length);

            m_Level1Cables[i].m_Instance = Instantiate(m_TubePart, new Vector3(m_Platforms[selectedObj].m_Width * i, 0, 0), Quaternion.identity) as GameObject; // prefab, position, rotation
            if (i == 0) m_Level1Cables[i].m_PadPos = new Vector3(0, -3, 0);
            else
            {
                float offSet = Random.Range(-8, 6);
                while (Mathf.Abs(offSet - m_Level1Cables[i - 1].m_PadPos[1]) > 4) offSet = Random.Range(-8, 6);
                m_Level1Cables[i].m_PadPos = new Vector3(Random.Range(-1, 1), offSet, 0);
            }
            m_Level1Cables[i].m_Platform = Instantiate(m_Platforms[selectedObj].m_GameObject, m_Level1Cables[i].m_PadPos + m_Level1Cables[i].m_Instance.transform.position, Quaternion.identity) as GameObject;
            m_Level1Cables[i].Setup();
        }
    }

    private void Update()
    {
        if (m_CurrentTime > 0)
        {
            m_CurrentTime -= Time.deltaTime;
            m_TimeText.text = "TTL: " + (int)m_CurrentTime;
        }


        float maxLength = (m_Level1Cables[0].m_Instance.transform.position - m_Level1Cables[m_Level1Cables.Length - 1].m_Instance.transform.position).magnitude;
        float distToDest = (m_Player.transform.position - m_Level1Cables[m_Level1Cables.Length - 1].m_Instance.transform.position).magnitude;
        float sliderValue = 100 - (distToDest / maxLength) * 100;
        sliderValue = sliderValue > 100 ? 100 : (sliderValue < 0 ? 0 : sliderValue);
        m_LevelProgressSlider.value = sliderValue;
    }

    private IEnumerator GameLoop()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        m_LevelStartText.text = "Level started";
        yield return StartCoroutine(RoundStarting());
        m_LevelStartText.text = "";
    }

    private IEnumerator RoundStarting()
    {
        yield return m_StartWait;
    }
}