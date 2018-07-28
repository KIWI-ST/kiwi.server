# kiwi.server
kiwi.server is a tool set that integrates gdal, tensroflow, orleans and so on... 

The commonly used operations are packaged, mainly the following modules

### Engine.GIS ####
>a little sample style api library based on gdal. 
>new raster layer
```c#
        GRasterLayer _layer = new GRasterLayer(rasterFilename);
        for (int i = 0; i < _layer.BandCollection.Count; i++)
          IBand band = _layer.BandCollection[0];
          band.BandName = "xxx";
        }
```
>read data form GRasterLayer
```c#
        //get the stretch pixel values of all layers at once
        rasterLayer.GetPixelFloat(x,y)
        //get the raw pixel value of specified band
        rasterLayer.BandCollection[0].GetRawPixel(x,y)
```

### Engine.Brain ###
>implemention of some machinelearning algorithm by tensorflowSharp 
>DQN
```c#
            //can implement the "IDEnv" interface according to your own needs
            IDEnv env = new DImageEnv(featureRasterLayer, labelRasterLayer);
            DQN dqn = new DQN(env);
            //report learning progress
            dqn.OnLearningLossEventHandler += Dqn_OnLearningLossEventHandler;
            dqn.Learn();
```
>use .pb model directly
```c#
            TensorflowBootstrap model = new TensorflowBootstrap(pbName);          
            float[] input = rasterLayer.GetPixelFloat(i, j).ToArray();
            //prediction
            long classified = model.Classify(input, shapeEuum);
```

### Host.UI ###
a winform-based user interface application 
