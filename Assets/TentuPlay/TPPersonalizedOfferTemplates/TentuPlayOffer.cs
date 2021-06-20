using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TentuPlay.Api;
using LitJson;

namespace TentuPlay.CRM
{
    public class TentuPlayOffer: TPAdviceWindowBaseController
    {
        public TPOfferCalculated thisOffer;
        private string thisplayer_id;
        private string thisLanguage;

        public override int PlaceOffer(TPOfferCalculated myOffer, string player_id, string language)
        {
            thisplayer_id = player_id;
            thisOffer = myOffer;
            thisLanguage = language;

            TPStashEvent myStashEvent = new TPStashEvent();
            myStashEvent.StashOfferEvent(player_uuid: player_id, offer_id: myOffer.offer_id, message_status: messageStatus.Open);
            new TPUploadData().UploadData(toCheckInterval: false);

            TPPersonalizedOffer myPerOffer = new TPPersonalizedOffer();
            myPerOffer.OfferOpened(myOffer.offer_id, (response) => {
            });
            
            try
            {
                gameObject.GetComponent<MessageController>().Show();
            }
            catch
            {
                return -1;
            }
            return 1;
        }

        public void GoToOfferEvent()
        {
            
            // Log the Offer interact event
            int r = new TPStashEvent().StashOfferEvent(
                player_uuid: thisplayer_id, offer_id: thisOffer.offer_id, message_status: messageStatus.Interact); 
            new TPUploadData().UploadData(toCheckInterval: false);

            JsonData offer = JsonMapper.ToObject(thisOffer.offer);
            if (offer.ContainsKey("message"))
            {
                if (offer["message"].ContainsKey(thisLanguage))
                {
                    if (offer["message"][thisLanguage].ContainsKey("url"))
                    {
                        string url = (string)offer["message"][thisLanguage]["url"];
                        Application.OpenURL(url);

                    }
                    else return;
                }
                else
                {
                    if (TPSettings.DEBUG) Debug.Log("TPError||Key Error. Check your language parameter.");
                    return;
                }
            }
            else return;


            OnGoToOfferEvent();
            return;
        }

        public void CloseOfferEvent()
        {
            // Log the Offer close event
            int r = new TPStashEvent().StashOfferEvent(
                player_uuid: thisplayer_id, offer_id: thisOffer.offer_id, message_status: messageStatus.Dismiss); 
            new TPUploadData().UploadData(toCheckInterval: false);

            OnCloseOfferEvent();
        }

        protected virtual void OnGoToOfferEvent()
        {
            // 1. Add 1 to OfferClosedCount
            new TPPersonalizedOffer().AddOfferClosedCount(offer_id: thisOffer.offer_id);

            // 2. Close
            gameObject.GetComponent<MessageController>().Close();
        }

        protected virtual void OnCloseOfferEvent()
        {
            // 1. Add 1 to OfferClosedCount
            new TPPersonalizedOffer().AddOfferClosedCount(offer_id: thisOffer.offer_id);

            // 2. Close
            gameObject.GetComponent<MessageController>().Close();
        }


    }
}

