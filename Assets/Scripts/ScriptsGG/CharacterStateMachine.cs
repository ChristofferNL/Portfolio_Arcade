using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    private CharacterBlackGolem blackGolem;
    private CharacterGreenGolem greenGolem;
    private CharacterGreyGolem greyGolem;

    public List<Character> characters = new();

    public ParticleSystem ChangeGolemEffect;
    public ParticleSystem ActiveBuffEffect;

    public List<CharacterBuffSO> Buffs = new();

    public enum GolemState { Black, Green, Grey };
    public GolemState ActiveGolem;

    public enum GolemBuffs { FireAura, CrushingLanding }
    public GolemBuffs ActiveBuff;

    private void Awake()
    {
        UIManagerGG.Instance.ChangeGolemButton.onClick.AddListener(ChangeGolem);
        UIManagerGG.Instance.ChangeEquipmentButton.onClick.AddListener(ChangeBuff);
    }
    private void Start()
    {
        blackGolem = GetComponent<CharacterBlackGolem>();
        greenGolem = GetComponent<CharacterGreenGolem>();
        greyGolem = GetComponent<CharacterGreyGolem>();
        characters.Add(blackGolem);
        characters.Add(greenGolem);
        characters.Add(greyGolem);
        ChangeGolem();
        SetRandomBuff();
    }

    public void InitializeGolem(int index)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (i == index)
            {
                characters[i].InitializeGolem();
            }
            else
            {
                characters[i].UnInitializeGolem();
            }
        }
    }

    public void ChangeGolem()
    {
        if (!characters[(int)ActiveGolem].isAttacking)
        {
            int startingGolem = (int)ActiveGolem;
            do
            {
                ActiveGolem = (GolemState)Random.Range(0, 3);
            } while (startingGolem == (int)ActiveGolem);
            InitializeGolem((int)ActiveGolem);
            EventManagerGG.Instance.ParticlePlayEvent(ChangeGolemEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z -4));
            Debug.Log(ActiveGolem + "Golem is active");
        }   
    }

    public void ActivateBuff(CharacterBuffSO characterBuffSO)
    {
        if (characterBuffSO.IsStatic)
        {
            Debug.Log("Fire Aura buff active!");
            if (ActiveBuffEffect != null)
            {
                ActiveBuffEffect.Play();
                return;
            }
            ParticleSystem effect = characterBuffSO.BuffEffect;
            ParticleSystem particle = Instantiate(effect, new Vector3(transform.position.x, transform.position.y + characterBuffSO.ParticleYOffset, transform.position.z), Quaternion.identity);
            particle.transform.parent = FindObjectOfType<CharacterStateMachine>().transform;
            ActiveBuffEffect = particle;
        }
        else
        {
            Debug.Log("Crushing Landing buff active!");
        }
    }

    public void DeactivateBuff(CharacterBuffSO characterBuffSO)
    {
        if (characterBuffSO.IsStatic && ActiveBuffEffect.gameObject != null)
        {
            Destroy(ActiveBuffEffect.gameObject);
        }
    }

    public void SetRandomBuff()
    {
        ActiveBuff = (GolemBuffs)Random.Range(0, 2);
        ActivateBuff(Buffs[(int)ActiveBuff]);
    }

    public void ChangeBuff()
    {
        if ((int)ActiveBuff == 0)
        {
            DeactivateBuff(Buffs[0]);
            ActivateBuff(Buffs[1]);
                       
            ActiveBuff = (GolemBuffs)1;
        }
        else
        {
            //DeactivateBuff(Buffs[1]);
            ActivateBuff(Buffs[0]);
            
            ActiveBuff = (GolemBuffs)0;
        }
    }
}
