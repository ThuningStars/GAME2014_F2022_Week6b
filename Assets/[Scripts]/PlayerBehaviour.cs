using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Player Properties")]
    [SerializeField]
    public float speed = 2.0f;
    public Boundary boundary;
    public float verticalPosition;
    public float verticalSpeed = 10.0f;
    public bool usingMobileInput = false;

    [Header("Bullet Properties")]
    public Transform bulletSpawnPoint;
    public float fireRate = 0.2f;

    private BulletManager bulletManager;
    private Camera camera;
    private ScoreManager scoreManager;

    void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();

        camera = Camera.main;

        usingMobileInput = Application.platform == RuntimePlatform.Android || 
                           Application.platform == RuntimePlatform.IPhonePlayer;

        scoreManager = FindObjectOfType<ScoreManager>();

        InvokeRepeating("FireBullets", 0.0f, fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        if(usingMobileInput)
        {
            Mobilenput();
        }
        else
        {
            ConventionalInput();
        }

        Move();

        if(Input.GetKeyDown(KeyCode.K))
        {
            scoreManager.AddPoints(10);
        }
    }

    public void Mobilenput()
    {
        foreach(var touch in Input.touches)
        {
            var destination = camera.ScreenToWorldPoint(touch.position);
            transform.position =Vector2.Lerp(transform.position, destination, verticalSpeed * Time.deltaTime);
        }
    }

    public void ConventionalInput()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        transform.position += new Vector3(x, 0.0f, 0.0f);
    }

    public void Move()
    {
        // check bounds
        var clampedPosition = Mathf.Clamp(transform.position.x, boundary.min, boundary.max);
        transform.position = new Vector2(clampedPosition, verticalPosition);
    }

    void FireBullets()
    {
        var bullet = bulletManager.GetBullet(bulletSpawnPoint.position, BulletType.PLAYER);
    }
}
