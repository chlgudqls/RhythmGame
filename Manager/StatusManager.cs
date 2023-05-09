using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    [SerializeField] float blinkSpeed = 0.1f;
    [SerializeField] int blinkCount = 10;
    int currentBlinkCount = 0;
    bool isBlink = false;

    bool isDead = false;

    int maxHp = 3;
    int currentHp = 3;

    int maxShield = 3;
    int currentShield = 0;

    [SerializeField] GameObject[] hpImage = null;
    [SerializeField] GameObject[] shieldImage = null;

    [SerializeField] int shieldIncreaseCombo = 5;
    int currentShieldCombo = 0;
    [SerializeField] Image shieldGuage = null;

    Result theResult;
    NoteManager theNote;

    [SerializeField] MeshRenderer playerMesh = null;

    private void Start()
    {
        theResult = FindObjectOfType<Result>();
        theNote = FindObjectOfType<NoteManager>();
    }

    public void Initialized()
    {
        currentHp = maxHp;
        currentShield = 0;
        currentShieldCombo = 0;
        shieldGuage.fillAmount = 0;
        isDead = false;
        SettingHPImage();
        SettingShieldImage();
    }

    public void CheckShield()
    {
        currentShieldCombo++;

        if(currentShieldCombo >= shieldIncreaseCombo)
        {
            IncreaseShield();
            currentShieldCombo = 0;
        }

        shieldGuage.fillAmount = (float)currentShieldCombo / shieldIncreaseCombo;
    }
    public void ResetShieldCombo()
    {
        currentShieldCombo = 0;
        shieldGuage.fillAmount = (float)currentShieldCombo / shieldIncreaseCombo;
    }
    public void IncreaseShield()
    {
        currentShield++;

        if (currentShield >= maxShield)
            currentShield = maxShield;

        SettingShieldImage();
    }
    public void DescreaseShield(int p_num)
    {
        currentShield -= p_num;

        if (currentShield <= 0)
            currentShield = 0;

        SettingShieldImage();
    }
    public void IncreaseHP(int p_num)
    {
        currentHp += p_num;

        if (currentHp >= maxHp)
            currentHp = maxHp;

        SettingHPImage();
    }

    public void DecreaseHP(int p_num)
    {
        // �� ü�� ���� ����κп��� �̶��� ������ ���������� blink�ϋ� �����̳� �ٸ��� ����
        if (!isBlink)
        {
            if (currentShield > 0)
                DescreaseShield(p_num);

            else
            {
                currentHp -= p_num;

                if (currentHp <= 0)
                {
                    // �� � ��Ȳ�� � �Լ��� ȣ����
                    isDead = true;
                    Debug.Log("����");
                    theResult.ShowResult();
                    theNote.RemoveNote();
                }
                else
                {
                    // �� ü���� ���������� ������ �ƴ� ��Ȳ  0���� Ŭ�� 
                    StartCoroutine(BlinkCo());
                }

                SettingHPImage();
            }
        }
    }

    private void SettingHPImage()
    {
        for (int i = 0; i < hpImage.Length; i++)
        {
            if (i < currentHp)
                hpImage[i].SetActive(true);
            else
                hpImage[i].SetActive(false);
        }
    }
    private void SettingShieldImage()
    {
        for (int i = 0; i < shieldImage.Length; i++)
        {
            if (i < currentShield)
                shieldImage[i].SetActive(true);
            else
                shieldImage[i].SetActive(false);
        }
    }
    public bool IsDead()
    {
        return isDead;
    }

    IEnumerator BlinkCo()
    {
        isBlink = true;

        while (currentBlinkCount <= blinkCount)
        {
            playerMesh.enabled = !playerMesh.enabled;
            yield return new WaitForSeconds(blinkSpeed);
            currentBlinkCount++;
        }

        playerMesh.enabled = true;
        currentBlinkCount = 0;
        isBlink = false;
    }
}
