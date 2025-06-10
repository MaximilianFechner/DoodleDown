using UnityEngine;

public class EnemySFXManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip enemySound;
    [SerializeField] private float pitchMin;
    [SerializeField] private float pitchMax;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (audioSource != null && GameManager.Instance.isSFXOn)
        {
            audioSource.clip = enemySound;
            audioSource.pitch = Random.Range(pitchMin, pitchMax);
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
