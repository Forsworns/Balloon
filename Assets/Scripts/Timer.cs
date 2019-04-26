using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float m_timerLength;
    public float m_timeNow;
    public Image m_image;
    
    // Start is called before the first frame update
    void Start()
    {
        m_timerLength = 60.0f;
        m_timeNow = 0;
        m_image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        m_timeNow += Time.deltaTime;
        m_image.fillAmount = m_timeNow / m_timerLength;
    }
}
