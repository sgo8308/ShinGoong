using UnityEngine;

public enum Sounds
{
    PLAYER_JUMP,
    PLAYER_LAND,
    PLAYER_ACQUIRE_COIN,
    PLAYER_BUY,
    PLAYER_SELL,
    PLAYER_EQUIP,
    PLAYER_UNEQUIP,
    PLAYER_HIT,
    PLAYER_OPEN_INVENTORY,
    PLAYER_LEVEL_UP,
    PLAYER_READY_ARROW,
    PLAYER_SHOOT_ARROW,
    PLAYER_ACQUIRE_ARROW,
    DOG_MONSTER_DIE,
    OCTOPUS_MOSTER_DIE,
    BOSS_MONSTER_DIE,
    ARROW_PIERCE_PLATFORM,
    ARROW_PIERCE_MONSTER,
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
    public AudioClip playerBuy;
    public AudioClip playerSell;
    public AudioClip playerEquip;
    public AudioClip playerUnEquip;
    public AudioClip playerLevelUp;
    public AudioClip readyArrow;
    public AudioClip shootArrow;
    public AudioClip acquireArrow;
    public AudioClip acquireCoin;
    public AudioClip openInventory;
    public AudioClip playerHit;
    public AudioClip dogMonsterDie;
    public AudioClip octopusMonsterDie;
    public AudioClip bossMonsterDie;
    public AudioClip arrowPiercePlatform;
    public AudioClip arrowPierceMonster;
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
                //effect.volume = 0.3f
                effect.PlayOneShot(playerJump, 0.3f);
                break;

            case Sounds.PLAYER_LAND:
                //effect.volume = 0.3f
                effect.PlayOneShot(playerLand);
                break;

            case Sounds.PLAYER_ACQUIRE_COIN:
                effect.PlayOneShot(acquireCoin, 1.3f);
                break;

            case Sounds.PLAYER_BUY:
                effect.PlayOneShot(playerBuy, 0.6f);
                break;

            case Sounds.PLAYER_SELL:
                effect.PlayOneShot(playerSell, 0.6f);
                break;

            case Sounds.PLAYER_EQUIP:
                effect.PlayOneShot(playerEquip, 0.6f);
                break;

            case Sounds.PLAYER_UNEQUIP:
                effect.PlayOneShot(playerUnEquip, 0.6f);
                break;

            case Sounds.PLAYER_HIT:
                effect.PlayOneShot(playerHit, 0.08f);
                break;

            case Sounds.PLAYER_OPEN_INVENTORY:
                effect.PlayOneShot(openInventory, 0.6f);
                break;

            case Sounds.PLAYER_LEVEL_UP:
                effect.PlayOneShot(playerLevelUp, 1.5f);
                break;

            case Sounds.PLAYER_READY_ARROW:
                effect.volume = 1f;
                effect.clip = readyArrow;
                effect.Play();
                break;

            case Sounds.PLAYER_SHOOT_ARROW:
                effect.Stop();
                effect.PlayOneShot(shootArrow, 0.5f);
                break;

            case Sounds.PLAYER_ACQUIRE_ARROW:
                effect.volume = 0.6f;
                effect.PlayOneShot(acquireArrow);
                break;

            case Sounds.DOG_MONSTER_DIE:
                //effect.volume = 0.3f
                effect.PlayOneShot(dogMonsterDie);
                break;

            case Sounds.OCTOPUS_MOSTER_DIE:
                //effect.volume = 0.3f
                effect.PlayOneShot(octopusMonsterDie);
                break;
            case Sounds.BOSS_MONSTER_DIE:
                //effect.volume = 0.3f
                effect.PlayOneShot(bossMonsterDie);
                break;

            case Sounds.ARROW_PIERCE_PLATFORM:
                effect.PlayOneShot(arrowPiercePlatform, 0.5f);
                break;

            case Sounds.ARROW_PIERCE_MONSTER:
                effect.PlayOneShot(arrowPierceMonster, 0.5f);
                break;

            case Sounds.SKILL_BOMB_SHOT:
                //effect.volume = 0.3f
                effect.PlayOneShot(bombShot);
                break;

            default:
                break;
        }
    }
}
