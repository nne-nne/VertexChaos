using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enemies;

public class ModifierMenuSc : MonoBehaviour
{
    public GameObject interLevelMenu;
    public LevelsScript LS;
    public List<string> bulletModifierNames;
    public List<Button> buttons;
    public CannonController cannon;
    public GameObject[] mods = new GameObject[2];
    public List<string> bulletModifierMessages;
    public List<string> enemyModifierNames;
    public List<string> enemyModifierMessages;

    private int chosenBulletModifier;
    private int chosenEnemyModifier;

    private List<int> bulletModifierOptions;
    private List<int> enemyModifierOptions;


    void Start()
    {
        bulletModifierOptions = GenerateNaturalNumbers(3);
        enemyModifierOptions = GenerateNaturalNumbers(3);

        LevelsScript.EndLevelEvent.AddListener(InitializeMenu);
    }

    

    private List<int> GenerateNaturalNumbers(int maxExclusive)
    {
        List<int> naturals = new List<int>();
        for(int i = 0; i < maxExclusive; i++)
        {
            naturals.Add(i);
        }
        return naturals;
    }

    private List<int> PickRandomNaturals(int howMany, int maxExclusive)
    {
        List<int> naturals = GenerateNaturalNumbers(maxExclusive);
        List<int> result = new List<int>();
        for(int i = 0; i < howMany; i++)
        {
            int index = UnityEngine.Random.Range(0, naturals.Count);
            int num = naturals[index];
            result.Add(num);
            naturals.Remove(num);
        }
        return result;
    }

    private void InitializeMenu()
    {
        Debug.Log("initializing menu");
        interLevelMenu.SetActive(true);

        List<int> modifiersIndices = PickRandomNaturals(buttons.Count, bulletModifierNames.Count);
        List<int> enemyIndices = PickRandomNaturals(buttons.Count, enemyModifierNames.Count);
        for (int i = 0; i < modifiersIndices.Count; i++)
        {
            string buttonText = bulletModifierNames[modifiersIndices[i]] + "\n\n" +
                                bulletModifierMessages[modifiersIndices[i]] + "\n\n" +
                                enemyModifierNames[enemyIndices[i]] + "\n\n" +
                                enemyModifierMessages[enemyIndices[i]] + "\n\n";
            buttons[i].GetComponentInChildren<TMP_Text>().text = buttonText;

            bulletModifierOptions[i] = modifiersIndices[i];
            enemyModifierOptions[i] = enemyIndices[i];
        }
    }

    public void ChooseModifier(int i)
    {
        int chosenBulletModifier = bulletModifierOptions[i];
        int chosenEnemyModifier = enemyModifierOptions[i];
        BulletModifier bm = null;
        EnemyModifier em = null;
        switch (chosenBulletModifier)
        {
            case 0:
                bm = new AddDemage();
                break;
            case 1:
                bm = new AddLifeTime();
                break;
            case 2:
                bm = new AddSpeed();
                break;
            case 3:
                bm = new CanonModifier();
                break;
            case 4:
                bm = new AddDemage(); // tu mia³a byæ aura
                break;
            case 5:
                bm = new ExplosionModifier(mods);
                break;
            case 6:
                bm = new FourWayShootModifier();
                break;
            case 7:
                bm = new HomingBullet();
                break;
            case 8:
                bm = new PierceModifier();
                break;
            case 9:
                bm = new ScatterModifier();
                break;
            case 10:
                bm = new ShieldModifier();
                break;
            case 11:
                bm = new TargetterModifier();
                break;
        }

        switch(chosenEnemyModifier)
        {
            case 0:
                foreach (NotSharedPool pool in LS.enemyPools)
                {
                    foreach (Transform enemy in pool.gameObject.transform)
                    {
                        em = new AddSpeedEnemyModifier();
                        EnemyController ec = enemy.gameObject.GetComponent<EnemyController>();
                        ec.AddModifier(em);
                    }
                }
                break;
            case 1:
                foreach (NotSharedPool pool in LS.enemyPools)
                {
                    foreach (Transform enemy in pool.gameObject.transform)
                    {
                        em = new EnrageEnemyModifier();
                        EnemyController ec = enemy.gameObject.GetComponent<EnemyController>();
                        ec.AddModifier(em);
                    }
                }
                break;
            case 2:
                foreach (NotSharedPool pool in LS.enemyPools)
                {
                    foreach (Transform enemy in pool.gameObject.transform)
                    {
                        em = new BombDropOnDeath(mods);
                        EnemyController ec = enemy.gameObject.GetComponent<EnemyController>();
                        ec.AddModifier(em);
                    }
                }
                break;
            case 3:
                foreach (NotSharedPool pool in LS.enemyPools)
                {
                    foreach (Transform enemy in pool.gameObject.transform)
                    {
                        em = new LowerTimeBetweenShotsModifier();
                        EnemyController ec = enemy.gameObject.GetComponent<EnemyController>();
                        ec.AddModifier(em);
                    }
                }
                break;
            case 4:
                foreach (NotSharedPool pool in LS.enemyPools)
                {
                    foreach (Transform enemy in pool.gameObject.transform)
                    {
                        em = new MoreHealthEnemyModifier();
                        EnemyController ec = enemy.gameObject.GetComponent<EnemyController>();
                        ec.AddModifier(em);
                    }
                }
                break;
            case 5:
                foreach (NotSharedPool pool in LS.enemyPools)
                {
                    foreach (Transform enemy in pool.gameObject.transform)
                    {
                        em = new ShootOnDeath();
                        EnemyController ec = enemy.gameObject.GetComponent<EnemyController>();
                        ec.AddModifier(em);
                    }
                }
                break;
        }

        /*
        Debug.Log(t);
        ConstructorInfo constructor = t.GetConstructor(Type.EmptyTypes);
        BulletModifier newModifier = (BulletModifier)constructor.Invoke(null);
        cannon.AddBulletModifier(newModifier);*/
        cannon.AddBulletModifier(bm);
        


        LevelsScript.StartLevelEvent.Invoke();
        interLevelMenu.SetActive(false);
    }


    void Update()
    {

    }
}
