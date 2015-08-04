using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserEditLdrPopup : MonoBehaviour {

    private GameObject m_goCurrLdr;

    [SerializeField] private GameObject Popup;
    [SerializeField] private InputField PopupPosX;
    [SerializeField] private InputField PopupPosY;
    [SerializeField] private InputField PopupNegX;
    [SerializeField] private InputField PopupNegY;
    [SerializeField] private InputField PopupXSpacing;
    [SerializeField] private InputField PopupYSpacing;
    [SerializeField] private InputField PopupGrpNum;
    [SerializeField] private InputField PopupGenType;

    [SerializeField]
    private string LdrTag;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == LdrTag && other.gameObject.GetComponent<LdrMain>())
        {
            m_goCurrLdr = other.gameObject;

            Popup.SetActive(true);
            PopupPosX.text = other.GetComponent<LdrMain>().GenXP.ToString();
            PopupPosY.text = other.GetComponent<LdrMain>().GenYP.ToString();
            PopupNegX.text = other.GetComponent<LdrMain>().GenXN.ToString();
            PopupNegY.text = other.GetComponent<LdrMain>().GenYN.ToString();
            PopupXSpacing.text = other.GetComponent<LdrMain>().GenXSpcng.ToString();
            PopupYSpacing.text = other.GetComponent<LdrMain>().GenYSpcng.ToString();
            PopupGenType.text = other.GetComponent<LdrMain>().iLdrGenType.ToString();
            PopupGrpNum.text = other.GetComponent<LdrMain>().AgntGroupNum.ToString();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == LdrTag)
        {
            m_goCurrLdr = null;
            Popup.SetActive(false);
        }
    }

    public void SetFormationValues()
    {
        if (m_goCurrLdr)
        {
            m_goCurrLdr.GetComponent<LdrMain>().GenXP = int.Parse(PopupPosX.text);
            m_goCurrLdr.GetComponent<LdrMain>().GenYP = int.Parse(PopupPosY.text);
            m_goCurrLdr.GetComponent<LdrMain>().GenXN = int.Parse(PopupNegX.text);
            m_goCurrLdr.GetComponent<LdrMain>().GenYN = int.Parse(PopupNegY.text);
            m_goCurrLdr.GetComponent<LdrMain>().GenXSpcng = float.Parse(PopupXSpacing.text);
            m_goCurrLdr.GetComponent<LdrMain>().GenYSpcng = float.Parse(PopupYSpacing.text);
            m_goCurrLdr.GetComponent<LdrMain>().AgntGroupNum = int.Parse(PopupGrpNum.text);
            m_goCurrLdr.GetComponent<LdrMain>().iLdrGenType = int.Parse(PopupGenType.text);

        }
    }

    public void ReGenerate()
    {
        if (m_goCurrLdr)
        {
            m_goCurrLdr.GetComponent<LdrMain>().vRegenerate();
        }
    }
}
