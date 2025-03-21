using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Psilat : Bot
{
    int turnDirection = 1; // Arah putaran awal
    int enemies; // Jumlah musuh yang terdeteksi

    static void Main(string[] args)
    {
        new Psilat().Start();
    }

    Psilat() : base(BotInfo.FromFile("Psilat.json")) { }

    public override void Run()
    {
        // Warna robot
        BodyColor = Color.Black;
        TurretColor = Color.White;
        RadarColor = Color.Black;

        // Jika tidak menemukan musuh, bot akan berputar mencari musuh
        while (IsRunning)
        {
            TurnLeft(5 * turnDirection);
            enemies = EnemyCount;
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        // Ketika musuh terdeteksi, sesuaikan arah gerak 
        TurnToFaceTarget(e.X, e.Y);
        var distance = DistanceTo(e.X, e.Y);
        // Jika musuh lebih dari satu, dekati musuh
        if (enemies>1){
            // Jika jarak lebih dari 200, maju setengah jarak
            if (distance > 200){
            Forward(distance/2);
            // Jika jarak kurang dari 200, tembak
            } else {
                Fire(3);
            }
        }
        // Jika musuh hanya satu, tabrak musuh
        else {
            Forward(distance+5);
        }
        // Rescan untuk terus mengunci target
        Rescan();
    }

    public override void OnHitBot(HitBotEvent e)
    {
        // Setelah tabrakan, arahkan bot ke musuh
        TurnToFaceTarget(e.X, e.Y);
        // Sesuaikan kekuatan tembakan sesuai energi musuh
        if (e.Energy > 15)
            Fire(3);
        else if (e.Energy > 9)
            Fire(2);
        else if (e.Energy > 6)
            Fire(1);
        else if (e.Energy > 3)
            Fire(.5);
        else if (e.Energy > 1.5)
            Fire(.1);

        // Terus menabrak untuk mengurangi energi musuh
        Forward(40);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        // Jika menabrak dinding, mundur sedikit dan ubah arah
        SetBack(50);
        SetTurnRight(90);
        Go();
    }
    // Fungsi untuk mengubah arah bot ke target
    private void TurnToFaceTarget(double x, double y)
    {
        var bearing = BearingTo(x, y);
        if (bearing >= 0)
            turnDirection = 1;
        else
            turnDirection = -1;

        TurnLeft(bearing);
    }
}
