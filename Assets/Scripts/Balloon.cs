using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    private bool isDestroyed;
    public bool isBlowing = false;

    public float m_speed;
    public float m_existTime;
    public float m_minTime = 2.0f;
    public float m_maxTime = 4.0f;

    public Vector3 m_constantBias = new Vector3(0,20,0);
    public float m_originalDist;
    public Vector3 m_targetPosition;
    
    public double m_boomP;
    public float m_additionalBoomP = 0;

    public delegate void VoidDelegate(Balloon balloon); // 创建一个委托类型，接受Balloon类型
    public VoidDelegate destroyNormally; // 声明一个委托，之后可以让他实例化
    public VoidDelegate destroyAccidently; 

    private SpriteRenderer m_spriteRenderer; // 用于更改透明度
    public Color m_color;


    // Start is called before the first frame update
    void Start()
    {
        isDestroyed = false;
        m_spriteRenderer = this.GetComponent<SpriteRenderer>();
        setTarget();
        setExistTime();
        setSpeed();
        boomProbability();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlowing)
        {
            boomProbability();
        }
        else if (!isDestroyed)
        {
            updatePosition();
            boomProbability();
        }
    }

    // 静态方法，通过引用prefab将他实例化创建GameObject，便于在点击打气筒时创建实例
    public static Balloon Create(Vector3 pos, Vector3 s,Color c)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/balloon");
        GameObject balloonSprite = (GameObject)Instantiate(prefab,pos,Quaternion.identity); // 创建实例
        Balloon balloon = balloonSprite.AddComponent<Balloon>();
        balloon.transform.localScale = s;
        balloon.GetComponent<SpriteRenderer>().color = c;
        balloon.isBlowing = true;
        return balloon;
    }

    public void updateScale(float t)
    {
        this.transform.localScale *= t;
    }

    private void setExistTime()
    {
        m_existTime = Random.Range(m_minTime,m_maxTime);
    }

    private void setTarget()
    {
        m_targetPosition = Random.insideUnitSphere*10+ m_constantBias;
        m_targetPosition.z = 0;
        m_originalDist = Vector3.Distance(this.transform.position, m_targetPosition);
    }

    // 速度由目标位置和存活时间确定，加快速度的逻辑要借由缩短时间实现
    private void setSpeed()
    {
        m_speed = m_targetPosition.magnitude / m_existTime;
    }

    private void updatePosition()
    {
        Vector3 pos = Vector3.MoveTowards(this.transform.position, m_targetPosition, m_speed * Time.deltaTime);
        this.transform.position = pos;
        m_color = m_spriteRenderer.color;
        m_color.a = Vector3.Distance(pos, m_targetPosition) / m_originalDist;
        m_spriteRenderer.material.color = m_color;
        if (Vector3.Distance(pos, m_targetPosition) < 0.1f)
        {
            isDestroyed = true;
            Destroy(this.gameObject);
            destroyNormally(this);
        }
    }

    // 爆炸的概率
    private void boomProbability()
    {
        m_boomP = System.Math.Pow(this.transform.localScale.magnitude, 2)/10000 + m_additionalBoomP;
        if (Random.value<m_boomP) { // 概率性爆炸
            GameObject prefab = Resources.Load<GameObject>("Prefabs/boom");
            GameObject boom = (GameObject)Instantiate(prefab, this.transform.position, this.transform.rotation);
            Destroy(boom, 1.0f);
            isDestroyed = true;
            Debug.Log(isDestroyed);
            Destroy(this.gameObject);
            destroyAccidently(this);
        }
    }
}
