using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    // ������ ��Ʈ�� �� ��´� => ���������� �ִ��� ��� ��Ʈ�� ���Ѵ�
    public List<GameObject> boxNoteList = new List<GameObject>();

    int[] judgementRecord = new int[5];

    [SerializeField] Transform Center = null;
    // �������� (Perfect, Cool, Good, Bad)
    [SerializeField] RectTransform[] timingRect = null;
    // ���������� �ּҰ�x, �ִ밪y
    Vector2[] timingBoxs = null;

    EffectManager theEffect;
    ScoreManager theScoreManager;
    ComboManager theCombo;
    StageManager theStage;
    PlayerController thePlayer;
    StatusManager theStatus;
    AudioManager theAudio;
    void Start()
    {
        theEffect = FindObjectOfType<EffectManager>();
        theScoreManager = FindObjectOfType<ScoreManager>();
        theCombo = FindObjectOfType<ComboManager>();
        theStage = FindObjectOfType<StageManager>();
        thePlayer = FindObjectOfType<PlayerController>();
        theStatus = FindObjectOfType<StatusManager>();
        // �� �� �ڵ�� ���������� �ٸ������ �� ������ null�� �����ϴ� ���� ���ܼ� AudioManager�� ���� ����ɼ��ְ� �켱���� ������
        // �� ����Ƽ���� ������ 
        theAudio = AudioManager.instance;
        // Ÿ�̹� �ڽ� ����
        timingBoxs = new Vector2[timingRect.Length];

        for (int i = 0; i < timingRect.Length; i++)
        {
            // �� ���Ϳ� �ִ� x��ǥ���� ���������� ������ ������ؼ� ������ ���ߴ�
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,
                              Center.localPosition.x + timingRect[i].rect.width / 2);
        }
    }

    // ����Ʈ�� �ִ� ��Ʈ���� Ȯ���ؼ� ���� �ڽ��� �ִ� ��Ʈ�� ã�´�
    public bool CheckTiming()
    {
        // ����Ʈ ��Ʈ�� ��ġ�� ����
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            float t_notePosX = boxNoteList[i].transform.localPosition.x;

            // ����������ŭ �ݺ��ϰ� ��� ������ �ִ��� Ȯ��
            for (int x = 0; x < timingBoxs.Length; x++)
            {
                // 0���ʹϱ� ����Ʈ���� �˻��ϸ� ���������� �˻������
                if(timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
                {
                    //Destroy(boxNoteList[i]);
                    // ��Ʈ ����
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);
                    // ����Ʈ ����
                    if(x < timingBoxs.Length -1)
                        theEffect.NoteHitEffect();
                    Debug.Log("Hit" + x);

                    // �� ������ ���������� ������ �� �κп��� �޸𸮿� ���� ���� ������Ű�� ����
                    if (CheckCanNextPlate())
                    {
                        theScoreManager.IncreaseScore(x); // ���� ����
                        theStage.ShowNextPlate(); // �Ƕ��� ����
                        theEffect.JudgementEffect(x); // ���� ����
                        judgementRecord[x]++; // ���� ���
                        theStatus.CheckShield(); // ���� üũ
                    }
                    else
                        theEffect.JudgementEffect(5);

                    theAudio.PlaySFX("Touch");
                    return true;
                }
            }
        }
        Debug.Log("Miss");
        MissRecord();
        theCombo.ResetCombo();
        theEffect.JudgementEffect(timingBoxs.Length);
        theStatus.ResetShieldCombo();
        return false;
    }
    // üũ���� ��갪��������
    private bool CheckCanNextPlate()
    {
        // �ء� ��������ġ�� �Ʒ����⿡ �÷���Ʈ���ִ��� üũ
        if (Physics.Raycast(thePlayer.destPos, Vector3.down, out RaycastHit t_hitInfo, 1.1f))
        {
            if (t_hitInfo.transform.CompareTag("BasicPlate"))
            {
                BasicPlate t_plate = t_hitInfo.transform.GetComponent<BasicPlate>();
                if (t_plate.flag)
                {
                    t_plate.flag = false;
                    return true;
                }
            }
        }
            return false;
    }

    public int[] GetJudgementRecord()
    {
        return judgementRecord;
    }
    public void MissRecord()
    {
        judgementRecord[4]++; // ���� ���
        theStatus.ResetShieldCombo();
    }

    public void Initialized()
    {
        for (int i = 0; i < judgementRecord.Length; i++)
            judgementRecord[i] = 0;
    }
}
