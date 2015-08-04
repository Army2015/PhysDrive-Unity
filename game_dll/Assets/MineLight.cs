using UnityEngine;
using System.Collections;

public class MineLight : MonoBehaviour {
	
	float interval = 1.0f;
	// Use this for initialization
	void Start () {
		m_color = 0;
		m_spotlight.enabled = false;
		InvokeRepeating ("ChangeColor", Random.value * interval, interval);
	}
	
	int m_color;
	public Material m_light;
	public Material m_dark;
	public Light m_spotlight;
	const int on = 1;
	const int off = 0;
	void ChangeColor(){
		if (m_color == off){
			GetComponent<Renderer>().material = m_light;
			m_spotlight.enabled = true;
			m_color = on;
		}
		else if (m_color == on) {
			GetComponent<Renderer>().material = m_dark;
			m_spotlight.enabled = false;
			m_color = off;
		}
	}

}
