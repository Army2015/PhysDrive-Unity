using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

    private KeyCode m_kcMoveL = KeyCode.A;
    private KeyCode m_kcMoveR = KeyCode.D;
    private KeyCode m_kcMoveU = KeyCode.W;
    private KeyCode m_kcMoveD = KeyCode.S;

    [SerializeField] private float m_fMoveSpeed = 5f;

	void Update () 
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Quit();
        }

	    if(Input.GetKey(m_kcMoveL))
        {
            gameObject.transform.Translate(-m_fMoveSpeed * Time.deltaTime,0f,0f);
        }
        else if (Input.GetKey(m_kcMoveR))
        {
            gameObject.transform.Translate(m_fMoveSpeed * Time.deltaTime, 0f, 0f);
        }

        if (Input.GetKey(m_kcMoveU))
        {
            gameObject.transform.Translate(0f, 0f, m_fMoveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(m_kcMoveD))
        {
            gameObject.transform.Translate(0f, 0f, -m_fMoveSpeed * Time.deltaTime);
        }
	}
    public void Quit()
    {
        Application.Quit();
    }
}
