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


    // ※ timingBoxs의 종류가 4가지이기때문에 이미지를 다르게해야됨 
    // 그냥 인덱스넘겨받아서 배열순서를 잘 설정하면 되겠네
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
