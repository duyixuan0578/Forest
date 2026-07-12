using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class FishingSystem : MonoBehaviour
{
    [Header("=== UI 配置 ===")]
    public GameObject tipText;       // 提示“按E钓鱼”的UI
    public TextMeshProUGUI statusText; // 用于显示状态的文本
    public Slider qteSlider;         // QTE 滑动条

    [Header("=== QTE 参数 ===")]
    public float qteWindowDuration = 1.5f; // 玩家必须在1.5秒内按下E

    private bool isPlayerInZone = false;   // 玩家是否在桥边
    private bool isFishing = false;        // 是否正在钓鱼中
    private bool isQTEActive = false;      // 是否进入了按键判定时间

    // 奖励列表
    private string[] lootTable = { "大鲫鱼", "草鱼", "破旧的皮鞋", "易拉罐", "小金鱼", "水草" };

    void Start()
    {
        if (tipText != null) tipText.SetActive(false);
        if (statusText != null) statusText.text = "";
        if (qteSlider != null) qteSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        // 只有在【区域内】且【完全没有在钓鱼】时，按E才能开始
        if (isPlayerInZone && !isFishing && Input.GetKeyDown(KeyCode.E))
        {
            StartFishing();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule")
        {
            isPlayerInZone = true;
            // 如果没在钓鱼结算期，才显示提示
            if (!isFishing && tipText != null)
                tipText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "PlayerCapsule")
        {
            isPlayerInZone = false;
            if (tipText != null) tipText.SetActive(false);

            // 离开区域，立刻强行停止钓鱼的所有动作
            StopAllCoroutines();
            ResetFishingState();
            if (statusText != null) statusText.text = "";
        }
    }

    void StartFishing()
    {
        isFishing = true;
        if (tipText != null) tipText.SetActive(false);
        if (statusText != null) statusText.text = "正在静静等待鱼儿上钩...";

        float waitTime = Random.Range(2f, 5f);
        StartCoroutine(WaitForFishRoutine(waitTime));
    }

    IEnumerator WaitForFishRoutine(float time)
    {
        yield return new WaitForSeconds(time);

        isQTEActive = true;
        if (statusText != null) statusText.text = "鱼咬钩了！！！";

        if (qteSlider != null)
        {
            qteSlider.gameObject.SetActive(true);
            qteSlider.value = 1f;
        }

        float elapsed = 0f;
        bool playerPressed = false;

        while (elapsed < qteWindowDuration)
        {
            elapsed += Time.deltaTime;

            if (qteSlider != null)
            {
                qteSlider.value = 1f - (elapsed / qteWindowDuration);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                playerPressed = true;
                break;
            }

            yield return null;
        }

        if (qteSlider != null) qteSlider.gameObject.SetActive(false);
        isQTEActive = false; // 结束按键窗口

        if (playerPressed)
        {
            FishingSuccess();
        }
        else
        {
            FishingFail();
        }
    }

    void FishingSuccess()
    {
        int randomIndex = Random.Range(0, lootTable.Length);
        string reward = lootTable[randomIndex];

        if (statusText != null) statusText.text = $"成功！你钓到了：{reward}";
        Debug.Log($"【钓鱼日志】成功钓到: {reward}");

        // 开启延迟清空协程
        StartCoroutine(ResetAfterDelay(3f));
    }

    void FishingFail()
    {
        if (statusText != null) statusText.text = "啊哦，动作慢了，鱼儿脱钩溜走了...";
        Debug.Log("【钓鱼日志】钓鱼超时失败。");

        // 开启延迟清空协程
        StartCoroutine(ResetAfterDelay(3f));
    }

    // 核心修正：统一由这个协程在 3 秒后清空文字并把钓鱼状态重置
    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        ResetFishingState();

        if (statusText != null) statusText.text = "";
    }

    void ResetFishingState()
    {
        isFishing = false;
        isQTEActive = false;
        if (qteSlider != null) qteSlider.gameObject.SetActive(false);

        // 只有当玩家还在桥边时，才重新恢复显示“按E开始钓鱼”
        if (isPlayerInZone && tipText != null)
            tipText.SetActive(true);
    }
}