using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class Login : MonoBehaviour
{
    [SerializeField] InputField id = null;
    [SerializeField] InputField pw = null;
    private void Start()
    {
        //Backend.Initialize(InitializeCallback);
    }
    private void InitializeCallback()
    {
        if (Backend.IsInitialized)
        {
            Debug.Log(Backend.Utils.GetServerTime());
            Debug.Log(Backend.Utils.GetGoogleHash());
        }
        else
            Debug.Log("초기화 실패 (인터넷 문제 등등)");
    }

    // 회원가입 관련버튼이 눌렸을때 변수에 텍스트값이 저장됨
    public void BtnRegist()
    {
        string t_id = id.text;
        string t_pw = pw.text;

        // 서버에 던진다   호출해서 돌아온값을 타입에 해당하는 변수에 저장후 성공실패 여부확인
        BackendReturnObject bro = Backend.BMember.CustomSignUp(t_id, t_pw, "Test");

        if (bro.IsSuccess())
        {
            Debug.Log("회원가입 완료");
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("회원가입 실패");
        }
    }
}
