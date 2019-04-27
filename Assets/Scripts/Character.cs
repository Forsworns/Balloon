using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public float m_sizeNow;
    public float m_deltaTime;
    public const float m_timeThreshold = 1.0f;
    public bool m_isBlowing;
    public Balloon balloonOnMouth;
    public float blowDegree;
    public float m_difficulty;

    public SpriteRenderer m_spriteRender;
    private AudioSource m_audioSource;

    public static int m_score;
    public GameObject m_scoreText; // bind in Unity, not in script

    // Start is called before the first frame update
    void Start()
    {
        m_isBlowing = false;
        m_sizeNow = 0.01f;
        m_deltaTime = 0;
        blowDegree = 1.01f;
        m_difficulty = 10000;
        m_score = 0;
        m_spriteRender = this.GetComponent<SpriteRenderer>();
        m_audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Menu.m_isPaused) {
            if (m_isBlowing && balloonOnMouth != null)
            {
                blowBalloon();
                if (m_deltaTime > m_timeThreshold)
                {
                    m_isBlowing = false;
                    balloonOnMouth.isBlowing = false;
                    balloonOnMouth = null;
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    createBalloon();
                    m_isBlowing = true;
                }
            }
        }
    }

    void blowBalloon()
    {
        if (Input.GetMouseButton(0))
        {
            balloonOnMouth.updateScale(blowDegree);
            m_deltaTime = 0;
            if (Menu.musicOnOff && !m_audioSource.isPlaying)
            {
                m_audioSource.Play();
            }
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
        balloonOnMouth = Balloon.Create(pos, scale, color, m_difficulty);
        balloonOnMouth.destroyNormally += destroyNormally;
    }

    void destroyNormally(Balloon balloon) {
        m_score += Mathf.RoundToInt(balloon.transform.localScale.sqrMagnitude*10); // more score for larger balloon as reward
        m_scoreText.GetComponent<Text>().text = m_score.ToString();
    }

    public void changeCharacter(string spriteName) {
        m_spriteRender.sprite = (Sprite)Resources.Load(spriteName, typeof(Sprite));
    }
}
