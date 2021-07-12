using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Player에 붙어 있는 player와 skill에 관한 스크립트. 
/// </summary>
public class PlayerSkill : MonoBehaviour
{
    public Image skillImage;
    public InventoryEquipSlot inventoryEquipSlot;

    public bool hasSkill = false;
    private bool isSkillOn = false;
    private string skillName;
    private PlayerMove playerMove;
    private PlayerAttack playerAttack;

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        if (!hasSkill || !playerMove.canMove)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1) && !playerAttack.isTeleportArrowOn )
        {
            if (!isSkillOn)
            {
                TurnOnSkill();
            }
            else
            {
                TurnOffSkill();
            }
        }
    }

    private void TurnOnSkill()
    {
        skillImage.gameObject.SetActive(true);
        isSkillOn = true;
        SoundManager.instance.PlayClickSound();
    }

    public void TurnOffSkill()
    {
        skillImage.gameObject.SetActive(false);
        isSkillOn = false;
        SoundManager.instance.PlayClickSound();
    }

    public void SetSkill(Item item)
    {
        skillImage.sprite = item.skillImage;
        skillName = item.skillName;
        hasSkill = true;
    }

    public void UnSetSkill()
    {
        skillImage.gameObject.SetActive(false);
        skillImage.sprite = null;
        skillName = null;
        hasSkill = false;
        isSkillOn = false;
    }

    public string GetSkillName()
    {
        return skillName;
    }

    public bool IsSkillOn()
    {
        return isSkillOn;
    }

    
}
