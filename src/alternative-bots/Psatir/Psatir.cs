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
// The bot will ram you to death if it sees you weak
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
        hittingWall = false;
        centerX = ArenaWidth / 2.0;
        centerY = ArenaHeight / 2.0;
        
        while (IsRunning)
        {
            if (!hittingWall) {
                SetTurnLeft(10_000);
                SetForward(10_000);
                Go();
            }
            else {
                HandleHittingWall();
            }
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        var distance = DistanceTo(e.X, e.Y);
        double firepower;
        if (distance < 100) {
            firepower = 3;
        } else if (distance > 300) {
            firepower = 0.5;
        } else {
            firepower = 3 - (((distance - 100)/200) * 2.5);
        }

        firepower = distance < 100 ? 3 : distance < 300 ? 2 : 1;
        
        if (e.Energy < 10)
        {
            TurnToFaceTarget(e.X, e.Y);
            SetForward(distance + 5);
            SetRescan(); 
        }

		if (Energy > 10) {
        	SetFire(firepower);
		}
        Go();
    }

    private double NormalizeBearing(double angle) {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }

    public override void OnHitBot(HitBotEvent e)
    {
        TurnToFaceTarget(e.X, e.Y);
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

    private void TurnToFaceTarget(double x, double y)
    {
        var bearing = BearingTo(x, y);

        SetTurnLeft(bearing);
        Go();
    }

    private void HandleHittingWall() {
        double distance = DistanceTo(centerX, centerY);
        if (distance > 100) { 
            double bearing = BearingTo(centerX, centerY);
            SetTurnLeft(bearing);
            SetForward(10_000);
        }
        else {
            hittingWall = false;
        }
        Go();
    }

    public override void OnHitWall(HitWallEvent e)
    {	
        hittingWall = true;
        Go();
    }
}