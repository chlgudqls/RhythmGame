using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] GameObject go_StageUI = null;

    public void BtnPlay()
    {
        go_StageUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
