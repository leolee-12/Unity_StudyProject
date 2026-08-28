using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : MonoBehaviour {
    public static GameManager instance; // 싱글톤을 할당할 전역 변수

    public bool isGameover = false; // 게임 오버 상태
    public TextMeshProUGUI scoreText; // 점수 UI
    public GameObject gameoverUI; // 게임오버 UI

    private int score = 0; // 게임 점수

    // 게임 시작과 동시에 싱글톤을 구성
    void Awake() {
        if (instance == null)
        {   // instance가 비어 있다면 그곳에 자기 자신을 할당
            instance = this;
        }
        else
        {   // instance에 이미 다른 GameManager 오브젝트가 할당되어 있는 경우
            // : 둘 이상의 GameManager 존재 -> 경고 후 자신의 GameObject를 파괴
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }

    void Update() {
        if (isGameover && Input.GetMouseButtonDown(0))
        {   // 게임오버 상태에서 좌클릭 시 현재 씬 재시작
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AddScore(int newScore) {
        if (!isGameover)
        {
            score += newScore;
            scoreText.text = "Score : " + score;
        }
    }

    public void OnPlayerDead() {
        isGameover = true;
        gameoverUI.SetActive(true);
    }
}