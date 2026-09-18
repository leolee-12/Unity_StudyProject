using System.Collections;
using UnityEngine;

// 총을 구현
public class Gun : MonoBehaviour
{
    // 총의 상태를 표현하는 데 사용할 타입을 선언
    public enum State
    {
        Ready, // 발사 준비됨
        Empty, // 탄알집이 빔
        Reloading // 재장전 중
    }

    public Transform fireTransform;             // 탄알이 발사될 위치
    public ParticleSystem muzzleFlashEffect;    // 총구 화염 효과
    public ParticleSystem shellEjectEffect;     // 탄피 배출 효과

    private LineRenderer bulletLineRenderer;    // 탄알 궤적을 그리기 위한 렌더러
    private AudioSource gunAudioPlayer;         // 총 소리 재생기

    public State state { get; private set; }    // 현재 총의 상태
    public GunData gunData;                     // 총의 현재 데이터

    public int ammoRemain = 100;        // 남은 전체 탄알
    public int magAmmo;                 // 현재 탄알집에 남아 있는 탄알

    private float fireDistance = 50f;   // 사정거리
    private float lastFireTime;         // 총을 마지막으로 발사한 시점

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer = GetComponent<LineRenderer>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;

        state = State.Ready;

        lastFireTime = 0;
    }

    // 발사 시도
    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)    // 발사 가능한 상태 && 마지막 발사로부터 발사 간격 이상의 시간 지남
        {
            lastFireTime = Time.time;   // 마지막 총 발사 시점 갱신
            Shot();                     // 실제 발사 처리 실행
        }
    }

    // 실제 발사 처리
    private void Shot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {   // 레이가 물체와 충돌한 경우
            IDamageable target = hit.collider.GetComponent<IDamageable>();  // 충돌한 상대로부터 IDamageable 컴포넌트 가져오기 시도

            if (target != null)
            {   // 가져왔다면 데미지를 주기
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }

            hitPosition = hit.point;    // 레이 충돌 위치 저장
        }
        else
        {   // 레이가 충돌하지 않은 경우 : 최대 사정거리까지 날아갔을 때의 위치를 충돌 위치로 사용
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition));    // 발사 이펙트 재생

        magAmmo--;
        if (magAmmo <= 0)
        {   // 탄알이 없다면 총의 상태를 Empty로 갱신
            state = State.Empty;
        }

    }

    // 발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();                       // 총구 화염 효과 재생
        shellEjectEffect.Play();                        // 탄피 배출 효과 재생
        gunAudioPlayer.PlayOneShot(gunData.shotClip);   // 총격 소리 재생

        bulletLineRenderer.SetPosition(0, fireTransform.position);  // 라인 시작점 : 총구 위치
        bulletLineRenderer.SetPosition(1, hitPosition);             // 라인 끝점 : 입력으로 들어온 충돌 위치
        bulletLineRenderer.enabled = true;  // 라인 렌더러 활성화(궤적 그리기)

        yield return new WaitForSeconds(0.03f); // 0.03초 처리 대기 (코루틴)
        
        bulletLineRenderer.enabled = false; // 라인 렌더러 비활성화(궤적 지우기)
    }

    // 재장전 시도
    public bool Reload()
    {
        if (state == State.Reloading
            || ammoRemain <= 0
            || magAmmo >= gunData.magCapacity)
        {
            return false;
        }

        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;                                // 총을 재장전 상태로 변경
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);         // 재장전 소리 재생
      
        yield return new WaitForSeconds(gunData.reloadTime);    // 소요 시간만큼 대기

        int ammoToFill = gunData.magCapacity - magAmmo;         // 탄창에 채울 탄알 계산

        if (ammoRemain < ammoToFill)
        {   // 남은 탄알이 부족하다면 채울 탄알 수를 맞춰서 줄임
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;      // 탄창을 채우기
        ammoRemain -= ammoToFill;   // 채운만큼 남은 탄알에서 빼기

        state = State.Ready;        // 총을 발사 준비 상태로 변경
    }
}