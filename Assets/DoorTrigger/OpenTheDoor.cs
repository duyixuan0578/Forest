using UnityEngine;

public class OpenTheDoor : MonoBehaviour
{
    public Animator doorAnimator;

    // 当玩家角色【走进】大门台阶（绿色框）时
    private void OnTriggerEnter(Collider other)
    {
        // 播放开门动画
        doorAnimator.Play("Door_Open");
    }

    // 当玩家角色【离开】大门台阶（绿色框）时
    private void OnTriggerExit(Collider other)
    {
        // 播放关门动画
        doorAnimator.Play("Door_Close");
    }
}