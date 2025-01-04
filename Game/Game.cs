using YarEngine;
using YarEngine.Entities;
using YarEngine.Graphics;
using YarEngine.Inputs;
using YarEngine.Saves;
using Raylib_cs;
using ArcadeJam.Entities;

namespace ArcadeJam;

public class Game {

	public static void Run() {
		//setting up raylib/engine stuff
		SaveManager.savePath = "res/saves/save.txt";
		GameBase.Init("Fishin' Time!");
		GameBase.GameSize = new(200, 150);
		GameBase.borderCol = Globals.palette[0];
		GameBase.debugScreen.SetFont(Assets.monoFont);
		GameBase.debugScreen.RegisterModule(delegate {
			return new YarEngine.Debug.ColVisualiser();
		});
		GameBase.debugScreen.terminal.AddCommand("clearEntityLayer", EntityManager.ClearCommand);


		Raylib.SetWindowIcon(Raylib.LoadImage("res/textures/icon.png"));
		Raylib.SetExitKey(0);

		//creating the camera
		GameCamera gameCam = new(GameBase.GameSize);
		/*gameCam.offset.X = -55;*/

		//setting the key/controller bindings
		InputHandler.AddButtonBind("L", KeyboardKey.Left);
		InputHandler.AddButtonBind("R", KeyboardKey.Right);
		InputHandler.AddButtonBind("U", KeyboardKey.Up);
		InputHandler.AddButtonBind("D", KeyboardKey.Down);
		InputHandler.AddButtonBind("A", KeyboardKey.X);
		InputHandler.AddButtonBind("B", KeyboardKey.Z);
		InputHandler.AddButtonBind("Debug", KeyboardKey.Grave);
		// controller binds
		InputHandler.AddButtonBind("L", GPadInput.RStickLeft);
		InputHandler.AddButtonBind("R", GPadInput.RStickRight);
		InputHandler.AddButtonBind("U", GPadInput.RStickUp);
		InputHandler.AddButtonBind("D", GPadInput.RStickDown);
		InputHandler.AddButtonBind("L", GPadInput.LStickLeft);
		InputHandler.AddButtonBind("R", GPadInput.LStickRight);
		InputHandler.AddButtonBind("U", GPadInput.LStickUp);
		InputHandler.AddButtonBind("D", GPadInput.LStickDown);
		InputHandler.AddButtonBind("A", GPadInput.FaceD);
		InputHandler.AddButtonBind("B", GPadInput.FaceR);

		GameBase.updateMethod = Update;
		GameBase.pixelDraw = PixelDraw;
		GameBase.gameCam = gameCam;

		SaveManager.SaveData<string>("version", "v0.0");

		//adding all the base game components
		EntityManager.QueueEntity(new Player());
		EntityManager.QueueEntity(new Fish(new(50, 50)));

		GameBase.Run();

	}
	private static void Update(float time) {
		EntityManager.Update(time);
		//switching fullscreen/scaling modes
		if (Raylib.IsKeyPressed(KeyboardKey.F1)) {
			switch (GameBase.FullScreen) {
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
		if (Raylib.IsKeyPressed(KeyboardKey.F2)) {
			GameBase.PerfectScaling = !GameBase.PerfectScaling;
		}
	}
	private static void PixelDraw(GameCamera gameCam) {
		Raylib.ClearBackground(Globals.palette[2]);
		EntityManager.Draw(gameCam);
	}
}
