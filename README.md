# kiwi.server
kiwi.server is a tool set that integrates gdal, accord.net, orleans and so on... you can use the [released software](https://github.com/axmand/kiwi.server/releases) directly

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
>implemention of some machinelearning algorithm by tensorflowSharp.as Deep Q-Learning:
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

### Host.UI ###
a winform-based user interface application 
