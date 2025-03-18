using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Prundung : Bot
{
    int cornerIndex;
    private bool movingToCorner = true;

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

        // Pindah ke corner terdekat
        if (movingToCorner) 
        {
            MoveToCorner();
        }

        while (IsRunning && !movingToCorner)
        {
            TurnGunLeft(10);
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        // Calculate direction of the scanned bot and bearing to it for the gun
        var bearingFromGun = GunBearingTo(e.X, e.Y);

        // Turn the gun toward the scanned bot
        TurnGunLeft(bearingFromGun);

        // If it is close enough, fire!
        if (Math.Abs(bearingFromGun) <= 3 && GunHeat == 0)
            Fire(Math.Min(3 - Math.Abs(bearingFromGun), Energy - .1));

        // Generates another scan event if we see a bot.
        // We only need to call this if the gun (and therefore radar)
        // are not turning. Otherwise, scan is called automatically.
        if (bearingFromGun == 0)
            Rescan();
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        // Pindah ke corner kosong setelah terkena tembakan
        cornerIndex = (cornerIndex + 1) % 4;
        MoveToCorner();

        TurnGunLeft(10);
    }

    public override void OnWonRound(WonRoundEvent e)
    {
        TurnLeft(36_000);
    }

    private int NearestCorner()
    {
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
        movingToCorner = true;

        cornerIndex = NearestCorner(); 
        double angleToCorner = CalcBearing(cornerIndex);

        TurnLeft(angleToCorner);
        Forward(2000);
        TurnRight(90);
        Forward(2000);
        TurnGunRight(90);

        movingToCorner = false; 
    }
}
