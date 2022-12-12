using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Report : MonoBehaviour
{

    public StreamWriter sw;
    public GameObject pts;
    private string path = "Assets/Reports/report.csv";

    public DateTime deltaTime, startGame, startSession, startLevel, now;
    public bool newGame = true;
    public bool newSession = true;
    public bool newLevel = true;
    public TimeSpan deltaTile, deltaGame, deltaLevel, deltaSession;
    void Start()
    {
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Reports/");
        path = Application.persistentDataPath + "/Reports/" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".csv";
        if (File.Exists(path)) File.Delete(path);
        Debug.Log(Application.persistentDataPath);
        
        using (sw = File.CreateText(path))
        {
            sw.WriteLine("Tile, Tile ID, Point, Session (current), Session (total), Level (current), Level (total), Delta Tile (s), Delta Level (s), Delta Session (s), Delta Total (s)");
        }

    }

    public void trackTile(int numTile, string message, bool isError)
    {
        now = DateTime.Now;
        
        using (sw = File.AppendText(path))
        {
            char error;
            if (newGame)startGame = now; newGame = false;
            if (newSession) startSession = now; newSession = false;
            if (newLevel) startLevel = now; newLevel = false;

            deltaTile = now - deltaTime;
            deltaGame = now - startGame;
            deltaLevel = now - startLevel;
            deltaSession = now - startSession;

            if (isError) error = 'X';
            else error = '-';

            // CALCOLI FINITI
            
            sw.WriteLine("{0},{1},{2},{3},{4:D2}.{5:D2}:{6:D3},{7:D2}.{8:D2}:{9:D3},{10:D2}.{11:D2}:{12:D3},{13:D2}.{14:D2}:{15:D3}", 
                message, 
                error,
                numTile, 
                pts.GetComponent<Points>().ReportData(),
                deltaTile.Minutes, 
                deltaTile.Seconds, 
                deltaTile.Milliseconds,
                deltaLevel.Minutes,
                deltaLevel.Seconds, 
                deltaLevel.Milliseconds,
                deltaSession.Minutes,
                deltaSession.Seconds, 
                deltaSession.Milliseconds,
                deltaGame.Minutes,
                deltaGame.Seconds, 
                deltaGame.Milliseconds);
            
            deltaTime = now;

        }




    }
}
