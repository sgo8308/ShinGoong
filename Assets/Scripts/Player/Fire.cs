using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject arrowPrefab = null; 
    public GameObject ropeArrowPrefab = null;

    public GameObject arrowDirection = null; //화살의 위치값을 담을 변수
    private GameObject player = null;
    private PlayerMove playerMove;

    public float arrowSpeed = 50f;    //화살 속도
    public float arrowMaxPower = 1f;    //화살 Max Power
    public float ropeArrowSpeed = 15f;    //화살 속도

    Camera mainCamera = null; //카메라 변수

    public static float power = 0.0f;

    public static bool ropeArrowState = false;

    Vector2 mousePosition;

    void Start()
    {
        player = GameObject.Find("Player");
        playerMove = player.GetComponent<PlayerMove>();
        mainCamera = Camera.main;
        arrowDirection = player.transform.Find("ArrowDirection").gameObject;
    }

    void Update()
    {
        if (!playerMove.canMove || playerMove.isJumping || 
                playerMove.isRopeMoving || Inventory.instance.GetArrowCount() < 0) // 로프가 나가는 중인 것도 제외 시키기
            return;

        SetArrowDirection();

        if (Input.GetMouseButton(0))
        {
            ControlPower();
        }

        if (Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.R))
        {
            Invoke("ShootArrow", 0.1f);
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShootRopeArrow();
            }
        }
    }

    private void SetArrowDirection()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); //스크린상의 마우스좌표 -> 게임상의 2d 좌표로 치환
        Vector2 direction = new Vector2(mousePosition.x - arrowDirection.transform.position.x,
                                          mousePosition.y - arrowDirection.transform.position.y);   //마우스 좌표 - 화살 좌표 = 바라볼 방향

        arrowDirection.transform.right = direction;  //화살의 x축 방향을 '바라볼 방향'으로 정한다
        arrowDirection.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 0.3f); // 플레이어 목 근처에서 화살이 나가게  y값 조정
    }

    private void ControlPower()
    {
        power += Time.deltaTime * arrowSpeed;

        MainUI.instance.UpdateGaugeBarUI(power, arrowMaxPower);

        if (power > arrowMaxPower)
            power = arrowMaxPower;
    }

    private void ShootArrow()
    {
        GameObject t_arrow = Instantiate(arrowPrefab, arrowDirection.transform.position, arrowDirection.transform.rotation); //화살 생성
        t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * power * 1 / 2;  //화살 발사 속도 = x축 방향 * 파워 * 속도값

        if (power >= arrowMaxPower)
        {
            t_arrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
            t_arrow.GetComponent<Rigidbody2D>().velocity = t_arrow.transform.right * power * 1 / 3;  //화살 발사 속도 = x축 방향 * 파워 * 속도값
        }

        power = 0.0f;

        MainUI.instance.UpdateArrowCountUI();
        Inventory.instance.UseArrow();
    }

    private void ShootRopeArrow()
    {
        GameObject RopeArrow = Instantiate(ropeArrowPrefab, arrowDirection.transform.position, arrowDirection.transform.rotation); //화살 생성
        RopeArrow.GetComponent<Rigidbody2D>().gravityScale = 0; //Max Power일때 직사로 발사된다. 중력 0
        RopeArrow.GetComponent<Rigidbody2D>().velocity = RopeArrow.transform.right * ropeArrowSpeed * 1 / 3;  //화살 발사 속도 = x축 방향 * 파워 * 속도값

        ropeArrowState = true;
    }
}
