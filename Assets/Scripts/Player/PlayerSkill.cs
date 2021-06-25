using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    public Image skillImage;
    public InventoryEquipSlot inventoryEquipSlot;

    private bool hasSkill = false;
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
                skillImage.gameObject.SetActive(true);
                isSkillOn = true;
            }
            else
            {
                skillImage.gameObject.SetActive(false);
                isSkillOn = false;
            }

            SoundManager.instance.PlayClickSound();
        }
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
