using System.Collections.Generic;
using UnityEngine;
using TentuPlay.CRM;

/// <summary>
/// Component of MailBoxOpen Prefab
/// </summary>
public class MailBoxOpen : MonoBehaviour
{
    private string player_uuid = "TentuPlayer";
    private GameObject tpAdviceController;
    private List<AdviceInfo> myAdvices = new List<AdviceInfo>(); // advice_id, closed_count, valid_until

    void Start()
    {
        myAdvices = GameObject.Find("Player").GetComponent<TentuPlayCRMPlayerController>().myAdvices;
        tpAdviceController = GameObject.Find("TPAdviceController");
    }

    public void ShowAdviceExample1()
    {
        int advice_id = (int)myAdvices[0].advice_id;
        int r = new TPMentor().ShowAdviceById(tpAdviceController, this.player_uuid, advice_id);

        // Each close event is at: TPAdviceWindowUINonpayingPlayersController, TPAdviceWindowUISmartAndBusyController , ...
    }

    public void ShowAdviceExample2()
    {
        int advice_id = (int)myAdvices[1].advice_id;

        TPMentor myMentor = new TPMentor();
        int r = myMentor.ShowAdviceById(tpAdviceController, this.player_uuid, advice_id);

        // Each close event is at: TPAdviceWindowUINonpayingPlayersController, TPAdviceWindowUISmartAndBusyController , ...
    }

    public void Close()
    {
        gameObject.GetComponent<MessageController>().Close();

    }
}
