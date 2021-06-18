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
                effect.PlayOneShot(playerJump);
                break;

            case Sounds.PLAYER_LAND:
                //effect.volume = 0.3f
                effect.PlayOneShot(playerLand);
                break;

            case Sounds.PLAYER_ACQUIRE_COIN:
                //effect.volume = 0.3f
                effect.PlayOneShot(acquireCoin);
                break;

            case Sounds.PLAYER_BUY:
                effect.volume = 0.6f;
                effect.PlayOneShot(playerBuy);
                break;

            case Sounds.PLAYER_SELL:
                effect.volume = 0.6f;
                effect.PlayOneShot(playerSell);
                break;

            case Sounds.PLAYER_EQUIP:
                effect.volume = 0.6f;
                effect.PlayOneShot(playerEquip);
                break;

            case Sounds.PLAYER_UNEQUIP:
                effect.volume = 0.6f;
                effect.PlayOneShot(playerUnEquip);
                break;

            case Sounds.PLAYER_HIT:
                effect.volume = 0.08f;
                effect.PlayOneShot(playerHit);
                break;

            case Sounds.PLAYER_OPEN_INVENTORY:
                effect.volume = 0.6f;
                effect.PlayOneShot(openInventory);
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
                effect.volume = 0.3f;
                effect.PlayOneShot(arrowPiercePlatform);
                break;

            case Sounds.ARROW_PIERCE_MONSTER:
                effect.volume = 0.1f;
                effect.PlayOneShot(arrowPierceMonster);
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
