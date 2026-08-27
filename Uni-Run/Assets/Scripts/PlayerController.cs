using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
   public AudioClip deathClip; // 사망시 재생할 오디오 클립
   public float jumpForce = 700f; // 점프 힘

   private int jumpCount = 0; // 누적 점프 횟수(바닥에 닿으면 0으로 리셋)
   private bool isGrounded = false; // 바닥에 닿았는지 나타냄
   private bool isDead = false; // 사망 상태

   private Rigidbody2D playerRigidbody;
   private Animator animator;
   private AudioSource playerAudio;

   private void Start() {
       // 초기화
       playerRigidbody = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
       playerAudio = GetComponent<AudioSource>();
   }

   private void Update() {
        if (isDead)
        {
            // 사망 시 갱신 작업 없이 종료

            return;
        }

        if (Input.GetMouseButtonDown(0) && jumpCount < 2)
        {   // 마우스 좌클릭 시 && 최대 점프 횟수 도달하지 않았을 때
            jumpCount++;
            playerRigidbody.linearVelocity = Vector2.zero;  // 점프 직전에 속도를 0으로 변경
            playerRigidbody.AddForce(new Vector2(0, jumpForce));    // 위쪽으로 힘을 주기
            playerAudio.Play();
        }
        else if (Input.GetMouseButtonUp(0) && playerRigidbody.linearVelocity.y > 0)
        {   // 마우스 좌클릭을 떼는 순간 && 속도의 y값이 양수라면(위로 상승 중)
            playerRigidbody.linearVelocity *= 0.5f; // 속도를 절반으로
        }

        animator.SetBool("Grounded", isGrounded);
   }

   private void Die() {
        animator.SetTrigger("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerRigidbody.linearVelocity = Vector2.zero;
        
        isDead = true;
   }

   private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Dead" && !isDead)
        {
            Die();
        }
    }

   private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts[0].normal.y > 0.7f)
        { // 어떤 콜라이더와 닿았으며, 충돌 표면이 위쪽을 보고 있으면 (y = 0.7f일 때 대략 45도 경사로 위를 향함)
            isGrounded = true;
            jumpCount = 0;
        }
    }

   private void OnCollisionExit2D(Collision2D collision) {
        isGrounded = false;
    }
}