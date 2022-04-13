using UnityEngine;

public class SavePlayerData : SaveScript
{
    // void Start()
    // {
    //     SaveMaster.KeepTrackOf(this);
    // }
    public override void SaveMe()
    {
        int money = PlayerData.GetMoney();
        int score = PlayerData.GetScore();
        int maxHealth = PlayerData.GetMaxHealth();
        int health = PlayerData.GetCurrentHealth();
        int level = PlayerData.GetLevel();
        int diamonds = PlayerData.GetDiamonds();
        int keys = PlayerData.GetKeys();
        int dMaxHealth = PlayerData.GetDefaultMaxHealth();
        
        float speed = PlayerData.GetSpeed();
        float dSpeed = PlayerData.GetDefaultSpeed();
        float soundRatio = PlayerData.GetSoundRatio();
        float bulletForce = PlayerData.GetBulletForce();
        float soundBonus = PlayerData.GetSoundBonus();

        bool upCoffee = PlayerData.GetUpgradeCoffee();
        bool upFood = PlayerData.GetUpgradeFood();
        bool upAlarm = PlayerData.getUpgradeAlarmD();
        bool upWeapon = PlayerData.getUpgradeSilentWeapon();
        
        string playerName = PlayerData.GetName();
        
        SaveMaster.SaveVariables(
            uid:this.uid,
            ints:new int[]{money, score, maxHealth, health, level, diamonds, keys, dMaxHealth},
            floats:new float[]{speed, dSpeed, soundRatio, bulletForce, soundBonus},
            bools:new bool[]{upCoffee, upFood, upAlarm, upWeapon},
            strings:new string[]{playerName}
            );
        Debug.Log($"PlayerData was saved. For reference, score was {score}");
    }

    public override void LoadMe()
    {
        var load = SaveMaster.LoadVariables(gameObject, uid);
        bool loadedProperly = load.Item1;
        var data = load.Item2;
        if (loadedProperly)
        {
            int[] ints = data.Item1;
            bool[] bools = data.Item2;
            float[] floats = data.Item3;
            string[] strings = data.Item4;

            
            // bad idea, not sure if player will exist when invoked... Too bad!
            PlayerScript ps = FindObjectOfType<PlayerScript>();
            ps.SyncPlayerData();

            PlayerData.SetMoney(ints[0]);
            PlayerData.SetScore(ints[1]);
            PlayerData.SetMaxHealth(ints[2]);
            PlayerData.SetCurrentHealth(ints[3]);
            PlayerData.SetLevel(ints[4]);
            PlayerData.ActuallySetDiamonds(ints[5]);
            PlayerData.ActuallySetKeys(ints[6]);
            PlayerData.SetDefaultMaxHealth(ints[7]);
            
            PlayerData.SetSpeed(floats[0]);
            PlayerData.SetDefaultSpeed(floats[1]);
            PlayerData.SetSilentShoes(floats[2]); // SetSoundRatio
            PlayerData.SetBulletForce(floats[3]);
            PlayerData.SetSoundBonus(floats[4]);
            
            PlayerData.SetCoffee(bools[0]);
            PlayerData.SetFood(bools[1]);
            PlayerData.SetUpgradeAlarmD(bools[2]);
            PlayerData.SetSilWeapon(bools[3]);
            
            PlayerData.SetName(strings[0]);
            
            Debug.Log($"PlayerData was loaded. For reference, score was {ints[1]}. Was it registered? {ints[1]==PlayerData.GetScore()}");
        }
        else
        {
            Debug.LogError("Failed to load state of PlayerData. Does the save exist?");
        }
        
    }
}

