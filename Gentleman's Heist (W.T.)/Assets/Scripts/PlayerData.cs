using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

/*
 * WARNING: PlayerData is now saved. Any new attributes may not be remembered unless you add them to the save OR make Gabe do it
 */
public static class PlayerData
{
    
    private static int _playerMoney;
    private static int _playerScore;
    
    private static string _playerName;
    private static float _soundBonus = 0f;
    
    private static float _playerSpeed;
    private static float _playerDefaultSpeed;
    private static int _playerMaxHealth;
    private static int _defaultMaxHealth;
    private static int _playerCurrentHealth;
    //turns out the player is destroyed in the upgrade screen so changes can't be nicely affected (at that time)
    private static GameObject _playerSoundCircle;

    private static int _level = 0;

    private static int _diamonds;

    private static int _keys;

    private static Animator _playerAnimator;
    private static Rigidbody2D _playerBody;
    private static Camera _playerCamera;
    private static PlayerScript _playerScript;
    private static GameObject _player;
    private static HealthBar _playerHealthBar;
    private static float _playerSoundRatio = 8;
    
    private static float _bulletForce;

    private static bool _upgradeCoffee;
    private static bool _upgradeFood;
    private static bool _upgradeSilentWeapon;

    private static bool _upgradeAlarmD = false;
    
    // unsaved as of now
    private static float _playerDefaultSoundRatio = 8;
    
    /*
     * Following clear Methods need to stay updated if you want everything to work
     * Please add any additions you make to PlayerData to them as well (unless you dont want them cleared (ie: persistent upgrades)
     */
    public static void ClearAll()
    {
        Debug.Log("PlayerData Cleared");
        _playerMoney = 0;
        _playerScore = 0;
        _playerName = "";
        _playerSpeed = 0;
        _playerDefaultSpeed = 0; // keep for permanent 
        _playerMaxHealth = 0;
        _playerCurrentHealth = 0;
        _level = 0;
        _diamonds = 0;
        _keys = 0;
        _soundBonus = 0; // keep for permanent
        _defaultMaxHealth = 0; // keep for permanent

        _playerSoundCircle = null;
        _playerAnimator = null;
        _playerBody = null;
        _playerCamera = null;
        _playerScript = null;
        _player = null;
        _playerHealthBar = null;
        
        _playerSoundRatio = 8; //as it currently stands, this will always be the default
        _bulletForce = 0;
        
        _upgradeCoffee = false;
        _upgradeFood = false;
        _upgradeAlarmD = false;
        _upgradeSilentWeapon = false;
    }

    public static void ClearAllButPermanents()
    {
        Debug.Log("PlayerData Cleared except for permanent");
        _playerMoney = 0;
        _playerScore = 0;
        _playerName = "";
        _playerSpeed = 0;
        // _playerDefaultSpeed = 0; // keep for permanent 
        _playerMaxHealth = 0;
        _playerCurrentHealth = 0;
        _level = 0;
        _diamonds = 0;
        _keys = 0;
        // _soundBonus = 0; // keep for permanent
        // _defaultMaxHealth = 0; // keep for permanent

        _playerSoundCircle = null;
        _playerAnimator = null;
        _playerBody = null;
        _playerCamera = null;
        _playerScript = null;
        _player = null;
        _playerHealthBar = null;
        
        // _playerSoundRatio = 8; //as it currently stands, this will always be the default
        _bulletForce = 0;
        
        _upgradeCoffee = false;
        _upgradeFood = false;
        _upgradeAlarmD = false;
        _upgradeSilentWeapon = false;
    }

    public static int GetMoney()
    {
        return _playerMoney;
    }
    public static void AddMoney(int toAdd)
    {
        _playerMoney += toAdd;
    }
    public static void SubMoney(int toSub)
    {
        _playerMoney -= toSub;
    }
    public static void SetMoney(int toSet)
    {
        _playerMoney = toSet;
    }

    public static int GetScore()
    {
        return _playerScore;
    }
    public static void AddScore(int toAdd)
    {
        _playerScore += toAdd;
    }
    public static void SetScore(int toSet)
    {
        _playerScore = toSet;
    }

    public static string GetName()
    {
        return _playerName;
    }
    public static void SetName(string toSet)
    {
        _playerName = toSet;
    }

    public static float GetSpeed()
    {
        return _playerSpeed;
    }
    public static void AddSpeed(float toAdd)
    {
        _playerSpeed += toAdd;
    }
    public static void SubSpeed(float toSub)
    {
        _playerSpeed -= toSub;
    }
    public static void SetSpeed(float toSet)
    {
        _playerSpeed = toSet;
    }
    public static float GetDefaultSpeed()
    {
        return _playerDefaultSpeed;
    }
    public static void SetDefaultSpeed(float toSet)
    {
        _playerDefaultSpeed = toSet;
    }
    public static void PermanentIncreaseSpeed(float amountIncrease)
    {
        _playerDefaultSpeed += amountIncrease;
    }
    public static void SetToDefaultSpeed()
    {
        _playerSpeed = _playerDefaultSpeed ;
    }

    public static int GetMaxHealth()
    {
        return _playerMaxHealth;
    }
    public static void AddMaxHealth(int toAdd)
    {
        _playerMaxHealth += toAdd;
        UpdateHealthBar();
    }
    public static void SubMaxHealth(int toSub)
    {
        _playerMaxHealth -= toSub;
        UpdateHealthBar();
    }
    public static void SetMaxHealth(int toSet)
    {
        if (_defaultMaxHealth == 0)
        {
            _defaultMaxHealth = toSet;
        }
        _playerMaxHealth = toSet;
        UpdateHealthBar();
    }

    public static void PermanentIncreaseMaxHealth(int amountIncrease)
    {
        _defaultMaxHealth += amountIncrease;
    }

    public static int GetDefaultMaxHealth()
    {
        return _defaultMaxHealth;
    }

    public static void SetDefaultMaxHealth(int toSet)
    {
        _defaultMaxHealth = toSet;
    }

    public static int GetCurrentHealth()
    {
        return _playerCurrentHealth;
    }
    public static void SetCurrentHealth(int toSet)
    {
        _playerCurrentHealth = toSet;
        UpdateHealthBar();
    }
    public static void TakeDamage(int damageAmount)
    {
        if (_playerCurrentHealth - damageAmount < 0)
        {
            _playerCurrentHealth = 0;
        }
        else
        {
            _playerCurrentHealth -= damageAmount;
        }
        UpdateHealthBar();
    }
    public static void HealAmount(int healAmount)
    {
        if (_playerCurrentHealth + healAmount > _playerMaxHealth)
        {
            _playerCurrentHealth = _playerMaxHealth;
        }
        else
        {
            _playerCurrentHealth += healAmount;
        }
        UpdateHealthBar();
    }
    public static void FullHeal()
    {
        _playerCurrentHealth = _playerMaxHealth;
        UpdateHealthBar();
    }

    public static float GetBulletForce()
    {
        return _bulletForce;
    }
    public static void SetBulletForce(float newBulletForce)
    {
        _bulletForce = newBulletForce;
    }

    public static Animator GetAnimator()
    {
        return _playerAnimator;
    }
    public static void SetAnimator(Animator animator)
    {
        _playerAnimator = animator;
    }

    public static Rigidbody2D GetBody()
    {
        return _playerBody;
    }
    public static void SetBody(Rigidbody2D toSet)
    {
        _playerBody = toSet;
    }
    public static Vector3 Velocity()
    {
        return _playerBody.velocity;
    }

    public static Camera GetCamera()
    {
        return _playerCamera;
    }

    public static void SetCamera(Camera newCam)
    {
        _playerCamera = newCam;
    }

    public static GameObject GetPlayer()
    {
        return _player;
    }
    public static void SetPlayer(GameObject toSet)
    {
        _player = toSet;
    }

    public static PlayerScript GetPlayerScript()
    {
        return _playerScript;
    }
    public static void SetPlayerScript(PlayerScript toSet)
    {
        _playerScript = toSet;
    }

    public static HealthBar GetHealthBar()
    {
        return _playerHealthBar;
    }
    public static void SetHealthBar(HealthBar toSet)
    {
        _playerHealthBar = toSet;
    }
    public static void UpdateHealthBar()
    {
        _playerHealthBar.setHealth(GetCurrentHealth());
        // _playerHealthBar.setMaxHealth(GetMaxHealth());
    }

    public static int GetLevel()
    {
        return _level;
    }
    public static void SetLevel(int toSet)
    {
        _level = toSet;
    }

    public static void NextLevel()
    {
        _level += 1;
    }
    public static int GetDiamonds(){
        return _diamonds;
    }

    public static void SetDiamonds(){
        _diamonds++;
    }

    // only slight shade joel ;)
    public static void ActuallySetDiamonds(int toSet)
    {
        _diamonds = toSet;
    }

     public static int GetKeys(){
        return _keys;
    }

    public static void SetKeys(){
        _keys++;
    }

    // only slight shade joel ;)
    public static void ActuallySetKeys(int toSet)
    {
        _keys = toSet;
    }

    public static void ChangeKeys(int toSet) {
        _keys = toSet;
    }

    public static void SetSoundCircle(GameObject toSet)
    {
        _playerSoundCircle = toSet;
    }

    public static GameObject GetSoundCircle()
    {
        return _playerSoundCircle;
    }

    // public static void PermanentLowerSilentShoes(float amountLower)
    // {
    //     _playerDefaultSoundRatio -= amountLower;
    // }

    public static void SetSilentShoes(float newRatio)
    {
        // ugly but easy way to assign the default without making setters getter etc for default
        if (_playerDefaultSoundRatio == 0)
        {
            _playerDefaultSoundRatio = _playerSoundRatio;
        }
        _playerSoundRatio = newRatio;
        // _entitySound.transitionDivs = newRatio;
    }
    // public static void AddSilentShoes(float newRatio){
    //     if (_playerSoundRatio <= 1){
    //         if (_playerDefaultSoundRatio == 0)
    //         {
    //             _playerDefaultSoundRatio = _playerSoundRatio;
    //         }
    //         _playerSoundRatio = newRatio;
    //     }
    // }

    public static void RemoveSilentShoes()
    {
        if (_playerDefaultSoundRatio != 0)
        {
            _playerSoundRatio = _playerDefaultSoundRatio;
        }
    }

    public static float GetDefaultSoundRatio()
    {
        return _playerDefaultSoundRatio;
    }
    public static float GetSoundRatio()
    {
        return _playerSoundRatio;
    }
    
    // public static void SetEntitySound(EntitySound toSet)
    // {
    //     _entitySound = toSet;
    // } 

    public static void SetUpgradeCoffee(){
        if(_upgradeCoffee){
            _upgradeCoffee = false;
        }
        else{
            _upgradeCoffee = true;
        }
    }

    public static bool GetUpgradeCoffee(){
        return _upgradeCoffee;
    }

    public static void SetUpgradeFood(){
        if(_upgradeFood){
            _upgradeFood = false;
        }
        else{
            _upgradeFood = true;
        }
    }

    public static void SetFood(bool toSet)
    {
        _upgradeFood = toSet;
    }

    public static void SetCoffee(bool toSet)
    {
        _upgradeCoffee = toSet;
    }
    
    public static bool GetUpgradeFood(){
        return _upgradeFood;
    }

    public static void SetUpgradeSilentWeapon(){
        if(_upgradeSilentWeapon){
            _upgradeSilentWeapon = false;
        }
        else{
            _upgradeSilentWeapon = true;
        }
    }

    public static void SetSilWeapon(bool toSet)
    {
        _upgradeSilentWeapon = toSet;
    }

    public static bool getUpgradeSilentWeapon(){
        return _upgradeSilentWeapon;
    }

    public static void SetUpgradeAlarmD(bool result){
        _upgradeAlarmD = result;
    }

    public static bool getUpgradeAlarmD(){
        return _upgradeAlarmD;
    }

    public static void UpdateSoundCircle()
    {
        _playerSoundCircle.GetComponentInChildren<EntitySound>().speedToRadiusRatio = _playerSoundRatio - _soundBonus;
    }

    public static void AddSoundBonus(float toAdd)
    {
        _soundBonus += toAdd;
    }

    public static float GetSoundBonus()
    {
        return _soundBonus;
    }

    public static void SetSoundBonus(float toSet)
    {
        _soundBonus = toSet;
    }

}
