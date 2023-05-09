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
    // �� ������ ����Ƽ �����Ϳ� Ű���� �ش��ϴ� ���� �����Ѵ� ����ǵ� ������ �����
    public void SaveScore()
    {
        PlayerPrefs.SetInt("Score1", score[0]);
        PlayerPrefs.SetInt("Score2", score[1]);
        PlayerPrefs.SetInt("Score3", score[2]);
    }

    // ����Ƽ �����Ϳ��� ������ ����������
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
