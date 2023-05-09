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
            Debug.Log("�ʱ�ȭ ���� (���ͳ� ���� ���)");
    }

    // ȸ������ ���ù�ư�� �������� ������ �ؽ�Ʈ���� �����
    public void BtnRegist()
    {
        string t_id = id.text;
        string t_pw = pw.text;

        // ������ ������   ȣ���ؼ� ���ƿ°��� Ÿ�Կ� �ش��ϴ� ������ ������ �������� ����Ȯ��
        BackendReturnObject bro = Backend.BMember.CustomSignUp(t_id, t_pw, "Test");

        if (bro.IsSuccess())
        {
            Debug.Log("ȸ������ �Ϸ�");
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("ȸ������ ����");
        }
    }
}
