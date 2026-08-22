using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText; // 텍스트를 변경하지 않고 활성화 여부만 조작할 것이므로 GameObject 타입으로 선언하여 사용
    public Text timeText;   // 텍스트 내용을 변경할 것이므로 Text 컴포넌트 타입으로 선언하여 사용
    public Text recordText;

    private float surviveTime;  // 생존 시간
    private bool isGameover;    // 게임오버 상태

    void Start()
    {
        surviveTime = 0f;
        isGameover = false;
    }

    void Update()
    {
        if (!isGameover)
        {
            surviveTime += Time.deltaTime;

            timeText.text = "Time : " + (int)surviveTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    public void EndGame()
    {
        isGameover = true;
        gameoverText.SetActive(true);

        float bestTime = PlayerPrefs.GetFloat("BestTime");

        if (surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        recordText.text = "Best Time: " + (int)bestTime;
    }
}