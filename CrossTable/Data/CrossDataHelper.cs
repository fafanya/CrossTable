using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrossTable
{
    public static class CrossDataHelper
    {
        /// <summary>
        /// Test method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static OffersData LoadTestOffers()
        {
            var test =
                @"{""offers"":[
                    {""CagentID"":""2"",
                    ""Area"":""Салово 56"",
                    ""Category"":""B"",
                    ""ID"":""3"",
                    ""DeliveryType"":""Доставка"",
                    ""Title"":""ООО \""Карго-Сервис СПБ\"""",
                    ""PaymentType"":""Постоплата"",
                    ""DelayDays"":""0"",
                    ""Area"":""Уманский 29"",
                    ""variants"":[
                        {
                            ""ID"":""1"",""code"":""R00192"",
                            ""name"":""Шина 16.9-28 12PR D314 DR-4"",
                            ""requestId"":""1"",
                            ""TransferQuantity"":""2"",
                            ""nomeclatureNameAnalog"": ""NameAnalog_02_1"",
                            ""nomeclatureCodeAnalog"": ""CodeAnalog0_2_1"",
                            ""AuthorHeadSelect"":""false"",
                            ""ManagerSelect"":""true"",
                            ""CostInRub"":""1700"",
                            ""Total"":""3400"",
                            ""Term"":""успеть к аудиту""
                        }
                    ]},
                    {""CagentID"":""1"",
                    ""Area"":""Заневка 59"",
                    ""Category"":""A"",
                    ""ID"":null,
                    ""DelayDays"":""3"",
                    ""PaymentType"":""Аванс"",
                    ""DeliveryType"":""Доставка"",
                    ""Title"":""ООО \""Аванто\"""",
                    ""variants"":[
                        {
                            ""ID"":""1"",
                            ""code"":""R00192"",
                            ""name"":""Шина 16.9-28 12PR D314 DR-4"",
                            ""requestId"":""1"",
                            ""transferQuantity"":""2"",
                            ""nomeclatureNameAnalog"": ""NameAnalog_01_1"",
                            ""nomeclatureCodeAnalog"": ""CodeAnalog_01_1"",
                            ""AuthorHeadSelect"":""false"",
                            ""ManagerSelect"":""false"",
                            ""CostInRub"":""5"",
                            ""Total"":""10"",
                            ""Term"":""""
                        },
                        {
                            ""ID"":""2"",
                            ""code"":""ЦS00000005512"",
                            ""name"":""Папка скорос-тель Комкс А4 оранжевый 1810"",
                            ""requestId"":""1"",
                            ""transferQuantity"":""5"",
                            ""nomeclatureNameAnalog"": ""NameAnalog_01_2"",
                            ""nomeclatureCodeAnalog"": ""CodeAnalog_01_2"",
                            ""AuthorHeadSelect"":""true"",
                            ""ManagerSelect"":""true"",
                            ""CostInRub"":""10"",
                            ""Total"":""50"",
                            ""Term"":""""
                        }
                    ]}]}";
            var offersData = JsonConvert.DeserializeObject<OffersData>(test);
            return offersData;
        }

        public static RequestsData LoadTestPurchseRequest()
        {
            var test = @"{""requests"":[
                {
                    ""ID"":""1"",
                    ""NomenclatureId"":""1"",
                    ""RequestNum"":""Ф - 143 - 16072017"",
                    ""Priority"":""Срок"",
                    ""NomenclatureType"":""Материал"",
                    ""NomenclatureName"":""Шина 16.9 - 28 12PR D314 DR - 4"",
                    ""NomenclatureCode"":""R00192"",
                    ""Availability"":""мурманский склад,15 шт."",
                    ""TransferQuantityMO"":""25"",
                    ""TotalQuantityMO"":""20""
                }]}";
            var requests = JsonConvert.DeserializeObject<RequestsData>(test);
            return requests;
        }

        
    }
}