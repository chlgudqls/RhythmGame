using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject goUI = null;

    [SerializeField] Text[] txtCount = null;
    [SerializeField] Text txtCoin = null;
    [SerializeField] Text txtScore = null;
    [SerializeField] Text txtMaxCombo = null;

    int currentSong = 0; public void setCurrentSong(int p_songNum) { currentSong = p_songNum; }

    ScoreManager theScore;
    ComboManager theCombo;
    TimingManager theTiming;
    DatabaseManager theDatabase;
    void Start()
    {
        theScore = FindObjectOfType<ScoreManager>();
        theCombo = FindObjectOfType<ComboManager>();
        theTiming = FindObjectOfType<TimingManager>();
        theDatabase = FindObjectOfType<DatabaseManager>();
    }

    public void ShowResult()
    {
        FindObjectOfType<CenterFlame>().ResetMusic();

        AudioManager.instance.StopBGM();

        goUI.SetActive(true);

        for (int i = 0; i < txtCount.Length; i++)
            txtCount[i].text = "0";

        txtCoin.text = "0";
        txtScore.text = "0";
        txtMaxCombo.text = "0";

        int[] t_judgement = theTiming.GetJudgementRecord();
        int t_currentScore = theScore.GetCurrentScore();
        int t_maxCombo = theCombo.GetMaxCombo();
        int t_coin = t_currentScore / 50;

        for (int i = 0; i < txtCount.Length; i++)
            txtCount[i].text = string.Format("{0:#,##0}", t_judgement[i]);

        txtScore.text = string.Format("{0:#,##0}", t_currentScore);
        txtMaxCombo.text = string.Format("{0:#,##0}", t_maxCombo);
        txtCoin.text = string.Format("{0:#,##0}", t_coin);

        // ※ 결과창 부분에서 최종 Maxscore를 구할수있으니 받을건데 인덱스가 필요함 해당 스테이지의 최고기록만
        // ※ 구할거니까 currentSong 을 받아온다
        // ※ 그런데 항상 대입하는게 아니라 최고기록만 가져와야된다
        if(t_currentScore > theDatabase.score[currentSong])
        {
            theDatabase.score[currentSong] = t_currentScore;
            theDatabase.SaveScore();
        }
    }

    public void BtnMainMenu()
    {
        goUI.SetActive(false);
        GameManager.instance.MainMenu();
        theCombo.ResetCombo();
    }
}
