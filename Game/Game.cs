using YarEngine;
using YarEngine.Entities;
using YarEngine.Graphics;
using YarEngine.Inputs;
using YarEngine.Saves;
using Raylib_cs;

namespace CaptGrapple;

public class Game
{

	public static void Run()
	{
		//setting up raylib stuff
		GameBase.Init("Fishin' Time!");
		GameBase.borderCol = Globals.palette[1];
		GameBase.debugScreen.SetFont(Assets.smallPixel);
		GameBase.debugScreen.RegisterModule(delegate
		{
			return new YarEngine.Debug.ColVisualiser();
		});
		GameBase.debugScreen.terminal.AddCommand("clearEntityLayer", EntityManager.ClearCommand);


		Raylib.SetWindowIcon(Raylib.LoadImage("res/textures/icon.png"));
		Raylib.SetExitKey(0);

		//adding all the base game components

		Menu menu = new();
		menu.StartLevels();
		// EntityManager.QueueEntity(Globals.player);
		EntityManager.QueueEntity(new GameBorders());

		//creatting the camera
		GameCamera gameCam = new(GameBase.GameSize);
		gameCam.offset.X = -55;

		//setting the key/controller bindings
		InputHandler.AddButtonBind("L", KeyboardKey.Left);
		InputHandler.AddButtonBind("R", KeyboardKey.Right);
		InputHandler.AddButtonBind("U", KeyboardKey.Up);
		InputHandler.AddButtonBind("D", KeyboardKey.Down);
		InputHandler.AddButtonBind("A", KeyboardKey.X);
		InputHandler.AddButtonBind("B", KeyboardKey.Z);
		InputHandler.AddButtonBind("Debug", KeyboardKey.Grave);

		GameBase.updateMethod = Update;
		GameBase.pixelDraw = PixelDraw;
		GameBase.gameCam = gameCam;

		SaveManager.SaveData<string>("version", "v0.0");

		GameBase.Run();

	}
	private static void Update(float time)
	{
		EntityManager.Update(time);
		GameMode.ActiveGameMode.Update(time);
		//switching fullscreen/scaling modes
		if (Raylib.IsKeyPressed(KeyboardKey.F1))
		{
			switch (GameBase.FullScreen)
			{
				case FullScreenMode.windowed:
					GameBase.FullScreen = FullScreenMode.borderless;
					break;
				case FullScreenMode.borderless:
					GameBase.FullScreen = FullScreenMode.windowed;
					break;
				case FullScreenMode.fullScreen:
					GameBase.FullScreen = FullScreenMode.borderless;
					break;
			}
		}
		if (Raylib.IsKeyPressed(KeyboardKey.F2))
		{
			GameBase.PerfectScaling = !GameBase.PerfectScaling;
		}
	}
	private static void PixelDraw(GameCamera gameCam)
	{
		Raylib.ClearBackground(Globals.palette[0]);
		EntityManager.Draw(gameCam);
		GameMode.ActiveGameMode.Draw(gameCam);
	}
}
