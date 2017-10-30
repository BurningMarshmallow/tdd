using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
	public class Game
	{
		private int[] score = new int[10];
		private int currentFrame = 0;
		private int currentRoll = 1;
		public int strikeFrame;
		public bool wasStrike = false;
		public int strikeAdding;

		
		public void Roll(int pins)
		{
			if (pins > 10) throw new ArgumentException("there is only ten cegls");
			if (pins < 0) throw new ArgumentException("pins should be non-negative number");
			if (pins == 10)
			{
				Strike();
				return;
			}

			if (wasStrike)
			{
				score[strikeFrame] += pins;
				strikeAdding++;
				if (strikeAdding == 2) wasStrike = false;
			}

			var currentScore = score[currentFrame] + pins;
			if (currentScore > 10) throw new ArgumentException();
			score[currentFrame] += pins;
			
			if (currentRoll % 2 == 0)
				currentFrame++;

			currentRoll++;
		}

		public void Strike()
		{
			strikeFrame = currentFrame;
			wasStrike = true;
			score[currentFrame] += 10;
			currentFrame++;
			currentRoll += 2;
		}

		public int GetScore()
		{
			return score.Sum();
		}
	}


	[TestFixture]
	public class Game_should : ReportingTest<Game_should>
	{
		// ReSharper disable once UnusedMember.Global
		public static string Names = "2 Кузьминов Котляров";

		public Game game;

		[SetUp]
		public void SetUp()
		{
			game = new Game();
		}


		[Test]
		public void HaveZeroScore_BeforeAnyRolls()
		{
			game
				.GetScore()
				.Should().Be(0);
		}

		[Test]
		public void HaveRightScore_AfterOneRoll()
		{
			game.Roll(5);
			game.GetScore().Should().Be(5);
		}

		[Test]
		public void ThrowsException_WhenRollIsBiggerThan10()
		{
			Action act = () => game.Roll(322);
			act.ShouldThrow<ArgumentException>();
		}

		[Test]
		public void ThrowsException_WhenScoreOfTwoRollsMoreThan10()
		{
			game.Roll(4);
			Action act = () => { game.Roll(9); };
			act.ShouldThrow<ArgumentException>();
		}

		[Test]
		public void WorksCorrectly_AfterThreeRolls()
		{
			game.Roll(3);
			game.Roll(3);
			game.Roll(3);
			game.GetScore().Should().Be(9);
		}

		[Test]
		public void WorksCorrectly_AfterStrikeBonus()
		{
			game.Roll(10);
			game.Roll(5);
			game.GetScore().Should().Be(20);
		}
	}
}