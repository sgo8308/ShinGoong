using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using TentuPlay.Api;
using LitJson;
using System;
using System.Text;

namespace TentuPlay.CRM
{
    public class TPAdviceWindowUISmartAndBusyController : TPAdviceWindowUIBaseController
    {
        public Text Title;
        public Text Contents;

        public Text EarnedCurrency;

        public Text LevelDelta;
        public Text LevelBefore;
        public Text LevelAfter;

        public Text SegmentScore;
        public Text SegmentPercentage;

        public GameObject RecommendContentImage;
        public Text RecommendContentName;

        public GameObject RecommendOfferImage;
        public Text RecommendOfferName;
        public Text RecommendOfferPrice;

        public override int PlaceAdvice(TPAdviceRawDataFromDB myAdvice, string player_id)
        {
            // initialize some variables
            string player_name;
            int segment_range;

            string currency_earned;

            int level_delta;
            int level_before;
            int level_after;

            int segment_score;
            int segment_percentage;

            string content_id;
            string content_name;
            string content_asset_name;

            string offer_id;
            string offer_name;
            string offer_asset_name;
            float offer_price;
            string currency_code;

            // Fix the line below to use in your game.
            string asset_path_prefix = "Images/M001(smart_and_busy_v2)/";

            // Json parsing
            try
            {
                #region JsonExample

                /*
                 * Example of "display_parameters" and "recommendations"
                 
                 * display_parameters = {
                    "player_name": "QTPie", 
                    "segment_range": 7, 
                    "segment_score": 32625, 
                    "segment_percentage": 5, 
                    "currency_earned": "2,487A", 
                    "level_delta": 14, 
                    "level_before": 14, 
                    "level_after": 28
                    }

                 * recommendations = {
                    "contents": [
                        {
                            "id": "water_dungeon", 
                            "name": "Water Dungeon", 
                            "asset_name": "dungeonImage@2x"
                            }
                    ], 
                    "offers": [
                        {
                            "id": "gold_box", 
                            "name": "3,000A Box", 
                            "asset_name": "goldImage@2x", 
                            "price": 55.99, 
                            "currency_code": "USD"
                        }
                    ]
                    }
                 */
                #endregion


                JsonData display_parameters = JsonMapper.ToObject(myAdvice.display_parameters);
                player_name = (string)display_parameters["player_name"];
                segment_range = (int)display_parameters["segment_range"];
                currency_earned = (string)display_parameters["currency_earned"];
                level_delta = (int)display_parameters["level_delta"];
                level_before = (int)display_parameters["level_before"];
                level_after = (int)display_parameters["level_after"];
                segment_score = (int)display_parameters["segment_score"];
                segment_percentage = (int)display_parameters["segment_percentage"];


                JsonData recommendations = JsonMapper.ToObject(myAdvice.recommendations);
                int i = 0;
                content_id = (string)recommendations["contents"][i]["id"];
                content_name = (string)recommendations["contents"][i]["name"];
                content_asset_name = (string)recommendations["contents"][i]["asset_name"];

                offer_id = (string)recommendations["offers"][i]["id"];
                offer_name = (string)recommendations["offers"][i]["name"];
                offer_asset_name = (string)recommendations["offers"][i]["asset_name"];
                offer_price = (float)recommendations["offers"][i]["price"];
                currency_code = (string)recommendations["offers"][i]["currency_code"];

            }
            catch (Exception e)
            {
                if (TPSettings.DEBUG) Debug.Log("TPError||Error parsing advice json" + e.ToString());
                return -1;
            }


            /*To fill in the Advice*/

            Title.text = player_name + ", Here's a Special Tip for You";
            Contents.text = "Congratulations on achieving explosive growth!\n" +
                "You've earned " + currency_earned + " of gold and " + level_delta + " level-ups for " + segment_range.ToString() + " days!";
            EarnedCurrency.text = currency_earned;

            LevelDelta.text = level_delta.ToString();
            LevelBefore.text = "Lv. " + level_before.ToString();
            LevelAfter.text = "Lv. " + level_after.ToString();

            SegmentScore.text = segment_score.ToString("N0");
            SegmentPercentage.text = segment_percentage.ToString() + "%";

            RecommendContentName.text = content_name;
            RecommendContentImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(asset_path_prefix + content_asset_name);

            RecommendOfferName.text = offer_name;
            RecommendOfferImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(asset_path_prefix + offer_asset_name);
            RecommendOfferPrice.text = "$" + offer_price.ToString(); // "₩" + offer_price.ToString("N0");


            // Example of ItemViewer & GoToShopExample
            itemIdViewer[] ItemViewerContainer = gameObject.GetComponentsInChildren<itemIdViewer>();
            ItemViewerContainer[0].itemId = content_id;
            ItemViewerContainer[1].itemId = offer_id;

            return 1;
        }

        // Include any events you want when closing the Advices.
        protected override void OnCloseAdviceEvent()
        {
            // 1. Add 1 to AdviceClosedCount
            int updateadvice = new TPMentor().AddAdviceClosedCount(advice_id: thisAdvice.advice_id);

            // 2. Close
            gameObject.GetComponent<MessageController>().Close();
        }

        // Include any events you want when interacting with the Advices.
        protected override void OnGoToAdviceEvent(Text item_name)
        {
            // 1. Add 1 to AdviceClosedCount
            int updateadvice = new TPMentor().AddAdviceClosedCount(advice_id: thisAdvice.advice_id);

            // 2. Close
            gameObject.GetComponent<MessageController>().Close();
        }
    }
}


