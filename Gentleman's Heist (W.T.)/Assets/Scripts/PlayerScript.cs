using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*

----Player Movement Script-----

What does this script do?
- Allows player to be controlled with WASD or Arrow Keys
    --> 8 ways of movement (left, right, up, down, and all diagonals)

- Allows boundaries to be set for player
    --> This needs to be replaced with colliders, it'll make life easier (will do later)

- Allows Walking animations to be determined
    --> Using a BlendTree in Animator

*/


public class PlayerScript : MonoBehaviour
{

    // The x and y values for the player to move to
    private float xMove;
    private float yMove;

    // Player health bar
    // public int maxHealth = 100;
    // public int currentHealth;
    public HealthBar healthBar;

    // Shooting & Crosshair/Cursor

    public GameObject crossHair;
    public Camera cam;

    public GameObject bulletPrefab;

    Vector2 mousePos;
    // public float bulletForce = 20f;

    public Transform firePoint;
    public float lookAngle;
    public Vector2 lookDirection;

    public ParticleSystem ps;



    // The speed the player moves
    public float speed = 10.0f;

    // evil sneak variable
    private float _sneak = 1;

    // The limits the player can move up, down, left, and right
    //Changed from 500,0,0,500 respectively
    private float yBoundTop = 999f;
    private float yBoundBottom = -999f;
    private float xBoundLeft = -999f;
    private float xBoundRight = 999f;


    // Reference to the Player's animator
    // --> Animator must include xChange, yChange, and walking as its parameters (i can set this up soon)
    public Animator animator;

    // Reference to the Player's rigid body
    public Rigidbody2D myBody;

    // Interactable Object within the Player's collision radius
    public GameObject currentInteractableObject = null;
    // Audio for walking sound
    public AudioSource walkingSound;

    public static bool detected = false;

    // Determines if Silent Weapon purchased for that round
    public bool noNoise = false;

    //Can't eat text notification
    [SerializeField] private Text noEat_text;

    // Start is called before the first frame update
    void Start()
    { 
        SyncPlayerData();
        Debug.Log(gameObject.GetComponentInChildren<EntitySound>().speedToRadiusRatio + " sound ratio");
        Debug.Log(PlayerData.GetSpeed()+ " speed");
        Debug.Log(PlayerData.GetMaxHealth()+ " max health ");
        walkingSound = gameObject.GetComponent<AudioSource>();
        noEat_text.enabled = false;
    }
    
    // Adds data on startup to PlayerData
    public void SyncPlayerData()
    {
        PlayerData.SetSoundCircle(gameObject.GetComponentInChildren<EntitySound>().gameObject);
        PlayerData.UpdateSoundCircle();
        PlayerData.SetBody(myBody);
        PlayerData.SetHealthBar(healthBar);
        PlayerData.SetAnimator(animator);
        PlayerData.SetCamera(cam);
        // PlayerData.SetEntitySound(gameObject.GetComponentInChildren<EntitySound>());
        if (PlayerData.GetDefaultMaxHealth() != 0)
        {
            PlayerData.SetMaxHealth(PlayerData.GetDefaultMaxHealth());
            PlayerData.FullHeal();
        }
        else
        {
            PlayerData.SetMaxHealth(100);
            PlayerData.SetCurrentHealth(100);
        }

        // If Player purchases food, change Player's max health for the round
        if(PlayerData.GetUpgradeFood()){
            PlayerData.AddMaxHealth(150);
            PlayerData.FullHeal();
            PlayerData.SetUpgradeFood();
        }

        if (PlayerData.GetDefaultSpeed() == 0)
        {
            PlayerData.SetDefaultSpeed(speed);

        }
        PlayerData.SetToDefaultSpeed();
        
        // If Player purchases a coffee, change Player's speed
        if(PlayerData.GetUpgradeCoffee()){
            PlayerData.AddSpeed(5.0f);
            PlayerData.SetUpgradeCoffee();
        }
        PlayerData.SetBulletForce(20.0f);
        PlayerData.SetPlayer(gameObject);
        PlayerData.SetPlayerScript(this);
        // if Player purchases silentWeapon
        if(PlayerData.getUpgradeSilentWeapon()){
            noNoise = true;
            PlayerData.SetUpgradeSilentWeapon();
        }
    }


 // Update is called once per frame
    void FixedUpdate()
    {
        // Gather the Keyboard Input from WASD or Arrow Keys
        // --> The x and y values for the player to move to
        xMove = Input.GetAxisRaw("Horizontal");;
        yMove = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _sneak = 2;
            if (Input.GetKeyDown(KeyCode.End))
            {
                // Dev speedrun for testing
                PlayerData.SetSpeed(30.0f);
                GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>()[0].pitch = 1.5f;
                gameObject.GetComponentInChildren<EntitySound>().gameObject.SetActive(false);
                // Destroy(gameObject.GetComponent<CircleCollider2D>());

            }
        }
        else
        {
            _sneak = 1;
        }

        // Crosshair/cursor movement
        Vector2 mosPos = Input.mousePosition.normalized;
        mosPos = cam.ScreenToWorldPoint(Input.mousePosition);
        crossHair.transform.position = new Vector2(mosPos.x, mosPos.y);
    

        // Keeping Player within the Boundaries
        // --> if player is about to move past the boundaries, set the move value to 0
        if(transform.position.x + xMove <= xBoundLeft || transform.position.x + xMove >= xBoundRight)
        {
            xMove = 0;
        }
        if(transform.position.y + yMove >= yBoundTop || transform.position.y + yMove<= yBoundBottom)
        {
            yMove = 0;
        }

        // setting movement regardless of input
        Vector2 movement = new Vector2(xMove, yMove).normalized;
        myBody.velocity = movement * PlayerData.GetSpeed() / _sneak;

        // if()
            
            
        // animation logic
        if(xMove != 0 || yMove != 0)
        {
            changeAnimation();
            animator.SetBool("walking", true); // set walking to true
            if (_sneak == 2){
                walkingSound.volume = .1f;
            }
            else{
                walkingSound.volume = .7f;
            }
            walkingSound.UnPause();
            
        }
        // If no movement, no walking animation is needed
        else
        {
            animator.SetBool("walking", false);
            walkingSound.Pause();
        }

        
        
    }
    void Update(){
        //Interacting with a pickup
        if (currentInteractableObject){
            currentInteractableObject.SendMessage("DoInteraction",currentInteractableObject.name);
        }
        // Shooting animation logic
        if(Input.GetKeyDown(KeyCode.Space)){
            animator.SetBool("shooting", true); // set shooting to true
        }
        else{
             animator.SetBool("shooting", false); // set shooting to false
        }
        
        Shooting();

    }
    

    // Updates the animation BlendTree
    void changeAnimation()
    {
            animator.SetFloat("xChange", xMove);
            animator.SetFloat("yChange", yMove);
    }

    // Player damage
    void takeDamage(int damage){
      PlayerData.TakeDamage(damage);
    }
    public void addHealth(int health){
        PlayerData.HealAmount(health);
    }

    // Marks an interactable object as the current interactable object when entering their collision area
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("InteractableObject")){
            ps.Play();
            currentInteractableObject = collision.gameObject;
            //Message for when player tries to pickup health on full health
            if (currentInteractableObject.name.StartsWith("Donut") || currentInteractableObject.name.StartsWith("Apple") && PlayerData.GetCurrentHealth() == PlayerData.GetMaxHealth()){
                noEat_text.enabled = true;
            }
        }
    }
    // Stay so the collider doesn't keep trying to find something while in a radius
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("InteractableObject")){
            currentInteractableObject = collision.gameObject;
        }
    }

    // Firepoint aiming(direction relative to cursor/crosshair)
    public void Shooting(){
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            animator.SetBool("shooting", true);
            lookDirection = (cam.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position).normalized;
            lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            firePoint.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f);
            Shoot();
        }
    }
    // Unmarks an interactable object as the current interactable object when exiting their collision area
   private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("InteractableObject")){
            currentInteractableObject = null;
            noEat_text.enabled = false;
        }
    }

    // Bullet init and shooting
    void Shoot(){
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
       Rigidbody2D bullBody = bullet.GetComponent<Rigidbody2D>();
       bullBody.AddForce(firePoint.up * PlayerData.GetBulletForce(), ForceMode2D.Impulse);
       if(!noNoise){
            PlayerData.GetSoundCircle().GetComponent<EntitySound>().SetSoundEvent(450, 30);
       }
    }
    
}
