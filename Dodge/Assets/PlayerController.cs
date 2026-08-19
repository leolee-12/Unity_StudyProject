using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;   // 이동에 사용할 Rigidbody
    public float speed = 8f;            // 이동 속력
    void Start()
    {
        // 게임오브젝트에서 Rigidbody 컴포넌트를 찾아 변수에 할당
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 수평축과 수직축 입력값을 감지하여 저장
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        // 실제 이동 속도를 입력값과 이동 속력을 통해 결정
        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;

        // Vector3 속도를 실제 이동 속도 데이터를 넣어 생성
        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        // Rigidbody의 속도에 newVelocity를 할당
        playerRigidbody.linearVelocity = newVelocity;
    }

    public void Die()
    {
        // 자신의 게임 오브젝트를 비활성화
        gameObject.SetActive(false);
    }
}
