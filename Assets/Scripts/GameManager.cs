using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    // collections for all levels
    public CableManager[] m_Level1Cables;
    

    private float m_CurrentTime = 120f;

    private int time, a;
    private float x;


    private void Start()
    {
        SpawnAllGameElements();

        m_LevelStartText.text = "";
        m_LevelText.text = "";
        m_TimeText.text = "TTL: " + m_CurrentTime;
        m_LevelProgressSlider.value = 100;
    }


    private void SpawnAllGameElements()
    {
        for (int i = 0; i < m_Level1Cables.Length; i++)
        {
            m_Level1Cables[i].m_Instance = Instantiate(m_TubePart, new Vector3(12 * i, 0, 0), Quaternion.identity) as GameObject; // prefab, position, rotation
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
}