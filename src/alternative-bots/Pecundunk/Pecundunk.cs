using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Pecundunk : Bot
{
    int enemies;
    bool movingForward;
    double myEnergy;
    int tickCounter = 0; // Menghitung iterasi

    static void Main(string[] args)
    {
        new Pecundunk().Start();
    }

    Pecundunk() : base(BotInfo.FromFile("Pecundunk.json")) { }

    public override void Run()
    {
        BodyColor = Color.Pink;
        TurretColor = Color.White;
        RadarColor = Color.Pink;

        enemies = EnemyCount;
        myEnergy = Energy;
        movingForward = true;

        while (IsRunning)
        {
            tickCounter++;  // Menambah iterasi

            if (enemies > 1)
            {
                // Bergerak zig-zag agar lebih sulit ditembak
                SetForward(200 + (tickCounter % 100));
                SetTurnRight(45 - (tickCounter % 90));
            }
            else
            {
                // Jika hanya ada satu musuh, tetap menghindar tetapi sesekali menembak
                SetForward(150);
                SetTurnRight(30);
                if (tickCounter % 20 == 0) // Menembak setiap 20 iterasi
                {
                    Fire(1);
                }
            }
            Go();
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        if (enemies == 1)
        {
            var distance = DistanceTo(e.X, e.Y);
            if (distance < 100)
            {
                Fire(3); // Jika musuh dekat, tembak dengan kekuatan penuh
            }
            else
            {
                Fire(1); // Jika jauh, tembak dengan kekuatan kecil
            }
        }
    }
    // Jika bertabrakan dengan musuh dan energi musuh lebih rendah, tembak
    public override void OnHitBot(HitBotEvent e)
    {
        if (enemies == 1)
        {
            if (myEnergy > e.Energy)
            {
                Fire(3);
                Evade();
            }
            else
            {
                Evade(); // Jika energi lebih rendah, menghindar
            }
        }
    }

    public override void OnHitWall(HitWallEvent e)
    {
        // Jika menabrak dinding, ubah arah agar tidak terjebak
        movingForward = !movingForward;
        SetTurnRight(90);
        if (movingForward)
        {
            SetBack(200);
        }
        else
        {
            SetForward(200);
        }
        Go();
    }

    public override void OnHitByBullet(HitByBulletEvent evt)
    {
        // Jika tertembak, ubah arah agar lebih sulit ditembak lagi
        SetTurnRight(60);
        SetBack(100);
        Go();
    }

    private void Evade()
    {
        // Metode untuk menghindar jika bertabrakan atau ditembak
        SetTurnRight(90);
        SetBack(150);
        Go();
    }
}
