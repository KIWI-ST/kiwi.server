using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;

namespace Engine.NLP.Utils
{
    public static class Annotation
    {
        /// <summary>
        /// 验证方法
        /// </summary>
        private static ApiKeyServiceClientCredentials credentials = new ApiKeyServiceClientCredentials(NLPConfiguration.SubscriptionKey);

        /// <summary>
        /// Text Analytic 客户端
        /// </summary>
        private static TextAnalyticsClient client = new TextAnalyticsClient(credentials) { Endpoint = NLPConfiguration.Endpoint };

        public static bool IsAlive()
        {
            //client.HttpClient.

            return false;
        }

    }
}
