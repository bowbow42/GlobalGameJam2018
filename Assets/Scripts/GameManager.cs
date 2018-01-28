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
    public Text m_LifeText;

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

    public GameObject m_GameObjectStart;
    public float m_WidthGOStart;
    public GameObject m_GameObjectEnd;
    public float m_WidthGOEnd;


    public float m_CurrentTime = 120f;
    private float m_StartTime;

    private int time, a;
    private float x;


    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);

        SpawnAllGameElements();

        //m_LevelStartText.text = "";
        m_LevelText.text = "Level 1 - Dei Mudda's Lan";
        m_TimeText.text = "TTL: " + m_CurrentTime;
        m_LevelProgressSlider.value = 0;
        m_StartTime = m_CurrentTime;

        StartCoroutine(GameLoop());
    }


    private void SpawnAllGameElements()
    {
        for (int i = 0; i < m_Level1Cables.Length; i++)
        {
            int selectedObj = Random.Range(0, m_Platforms.Length);

            if (i == 0 || i == m_Level1Cables.Length - 1)
            {
                if(i == 0)
                {
                    m_Level1Cables[i].m_Instance = Instantiate(m_TubePart, new Vector3(0, 0, 0), Quaternion.identity) as GameObject; 
                    m_Level1Cables[i].m_PadPos = new Vector3(0, 0, 0);
                    m_Level1Cables[i].m_Platform = Instantiate(m_GameObjectStart, m_Level1Cables[i].m_Instance.transform.position, Quaternion.identity) as GameObject;
                    m_Level1Cables[i].m_ItemThickness = m_WidthGOStart;
                    m_Level1Cables[i].Setup();
                }
                else
                {
                    m_Level1Cables[i].m_Instance = Instantiate(m_TubePart, new Vector3(m_Level1Cables[i-1].m_ItemThickness + m_Level1Cables[i - 1].m_Instance.transform.position[0], 0, 0), Quaternion.identity) as GameObject; 
                    m_Level1Cables[i].m_PadPos = new Vector3(0, 0, 0);
                    m_Level1Cables[i].m_Platform = Instantiate(m_GameObjectEnd, m_Level1Cables[i].m_Instance.transform.position, Quaternion.identity) as GameObject;
                    m_Level1Cables[i].m_ItemThickness = m_WidthGOEnd;
                    m_Level1Cables[i].Setup();
                }
            }
            else
            {
                // set next position
                m_Level1Cables[i].m_Instance = Instantiate(m_TubePart, new Vector3(m_Level1Cables[i-1].m_ItemThickness + m_Level1Cables[i - 1].m_Instance.transform.position[0], 0, 0), Quaternion.identity) as GameObject;
                // when randomness is disabled for this go
                if (m_Platforms[selectedObj].m_DisableRandomness)
                {
                    m_Level1Cables[i].m_PadPos = new Vector3(0, 0, 0);
                }
                else
                {
                    float offSet = Random.Range(-8, 6);
                    while (Mathf.Abs(offSet - m_Level1Cables[i - 1].m_PadPos[1]) > 4) offSet = Random.Range(-8, 6);
                    m_Level1Cables[i].m_PadPos = new Vector3(Random.Range(-1, 1), offSet, 0);
                }
                m_Level1Cables[i].m_Platform = Instantiate(m_Platforms[selectedObj].m_GameObject, m_Level1Cables[i].m_PadPos + m_Level1Cables[i].m_Instance.transform.position, Quaternion.identity) as GameObject;
                m_Level1Cables[i].m_ItemThickness = m_Platforms[selectedObj].m_Width;
                m_Level1Cables[i].Setup();
            }
        }
    }

    private void Update()
    {
        if (m_CurrentTime > 0)
        {
            m_CurrentTime -= Time.deltaTime;
            m_TimeText.text = "TTL: " + (int)m_CurrentTime;
        }
        if (m_Level1Cables.Length != 0) { 
            float maxLength = (m_Level1Cables[0].m_Instance.transform.position - m_Level1Cables[m_Level1Cables.Length - 1].m_Instance.transform.position).magnitude;
            float distToDest = (m_Player.transform.position - m_Level1Cables[m_Level1Cables.Length - 1].m_Instance.transform.position).magnitude;
            float sliderValue = 100 - (distToDest / maxLength) * 100;

            sliderValue = sliderValue > 100 ? 100 : (sliderValue < 0 ? 0 : sliderValue);
            if (m_Player.transform.position[0] > maxLength) sliderValue = 100;
            m_LevelProgressSlider.value = sliderValue;
        }

        int life = m_Player.GetComponent<PlayerManager>().life;
        m_LifeText.text = "CRC Life: " + life;

        if (WinCheck())
        {
            Win();
        }
        else if (LoseCheck())
        {
            Lose();
        }
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

    private bool LoseCheck()
    {
        return m_Player.GetComponent<PlayerManager>().life < 1;
    }

    private void Lose()
    {
        m_Player.GetComponent<PlayerManager>().Respawn();
        m_CurrentTime = m_StartTime;
        GetComponent<EnemySpawner>().SetBack();
    }

    private bool WinCheck()
    {
        Debug.Log("endX" + m_Level1Cables[m_Level1Cables.Length - 1].m_Instance.transform.position[0]);
        return m_Level1Cables[m_Level1Cables.Length-1].m_Instance.transform.position.x + 10 < m_Player.transform.position.x;
    }

    private void Win()
    {
        m_LevelStartText.text = "You won!";
        GetComponent<EnemySpawner>().Stop();
        m_Player.GetComponent<PlayerMovement>().movementSpeed = 0;
        m_Player.GetComponent<PlayerMovement>().airSpeed = 0;
        m_Player.GetComponent<PlayerMovement>().dashForce = 0;
    }
}