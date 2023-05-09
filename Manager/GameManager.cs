using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] goGameUI = null;
    [SerializeField] GameObject goTitleUI = null;

    public static GameManager instance;

    public bool isStartGame = false;

    ComboManager theCombo;
    ScoreManager theScore;
    TimingManager theTiming;
    StatusManager theStatus;
    PlayerController thePlayer;
    StageManager theStage;
    NoteManager theNote;
    Result theResult;
    // ※ 비활성화 오브젝트는 find로 찾을수가 없다
    [SerializeField] CenterFlame theMusic = null;

    private void Start()
    {
        instance = this;

        theCombo = FindObjectOfType<ComboManager>();
        theScore = FindObjectOfType<ScoreManager>();
        theTiming = FindObjectOfType<TimingManager>();
        theStatus = FindObjectOfType<StatusManager>();
        thePlayer = FindObjectOfType<PlayerController>();
        theStage = FindObjectOfType<StageManager>();
        theNote = FindObjectOfType<NoteManager>();
        theResult = FindObjectOfType<Result>();

    }


    public void GameStart(int p_songNum, int p_bpm)
    {
        for (int i = 0; i < goGameUI.Length; i++)
        {
            goGameUI[i].SetActive(true);
        }

        theResult.setCurrentSong(p_songNum);
        theMusic.bgmName = "BGM" + p_songNum;
        theNote.bpm = p_bpm;
        theStage.RemoveStage();
        theStage.SettingStage(p_songNum);
        theCombo.ResetCombo();
        theScore.Initialized();
        theTiming.Initialized();
        theStatus.Initialized();
        thePlayer.Initialized();


        isStartGame = true;
    }

    public void MainMenu()
    {
        for (int i = 0; i < goGameUI.Length; i++)
        {
            goGameUI[i].SetActive(false);
        }
        goTitleUI.SetActive(true);
        //isStartGame = false;
    }
}
