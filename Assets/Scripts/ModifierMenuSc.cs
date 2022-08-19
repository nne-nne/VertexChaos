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
    
    public List<Button> buttons;
    public CannonController cannon;
    public PlayerController player;
    public GameObject[] mods = new GameObject[2];
    private List<GeneralModifier> modifiers;
    private List<string> bulletModifierNames;
    private List<string> bulletModifierMessages;
    public List<string> enemyModifierNames;
    public List<string> enemyModifierMessages;

    private int chosenBulletModifier;
    private int chosenEnemyModifier;

    private List<int> bulletModifierOptions;
    private List<int> enemyModifierOptions;


    void Start()
    {
        
        bulletModifierOptions = GenerateNaturalNumbers(4);
        enemyModifierOptions = GenerateNaturalNumbers(4);

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
        modifiers = AllModifiers.instance.GetModifiers();
        bulletModifierNames = AllModifiers.instance.modifierNames;
        bulletModifierMessages = AllModifiers.instance.modifierDesc;
        interLevelMenu.SetActive(true);

        List<int> modifiersIndices = PickRandomNaturals(buttons.Count, bulletModifierNames.Count);
        List<int> enemyIndices = PickRandomNaturals(buttons.Count, enemyModifierNames.Count);
        for (int i = 0; i < modifiersIndices.Count; i++)
        {
            string buttonText = bulletModifierNames[modifiersIndices[i]] + "\n\n" +
                                bulletModifierMessages[modifiersIndices[i]] + "\n\n------\n\n" +
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
        GeneralModifier bm = null;
        EnemyModifier em = null;
        bm = modifiers[chosenBulletModifier];

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
        if (bm.type == ModType.modType.Bullet)
        {
            cannon.AddBulletModifier(bm as BulletModifier);
        }
        else if (bm.type == ModType.modType.Player)
        {
            player.AddModifier(bm as PlayerModifier);
        }
        
        


        LevelsScript.StartLevelEvent.Invoke();
        interLevelMenu.SetActive(false);
    }


    void Update()
    {

    }
}
