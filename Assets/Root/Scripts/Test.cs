using System;
using System.Collections.Generic;
using System.Diagnostics;
using Root.Scripts.Utilities.Logger;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace Root.Scripts
{
    public class Test : MonoBehaviour
    {
        private void Awake()
        {
            
            SceneManager.LoadScene(2);
            /*Random random = new Random();
            SortedSet<double> sortedSet = new SortedSet<double>();
            SortedList<double, double> sortedList = new SortedList<double, double>();
            List<double> list = new List<double>();
            int n = 50000;

            Stopwatch sortedAddTimer = new Stopwatch();
            sortedAddTimer.Start();
            for (int i = 0; i < n; i++)
            {
                sortedSet.Add(i + random.NextDouble() * random.NextDouble());
            }

            sortedAddTimer.Stop();
            TimeSpan sortedAddTimeSpan = sortedAddTimer.Elapsed;
            Log.Console(
                ($@"n = {sortedSet.Count}, Sorted Set Add Elapsed Time: {sortedAddTimeSpan:hh\:mm\:ss\.fff}"));

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            Stopwatch sortedSetTraverseTimer = new Stopwatch();
            sortedSetTraverseTimer.Start();

            foreach (var temp in sortedSet)
            {
                int test = 5;
            }

            sortedSetTraverseTimer.Stop();
            TimeSpan sortedSetTraverseTimeSpan = sortedSetTraverseTimer.Elapsed;
            Log.Console(
                ($@"n = {sortedSet.Count}, Sorted Set Traverse Elapsed Time: {sortedSetTraverseTimeSpan:hh\:mm\:ss\.fff}"));
            //////////////////////////////////////////////////////////////////////////////////////////////////////////


            Stopwatch sortedListAddTimer = new Stopwatch();
            sortedListAddTimer.Start();
            for (int i = 0; i < n; i++)
            {
                sortedList.Add(i + random.NextDouble() * random.NextDouble(), i);
                // sortedList.Add(i, i);
            }

            sortedListAddTimer.Stop();
            TimeSpan sortedListAddTimeSpan = sortedListAddTimer.Elapsed;
            Log.Console(
                ($@"n = {sortedList.Count}, Sorted List Add Elapsed Time: {sortedListAddTimeSpan:hh\:mm\:ss\.fff}"));

            ////////////////////////////////////////////////////////////////////////////////////////////////////////


            Stopwatch sortedListTraverseTimer = new Stopwatch();
            sortedListTraverseTimer.Start();
            for (var i = 0; i < sortedList.Count; i++)
            {
                int test = 5;
            }

            sortedListTraverseTimer.Stop();
            TimeSpan sortedListTraverseTimeSpan = sortedListTraverseTimer.Elapsed;
            Log.Console(
                ($@"n = {sortedList.Count}, Sorted List Traverse Elapsed Time: {sortedListTraverseTimeSpan:hh\:mm\:ss\.fff}"));


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////


            Stopwatch listAddTimer = new Stopwatch();
            listAddTimer.Start();

            for (int i = 0; i < n; i++)
            {
                list.Add(i + random.NextDouble() * random.NextDouble());
                if (random.NextDouble() < 0.0005f)
                {
                    list.Sort();
                }
            }

            listAddTimer.Stop();
            TimeSpan listAddTimeSpan = listAddTimer.Elapsed;
            Log.Console(($@"n = {list.Count}, List Add Elapsed Time: {listAddTimeSpan:hh\:mm\:ss\.fff}"));

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            Stopwatch listTraverseTimer = new Stopwatch();
            listTraverseTimer.Start();

            for (int i = 0; i < n; i++)
            {
                int test = 5;
            }

            listTraverseTimer.Stop();
            TimeSpan listTraverseTimeSpan = listTraverseTimer.Elapsed;

            Log.Console(($@"n = {list.Count}, List Traverse Elapsed Time: {listTraverseTimeSpan:hh\:mm\:ss\.fff}"));


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////


            Stopwatch guidGeneration = new Stopwatch();
            guidGeneration.Start();
            int guid = 99_000;
            for (int i = 0; i < guid; i++)
            {
                Guid test = Guid.NewGuid();
            }

            guidGeneration.Stop();
            TimeSpan guidGenerationTimeSpan = guidGeneration.Elapsed;
            Log.Console(($@"n = {guid}, Guid Generation Elapsed Time: {guidGenerationTimeSpan:hh\:mm\:ss\.fff}"));
        }*/
        }
    }
}