using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsGUIScript : MonoBehaviour
{
    private static StatsGUIScript m_DataInstance;
    public static StatsGUIScript Instance
    {
        get
        {
            if (!m_DataInstance) { m_DataInstance = FindObjectOfType<StatsGUIScript>(); }
            return m_DataInstance;
        }
    }

    private float m_fCheckInterval = 1f;
    [SerializeField] private Text StatTitleText;
    [SerializeField] private Text StatsDataText;

    private int m_iFPS;
    private int m_CurrNumFrames = 0;

    private int m_iAgentsLdrs;
    private int m_iAgentsFlrs;
    private int m_iObss;
    private int m_iFors;
    private int m_iFormMoveCalls;
    private int m_iFormObsCalls;

    private const string NL = "\n";
    private const string SP = " | ";

    void Start()
    {
        m_DataInstance = this;

        StartCoroutine("DoStats");
        StartCoroutine("DoFPS");

        StatTitleText.text = 
            "FPS ::" + NL 
            + "Agents (L/F/T) ::" + NL
            + "(Ldr/Flr/Total)" + NL
            + "Obstacles ::" + NL
            + "Formation Pos.s ::" + NL
            + "Formation Move Calls ::" + NL
            + "Formation Obs Calls ::";
    }

    IEnumerator DoStats()
    {
        while(true)
        {
            GetAgnNumber();
            GetObsNumber();
            GetForNumber();            
            GetFPS();

            SetText();

            m_iFormMoveCalls = 0;
            m_iFormObsCalls = 0;

            yield return new WaitForSeconds(m_fCheckInterval);
        }
    }

    IEnumerator DoFPS()
    {
        while (true)
        {
            m_CurrNumFrames++;
            yield return new WaitForEndOfFrame();
        }
    }

    void SetText()
    {
        StatsDataText.text =
            m_iFPS.ToString() + NL
            + m_iAgentsLdrs.ToString() + SP + m_iAgentsFlrs.ToString() + SP + (m_iAgentsFlrs + m_iAgentsLdrs).ToString() + NL + NL
            + m_iObss.ToString() + NL
            + m_iFors.ToString() + NL
            + m_iFormMoveCalls.ToString() + NL
            + m_iFormObsCalls.ToString();
    }
    
    void GetFPS()
    {
        m_iFPS = m_CurrNumFrames;
        m_CurrNumFrames = 0;
    }
    void GetAgnNumber()
    {
        m_iAgentsLdrs = GameObject.FindGameObjectsWithTag("Leader").Length;
        m_iAgentsFlrs = GameObject.FindGameObjectsWithTag("Follower").Length;
    }
    void GetObsNumber()
    {
        m_iObss = GameObject.FindGameObjectsWithTag("Obstacle").Length;
    }
    void GetForNumber()
    {
        m_iFors = GameObject.FindGameObjectsWithTag("PosFor").Length;
    }
    public void UpMoveCall()
    {
        m_iFormMoveCalls++;
    }
    public void UpObsCall()
    {
        m_iFormObsCalls++;
    }
}
