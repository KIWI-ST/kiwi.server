﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engine.NLP;
using Engine.NLP.Annotation;
using Engine.NLP.Entity;
using Engine.NLP.Utils;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Matcher;
using Microsoft.Recognizers.Text.Number;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Examples
{
    /// <summary>
    /// https://github.com/microsoft/Recognizers-Text/tree/master/.NET/Samples
    /// </summary>
    [TestClass]
    public class RecognizersText
    {

        string rawText = "TASMAN SPIRIT, Pakistan,Incident. The Maltese tanker TASMAN SPIRIT grounded at the entrance to Karachi Port, Pakistan in the early hours on 27th July 2003. The vessel was carrying 67,800 tonnes of Iranian Light crude oil destined for the national refinery in Karachi.There were also 440 tonnes of heavy fuel oil in aft bunker tanks.The condition of the grounded tanker deteriorated as she was subjected to continuous stress from the heavy swell of the prevailing south-west monsoon and the vessel subsequently broke in two.In total, it is estimated that some 30,000 tonnes of oil was spilled from the TASMAN SPIRIT. In the course of inspections on board the TASMAN SPIRIT it became apparent that most of the cargo tanks had been ruptured, whilst the bunker tanks remained intact.The owners' appointed salvors and also hired a succession of small tankers and barges for the purpose of shuttling and storing oil lightered from the casualty. During the next few weeks roughly half of the crude oil cargo and most of the bunker fuel was successfully transferred from the casualty.On 11 August the tanker began to show signs of breaking up and eventually broke in two overnight on 13/14 August, spilling several thousand tonnes of crude oil.Much of the spilled oil quickly stranded on Clifton Beach, the main tourist beach in Karachi, but significant quantities remained afloat both inside and outside Karachi port.Dispersants were applied offshore from a Hercules C-130 aircraft equipped with an aerial dispersant spraying system(ADDS Pack) in response to two distinct pollution events involving the progressive break-up of the tanker.Approval for large scale dispersant use was given by the Karachi Port Trust (KPT) and the Pakistan Environment Protection Agency.Oil entering the port of Karachi was confined by deploying booms at suitable collection sites, and in total some 140 tonnes of oil were recovered by skimmers.KPT also deployed vessels to apply dispersant on oil drifting through the port entrance.The severe pollution of Clifton Beach created very strong oil vapours causing considerable discomfort to local residents and clean-up personnel. Local hospitals reported many cases of headaches, nausea and dizziness and seventeen schools in the vicinity were closed for about a week.The beach was cleaned by a combination of manual and mechanical means, but work was hampered by a lack of suitable disposal sites for collected oily waste.Agreement was eventually reached for disposal at one of the municipal waste sites serving Karachi City.Clifton Beach was re-opened to the public in the middle of October.Given the low persistence of Iranian Light crude oil and the high mixing energy in the many damaged cargo tanks generated by the incessant heavy swell, it is likely that most of the spilled oil dispersed naturally.Field surveys conducted showed little or no impact on mangroves, salt pans and other sensitive resources in the vicinity. The geographical extent of shoreline oiling was limited to a ten-mile radius around the grounded tanker.Whilst there were few reports of impacts of the oil on fisheries, a three-month fishing ban was imposed by the Marine Fisheries Department along the coastline directly affected by oil, extending five nautical miles offshore.";

        TextAnalyticsClient _client;

        public RecognizersText()
        {
            ApiKeyServiceClientCredentials credentials = new ApiKeyServiceClientCredentials(NLPConfiguration.SubscriptionKey);
            _client = new TextAnalyticsClient(credentials) { Endpoint = NLPConfiguration.Endpoint };
        }

        /// <summary>
        /// 命名实体识别
        /// </summary>
        /// <param name="client"></param>
        /// <param name="rawText"></param>
        /// <returns></returns>
        private static async Task<EntitiesBatchResult> RecognizeNamedEntity(TextAnalyticsClient client, List<string> sentences)
        {
            List<MultiLanguageInput> list = new List<MultiLanguageInput>() {
                new MultiLanguageInput(language:"en",id: "0",text: string.Join(" ", sentences.ToArray()))
            };
            MultiLanguageBatchInput inputDocuments = new MultiLanguageBatchInput(list);
            EntitiesBatchResult entitiesResult = await client.EntitiesAsync(false, inputDocuments);
            return entitiesResult;
        }

        private static async Task<KeyPhraseBatchResult> RecognizeKeyPhrase(TextAnalyticsClient client, List<string> sentences)
        {
            List<MultiLanguageInput> list = new List<MultiLanguageInput>() {
                new MultiLanguageInput(language:"en",id: "0",text: string.Join(" ", sentences.ToArray()))
            };
            MultiLanguageBatchInput inputDocuments = new MultiLanguageBatchInput(list);
            KeyPhraseBatchResult keyPhraseResult = await client.KeyPhrasesAsync(multiLanguageBatchInput: inputDocuments);
            return keyPhraseResult;
        }

        private static async Task<KeyPhraseBatchResult> RecognizeEntityMention(TextAnalyticsClient client, List<string> sentences)
        {
            List<MultiLanguageInput> list = new List<MultiLanguageInput>() {
                new MultiLanguageInput(language:"en",id: "0",text: string.Join(" ", sentences.ToArray()))
            };
            MultiLanguageBatchInput inputDocuments = new MultiLanguageBatchInput(list);
            KeyPhraseBatchResult keyPhraseResult = await client.KeyPhrasesAsync(multiLanguageBatchInput: inputDocuments);
            return keyPhraseResult;
        }

        [TestMethod]
        public void RecognizeTextNumber()
        {
            List<ModelResult> result = NumberRecognizer.RecognizeNumber(rawText, Culture.English);
        }

        [TestMethod]
        public void RecognizeTextDatetime()
        {
            DateTimeRecognizer recognizer = new DateTimeRecognizer(Culture.English);
            DateTimeModel model = recognizer.GetDateTimeModel();
            List<ModelResult> result = model.Parse(rawText);
        }

        [TestMethod]
        public void RecognizeTextTokenize()
        {
            var result = new SimpleTokenizer().Tokenize(rawText);
        }

        [TestMethod]
        public void RecognizeTextDateTimeSpec()
        {
            //DateTimeSpec timeML = new DateTimeSpec()
            //Microsoft.Bot.Builder.AI.Luis.
        }

        [TestMethod]
        public void RecognizeScenario()
        {
            //1. timeline group
            SentenceGroup sGroup = new SentenceGroup(rawText);
            sGroup.RegroupByTimeline();
            //2. annotation
            IAnnotation annotation = new DepenencyParseAnnotation();
            foreach (var group in sGroup.Groups)
            {
                string rawText = string.Join(".", group.Value.ToArray());
                annotation.Process(rawText);
            }

            //2. key parse for each group
            //foreach (var group in sGroup.Groups)
            //{
            //    //1. entity
            //    EntitiesBatchResult entityResult = RecognizeNamedEntity(_client, group.Value).Result;
            //    KeyPhraseBatchResult keyPhraseResult = RecognizeKeyPhrase(_client, group.Value).Result;
            //    //2. get location and time as scenario 
            //    for(int i=0;i<sGroup.Groups.Count;i++)
            //    {
            //        EntitiesBatchResultItem entityDoc = entityResult.Documents.FirstOrDefault();
            //        KeyPhraseBatchResultItem keyPhraseDoc = keyPhraseResult.Documents.FirstOrDefault();
            //        //3. 根据keyphrase提取的关键词，处理entity

                //    }
                //}

                //3. number with unit parse

                //4. GolVe embedding

                //5. property
        }

        [TestMethod]
        public void RegroupTextByTimeline()
        {
            // split into sentences
            SentenceGroup group = new SentenceGroup(rawText);
            group.RegroupByTimeline();
        }

        [TestMethod]
        public void AnalysisTextEntity()
        {
            //EntitiesBatchResult entitiesResult = RecognizeNamedEntity(_client, rawText).Result;
            //KeyPhraseBatchResult keyPhraseResult = RecognizeKeyPhrase(_client, rawText).Result;
        }


    }
}