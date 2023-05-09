using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public int[] score;

    private void Start()
    {
        LoadScore();
    }
    // 이 행위는 유니티 데이터에 키값에 해당하는 값을 저장한다 종료되도 정보를 기록함
    public void SaveScore()
    {
        PlayerPrefs.SetInt("Score1", score[0]);
        PlayerPrefs.SetInt("Score2", score[1]);
        PlayerPrefs.SetInt("Score3", score[2]);
    }

    // 유니티 데이터에서 가져옴 게임켜질때
    public void LoadScore()
    {
        if (PlayerPrefs.HasKey("Score1"))
        {
            score[0] = PlayerPrefs.GetInt("Score1");
            score[1] = PlayerPrefs.GetInt("Score2");
            score[2] = PlayerPrefs.GetInt("Score3");
        }
    }
}
