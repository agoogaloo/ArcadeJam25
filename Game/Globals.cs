using Raylib_cs;

namespace ArcadeJam;

class Globals {
	public static int globalGameCentre = 83 + 33;
	public static Color[] palette = [new(13, 8, 18, 255), new(22, 44, 84,255),
				 new(28, 76, 98, 255), new(59, 97, 102, 255),
				 new(70, 133, 123,255), new(143, 21, 79, 255),
				 new(184, 45, 70, 255), new(223, 128, 62,255),
				 new(226, 165, 96,255),new(237, 206, 94,255) ];
	public static UI ui = new();
	public static Levels levels;
	public static Score score = new();
}
