using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text txtScore = null;

    [SerializeField] int increaseScore = 10;
    int currentScore = 0;

    [SerializeField] float[] weight = null;
    [SerializeField] int comboBonusScore = 10;

    Animator myAnim;
    string animScoreUp = "ScoreUp";

    ComboManager theCombo;
    void Start()
    {
        myAnim = GetComponent<Animator>();
        theCombo = FindObjectOfType<ComboManager>();
        currentScore = 0;
        txtScore.text = "0";
    }

    public void Initialized()
    {
        currentScore = 0;
        txtScore.text = "0";
    }

    public void IncreaseScore(int p_JudgementState)
    {
        // 콤보 증가
        theCombo.IncreaseCombo();

        // 콤보 보너스 점수 계산
        int t_currentCombo = theCombo.GetCurrentCombo();
        // ※ 10으로 나눈몫의 숫자에 10을 곱하기때문에 값이 깔끔하게 떨어짐
        int t_bonusComboScore = (t_currentCombo / 10) * comboBonusScore;

        // 가중치 계산
        int t_increaseScore = increaseScore + t_bonusComboScore;
        // ※ timingBoxs 의 인덱스를 받아서 배열순서를 잘 배치시켜놓은 weight의 인덱스에 넣고 곱함 timingBoxs값에 따라 점수가 달라지게함
        // 인덱스를 받아서 다른 배열의 인덱스로 사용
        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]);

        // 점수 반영
        currentScore += t_increaseScore;
        txtScore.text = string.Format("{0:#,##0}", currentScore);

        // 애니 실행
        myAnim.SetTrigger(animScoreUp);
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}
