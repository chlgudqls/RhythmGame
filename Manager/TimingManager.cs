using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    // 생성된 노트를 다 담는다 => 판정범위에 있는지 모든 노트를 비교한다
    public List<GameObject> boxNoteList = new List<GameObject>();

    int[] judgementRecord = new int[5];

    [SerializeField] Transform Center = null;
    // 판정범위 (Perfect, Cool, Good, Bad)
    [SerializeField] RectTransform[] timingRect = null;
    // 판정범위의 최소값x, 최대값y
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
        // ※ 내 코드는 멀쩡했지만 다른사람은 이 변수가 null을 참조하는 일이 생겨서 AudioManager가 먼저 실행될수있게 우선순위 높여줌
        // ※ 유니티에서 수정함 
        theAudio = AudioManager.instance;
        // 타이밍 박스 설정
        timingBoxs = new Vector2[timingRect.Length];

        for (int i = 0; i < timingRect.Length; i++)
        {
            // ※ 센터에 있는 x좌표값을 기준점으로 반절을 빼고더해서 범위를 구했다
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,
                              Center.localPosition.x + timingRect[i].rect.width / 2);
        }
    }

    // 리스트에 있는 노트들을 확인해서 판정 박스에 있는 노트를 찾는다
    public bool CheckTiming()
    {
        // 리스트 노트의 위치를 저장
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            float t_notePosX = boxNoteList[i].transform.localPosition.x;

            // 판정범위만큼 반복하고 어느 범위에 있는지 확인
            for (int x = 0; x < timingBoxs.Length; x++)
            {
                // 0부터니까 퍼펙트부터 검사하면 순차적으로 검사진행됨
                if(timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
                {
                    //Destroy(boxNoteList[i]);
                    // 노트 제거
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);
                    // 이펙트 연출
                    if(x < timingBoxs.Length -1)
                        theEffect.NoteHitEffect();
                    Debug.Log("Hit" + x);

                    // ※ 판정이 정상적으로 나오는 이 부분에서 메모리에 판정 값을 증감시키고 리턴
                    if (CheckCanNextPlate())
                    {
                        theScoreManager.IncreaseScore(x); // 점수 증가
                        theStage.ShowNextPlate(); // 판떼기 등장
                        theEffect.JudgementEffect(x); // 판정 연출
                        judgementRecord[x]++; // 판정 기록
                        theStatus.CheckShield(); // 쉴드 체크
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
    // 체크전에 계산값구했으니
    private bool CheckCanNextPlate()
    {
        // ※※ 목적지위치의 아래방향에 플레이트가있는지 체크
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
        judgementRecord[4]++; // 판정 기록
        theStatus.ResetShieldCombo();
    }

    public void Initialized()
    {
        for (int i = 0; i < judgementRecord.Length; i++)
            judgementRecord[i] = 0;
    }
}
