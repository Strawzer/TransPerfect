using NUnit.Framework;
using System.Reflection;
using TransGr8_DD_Test.Core;
using TransGr8_DD_Test.Helpers;
using TransGr8_DD_Test.Models;

namespace TransGr8_DD_Test.Tests
{

	[TestFixture]
	public class SpellCheckerTests
	{


		private List<Spell> spells;
		private User user;

		[SetUp]
		public async Task Setup()
		{
			// Load spells list from json file
            string spellsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data/Spells.json");
            spells = await JsonFileReader.ReadAsync<List<Spell>>(spellsPath);
			
			// Load users list from json file
            string usersPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data/Users.json");
            List<User> users = await JsonFileReader.ReadAsync<List<User>>(usersPath);

			// Get The first user for test
			user = users.FirstOrDefault();
		}

		[Test]
		public void TestCanUserCastSpellReturnsTrue()
		{
			// Arrange
			SpellChecker spellChecker = new SpellChecker(spells);
			string spellName = "Fireball";

			// Act
			bool result = spellChecker.CanUserCastSpell(user, spellName);

			// Assert
			Assert.True(result);
		}

		[Test]
		public void TestCanUserCastSpellReturnsFalseForInsufficientLevel()
		{
			// Arrange
			SpellChecker spellChecker = new SpellChecker(spells);
			string spellName = "Fireball";
			user.Level = 2;

			// Act
			bool result = spellChecker.CanUserCastSpell(user, spellName);

			// Assert
			Assert.False(result);
		}

		[Test]
		public void TestCanUserCastSpellReturnsFalseForMissingVerbalComponent()
		{
			// Arrange
			SpellChecker spellChecker = new SpellChecker(spells);
			string spellName = "Command";
			user.HasVerbalComponent = false;

			// Act
			bool result = spellChecker.CanUserCastSpell(user, spellName);

			// Assert
			Assert.False(result);
		}

		[Test]
		public void TestCanUserCastSpellReturnsFalseForMissingSomaticComponent()
		{
			// Arrange
			SpellChecker spellChecker = new SpellChecker(spells);
			string spellName = "Cure Wounds";
			user.HasSomaticComponent = false;

			// Act
			bool result = spellChecker.CanUserCastSpell(user, spellName);

			// Assert
			Assert.False(result);
		}

		[Test]
		public void TestCanUserCastSpellReturnsFalseForMissingMaterialComponent()
		{
			// Arrange
			SpellChecker spellChecker = new SpellChecker(spells);
			string spellName = "Identify";
			user.HasMaterialComponent = false;

			// Act
			bool result = spellChecker.CanUserCastSpell(user, spellName);

			// Assert
			Assert.False(result);
		}

		[Test]
		public void TestCanUserCastSpellReturnsFalseForInsufficientRange()
		{
			// Arrange
			SpellChecker spellChecker = new SpellChecker(spells);
			string spellName = "Fireball";
			user.Range = 20;

			// Act
			bool result = spellChecker.CanUserCastSpell(user, spellName);

			// Assert
			Assert.False(result);
		}

		[Test]
		public void TestCanUserCastSpellReturnsFalseForMissingConcentration()
		{
			// Arrange
			SpellChecker spellChecker = new SpellChecker(spells);
			string spellName = "Hold Person";
			user.HasConcentration = false;

			// Act
			bool result = spellChecker.CanUserCastSpell(user, spellName);

			// Assert
			Assert.False(result);
		}
	}
}
