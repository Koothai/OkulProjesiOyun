using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    // baslangic statlari
    //[HideInInspector]
    public float currentMoveSpeed;
    //[HideInInspector]
    public float currentHealth;
    //[HideInInspector]
    public float currentRecovery;
    //[HideInInspector]
    public float currentArmor;
    //[HideInInspector]
    public float currentPierceAddend;
    //[HideInInspector]
    public float currentDamageMultiplier;
    //[HideInInspector]
    public float currentAmountAddend;
    //[HideInInspector]
    public float currentCooldownReduction;
    //[HideInInspector]
    public float currentAreaMultiplier;
    //[HideInInspector]
    public float currentCritChance;
    //[HideInInspector]
    public float currentMagnet;

    // sprite ve animasyon

    public Animator currentAnimator;
    public SpriteRenderer currentCharacterSpriteRenderer;
    public AnimatorController currentAnimatorController;


    // deneyim ve seviye statlari

    [Header("Experience/Level")]
    public int experience = 0;
    public int level= 1;
    public int experienceCap;


    // deneyim ve seviye ayari
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }



    public List<LevelRange> levelRanges;

    InventoryManager inventoryManager;
    public int weaponIndex;
    public int passiveItemIndex;



    public GameObject fpitemtest;
    public GameObject fpitemtest2;
    public GameObject weapontest;




    void Awake()
    {
        

        

        //datayi statlardan once cagir yoksa null reference veriyor
        // player statlarini character selector classindaki datalardan cekiyoruz
        characterData = CharacterSelector.GetData();


        currentCharacterSpriteRenderer = GetComponent<SpriteRenderer>();
        currentAnimator = GetComponent<Animator>();

        currentAnimatorController = characterData.CharacterAnimator;
        currentAnimator.runtimeAnimatorController = currentAnimatorController;
        currentCharacterSpriteRenderer.sprite = characterData.CharacterSprite;
        
        // playerin statlari player objesine gectigi icin selectorde yarattigimiz instance i yok ediyoruz
        CharacterSelector.instance.DestroySingleton();

        inventoryManager = GetComponent<InventoryManager>();

        // guncel statlar
        currentMoveSpeed = characterData.MoveSpeed;
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentArmor = characterData.Armor;
        currentPierceAddend = characterData.PierceAddend;
        currentDamageMultiplier = characterData.DamageMultiplier;
        currentAmountAddend = characterData.AmountAddend;
        currentCooldownReduction = characterData.CooldownReduction;
        currentAreaMultiplier = characterData.AreaMultiplier;
        currentCritChance = characterData.CritChance;
        currentMagnet = characterData.Magnet;

        // spawn fonksiyonu
        //SetSpriteAndAnimator(characterData.CharacterSprite);
        SpawnWeapon(characterData.StartingWeapon);
        SpawnWeapon(weapontest);
        SpawnPassiveItem(fpitemtest);
        SpawnPassiveItem(fpitemtest2);
    }

    void Start()
    {
        // deneyim sinirini  baslat
        experienceCap = levelRanges[0].experienceCapIncrease;
    }
    void Update()
    {
        Recover();
    }
    public void IncreaseExperience(int Amount)
    {  
        experience += Amount;
        LevelUpChecker();
    }


    void LevelUpChecker()    // Fazla gelen tecrubenin sonraki levele aktarilabilmesi icin kod
    {
        if (experience >= experienceCap)
        {
            // seviyeyi arttir ve tecrube puanini duzelt

            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;

        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= (dmg); 

        if (currentHealth <=0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Debug.Log("Player Died");
    }

   void Recover()
    {
        if (currentHealth <characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;

            if (currentHealth>characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {

        if (weaponIndex >= inventoryManager.weaponSlots.Count -1)
        {
            Debug.LogError("waaa weapon indexi duzeltmen lazim");
            return;
        }
        //baslangic silahini yarat
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        // playerin childi yap
        spawnedWeapon.transform.SetParent(transform);
        // olusturulmus silahlar listesine ekle
        inventoryManager.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());
        weaponIndex++;
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {

        if (passiveItemIndex >= inventoryManager.passiveItemSlots.Count - 1)
        {
            Debug.LogError("waaa passiveitem indexi duzeltmen lazim");
            return;
        }
        
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        // playerin childi yap
        spawnedPassiveItem.transform.SetParent(transform);
        // olusturulmus pasifler listesine ekle
        inventoryManager.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());
        passiveItemIndex++;
    }







}
