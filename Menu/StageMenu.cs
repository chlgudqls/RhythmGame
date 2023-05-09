using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ※ 스테이지메뉴의 UI들을 Song정보를 이용해서 변경시킨다 setting
[System.Serializable]
public class Song
{
    public string name;
    public string composer;
    public int bpm;
    public Sprite sprite;
}

public class StageMenu : MonoBehaviour
{
    [SerializeField] Song[] songList = null;

    [SerializeField] Text txtSongName = null;
    [SerializeField] Text txtSongComposer = null;
    [SerializeField] Text txtSongScore = null;
    [SerializeField] Image imgDisk = null;

    [SerializeField] GameObject TitleMenu = null;

    DatabaseManager theDatabase;

    // ※ 이게 핵심 이라고봄 
    // ※※ 핵심이였음 
    int currentSong = 0;
    
    // ※ 세팅이 한번밖에 안되서 게임이 종료되고 점수반영이 안되는 문제 발생
    // ※ 메뉴가 스테이지가 끝나면 메뉴가 다시 활성화되는점을 이용해서 setting이 한번더 실행되도록했음
    // ※ 대단한듯 이 방법이 좋은거같음
    private void OnEnable()
    {
        if (theDatabase == null)
            theDatabase = FindObjectOfType<DatabaseManager>();

        SettingSong();
    }

    public void BtnNext()
    {
        AudioManager.instance.PlaySFX("Touch");
        // ※ if안에 있지만 증감은 계속됨 증감하다가 조건이 걸리면 그때 0이된다는 뜻
        if (++currentSong > songList.Length - 1)
            currentSong = 0;

        SettingSong();
    }
    public void BtnPrior()
    {
        AudioManager.instance.PlaySFX("Touch");
        if (--currentSong < 0)
            currentSong = songList.Length - 1;
        
        SettingSong();
    }

    public void SettingSong()
    {
        txtSongName.text = songList[currentSong].name;
        txtSongComposer.text = songList[currentSong].composer;
        txtSongScore.text = string.Format("{0:#,##0}", theDatabase.score[currentSong]);
        imgDisk.sprite = songList[currentSong].sprite;

        AudioManager.instance.PlayBGM("BGM" + currentSong);
    }

    public void BtnBack()
    {
        TitleMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void BtnPlay()
    {
        int t_bpm = songList[currentSong].bpm;
        AudioManager.instance.StopBGM();

        GameManager.instance.GameStart(currentSong, t_bpm);
        this.gameObject.SetActive(false);
    }
}
