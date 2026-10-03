using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;         // 체력을 표시할 UI 슬라이더

    public AudioClip deathClip;         // 사망 소리
    public AudioClip hitClip;           // 피격 소리
    public AudioClip itemPickupClip;    // 아이템 습득 소리

    private Animator playerAnimator;        // 플레이어의 애니메이터
    private AudioSource playerAudioPlayer;  // 플레이어 소리 재생기

    private PlayerMovement playerMovement;  // 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter;    // 플레이어 슈터 컴포넌트

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        // LivingEntity.OnEnable() - 상태 초기화
        base.OnEnable();

        // 체력 슬라이더 활성화 및 초기화
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;

        // 플레이어 조작 관련 컴포넌트 활성화
        playerMovement.enabled = true;
        playerShooter.enabled = true;

        // 부활 기능을 고려하여 OnEnable에 구현
    }

    public override void RestoreHealth(float newHealth)
    {   // 체력 회복
        // LivingEntity.RestoreHealth() - 체력 증가
        base.RestoreHealth(newHealth);

        healthSlider.value = health;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {   // 데미지 처리
        if(!dead) { playerAudioPlayer.PlayOneShot(hitClip); }

        // LivingEntity.OnDamage() - 데미지 적용
        base.OnDamage(damage, hitPoint, hitDirection);

        healthSlider.value = health;
    }

    public override void Die()
    {   // 사망 처리
        // LivingEntity.Die() - 사망 적용
        base.Die();

        healthSlider.gameObject.SetActive(false);

        playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {   // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        if (!dead)
        {
            IItem item = other.GetComponent<IItem>();

            if (item != null)
            {
                item.Use(gameObject);

                playerAudioPlayer.PlayOneShot(itemPickupClip);
            }
        }
    }
}