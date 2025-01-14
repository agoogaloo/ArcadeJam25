
using Raylib_cs;

namespace ArcadeJam;

public class Assets {
	public static Font monoFont = LoadFont("res/fonts/monofont.png");
	public static Font smallPixel = LoadFont("res/fonts/smallPixel.png");

	public static Texture2D ui = LoadTexture("res/textures/ui.png");
	public static Texture2D cursor = LoadTexture("res/textures/cursor.png");
	public static Texture2D reelTut = LoadTexture("res/textures/tut/reel.png");
	public static Texture2D rodTut = LoadTexture("res/textures/tut/rod.png");
	public static Texture2D castTut = LoadTexture("res/textures/tut/cast.png");

	public static Texture2D playerPaddle = LoadTexture("res/textures/playerPaddle.png");
	public static Texture2D playerRod = LoadTexture("res/textures/playerRod.png");
	public static Texture2D playerReel = LoadTexture("res/textures/playerReel.png");
	public static Texture2D playerRoll = LoadTexture("res/textures/playerRoll.png");
	public static Texture2D lure = LoadTexture("res/textures/lure.png");

	public static Texture2D fish = LoadTexture("res/textures/fish.png");
	public static Texture2D smallFish = LoadTexture("res/textures/fish/small.png");

	public static Texture2D rock = LoadTexture("res/textures/rock.png");
	public static Texture2D cat = LoadTexture("res/textures/cat.png");
	public static Texture2D ileneL = LoadTexture("res/textures/ileneL.png");
	public static Texture2D ileneR = LoadTexture("res/textures/ileneR.png");
	public static Texture2D warn = LoadTexture("res/textures/warning.png");

	private static Texture2D LoadTexture(String path) {
		Texture2D val = Raylib.LoadTexture(System.AppDomain.CurrentDomain.BaseDirectory + path);
		if (val.Id <= 0) {
			val = Raylib.LoadTexture(path);
		}
		return val;

	}
	private static Font LoadFont(String path) {
		string globalPath = System.AppDomain.CurrentDomain.BaseDirectory + path;
		if (File.Exists(globalPath)) {
			return Raylib.LoadFont(System.AppDomain.CurrentDomain.BaseDirectory + path);
		}
		return Raylib.LoadFont(path);

	}

}

