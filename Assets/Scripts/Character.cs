using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float m_sizeNow;
    public float m_deltaTime;
    public const float m_timeThreshold = 1.0f;
    public bool m_isBlowing;
    public Balloon balloonOnMouth;
    public float blowDegree;

    // Start is called before the first frame update
    void Start()
    {
        m_isBlowing = false;
        m_sizeNow = 0.01f;
        m_deltaTime = 0;
        blowDegree = 1.01f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isBlowing)
        {
            blowBalloon();
            if (m_deltaTime> m_timeThreshold) {
                m_isBlowing = false;
                balloonOnMouth.isBlowing = false;
                balloonOnMouth = null;
            }
        }
        else {
            if (Input.GetMouseButton(0))
            {
                createBalloon();
                m_isBlowing = true;
            }
        }
    }

    void blowBalloon()
    {
        if (Input.GetMouseButton(0))
        {
            balloonOnMouth.updateScale(blowDegree);
            m_deltaTime = 0;
        }
        else {
            m_deltaTime += Time.deltaTime;
        }
    }

    void createBalloon()
    {
        Vector3 pos = this.transform.TransformPoint(-1, 1, 0);
        Vector3 scale = new Vector3(0.7f, 0.7f, 0.7f);
        Color color = new Color(Random.value,Random.value,Random.value,1.0f);
        balloonOnMouth = Balloon.Create(pos, scale, color);
    }

}
