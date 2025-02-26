using System;
using System.Collections;
using TMPro;
using Unity.Notifications.Android;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool haveMoney;
    public GameObject InShopAlertNoMoney;
    public Animator anim;
    public AudioSource playerGetDamage;
    public AudioSource healthSound;
    public AudioSource coinsSound;
    public Joystick joystick;
    private Rigidbody2D _rigidbody;
    public TextMeshProUGUI HPUIAmount;
    public TextMeshProUGUI CoinsUIAmount;
    private float horizontalMove;
    private float verticalmove;
    public Vector3 respawnPoint;
    public float MovementSpeed = 10;
    public bool isCollide = false;
    public float Hitpoints;
    public float MaxHitpoints;
    public float coins = 0;
    public float playerHitStrength = 2;
    public GameObject Controlls1;
    public GameObject Controlls2;
    public GameObject Controlls3;
    public GameObject Controlls4;
    public GameObject Controlls5;
    public GameObject Controlls6;
    public GameObject Controlls7;
    public GameObject DeathScreen;
    public GameObject ShopScreen;
    public bool isCollEnemy = false;
    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = 0.139f;	// How much to smooth out the movement

    void Start()
    {
        
        haveMoney = false;
        InShopAlertNoMoney.SetActive(false);
        anim = GetComponent<Animator>();
        Application.targetFrameRate = 60;
        _rigidbody = GetComponent<Rigidbody2D>();
        coins = 0;
        respawnPoint = transform.position;
        DeathScreen.gameObject.SetActive(false);
        Controlls1.gameObject.SetActive(false);
        Controlls2.gameObject.SetActive(false);
        Controlls3.gameObject.SetActive(false);
        Controlls4.gameObject.SetActive(false);
        Controlls5.gameObject.SetActive(false);
        Controlls6.gameObject.SetActive(false);
        Controlls7.gameObject.SetActive(false);
        DeathScreen.gameObject.SetActive(false);
        ShopScreen.gameObject.SetActive(false);
        MaxHitpoints = 10;
        Hitpoints = 10;

        //~TODO za ka�dym razem przed kompilacj� zresetwoa� Prefs
     //   coins = PlayerPrefs.GetFloat("playerCoins");
         //Hitpoints = PlayerPrefs.GetFloat("playerHealth");
      //  MaxHitpoints = PlayerPrefs.GetFloat("playerMaxHealth");
        Hitpoints = Hitpoints + 99999999999;

    }

    public void FixedUpdate()
    {
        float movementHorizontal = joystick.Horizontal;
        bool isGrounded = GetComponent<JumpButton>().isGrounded;

        if (!Mathf.Approximately(0, movementHorizontal))
            transform.rotation = movementHorizontal < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;


        if (movementHorizontal >= 0.2f)
        {

            //_rigidbody.AddForce(Vector2.right * MovementSpeed);
            Vector3 targetVelocity = new Vector2(movementHorizontal * 15f, _rigidbody.velocity.y);
            _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);


            if (isGrounded == false)
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isRunning", false);

            }
            horizontalMove = MovementSpeed;
            anim.SetBool("isRunning", true);
        }
        else if (movementHorizontal <= -0.2f)
        {
            //_rigidbody.AddForce(Vector2.right * -MovementSpeed);
            Vector3 targetVelocity = new Vector2(movementHorizontal * 15f, _rigidbody.velocity.y);
            _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);


            if (isGrounded == false)
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isRunning", false);

            }
            horizontalMove = -MovementSpeed;
            anim.SetBool("isRunning", true);
        }
        else
        {
            horizontalMove = 0;
            anim.SetBool("isRunning", false);
        }


        // transform.position += MovementSpeed * Time.deltaTime * new Vector3(movementHorizontal, 0, 0);

    }

    void Update()
    {
        m_MovementSmoothing = 0.139f;
        _rigidbody.gravityScale = 4.85f;
        //~TODO za ka�dym razem przed kompilacj� zresetwoa� Prefs
        // PlayerPrefs.SetFloat("playerCoins", coins);
        //   PlayerPrefs.SetFloat("playerHealth", Hitpoints);
        // PlayerPrefs.SetFloat("playerMaxHealth", MaxHitpoints);

        if (coins <= 0) { }
        else
        {
            haveMoney = true;
        }
        HPUIAmount.text = Convert.ToString(Hitpoints + " / " + MaxHitpoints);
        CoinsUIAmount.text = Convert.ToString(coins);
        if(Hitpoints > MaxHitpoints)
        {
            Hitpoints = MaxHitpoints;
        }


        if (Hitpoints <= 0)
        {
            DeathScreen.gameObject.SetActive(true);
            Controlls1.gameObject.SetActive(false);
            Controlls2.gameObject.SetActive(false);
            Controlls3.gameObject.SetActive(false);
            Controlls4.gameObject.SetActive(false);
            Controlls5.gameObject.SetActive(false);
            Controlls6.gameObject.SetActive(false);
            Controlls7.gameObject.SetActive(false);
            ShopScreen.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
    public void GetCoins(float addMoney)
    {
        coinsSound.Play();
        coins += addMoney;
    }
    public void LoseCoins(float loseMoney)
    {
        coins -= loseMoney;
    }

    public void TakeHit(float damage)
    {
        playerGetDamage.Play();
        Hitpoints -= damage;
    }
    public void GetHealth(float damage)
    {
        healthSound.Play();
        Hitpoints += damage;
    }
    public void UpgradeMaxHealth(float damage)
    {
        MaxHitpoints += damage;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FallDetector")
        {
            TakeHit(1);
            transform.position = respawnPoint;
        }
        else if (collision.gameObject.tag == "CheckPoint")
        {
            isCollide = true;
            respawnPoint = transform.position;
        }

        if(collision.gameObject.tag == "Merchant")
        {
            Controlls1.gameObject.SetActive(false);
            Controlls2.gameObject.SetActive(false);
            Controlls3.gameObject.SetActive(false);
            Controlls4.gameObject.SetActive(false);
            Controlls5.gameObject.SetActive(true);
            Controlls6.gameObject.SetActive(true);
            Controlls7.gameObject.SetActive(true);
            ShopScreen.gameObject.SetActive(true);
            Time.timeScale = 0;
            horizontalMove = 0;
        }
    }

    public void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "Merchant")
        {
            Controlls1.gameObject.SetActive(true);
            Controlls2.gameObject.SetActive(true);
            Controlls3.gameObject.SetActive(true);
            Controlls4.gameObject.SetActive(true);
            Controlls5.gameObject.SetActive(true);
            Controlls6.gameObject.SetActive(true);
            Controlls7.gameObject.SetActive(true);
            ShopScreen.gameObject.SetActive(false);
        };
    }

    /// <summary>
    /// Shop Behaviours
    /// </summary>
    /// <returns>buying methods</returns>
    public bool GetHaveMoney()
    {
        return haveMoney;
    }

    public void BuyHP()
    {
        if(coins <= 4)
        {
            InShopAlertNoMoney.SetActive(true);
        }
        else
        {
            LoseCoins(5);
            GetHealth(50);
            InShopAlertNoMoney.SetActive(false);
        }
    }

    public void BuyMaxHP()
    {
        if (coins <= 19)
        {
            InShopAlertNoMoney.SetActive(true);
        }
        else
        {
            healthSound.Play();
            LoseCoins(20);
            UpgradeMaxHealth(5);
            InShopAlertNoMoney.SetActive(false);
        }
    }

}
