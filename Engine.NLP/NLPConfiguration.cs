using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Engine.NLP
{
    public class NLPConfiguration
    {
        public static void SetGloVeFilename(string gloVeFilename)
        {
            ConfigurationManager.RefreshSection("gloVeFilename");
            ConfigurationManager.AppSettings["gloVeFilename"] = gloVeFilename;
        }
        public static void SetCorpusDir(string corpusDir)
        {
            ConfigurationManager.RefreshSection("corpusDir");
            ConfigurationManager.AppSettings["corpusDir"] = corpusDir;
        }
    }
}
