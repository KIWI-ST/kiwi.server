# kiwi.server
kiwi.server is a tool set that integrates gdal, accord.net, cntk to solve some problems in the field of GIS by using machine learning algorihtms...There is complied executable program [released software](https://github.com/axmand/kiwi.server/releases) for testing.
### Examples ###
>code samples in [Examples and Tests](https://github.com/axmand/kiwi.server/tree/master/Examples) are updated as functionality increases
The commonly used operations are packaged, mainly the following modules
### Engine.GIS ####
>a little sample style api library based on gdal. 
```c#
        GRasterLayer _layer = new GRasterLayer(rasterFilename);
        for (int i = 0; i < _layer.BandCollection.Count; i++)
          IBand band = _layer.BandCollection[0];
          band.BandName = "xxx";
        }
```
>read data form GRasterLayer by IRasterTools.
```c#
        //use raster band tool
        IBandCursorTool pBandCursorTool = new GBandCursorTool();
        pBandCursorTool.Visit(band);
        //pick noramlized value at positon (100,200)
        pBandCursorTool.PickNormalValue(100,200);
        //pick raw value at position (100,200)
        pBandCursorTool.PickRawValue(100,200);
        //user raster band stastic tool
        IBandStasticTool pBandStasticTool = new GBandStasticTool();
        pBandStasticTool.Visit(band);
        foreach(var (classIndex,point) in pBandStasticTool.StaisticalRawGraph)
                //do something as you need
```
### Engine.Brain ###
>implemention of some machinelearning algorithm, such as Deep Q-Learning:
```c#
         //can implement the "IDEnv" interface according to your own needs
         IDEnv env = new DImageEnv(featureRasterLayer, labelRasterLayer);
         DQN dqn = new DQN(env);
         //report learning progress
         dqn.OnLearningLossEventHandler += Dqn_OnLearningLossEventHandler;
         dqn.Learn();
```
>the user interface as follow: 

![image](https://user-images.githubusercontent.com/5127112/43514772-77f87c80-95b3-11e8-9a80-20b3945c0f52.png) 

>effective training 

![result3000 1](https://user-images.githubusercontent.com/5127112/46065783-9a762a00-c1a5-11e8-85c3-f800f023791a.png) 

>support kappa index calcute 

![4khp7 z3kjxj n8 xrk8m 0](https://user-images.githubusercontent.com/5127112/46065786-9ba75700-c1a5-11e8-832c-4f7f4fcb9996.png) 

>use .pb model directly.
```c#
         TensorflowBootstrap model = new TensorflowBootstrap(pbName);          
         float[] input = rasterLayer.GetPixelFloat(i, j).ToArray();
         //prediction
         long classified = model.Classify(input, shapeEuum);
```
### Engine.NLP ###
there are servel steps before use it:
+ install java8, setting user environment variables
+ download [stanford nlp](https://stanfordnlp.github.io/CoreNLP/), decompression and move to Debug , rename flodar name to "stanford-corenlp-full"
+ download [glove](https://nlp.stanford.edu/projects/glove/) embedding lexicon, decompression and move to Debug, rename flodar name to "glove-embedding"
+ error: [IKVM BUG 292](https://sourceforge.net/p/ikvm/bugs/292/), copy IKVM lib dlls with name contains "OpenJDK" to Debug. [IKVM ISSUE 296](https://sourceforge.net/p/ikvm/bugs/296/)
```
            //   CC  并列连词           Coordinating conjunction
            //   CD  基数               Cardinal number
            //   DT  限定词             Determiner
            //   EX  存在词             Existential there
            //   FW  外来词             Foreign word
            //   IN  介词               Preposition or subordinating conjunction
            //   JJ  形容词             Adjective
            //  JJR  形容词比较级        Adjective, comparative
            //  JJS  形容词最高级        Adjective, superlative
            //   LS  括号内的数量词       List item marker
            //   MD  情态动词            Modal(can,may,could,might)
            //   NN  名词               Noun, singular or mass
            //  NNS  名词复数            Noun, plural
            //  NNP  专有名词单数        Proper noun, singular
            // NNPS  专有名词复数        Proper noun, plural
            //   NP  专有名词               
            //   NT  词               
            //  PDT  前限定词            Predeterminer
            //  POS  所有格结束词        Possessive ending
            //  PRP  人称代词            Personal pronoun
            // PRP$  物主代词            Possessive pronoun
            //   RB  副词               Adverb
            //  RBR  副词比较级          Adverb, comparative
            //  RBS  副词最高级          Adverb, superlative
            //   RP  助词               Particle
            //  SYM  符号               Symbol
            //   TO                     to
            //   UH  感叹词              Interjection
            //   VB  动词原形            Verb, base form
            //  VBD  动词过去式           Verb, past tense
            //  VBG  动词现在分词         Verb, gerund or present participle
            //  VBN  动词过去分词         Verb, past participle
            //  VBP  动词非第三人称       Verb, non­3rd person singular present
            //  VBZ  动词第三人称单数     Verb, 3rd person singular present
            //  WDT  Wh限定词            Wh­-determiner
            //  WP   Wh代词              Wh­pronoun
            //  WP$  Wh物主代词          Possessive wh-­pronoun
            //  WRB  Wh副词              Wh -­adverb
```
### Host.UI ###
Based on winform user interface, providing related functions above.
