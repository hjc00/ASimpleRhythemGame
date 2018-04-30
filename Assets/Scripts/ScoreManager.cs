using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class ScoreManager : MonoBehaviour
{

    public Text scoreText;
    public static ScoreManager Instance;
    public string currentSongInRank;  //当前歌曲
    public Text curretnSongText;   //当前歌曲的UItext
    public Dropdown drop;
    public GameObject rankPanel;

    public Dictionary<string, List<int>> rank = new Dictionary<string, List<int>>();
    public Text[] rankText;  //显示得分的每个text

    public int currentScore;

    void Awake()
    {
        Instance = this;
        currentScore = 0;
        GetSongName();
        UpdateCurrentSongName();
    }

    void Update()
    {
        scoreText.text = "Score:" + currentScore.ToString();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
    }

    public void MinusScore(int amount)
    {
        currentScore -= amount;
    }


    void GetSongName()         //获取dropdown里面所有的歌曲名并创建一个txt类型储存的排行榜
    {
        foreach (var item in drop.options)
        {
            rank.Add(item.text, new List<int>());

            string fileName = item.text + "_排行榜.rank";
            string path = Application.dataPath + "/RankFiles/" + fileName;

            if (!File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                fs.Close();
            }
        }
    }

    public void UpdateRank()  //更新排行榜得分，并保存到TXT中
    {
        string fileName = currentSongInRank + "_排行榜.rank";
        string path = Application.dataPath + "/RankFiles/" + fileName;

        List<int> rank = ReadInTxt();

        if (rank.Count > 8)
        {
            if (currentScore > rank[8])
            {
                rank.RemoveAt(8);
                rank.Add(currentScore);
                rank.Sort((x, y) => -x.CompareTo(y));//降序
                StreamWriter sw = new StreamWriter(path);
                foreach (var item in rank)
                {
                    sw.WriteLine(item.ToString());
                }
                sw.Close();
            }
        }
        else
        {
            rank.Add(currentScore);
            rank.Sort((x, y) => -x.CompareTo(y));//降序

            StreamWriter sw = new StreamWriter(path);
            foreach (var item in rank)
            {
                sw.WriteLine(item.ToString());
            }
            sw.Close();
        }

    }

    public void UpdateCurrentSongName()
    {
        currentSongInRank = drop.captionText.text;
        curretnSongText.text = currentSongInRank;
    }

    public void ShowRankUI()
    {
        UpdateCurrentSongName();
        List<int> rank = ReadInTxt();

        for (int i = 0; i < rankText.Length; i++)
        {
            rankText[i].text = 0.ToString();
        }
        if (rank.Count != 0)
        {
            for (int i = 0; i < rank.Count; i++)
            {
                rankText[i].text = rank[i].ToString();
            }
        }
        rankPanel.SetActive(true);
        rank.Clear();
    }

    public void HideRankUI()
    {
        rankPanel.SetActive(false);
    }

    List<int> ReadInTxt()  //从TXT读取数据
    {
        string fileName = currentSongInRank + "_排行榜.rank";
        string path = Application.dataPath + "/RankFiles/" + fileName;
        StreamReader sr = new StreamReader(path);

        List<int> rank = new List<int>(9);


        //读取排行榜数据
        while (sr.Peek() >= 0)
        {
            rank.Add(int.Parse(sr.ReadLine()));
        }

        sr.Close();
        return rank;
    }
}
