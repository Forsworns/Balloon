using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private const float EASY = 10000;
    private const float MEDIUM = 1000;
    private const float HARD = 100;

    private const float TIME_LENGTH = 10;

    public static bool m_isPaused;

    public Character m_character; // bind in the unity, not in script
    private string[] CHARACTERS = { "Sprites/child", "Sprites/monkey" };
    private string[] INFOES = { "一名普通的小男孩", "一只普通的小猴子" };
    public int m_now = 0; // character No.

    public GameObject m_camera; // bind in the unity, not in script

    public GameObject m_menu;
    public GameObject m_music;
    public GameObject m_musicOff;
    public GameObject m_helpPanel;
    public GameObject m_rankPanel;
    public GameObject m_rankInfo;
    public GameObject m_characterPanel;
    public GameObject m_score;

    public GameObject m_easyB;
    public GameObject m_mediumB;
    public GameObject m_hardB;

    public float m_timerLength;
    public float m_timeNow;
    public GameObject timerSlider;
    public Image m_timerImage;
    public GameObject m_timeoutPanel;

    public static bool musicOnOff;

    public List<int> scoreList;
    GameObject recordPrefab;
    const int RANK_LENGTH = 8;

    // Start is called before the first frame update
    void Start()
    {
        m_menu = this.transform.Find("menu").gameObject;
        m_music = m_menu.transform.Find("music").gameObject;
        m_musicOff = m_menu.transform.Find("music_off").gameObject;
        m_helpPanel = this.transform.Find("helpPanel").gameObject;
        m_characterPanel = this.transform.Find("characterPanel").gameObject;
        m_score = this.transform.Find("score").gameObject;

        musicOnOff = true;

        // three kinds of difficulty
        m_easyB = m_menu.transform.Find("easy").gameObject;
        m_mediumB = m_menu.transform.Find("medium").gameObject;
        m_hardB = m_menu.transform.Find("hard").gameObject;

        // time
        m_timerLength = TIME_LENGTH;
        m_timeNow = 0;
        timerSlider = this.transform.Find("timerBase").Find("timerSlider").gameObject;
        m_timerImage = timerSlider.GetComponent<Image>();
        m_timeoutPanel = this.transform.Find("timerBase").Find("timeoutPanel").gameObject;

        // rank
        m_rankPanel = this.transform.Find("rankPanel").gameObject;
        m_rankInfo = m_rankPanel.transform.Find("rankInfo").gameObject;
        scoreList = new List<int>();
        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/rank.txt");
        string nextLine;
        while ((nextLine = sr.ReadLine()) != null) {
            scoreList.Add(int.Parse(nextLine));
        }
        sr.Close();
        recordPrefab = Resources.Load<GameObject>("Prefabs/record");
    }

    // Update is called once per frame
    void Update()
    {
        m_timeNow += Time.deltaTime;
        m_timerImage.fillAmount = m_timeNow / m_timerLength;
        if (m_timerImage.fillAmount >= 1)
        {
            m_timeoutPanel.transform.Find("timeInfoScore").GetComponent<Text>().text = m_score.GetComponent<Text>().text;
            m_timeoutPanel.SetActive(true);
            scoreList.Add(Character.m_score);
            // sort and store it
            scoreList.Sort((x,y)=>-x.CompareTo(y));
            StreamWriter sw = new StreamWriter(Application.dataPath + "/Resources/rank.txt");
            if (scoreList.Count > RANK_LENGTH)
            {
                for (int i = RANK_LENGTH; i <= scoreList.Count; i++)
                {
                    scoreList.RemoveAt(i);
                }
            }
            for (int i = 0; i < scoreList.Count; i++)
            {
                sw.WriteLine(scoreList[i]);
                Debug.Log(scoreList[i].ToString());
            }
            sw.Close();
            m_timerImage.fillAmount = 0;
            m_timeNow = 0;
        }
    }

    public void timeout()
    {
        m_timeoutPanel.SetActive(false);
        m_timeNow = 0;
        m_score.GetComponent<Text>().text = "0";
        Character.m_score = 0;
    }

    public void pause()
    {
        m_character.GetComponent<AudioSource>().Pause();
        Time.timeScale = 0.0f;
        m_menu.SetActive(true);
        m_camera.GetComponents<AudioSource>()[1].Pause();
        m_isPaused = true;
    }

    public void resume()
    {
        m_character.GetComponent<AudioSource>().Play();
        m_menu.SetActive(false);
        Time.timeScale = 1.0f;
        m_camera.GetComponents<AudioSource>()[1].Play();
        m_isPaused = false;
    }

    public void quit() {
        Application.Quit();
    }

    public void music() {
        musicOnOff = !musicOnOff;
        m_music.SetActive(musicOnOff);
        m_musicOff.SetActive(!musicOnOff);
        m_camera.GetComponents<AudioSource>()[1].mute = musicOnOff;
    }

    public void help() {
        m_helpPanel.SetActive(true);
        m_menu.SetActive(false);
    }

    public void understand() {
        m_helpPanel.SetActive(false);
        m_menu.SetActive(true);
    }

    public void changeCharacter() {
        m_characterPanel.SetActive(true);
        m_menu.SetActive(false);
    }

    public void previous() {
        if (m_now > 0)
        {
            m_now -= 1;
            m_characterPanel.transform.Find("characterImage").GetComponent<Image>().sprite = (Sprite)Resources.Load(CHARACTERS[m_now],typeof(Sprite));
            m_characterPanel.transform.Find("characterImage").GetComponent<Image>().preserveAspect = true;
            m_characterPanel.transform.Find("characterInfo").GetComponent<Text>().text = INFOES[m_now];
        }
    }

    public void next() {
        if (m_now < CHARACTERS.Length - 1)
        {
            m_now += 1;
            m_characterPanel.transform.Find("characterImage").GetComponent<Image>().sprite = (Sprite)Resources.Load(CHARACTERS[m_now], typeof(Sprite));
            m_characterPanel.transform.Find("characterImage").GetComponent<Image>().preserveAspect = true;
            m_characterPanel.transform.Find("characterInfo").GetComponent<Text>().text = INFOES[m_now];
        }
    }

    public void choose()
    {
        m_characterPanel.SetActive(false);
        m_menu.SetActive(true);
        m_character.changeCharacter(CHARACTERS[m_now]);
    }

    public void recordPanel() {
        m_rankPanel.SetActive(true);
        m_menu.SetActive(false);

        // use prefab to instantiate
        for (int i = 0; i < scoreList.Count; i++)
        {
            GameObject item = Instantiate(recordPrefab);
            item.gameObject.SetActive(true);
            item.transform.SetParent(m_rankInfo.transform, false);
            item.transform.Find("order").gameObject.GetComponent<Text>().text = (i + 1).ToString();
            item.transform.Find("score").gameObject.GetComponent<Text>().text = scoreList[i].ToString();
        }
    }

    public void recordPanelClose()
    {
        m_rankPanel.SetActive(false);
        m_menu.SetActive(true);
    }

    public void easy() {
        m_character.m_difficulty = EASY;
        m_easyB.GetComponent<Image>().color = new Color(255,255,255,1);
        m_mediumB.GetComponent<Image>().color = new Color(255, 255, 255, 0.3f);
        m_hardB.GetComponent<Image>().color = new Color(255, 255, 255, 0.3f);
    }

    public void medium()
    {
        m_character.m_difficulty = MEDIUM;
        m_easyB.GetComponent<Image>().color = new Color(255, 255, 255, 0.3f);
        m_mediumB.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        m_hardB.GetComponent<Image>().color = new Color(255, 255, 255, 0.3f);
    }

    public void hard()
    {
        m_character.m_difficulty = HARD;
        m_easyB.GetComponent<Image>().color = new Color(255, 255, 255, 0.3f);
        m_mediumB.GetComponent<Image>().color = new Color(255, 255, 255, 0.3f);
        m_hardB.GetComponent<Image>().color = new Color(255, 255, 255, 1);
    }
}
