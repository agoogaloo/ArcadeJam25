
using Raylib_cs;

namespace ArcadeJam;

public class Assets {
	public static Font monoFont = Raylib.LoadFont("res/fonts/monofont.png");
	public static Font smallPixel = Raylib.LoadFont("res/fonts/smallPixel.png");

	public static Texture2D ui = Raylib.LoadTexture("res/textures/ui.png");
	public static Texture2D reelTut = Raylib.LoadTexture("res/textures/tut/reel.png");
	public static Texture2D rodTut = Raylib.LoadTexture("res/textures/tut/rod.png");
	public static Texture2D castTut = Raylib.LoadTexture("res/textures/tut/cast.png");

	public static Texture2D playerPaddle = Raylib.LoadTexture("res/textures/playerPaddle.png");
	public static Texture2D playerRod = Raylib.LoadTexture("res/textures/playerRod.png");
	public static Texture2D playerReel = Raylib.LoadTexture("res/textures/playerReel.png");
	public static Texture2D playerRoll = Raylib.LoadTexture("res/textures/playerRoll.png");
	public static Texture2D lure = Raylib.LoadTexture("res/textures/lure.png");

	public static Texture2D fish = Raylib.LoadTexture("res/textures/fish.png");
	public static Texture2D smallFish = Raylib.LoadTexture("res/textures/fish/small.png");

	public static Texture2D obstacle = Raylib.LoadTexture("res/textures/icon.png");


}

