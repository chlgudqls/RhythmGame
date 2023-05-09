using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    // 오차가 적음 double
    double currentTime = 0d;


    [SerializeField] Transform tfNoteAppear = null;
    //[SerializeField] GameObject goNote = null;

    // ※ 그래서 list에 저장은 어디서하나 바로 생성되자마자 추가해버린다
    TimingManager theTimingManager;
    EffectManager theEffect;
    ComboManager theComboManager;

    private void Start()
    {
        theTimingManager = FindObjectOfType<TimingManager>();
        theEffect = FindObjectOfType<EffectManager>();
        theComboManager = FindObjectOfType<ComboManager>();
    }

    void Update()
    {
        if (GameManager.instance.isStartGame)
        {
            // 1초에 1씩증가
            currentTime += Time.deltaTime;

            if (currentTime >= 60d / bpm)
            {
                GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
                t_note.transform.position = tfNoteAppear.position;
                t_note.SetActive(true);
                //GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity);
                // 이미지와 텍스트 캔버스내에서만 보인다
                //t_note.transform.SetParent(this.transform);
                theTimingManager.boxNoteList.Add(t_note);
                currentTime -= 60d / bpm;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Note")) 
        {
            // ※ 이미지가 비활성화만 될뿐이라 Miss가 계속뜸  이미지가 활성화일때만 miss가 뜨도록 변경
            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                theTimingManager.MissRecord();
                theEffect.JudgementEffect(4);
                theComboManager.ResetCombo();
            }

            theTimingManager.boxNoteList.Remove(collision.gameObject);
            ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);
            //Destroy(collision.gameObject);

        }
    }
    public void RemoveNote()
    {
        GameManager.instance.isStartGame = false;

        for (int i = 0; i < theTimingManager.boxNoteList.Count; i++)
        {
            theTimingManager.boxNoteList[i].SetActive(false);
            ObjectPool.instance.noteQueue.Enqueue(theTimingManager.boxNoteList[i]);
        }
        // ※ 리스트의 인덱스를 다 제거한다는데 그럼 아예 사라지는게 아닌가
        theTimingManager.boxNoteList.Clear();
    }
}
