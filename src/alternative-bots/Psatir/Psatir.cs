using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using System.Drawing;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

// ------------------------------------------------------------------
// Psatir
// ------------------------------------------------------------------
// The bot will ram you to death if it sees you weak
// ------------------------------------------------------------------
public class Psatir : Bot
{
    double centerX;
    double centerY;
    
    // The main method starts our bot
    static void Main(string[] args)
    {
        new Psatir().Start();
    }

    // Constructor taking a BotInfo that is forwarded to the base class
    Psatir() : base(BotInfo.FromFile("Psatir.json")) { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        BodyColor = Color.Blue;
        TurretColor = Color.Blue;
        RadarColor = Color.Black;
        ScanColor = Color.Yellow;

		centerX = ArenaWidth / 2.0;
		centerY = ArenaHeight / 2.0;
		double wallMargin = 50;
        // Repeat while the bot is running
        while (IsRunning)
        {
            // Tell the game that when we take move, we'll also want to turn right... a lot
            SetTurnLeft(10_000);
			
            // Limit our speed to 5
            MaxSpeed = 200;
            // Start moving (and turning)
            Forward(10_000);
        }
    }

    // We scanned another bot -> fire hard!
    public override void OnScannedBot(ScannedBotEvent e)
    {
        var distance = DistanceTo(e.X, e.Y);
        double firepower = distance < 100 ? 3 : distance < 300 ? 2 : 1;
        if (e.Energy < 10)
        {
            TurnToFaceTarget(e.X, e.Y);
            Forward(distance + 5);
            Rescan(); // Might want to move forward again!    
        }
		if (Energy > 10) {
        	Fire(firepower);
		}
    }

    private double NormalizeBearing(double angle) {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }

    public override void OnHitBot(HitBotEvent e)
    {
        TurnToFaceTarget(e.X, e.Y);

        // Determine a shot that won't kill the bot...
        // We want to ram it instead for bonus points
        if (e.Energy > 16)
            Fire(3);
        else if (e.Energy > 10)
            Fire(2);
        else if (e.Energy > 4)
            Fire(1);
        else if (e.Energy > 2)
            Fire(.5);
        else if (e.Energy > .4)
            Fire(.1);

        Forward(40); // Ram it again!
    }

    // Method that turns the bot to face the target at coordinate x,y, but also sets the
    // default turn direction used if no bot is being scanned within in the Run() method.
    private void TurnToFaceTarget(double x, double y)
    {
        var bearing = BearingTo(x, y);

        TurnLeft(bearing);
    }

    public override void OnHitWall(HitWallEvent e)
    {	
        
    }
}