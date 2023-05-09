using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject[] stageArray = null;
    GameObject currentStage;
    Transform[] stagePlates;

    [SerializeField] float offsetY = -3;
    [SerializeField] float plateSpeed = 10;

    int stepCount = 0;
    int totalPlateCount = 0;
    // ※ 시작하자마자 호출되는게 아닌 세팅을 호출할때, stepCount도 0으로 초기화
    // ※ 스테이지를 클리어후 다시 할때 초기화할게 엄청많다는거
    // ※ 클리어한스테이지는 remove를 시키고 setting에서 다시 생성시키기때문에 프리팹이 위력을 발휘했다 프리팹을 다시호출하는게 초기화랑같음
    public void RemoveStage()
    {
        if (currentStage != null)
            Destroy(currentStage);
    }
    public void SettingStage(int p_songNum)
    {
        stepCount = 0;

        currentStage = Instantiate(stageArray[p_songNum], Vector3.zero,Quaternion.identity);
        stagePlates = currentStage.GetComponent<Stage>().plates;
        totalPlateCount = stagePlates.Length;

        // ※ 시작할때 offsetY를 더해서 Plate들이 아래에서 시작하게함
        for (int i = 0; i < totalPlateCount; i++)
        {
            stagePlates[i].position = new Vector3(stagePlates[i].position.x, 
                                                  stagePlates[i].position.y + offsetY, 
                                                  stagePlates[i].position.z);
        }
    }



    public void ShowNextPlate()
    {
        if(stepCount < totalPlateCount)
            StartCoroutine(MovePlateCo(stepCount++));
            
    }
    IEnumerator MovePlateCo(int p_num)
    {
        // plate를 활성화시키기 위한 인덱스필요 - 플레이어의 발걸음 수 사용 - 그러기위해서 변수선언
        // ※ 변수 하나만들고 호출된 만큼 증감하게 된다
        stagePlates[p_num].gameObject.SetActive(true);

        // ※ 원상태로 돌아오기위해서 임시변수에 dest값을 저장
        Vector3 t_destPos = new Vector3(stagePlates[p_num].position.x, 
                                        stagePlates[p_num].position.y - offsetY, 
                                        stagePlates[p_num].position.z);
        while(Vector3.SqrMagnitude(stagePlates[p_num].position - t_destPos) >= 0.001f)
        {
            stagePlates[p_num].position = Vector3.Lerp(stagePlates[p_num].position, t_destPos, plateSpeed * Time.deltaTime);
            yield return null;
        }
        stagePlates[p_num].position = t_destPos;
    }
}
