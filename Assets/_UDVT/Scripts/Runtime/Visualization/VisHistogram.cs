using System.Linq;
using UnityEngine;
using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//NewCode_Group3
public class VisHistogram : Vis
{
    public VisHistogram()
    {
        title = "Histogram";

        //Define Data Mark and Tick Prefab
        dataMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Cube");
        tickMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/VisContainer/Tick");
    }

    public override GameObject CreateVis(GameObject container)
    {
        base.CreateVis(container);

    
        int numBins = 21; // The number of bins varies based on the formula used in the "Binning" part.
        List<Dictionary<string, double[]>> targetList = new List<Dictionary<string, double[]>>();

        double[] binList = new double[numBins];  // The array that holds the maximum value of the bin intervals.
        double[] numOfApp = new double[numBins]; // Number of appearance for each interval.

        foreach (var dictionary in dataSets)
        {
            Dictionary<string, double[]> newDictionary = new Dictionary<string, double[]>();

            foreach (var keyValuePair in dictionary)
            {

                if (keyValuePair.Key == "economy (mpg)") // keyValuePair.Key is the column that we are using.
                {
                    double[] originalArray = keyValuePair.Value;  // Creating an array that holds every element of the column.


                    /*
                    //NewCode_Group3
                    Task 2: Implement a Histogram
                    Binning
                    At this stage, we used 3 different techniques when determining the number of bins. These are, in order:
                    1 - Sturges' rule
                    2 - Square root rule
                    3 - Rice Root
                    */

                    int numBinsSturges = (int)Math.Ceiling(Math.Log(originalArray.Length, 2) + 1);
                    int numBinsSquareRoot = (int)Math.Ceiling(Math.Sqrt(originalArray.Length));
                    int CubeRoot = (int)Math.Ceiling(Math.Cbrt(originalArray.Length));
                    int numBinsRiceRoot = CubeRoot * 2;


                    Debug.Log("Number of bins (According to Sturges' rule): " + numBinsSturges); // Sturges' rule
                    Debug.Log("Number of bins (According to Square-root choice): " + numBinsSquareRoot); // Square root rule
                    Debug.Log("Number of bins (According to Rice Root): " + numBinsRiceRoot); // Rice Root

                    /*
                    //NewCode_Group3
                    After determining the appropriate number of bins 
                    we calculated the bin size (interval).
                    */

                    Debug.Log("Bin size - Interval (According to Sturges' rule): " + ((originalArray.Max() - originalArray.Min()) / numBinsSturges)); // Sturges' rule
                    Debug.Log("Bin size - Interval (According to Square-root choice): " + ((originalArray.Max() - originalArray.Min()) / numBinsSquareRoot)); // Square root rule
                    Debug.Log("Bin size - Interval (According to Rice Root): " + ((originalArray.Max() - originalArray.Min()) / numBinsRiceRoot)); // Rice Root

            
                    // Determining minimum and maximum values of the column.
                    double minValue = originalArray.Min();
                    double maxValue = originalArray.Max();
                    // Determining the bin size accoring to minimum and maximum values.
                    double binSize = (maxValue - minValue) / numBins;


                    // Determining the maximum value of the each interval by using bin size.
                    for (int i = 0; i < binList.Length; i++)
                    {
                        binList[i] = minValue + ((i+1)*binSize);
                    }

                    // Initializing the each element of the numOfApp array to 0.
                    for(int i = 0; i < numOfApp.Length; i++)
                        numOfApp[i] = 0;


                    foreach (var value in originalArray)
                    {
                        for(int i = 0; i < binList.Length; i++)
                        {
                            if(i == 0) // First element of the loop.
                            {
                                if(value < binList[i])
                                    numOfApp[i] = numOfApp[i] + 1;
                            }
                            else if(value >= binList[i-1] && value < binList[i]) // Non-first element of the loop.
                            {
                                numOfApp[i] = numOfApp[i] + 1;
                            } 

                        }                
                    }
      
                }
            }

        }
    
  

        //## 01:  Create Axes and Grids

        // X Axis
        visContainer.CreateAxis("economy (mpg)", binList, Direction.X);
        visContainer.CreateGrid(Direction.X, Direction.Y); 

        // Y Axis
        visContainer.CreateAxis("Number of Appearance", numOfApp, Direction.Y);


        //## 02: Set Remaining Vis Channels (Color,...)
        visContainer.SetChannel(VisChannel.XPos, binList);
        visContainer.SetChannel(VisChannel.YSize, numOfApp);
        visContainer.SetChannel(VisChannel.Color, dataSets[0].ElementAt(3).Value);

        //## 03: Draw all Data Points with the provided Channels 
        visContainer.CreateDataMarks(dataMarkPrefab);

        //## 04: Rescale Chart
        visContainerObject.transform.localScale = new Vector3(width, height, depth);


        return visContainerObject;
    }
}
