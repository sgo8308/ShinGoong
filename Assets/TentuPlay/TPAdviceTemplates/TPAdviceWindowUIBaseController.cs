using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TentuPlay.Api;

namespace TentuPlay.CRM
{
    public class TPAdviceWindowUIBaseController : TPAdviceWindowBaseController
    {
        public TPAdviceRawDataFromDB thisAdvice;
        private string thisplayer_id;

        public virtual int PlaceAdvice(TPAdviceRawDataFromDB myAdvice, string player_id)
        {
            if (TPSettings.DEBUG) Debug.Log("TPError||NotImplementedException: The requested feature is not implemented.");

            return -1;
        }

        public override int PlaceAdviceWrapper(TPAdviceRawDataFromDB myAdvice, string player_id)
        {
            thisplayer_id = player_id;
            thisAdvice = myAdvice;

            int r = PlaceAdvice(myAdvice: myAdvice, player_id: player_id);
            if (r < 0)
                return -1;

            // Log the Advice open event
            new TPStashEvent().StashAdviceEvent(player_uuid: player_id, advice_id: thisAdvice.advice_id, advice_status: adviceStatus.Open, advice_detail: null);
            new TPUploadData().UploadData(toCheckInterval: false);

            try
            {
                // Show the Advice
                gameObject.GetComponent<MessageController>().Show();
            }
            catch
            {
                return -1;
            }

            // An empty Advice UI should not be loaded.
            return 1;
        }

        public void GoToAdviceEvent(Text item_name)
        {
            // Log the Advice interact event
            int r = new TPStashEvent().StashAdviceEvent(player_uuid: thisplayer_id, advice_id: thisAdvice.advice_id, advice_status: adviceStatus.Interact, advice_detail: item_name.text);
            new TPUploadData().UploadData(toCheckInterval: false);

            OnGoToAdviceEvent(item_name);
        }

        public void CloseAdviceEvent()
        {
            // Log the Advice close event
            int r = new TPStashEvent().StashAdviceEvent(player_uuid: thisplayer_id, advice_id: thisAdvice.advice_id, advice_status: adviceStatus.Dismiss, advice_detail: null);
            new TPUploadData().UploadData(toCheckInterval: false);

            OnCloseAdviceEvent();
        }

        protected virtual void OnGoToAdviceEvent(Text item_name)
        {
        }

        protected virtual void OnCloseAdviceEvent()
        {
        }


    }
}

