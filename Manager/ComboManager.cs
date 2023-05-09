using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] GameObject goComboImage = null;
    [SerializeField] UnityEngine.UI.Text txtCombo = null;

    Animator myAnim;
    string animComboUp = "ComboUp";

    int currentCombo = 0;
    int maxCombo = 0;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        txtCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }
    // ※ 보통 텍스트로 나타낼때 증감하고 text에 넣음

    public void IncreaseCombo(int p_num = 1)
    {

        currentCombo += p_num;
        txtCombo.text = string.Format("{0:#,##0}", currentCombo);

        if (maxCombo < currentCombo)
            maxCombo = currentCombo;

        if(currentCombo > 2)
        {
            txtCombo.gameObject.SetActive(true);
            goComboImage.SetActive(true);

            myAnim.SetTrigger(animComboUp);
        }
    }
    public int GetCurrentCombo()
    {
        return currentCombo;
    }
    public int GetMaxCombo()
    {
        return maxCombo;
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        txtCombo.text = "0";
        txtCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }

    public int CurrentCombo()
    {
        return maxCombo;
    }

}
