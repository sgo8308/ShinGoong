using UnityEngine;
using UnityEngine.SceneManagement;

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
    CLICK,
    MONSTER_DIE,
    ARROW_PIERCE_PLATFORM,
    ARROW_PIERCE_MONSTER,
    SKILL_BOMB_SHOT,
    TELEPORT
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource bgm;
    public AudioSource playerSound;
    private AudioSource nonPlayerSound;
    private AudioSource windSound;

    public AudioClip shelterSceneMusic;
    public AudioClip stage1SceneMusic;
    public AudioClip bossSceneMusic;

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
    public AudioClip click;
    public AudioClip playerHit;
    public AudioClip monsterDie;
    public AudioClip arrowPiercePlatform;
    public AudioClip arrowPierceMonster;
    public AudioClip bombShot;
    public AudioClip shelterWindSound;
    public AudioClip stageWindSound;
    public AudioClip teleport;

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

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += PlayWindSound;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += PlayBGM;
    }

    void Initialize()
    {
        bgm = transform.Find("Bgm").GetComponent<AudioSource>();
        playerSound = transform.Find("PlayerSound").GetComponent<AudioSource>();
        nonPlayerSound = transform.Find("NonPlayerSound").GetComponent<AudioSource>();
        windSound = transform.Find("WindSound").GetComponent<AudioSource>();
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
                playerSound.PlayOneShot(playerHit, 0.3f);
                break;

            case PlayerSounds.PLAYER_READY_ARROW:
                playerSound.volume = 0.5f;
                playerSound.clip = readyArrow;
                playerSound.Play();
                break;

            case PlayerSounds.PLAYER_SHOOT_ARROW:
                playerSound.PlayOneShot(shootArrow, 0.25f);
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

            case NonPlayerSounds.CLICK:
                nonPlayerSound.PlayOneShot(openInventory, 0.6f);
                break;

            case NonPlayerSounds.LEVEL_UP:
                nonPlayerSound.PlayOneShot(playerLevelUp, 1.5f);
                break;

            case NonPlayerSounds.MONSTER_DIE:
                nonPlayerSound.PlayOneShot(monsterDie, 0.6f);
                break;

            case NonPlayerSounds.ARROW_PIERCE_PLATFORM:
                nonPlayerSound.PlayOneShot(arrowPiercePlatform, 0.5f);
                break;

            case NonPlayerSounds.ARROW_PIERCE_MONSTER:
                nonPlayerSound.PlayOneShot(arrowPierceMonster, 0.25f);
                break;

            case NonPlayerSounds.SKILL_BOMB_SHOT:
                nonPlayerSound.PlayOneShot(bombShot, 0.7f);
                break;
            case NonPlayerSounds.TELEPORT:
                nonPlayerSound.PlayOneShot(teleport, 0.7f);
                break;

            default:
                break;
        }
    }

    public void PlayBGM(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "ShelterScene":
                bgm.clip = shelterSceneMusic;
                break;

            case "Stage1Scene":
                bgm.clip = stage1SceneMusic;
                break;

            case "BossScene":
                bgm.clip = bossSceneMusic;
                
                break;
            default:
                break;
        }

        bgm.Play();
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

    public void PlayWindSound(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "ShelterScene":
                windSound.clip = shelterWindSound;
                windSound.Play();
                break;

            case "Stage1Scene":
                windSound.clip = stageWindSound;
                windSound.Play();
                break;

            case "BossScene":
                windSound.clip = stageWindSound;
                windSound.Play();
                break;

            default:
                break;
        }
    }
}
