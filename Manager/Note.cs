using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float noteSpeed = 400;

    // ��Ʈ�� �����Ǹ鼭 ���ǵ� ����������  ���͸� �����ľ� �����̳����⶧���� �̹����� ��Ȱ��ȭ �ı� ���
    UnityEngine.UI.Image noteImage;

    private void OnEnable()
    {
        if(noteImage == null)
            noteImage = GetComponent<UnityEngine.UI.Image>();

        noteImage.enabled = true;
    }

    void Update()
    {
        transform.localPosition += Vector3.right * noteSpeed * Time.deltaTime;
    }

    public void HideNote()
    {
        noteImage.enabled = false;
    }
    public bool GetNoteFlag()
    {
        return noteImage.enabled;
    }
}
