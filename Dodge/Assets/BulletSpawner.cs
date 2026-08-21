using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;     // 생성할 탄알의 원본 프리팹
    public float spawnRateMin = 0.5f;   // 최소 생성 주기
    public float spawnRateMax = 3f;     // 최대 생성 주기

    private Transform target;           // 발사할 대상
    private float spawnRate;            // 생성 주기
    private float timeAfterSpawn;       // 최근 생성 시점에서 지난 시간

    void Start()
    {
        // 누적 시간 초기화
        timeAfterSpawn = 0f;
        // 탄알 생성 간격 랜덤 지정
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        // PlayerController 찾아서 target에 할당
        target = FindFirstObjectByType<PlayerController>().transform;
    }

    void Update()
    {
        // 시간 누적
        timeAfterSpawn += Time.deltaTime;

        // 생성 주기 체크
        if (timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn = 0f;    // 누적 시간 리셋

            // target을 바라보는 bullet 생성
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.transform.LookAt(target);

            // 다음 생성 간격 랜덤 지정
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }

    }
}
