using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TentuPlay.CRM; // import TentuPlay CRM to load/show advice data

/// <summary>
/// Component of Player in the scene
/// </summary>
public class TentuPlayCRMPlayerController : MonoBehaviour
{
    public GameObject tpAdviceController;
    public GameObject tpPersonalizedOfferController;
    public GameObject MailBox_advice;
    public GameObject MailBox_offer;
    private string player_uuid = "TentuPlayer"; // player_uuid can be anything that uniquely identifies each of your game user.
    public List<AdviceInfo> myAdvices = new List<AdviceInfo>(); // advice_id, closed_count, valid_until
    public List<OfferInfo> myOffers= new List<OfferInfo>();

    void Start() //Game starts
    {
        // Download mail when the game starts.
        LoadAdvice();
        LoadOffer();
    }

    private void LoadAdvice()
    {
        TPMentor myMentor = new TPMentor();
        myMentor.GetAdvicesAsync(this.player_uuid, (response) =>
        {
            myAdvices = myMentor.SelectAdvicesInfo(player_uuid);
            Debug.Log("I HAVE " + myAdvices.Count.ToString() + " ADVICES.");

            // When unread Advices exist.
            bool has_unread_advice = false;
            foreach (AdviceInfo myAdvice in myAdvices)
            {
                if (myAdvice.closed_count == 0)
                    has_unread_advice = true;
            }

            if (has_unread_advice)
                MailBox_advice.GetComponent<Image>().sprite = Resources.Load<Sprite>("note_noti");
        });
    }

    private void LoadOffer()
    {
        TPPersonalizedOffer myPO = new TPPersonalizedOffer();

        myPO.GetOfferAsync(this.player_uuid, (response) =>
        {
            if (response > 0)
            {
                myOffers = myPO.SelectOfferInfo(player_uuid, "ko");
                Debug.Log("I HAVE " + myOffers.Count.ToString() + " OFFERS.");

                // When unread offer exists.
                bool has_unread_offer = false;
                foreach (OfferInfo myOffer in myOffers)
                {
                    if (myOffer.closed_count == 0)
                        has_unread_offer = true;
                }

                if (has_unread_offer)
                    MailBox_offer.GetComponent<Image>().sprite = Resources.Load<Sprite>("note_noti");
            }
        }
        );
    }

    public void OpenMailBox() // When clicking the mailbox button.
    {
        foreach (Transform child in tpAdviceController.transform)
        {
            if (child.name.StartsWith("MailBoxOpen"))
            {
                Destroy(child.gameObject);
            }
        }

        tpAdviceController = GameObject.Find("TPAdviceController");

        GameObject mailbox = Resources.Load<GameObject>("MailBoxOpen");

        GameObject adviceWindow = Instantiate(mailbox, Vector2.zero, Quaternion.identity);
        adviceWindow.transform.SetParent(tpAdviceController.transform, false);

        adviceWindow.GetComponent<MessageController>().Show();

        MailBox_advice.GetComponent<Image>().sprite = Resources.Load<Sprite>("note");
    }

    public void ShowPersonalizedOffer()
    {
        TPPersonalizedOffer myTPOffer = new TPPersonalizedOffer();

        StartCoroutine(
            myTPOffer.ShowOffer(tpPersonalizedOfferController, player_uuid, "ko", (response) =>
            {
                if (response > 0)
                    MailBox_offer.GetComponent<Image>().sprite = Resources.Load<Sprite>("note");
            }));
    }
}



