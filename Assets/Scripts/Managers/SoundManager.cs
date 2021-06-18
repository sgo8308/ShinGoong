using UnityEngine;

public enum PlayerSounds
{
    PLAYER_RUN,
    PLAYER_JUMP,
    PLAYER_LAND,
    PLAYER_BUY,
    PLAYER_SELL,
    PLAYER_EQUIP,
    PLAYER_UNEQUIP,
    PLAYER_HIT,
    PLAYER_READY_ARROW,
    PLAYER_SHOOT_ARROW,
    
}

public enum NonPlayerSounds
{
    ACQUIRE_COIN,
    ACQUIRE_ARROW,
    OPEN_INVENTORY,
    LEVEL_UP,
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

    private AudioSource bgm;
    public AudioSource playerSound;
    private AudioSource nonPlayerSound;

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
        playerSound = transform.Find("PlayerSound").GetComponent<AudioSource>();
        nonPlayerSound = transform.Find("NonPlayerSound").GetComponent<AudioSource>();
    }

    public void PlayPlayerSound(PlayerSounds sounds)
    {
        playerSound.Stop();
        playerSound.loop = false;
        playerSound.mute = false;

        switch (sounds)
        {
            case PlayerSounds.PLAYER_RUN:
                playerSound.volume = 3f;
                playerSound.clip = playerRun;
                playerSound.loop = true;
                playerSound.Play();
                break;

            case PlayerSounds.PLAYER_JUMP:
                playerSound.PlayOneShot(playerJump, 1.5f);
                break;

            case PlayerSounds.PLAYER_LAND:
                playerSound.PlayOneShot(playerLand, 2.0f);
                break;

            case PlayerSounds.PLAYER_BUY:
                playerSound.PlayOneShot(playerBuy, 0.6f);
                break;

            case PlayerSounds.PLAYER_SELL:
                playerSound.PlayOneShot(playerSell, 0.6f);
                break;

            case PlayerSounds.PLAYER_EQUIP:
                playerSound.PlayOneShot(playerEquip, 0.6f);
                break;

            case PlayerSounds.PLAYER_UNEQUIP:
                playerSound.PlayOneShot(playerUnEquip, 0.6f);
                break;

            case PlayerSounds.PLAYER_HIT:
                playerSound.PlayOneShot(playerHit, 0.08f);
                break;

            case PlayerSounds.PLAYER_READY_ARROW:
                playerSound.volume = 0.5f;
                playerSound.clip = readyArrow;
                playerSound.Play();
                break;

            case PlayerSounds.PLAYER_SHOOT_ARROW:
                playerSound.PlayOneShot(shootArrow, 0.5f);
                break;
        }
    }

    public void PlayNonPlayerSound(NonPlayerSounds sounds)
    {
        switch (sounds)
        {
            case NonPlayerSounds.ACQUIRE_COIN:
                nonPlayerSound.PlayOneShot(acquireCoin, 1.3f);
                break;

            case NonPlayerSounds.ACQUIRE_ARROW:
                nonPlayerSound.PlayOneShot(acquireArrow);
                break;

            case NonPlayerSounds.OPEN_INVENTORY:
                nonPlayerSound.PlayOneShot(openInventory, 0.6f);
                break;

            case NonPlayerSounds.LEVEL_UP:
                nonPlayerSound.PlayOneShot(playerLevelUp, 1.5f);
                break;

            case NonPlayerSounds.DOG_MONSTER_DIE:
                //effect.volume = 0.3f
                nonPlayerSound.PlayOneShot(dogMonsterDie);
                break;

            case NonPlayerSounds.OCTOPUS_MOSTER_DIE:
                //effect.volume = 0.3f
                nonPlayerSound.PlayOneShot(octopusMonsterDie);
                break;
            case NonPlayerSounds.BOSS_MONSTER_DIE:
                //effect.volume = 0.3f
                nonPlayerSound.PlayOneShot(bossMonsterDie);
                break;

            case NonPlayerSounds.ARROW_PIERCE_PLATFORM:
                nonPlayerSound.PlayOneShot(arrowPiercePlatform, 0.5f);
                break;

            case NonPlayerSounds.ARROW_PIERCE_MONSTER:
                nonPlayerSound.PlayOneShot(arrowPierceMonster, 0.5f);
                break;

            case NonPlayerSounds.SKILL_BOMB_SHOT:
                //effect.volume = 0.3f
                nonPlayerSound.PlayOneShot(bombShot);
                break;

            default:
                break;
        }
    }

    public void StopPlayerSound()
    {
        playerSound.Stop();
    }

    public void StopNonPlayerSound()
    {
        nonPlayerSound.Stop();
    }

    public void MutePlayerSound()
    {
        playerSound.mute = true;
    }

    public void UnMutePlayerSound()
    {
        playerSound.mute = false;
    }
}
