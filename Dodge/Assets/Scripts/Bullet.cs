using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;            // 탄알 이동 속력
    private Rigidbody bulletRigidbody;  // 이동에 사용할 Rigidbody

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();

        // Rigidbody의 속도 = 앞쪽 방향 * 속력
        bulletRigidbody.linearVelocity = transform.forward * speed;

        // 3초 뒤 자신의 게임 오브젝트를 파괴
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter(Collider other) // 트리거 충돌 시 자동 실행
    {
        if (other.tag == "Player")
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            
            if (playerController != null)
            {
                playerController.Die();
            }
        }
    }
}
