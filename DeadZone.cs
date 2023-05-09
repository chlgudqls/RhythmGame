using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 함수를 호출 // 하긴 여기서 컴포넌트 가져와서 로직을 짤바에 플레이어에서 함수구현한거 가져와서 쓰는게 나음
            other.GetComponentInParent<PlayerController>().ResetFalling();
        }
    }
}
