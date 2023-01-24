using System.Linq;
using UnityEngine;

public class ThrowMeFX: MonoBehaviour
{
    [SerializeField] public GameObject textInfoPrefab;
    [SerializeField] public Transform textAnchor;
    [SerializeField] public AudioSource audioSource;
    private AudioClip[] clips;

    private GameManager game;

    private void Start()
    {
        clips = Resources.LoadAll("SFX", typeof(AudioClip)).Cast<AudioClip>().ToArray();
        game = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        game.OnDamageReceived += HandleDamage;
    }

    public void HandleDamage(float damage)
    {
        SpawnDamageText(damage);
        PlaySFX();
    }

    private void SpawnDamageText(float damage)
    {
        var text = SpawnText(Mathf.CeilToInt(damage).ToString());
        var t = Mathf.InverseLerp(0, 300, damage);
        text.fontScale *= Mathf.Lerp(1, 2, t);
    }

    private TextInfo SpawnText(string text)
    {
        var textInfo = GameObject.Instantiate(textInfoPrefab, textAnchor.position + Vector3.up * 2, Quaternion.identity).GetComponent<TextInfo>();
        textInfo.Init(text, Color.red);
        return textInfo;
    }

    public void PlaySFX()
    {
        if (clips.Length > 0)
        {
            audioSource.clip = clips[Random.Range(0, clips.Length - 1)];
            audioSource.Play();
        }
    }
}
