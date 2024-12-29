// See https://aka.ms/new-console-template for more information
using Raylib_cs;
using YarEngine;
using YarEngine.Inputs;

class Program 
{
	public static void Main() {
		GameBase.Init("Are you telling me a ray libed this game?");
		/*InputHandler.AddButtonBind("Debug", KeyboardKey.Grave);*/
		GameBase.Run();
	}
}


