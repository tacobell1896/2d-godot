using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }
	private int _score;

	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<HUD>("HUD").ShowGameOver();
		GetNode<AudioStreamPlayer>("Music").Stop();
		GetNode<AudioStreamPlayer>("DeathSound").Play();
	}

	public void NewGame()
	{
		_score = 0;
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
		player.Start(startPosition.Position);
		
		GetNode<Timer>("StartTimer").Start();
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		GetNode<AudioStreamPlayer>("Music").Play();
	}

	public void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}
	public void OnMobTimerTimeout()
	{
		// create new mob scene instance
		Mob mob = MobScene.Instantiate<Mob>();

		// choose random location on path
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		// set mob's direction perpendicular to path direction
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		// set mob's position to a random location
		mob.Position = mobSpawnLocation.Position;

		// randomize the direction
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;

		// choose the velocity
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		// add mob to scene
		AddChild(mob);

	}
	public void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// NewGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
