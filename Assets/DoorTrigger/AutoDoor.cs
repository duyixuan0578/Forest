using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    [Header("=== 拖入总父物体 Door ===")]
    public Animator doorAnimator;

    // 当玩家走进触发区域
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule")
        {
            if (doorAnimator != null)
            {
                // 触发开门
                doorAnimator.SetTrigger("Open");
                Debug.Log("【木屋机关】玩家接近，大门自动开启！");
            }
        }
    }

    // 当玩家离开触发区域
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule")
        {
            if (doorAnimator != null)
            {
                // 触发关门
                doorAnimator.SetTrigger("Close");
                Debug.Log("【木屋机关】玩家离开，大门自动关闭！");
            }
        }
    }
}