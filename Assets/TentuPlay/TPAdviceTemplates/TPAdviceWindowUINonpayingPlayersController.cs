using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System;

namespace TentuPlay.CRM
{
    class TPAdviceWindowUINonpayingPlayersController : TPAdviceWindowUIBaseController
    {
        public Text Title;
        public Text RankingTitle;

        public Text TopTag1st;
        public Text TopName1st;
        public GameObject TopImage1st;
        public GameObject TopBar1st;
        public Text TopFidelity1st;

        public Text TopTag2nd;
        public Text TopName2nd;
        public GameObject TopImage2nd;
        public GameObject TopBar2nd;
        public Text TopFidelity2nd;

        public Text TopTag3rd;
        public Text TopName3rd;
        public GameObject TopImage3rd;
        public GameObject TopBar3rd;
        public Text TopFidelity3rd;

        public GameObject ContentImage;
        public Text RecommendDesc;
        public Text OfferButtonText;

        public override int PlaceAdvice(TPAdviceRawDataFromDB myAdvice, string player_id)
        {
            // initialize some variables
            string player_name;
            string stage_name;

            string top_tag_1st;
            string top_name_1st;
            int top_fidelity_1st;
            string top_asset_name_1st;

            string top_tag_2nd;
            string top_name_2nd;
            int top_fidelity_2nd;
            string top_asset_name_2nd;

            string top_tag_3rd;
            string top_name_3rd;
            int top_fidelity_3rd;
            string top_asset_name_3rd;
            
            string content_name;
            string content_asset_name;
            string offer_id;
            string offer_name;

            float bar_width = 2.67F;
            float bar_height = 14.8F;

            // Fix the line below to use in your game.
            //string asset_path_prefix = "Images/M002(nonpaying_players)/";
            string asset_path_prefix = "Sprites/Item/";


            // Json parsing
            try
            {
                #region JsonExample
                /* 
                 * Example of "display_parameters" and "recommendations"

                 * display_parameters = {
                    "player_name": "QTPie",
                    "stage_name": "Stage 3",
                    "top_usage": [   
                        {
                            "id": "WEAPON_01",
                            "tag": "Legendary Weapn",
                            "name": "Blade of Infinite Thirst", 
                            "asset_name": "primaryItem",
                            "fidelity": 97, 
                            "display_order": 1,
                        }, 
                        {
                            "id": "RING_01",
                            "tag": "Epic Ring",
                            "name": "Unstably Frozen Orb", 
                            "asset_name": "secondaryItem",
                            "fidelity": 56, 
                            "display_order": 2,
                        }, 
                        {
                            "id": "EQUIPMENT_01",
                            "tag": "Rare Amulet",
                            "name": "Talisman of the Windcaller", 
                            "asset_name": "tertiaryItem",
                            "fidelity": 32, 
                            "display_order": 3,
                        }
                    ],
                }


                 * recommendations = {
                    "contents": [
                        {
                            "id": "weapon_01",
                            "name": "Blade of Infinite Thirst", 
                            "asset_name": "primaryItem",
                        },
                    ],
                    "offers": [
                        {
                            "id": "SPECIAL_AD_01",
                            "name": "10 Gems",
                            "asset_name": "ingameCashIcon",
                        },
                    ]
                }


                 */
                #endregion

                JsonData display_parameters = JsonMapper.ToObject(myAdvice.display_parameters);

                player_name = (string)display_parameters["player_name"];
                stage_name = (string)display_parameters["stage_name"];

                top_tag_1st = (string)display_parameters["top_usage"][0]["tag"];
                top_name_1st = (string)display_parameters["top_usage"][0]["name"];
                top_fidelity_1st = (int)display_parameters["top_usage"][0]["fidelity"];
                top_asset_name_1st = (string)display_parameters["top_usage"][0]["asset_name"];

                top_tag_2nd = (string)display_parameters["top_usage"][1]["tag"];
                top_name_2nd = (string)display_parameters["top_usage"][1]["name"];
                top_fidelity_2nd = (int)display_parameters["top_usage"][1]["fidelity"];
                top_asset_name_2nd = (string)display_parameters["top_usage"][1]["asset_name"];

                top_tag_3rd = (string)display_parameters["top_usage"][2]["tag"];
                top_name_3rd = (string)display_parameters["top_usage"][2]["name"];
                top_fidelity_3rd = (int)display_parameters["top_usage"][2]["fidelity"];
                top_asset_name_3rd = (string)display_parameters["top_usage"][2]["asset_name"];
                
                JsonData recommendations = JsonMapper.ToObject(myAdvice.recommendations);

                content_name = (string)recommendations["contents"][0]["name"];
                content_asset_name = (string)recommendations["contents"][0]["asset_name"];

                offer_id = (string)recommendations["offers"][0]["id"];
                offer_name = (string)recommendations["offers"][0]["name"];
            }
            catch (Exception e)
            {
                if (TPSettings.DEBUG) Debug.Log("TPError||Error parsing advice json" + e.ToString());
                return -1;
            }


            /*To fill in the Advice*/
            //Title.text = player_name + ", Here's a Special Tip for You";
            //RankingTitle.text = stage_name;

            //TopTag1st.text = top_tag_1st;
            //TopName1st.text = top_name_1st;
            //TopImage1st.GetComponent<Image>().sprite = Resources.Load<Sprite>(asset_path_prefix + top_asset_name_1st);
            //TopBar1st.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(top_fidelity_1st * bar_width, bar_height);
            //TopFidelity1st.text = top_fidelity_1st.ToString() + "%";

            //TopTag2nd.text = top_tag_2nd;
            //TopName2nd.text = top_name_2nd;
            //TopImage2nd.GetComponent<Image>().sprite = Resources.Load<Sprite>(asset_path_prefix + top_asset_name_2nd);
            //TopBar2nd.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(top_fidelity_2nd * bar_width, bar_height);
            //TopFidelity2nd.text = top_fidelity_2nd.ToString() + "%";

            //TopTag3rd.text = top_tag_3rd;
            //TopName3rd.text = top_name_3rd;
            //TopImage3rd.GetComponent<Image>().sprite = Resources.Load<Sprite>(asset_path_prefix + top_asset_name_3rd);
            //TopBar3rd.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(top_fidelity_3rd * bar_width, bar_height);
            //TopFidelity3rd.text = top_fidelity_3rd.ToString() + "%";

            //ContentImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(asset_path_prefix + content_asset_name);
            //RecommendDesc.text = "Watch ads and get coins to buy " + content_name + " IMMEDIATELY!";
            //OfferButtonText.text = "Watch Ads and Get " + offer_name + "!";

            Title.text = player_name + ", Here's a Special Tip for You";
            RankingTitle.text = "Stage1-1";

            TopTag1st.text = "Bow";
            TopName1st.text = "Bomb Bow";
            TopImage1st.GetComponent<Image>().sprite = Resources.Load<Sprite>(asset_path_prefix + "BombBow");
            TopBar1st.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(top_fidelity_1st * bar_width, bar_height);
            TopFidelity1st.text = top_fidelity_1st.ToString() + "%";

            TopTag2nd.text = "Consumable";
            TopName2nd.text = "Arrow + 5";
            TopImage2nd.GetComponent<Image>().sprite = Resources.Load<Sprite>(asset_path_prefix + "Arrow");
            TopBar2nd.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(top_fidelity_2nd * bar_width, bar_height);
            TopFidelity2nd.text = top_fidelity_2nd.ToString() + "%";

            TopTag3rd.text = "Consumable";
            TopName3rd.text = "Jump Potion";
            TopImage3rd.GetComponent<Image>().sprite = Resources.Load<Sprite>(asset_path_prefix + "JumpPotion");
            TopBar3rd.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(top_fidelity_3rd * bar_width, bar_height);
            TopFidelity3rd.text = top_fidelity_3rd.ToString() + "%";

            ContentImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(asset_path_prefix + "BombBow");
            RecommendDesc.text = "Watch ads and get coins to buy " + "Bomb Bow" + " IMMEDIATELY!";
            OfferButtonText.text = "Watch Ads and Get " + "1000 Coin " + "!";

            // Example of ItemViewer & GoToShopExample
            itemIdViewer[] ItemViewerContainer = gameObject.GetComponentsInChildren<itemIdViewer>();
            ItemViewerContainer[0].itemId = offer_id;

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