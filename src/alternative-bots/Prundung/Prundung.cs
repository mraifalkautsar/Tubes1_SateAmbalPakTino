using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Prundung : Bot
{
    int cornerIndex; // index sudut yang akan dituju
    private bool hasTarget = false; // apakah bot memiliki target
    private double targetX = 0; // posisi x target
    private double targetY = 0; // posisi y target
    private int targetLostTurns = 0; // jumlah turn sejak target hilang

    static void Main(string[] args)
    {
        new Prundung().Start();
    }

    Prundung() : base(BotInfo.FromFile("Prundung.json")) { }

    public override void Run()
    {
        BodyColor = Color.Orange;
        TurretColor = Color.Red;
        RadarColor = Color.Yellow;
        ScanColor = Color.Green;
        BulletColor = Color.White;

        cornerIndex = NearestCorner();
        MoveToCorner();

        while (IsRunning)
        {
            // jika bot memiliki target, bot akan me-lock target dan menembak
            if (hasTarget)
            {
                double bearingFromGun = GunBearingTo(targetX, targetY);
                double distance = DistanceTo(targetX, targetY);

                TurnGunLeft(bearingFromGun);

                double firepower;
                if (distance < 100)
                    firepower = 3;
                else if (distance < 300)
                    firepower = 2;
                else
                    firepower = 1;

                if (Math.Abs(bearingFromGun) <= 3 && GunHeat == 0)
                    Fire(Math.Min(firepower, Energy - .1));

                targetLostTurns++;
                if (targetLostTurns > 20)
                {
                    hasTarget = false;
                    targetLostTurns = 0;
                }
            }

            // jika bot tidak memiliki target, gun bot akan berputar ke kiri
            else
            {
                TurnGunLeft(10);
            }
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        hasTarget = true;
        targetX = e.X;
        targetY = e.Y;
        targetLostTurns = 0;
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        // jika terkena peluru, bot akan bergerak ke sudut lain dan berhenti menembak
        cornerIndex = (cornerIndex + 1) % 4;
        hasTarget = false;
        targetLostTurns = 0;
        MoveToCorner();
        TurnGunLeft(10);
    }

    public override void OnWonRound(WonRoundEvent e)
    {
        TurnLeft(36_000);
    }

    private int NearestCorner()
    {   
        // menghitung sudut terdekat dari arah hadapa bot
        double currentHeading = Direction;
        double[] corners = { 0, 90, 180, 270 };
        double minDistance = double.MaxValue;
        int nearestCorner = 0;

        foreach (var corner in corners)
        {
            double distance = Math.Abs(currentHeading - corner);
            if (distance > 180)
                distance = 360 - distance;

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCorner = (int)corner;
            }
        }
        return nearestCorner;
    }

    private void MoveToCorner()
    {
        // bergerak ke sudut yang ditentukan
        cornerIndex = NearestCorner(); 
        double angleToCorner = CalcBearing(cornerIndex);

        TurnLeft(angleToCorner);
        Forward(2000);
        TurnRight(90);
        Forward(2000);
        TurnGunRight(90);
    }
}