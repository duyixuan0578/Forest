using UnityEngine;

public class WaterReset : MonoBehaviour
{
    [Header("把岸上的 SpawnPoint 拖到这里")]
    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        // 针对 Starter Assets 的物体名字或 Tag 检查
        if (other.gameObject.name == "PlayerCapsule" || other.CompareTag("Player") || other.gameObject.name == "Capsule")
        {
            // 1. 尝试抓取玩家身上的 CharacterController Component
            CharacterController cc = other.GetComponent<CharacterController>();

            // 2. 如果没抓到，去它的父物体或者子物体上找
            if (cc == null) cc = other.GetComponentInParent<CharacterController>();
            if (cc == null) cc = other.GetComponentInChildren<CharacterController>();

            // 3. 传送前必须先关闭角色控制器，否则位置会被强行拉回水里
            if (cc != null)
            {
                cc.enabled = false;
            }

            // 4. 清空刚体的下落速度（解决老版 Unity 的兼容性写法）
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;       // 替换成了兼容旧版 Unity 的属性名
                rb.angularVelocity = Vector3.zero; // 清空旋转速度
            }

            // 5. 瞬间转移位置
            other.transform.position = spawnPoint.position;
            other.transform.rotation = spawnPoint.rotation;

            // 6. 转移完毕后，重新把角色控制器恢复启用
            if (cc != null)
            {
                cc.enabled = true;
            }

            Debug.Log("【系统】已强行解除 CharacterController 锁定，玩家安全重置！");
        }
    }
}