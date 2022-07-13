using System;
using System.Collections.Generic;

// Step Option structure for AI. 
// When an winning path is found( there's only 1 point left after AI's turn ),
// an entity would be push into the stack
public class Best_Path
{
	public int steps_have_done;
	public int points_left;
	public int first_step;
}

public static class Globals
{

	// the point player is allowed to remove from the total points
	public static readonly int[] options = {8, 7, 3, 1};

	// stack for storing winning path of AI
	public static Stack<Best_Path> mystack = new Stack<Best_Path>();

/*
purpose: let player take away n points( n is in the scope of [1.3.7,8] ) randomly
*/

	// random_bot pick away random points from allowed scope
	public static int random_bot(int dots_left, int type)
	{
		string botType = (DefineConstants.AI == type)? "AI":"Random";

		if (1 == dots_left)
		{
			Console.Write("Only one point left, {0} bot lose!\n",botType);
			return -1;
		}


		if (dots_left <= 0)
		{
			Console.Write("{0} bot wins!\n", botType);
			return -1;
		}

		int i = 0;
		int k = 0;
		// loop until the option is smaller than dots_left
		do
		{
			k = RandomNumbers.NextNumber() % 4;
			/* code */
		} while (options[k] > dots_left);

		i = options[k];

	  //  while ( options[k]) > dots_left  );

		Console.Write("{0} randomly picks {1:D}, Points left is {2:D}\n", botType, i, dots_left - i);

		if (dots_left - i <= 0)
		{
			Console.Write(" {0} loses!", botType);
		}

		return dots_left - i;
	}

/*
enumerate all the possible path to the result of left point is 1. Pick the path that ensures when the left point is 1, it is the opponent's turn
*/

	// AI would list all possible point taking paths and pick the path that allow it to win
	public static int Ai_bot(Best_Path k)
	{
		if (k.points_left == 1 && 1 == k.steps_have_done % 2)
		{
			mystack.Push(k);
			return 1;
		}

		if (k.points_left <= 0)
		{
		return k.points_left;
		}

		foreach (int i in options)
		{
			return Ai_bot(new Best_Path() {steps_have_done = k.steps_have_done + 1, points_left = k.points_left - i, first_step = k.first_step});
		}

		return -1;
	}

	// Record the points taken away in the first step of the rest game process and points left after this turn
	public static Best_Path init_path(int points_left, int first_step)
	{
		return new Best_Path() {steps_have_done = 1, points_left = points_left - first_step, first_step = first_step};
	}


	internal static void Main()
	{

		int n_points = 50;
		Best_Path AiLastPath = new Best_Path();

		RandomNumbers.Seed();

		// emulate the point taking process and stops when there is no point left and game came to the end
		// n_points is the initial points in the game
		do
		{
		 // random play's turn
		 n_points = random_bot(n_points, DefineConstants.RANDOM);

		if (n_points <= 0)
		{
			break;
		}

		// Ai's turn, when it comes to AI's turn, it needs to calculate the whole tree
		foreach (int first_step in options)
		{
			Ai_bot(init_path(n_points, first_step));
		}

		// view possible best path, if there's best option, pick it, if no, pick a random option.
		while ((mystack.Count > 0) && (1 != mystack.Peek().points_left))
		{
			mystack.Pop();
		}

		if (mystack.Count == 0)
		{
			Console.Write("Ai failed to find an ideal path , so ");
			n_points = random_bot(n_points, DefineConstants.AI);
		}
		else
		{
			AiLastPath = mystack.Peek();
			n_points -= AiLastPath.first_step;
			Console.Write("Ai steps_left is {0:D}, points_left is {1:D}  pick is {2:D}\n", AiLastPath.steps_have_done, n_points, AiLastPath.first_step);
		}

		// empty the stack for AI's next calculation
		mystack = new Stack<Best_Path>();

		} while (n_points > 0);

	}


}

internal static class DefineConstants
{
	public const int AI = 1;
	public const int RANDOM = 0;
}

//Helper class added by C++ to C# Converter:

//----------------------------------------------------------------------------------------
//	Copyright ? 2006 - 2022 Tangible Software Solutions, Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class provides the ability to replicate the behavior of the C/C++ functions for 
//	generating random numbers, using the .NET Framework System.Random class.
//	'rand' converts to the parameterless overload of NextNumber
//	'random' converts to the single-parameter overload of NextNumber
//	'randomize' converts to the parameterless overload of Seed
//	'srand' converts to the single-parameter overload of Seed
//----------------------------------------------------------------------------------------
internal static class RandomNumbers
{
	private static System.Random r;

	public static int NextNumber()
	{
		if (r == null)
			Seed();

		return r.Next();
	}

	public static int NextNumber(int ceiling)
	{
		if (r == null)
			Seed();

		return r.Next(ceiling);
	}

	public static void Seed()
	{
		r = new System.Random();
	}

	public static void Seed(int seed)
	{
		r = new System.Random(seed);
	}
}
