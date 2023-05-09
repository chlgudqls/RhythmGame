using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �Լ��� ȣ�� // �ϱ� ���⼭ ������Ʈ �����ͼ� ������ ©�ٿ� �÷��̾�� �Լ������Ѱ� �����ͼ� ���°� ����
            other.GetComponentInParent<PlayerController>().ResetFalling();
        }
    }
}
