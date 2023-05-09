using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] Animator noteHitAnimator = null;
    string hit = "Hit";

    [SerializeField] Animator judgementAnimator = null;
    [SerializeField] UnityEngine.UI.Image judgementImage = null;
    [SerializeField] Sprite[] judgementSprite = null;


    // �� timingBoxs�� ������ 4�����̱⶧���� �̹����� �ٸ����ؾߵ� 
    // �׳� �ε����Ѱܹ޾Ƽ� �迭������ �� �����ϸ� �ǰڳ�
    public void JudgementEffect(int p_num)
    {
        judgementImage.sprite = judgementSprite[p_num];
        judgementAnimator.SetTrigger(hit);
    }
    public void NoteHitEffect()
    {
        noteHitAnimator.SetTrigger(hit);
    }
}
