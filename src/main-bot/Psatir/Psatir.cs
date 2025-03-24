using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using System.Drawing;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Runtime.CompilerServices;

// ------------------------------------------------------------------
// Psatir
// ------------------------------------------------------------------
// A spinning bot that greedily chases low-on-energy bots.
// ------------------------------------------------------------------
public class Psatir : Bot
{

    static void Main(string[] args)
    {
        new Psatir().Start();
    }

    Psatir() : base(BotInfo.FromFile("Psatir.json")) { }
    bool hittingWall;
    double centerX;
    double centerY;

    public override void Run()
    {
        BodyColor = Color.Blue;
        TurretColor = Color.Blue;
        RadarColor = Color.Black;
        ScanColor = Color.Yellow;

        hittingWall = false; // Boolean untuk mengetahui apakah bot baru saja menabrak tembok
        centerX = ArenaWidth / 2.0;
        centerY = ArenaHeight / 2.0;
        
        while (IsRunning) // Loop utama
        {
            if (!hittingWall) { // Bot berputar-putar dalam state normal
                SetTurnLeft(1000);
                SetForward(1000);
                Go();
            }
            else { // Penanganan untuk state baru saja menabrak tembok
                HandleHittingWall();
            }
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        var distance = DistanceTo(e.X, e.Y); // Hitung jarak ke bot yang di-scan
        double firepower;

        // Hitung firepower berdasarkan jarak
        if (distance < 100) {
            firepower = 3;
        } else if (distance > 300) {
            firepower = 0.5;
        } else {
            firepower = 3 - (((distance - 100)/200) * 2.5);
        }

        // Jika musuh memiliki energi rendah, kejar dan ram!
        if (e.Energy <= 20)
        {
            TurnToFaceTarget(e.X, e.Y);
            SetForward(distance + 5);
            SetRescan(); 
        }

        // Jika musuh masih memiliki energi tinggi, tembak.
		if (Energy > 20) { 
        	SetFire(firepower);
		}
        Go();
    }

    // Metode untuk penanganan jika menabrak bot.
    public override void OnHitBot(HitBotEvent e)
    {
        // Hadapkan tank ke target.
        TurnToFaceTarget(e.X, e.Y);

        // Memaksimalkan poin ramming dengan mengurangi firepower semakin rendah energi musuh.
        if (e.Energy > 16)
            SetFire(3);
        else if (e.Energy > 10)
            SetFire(2);
        else if (e.Energy > 4)
            SetFire(1);
        else if (e.Energy > 2)
            SetFire(.5);
        else if (e.Energy > .4)
            SetFire(.1);

        SetForward(40);
        Go();
    }

    // Hadapkan tank ke target.
    private void TurnToFaceTarget(double x, double y)
    {
        var bearing = BearingTo(x, y);

        SetTurnLeft(bearing);
        Go();
    }


    // Metode untuk penanganan kasus menabrak tembok
    private void HandleHittingWall() {
        double distance = DistanceTo(centerX, centerY); // Hitung jarak ke pusat arena
        if (distance > 100) { 
            // Jika jarak ke pusat masih lebih dari 100 pixel, tank bergerak ke pusat arena.
            double bearing = BearingTo(centerX, centerY);
            SetTurnLeft(bearing);
            SetForward(10_000);
        }
        else {
            // Jika sudah dalam radius 100 dari pusat, kembali ke state default.
            hittingWall = false;
        }
        Go();
    }

    public override void OnHitWall(HitWallEvent e)
    {	
        hittingWall = true; // Aktifkan state hittingWall ketika menabrak tembok.
        Go();
    }
}