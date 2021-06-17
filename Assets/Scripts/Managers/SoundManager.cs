using UnityEngine;

public enum Sounds
{
    PLAYER_JUMP,
    PLAYER_LAND,
    PLAYER_ACQUIRE_COIN,
    PLAYER_HIT,
    DOG_MONSTER_DIE,
    OCTOPUS_MOSTER_DIE,
    BOSS_MONSTER_DIE,
    ARROW_PIERCE,
    SKILL_BOMB_SHOT
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgm;
    public AudioSource effect;

    public AudioClip playerRun;
    public AudioClip playerJump;
    public AudioClip playerLand;
    public AudioClip acquireCoin;
    public AudioClip playerHit;
    public AudioClip dogMonsterDie;
    public AudioClip octopusMonsterDie;
    public AudioClip bossMonsterDie;
    public AudioClip arrowPierce;
    public AudioClip bombShot;



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        bgm = transform.Find("Bgm").GetComponent<AudioSource>();
        effect = transform.Find("Effect").GetComponent<AudioSource>();
    }

    public void PlaySound(Sounds sounds)
    {
        switch (sounds)
        {
            case Sounds.PLAYER_JUMP:
                break;
            case Sounds.PLAYER_LAND:
                break;
            case Sounds.PLAYER_ACQUIRE_COIN:
                break;
            case Sounds.PLAYER_HIT:
                effect.volume = 0.1f;
                effect.PlayOneShot(playerHit);
                break;
            case Sounds.DOG_MONSTER_DIE:
                break;
            case Sounds.OCTOPUS_MOSTER_DIE:
                break;
            case Sounds.BOSS_MONSTER_DIE:
                break;
            case Sounds.ARROW_PIERCE:
                break;
            case Sounds.SKILL_BOMB_SHOT:
                break;
            default:
                break;
        }
    }
}
