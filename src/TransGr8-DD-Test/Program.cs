using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Reflection;
using TransGr8_DD_Test.Core;
using TransGr8_DD_Test.Helpers;
using TransGr8_DD_Test.Models;

namespace TransGr8_DD_Test
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            // Configure the logger
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            // Specifying the configuration for serilog
            Log.Logger = new LoggerConfiguration() // initiate the logger configuration
                            .ReadFrom.Configuration(builder.Build()) // connect serilog to our configuration folder
                            .Enrich.FromLogContext() //Adds more information to our logs from built in Serilog 
                            .WriteTo.Console() // decide where the logs are going to be shown
                            .CreateLogger(); //initialise the logger

            Log.Logger.Information("Application Starting");

            Log.Logger.Information("Hello, Welcome to the Dungeons & Dragons Spell Checker!");

            // Load spells list from json file
            string spellsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data/Spells.json");
            List<Spell> spells = await JsonFileReader.ReadAsync<List<Spell>>(spellsPath);

            // Load users list from json file
            string usersPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data/Users.json");
            List<User> users = await JsonFileReader.ReadAsync<List<User>>(usersPath);

            // Initiate the spell checker
            SpellChecker spellChecker = new SpellChecker(spells);

            // Main 
            bool exit = false;
            while (!exit)
            {
                User selectedUser = null;
                Spell selectedSpell = null;

                Log.Logger.Information($"-------------------------------------------------------------");
                Log.Logger.Information($"-Player selection phase--------------------------------------");
                Log.Logger.Information($"-------------------------------------------------------------");
                for (int i = 0; i< users.Count; i++)
                {
                    Log.Logger.Information($"{i} - Player {i}");
                    Log.Logger.Information($"Level : {users[i].Level}");
                    Log.Logger.Information($"Range : {users[i].Range}");
                    Log.Logger.Information($"HasConcentration : {users[i].HasConcentration}");
                    Log.Logger.Information($"HasMaterialComponent : {users[i].HasMaterialComponent}");
                    Log.Logger.Information($"HasSomaticComponent : {users[i].HasSomaticComponent}");
                    Log.Logger.Information($"HasVerbalComponent : {users[i].HasVerbalComponent}");
                    Log.Logger.Information($"-------------------------------------------------------------");
                }
                Log.Logger.Information($"Type a number between {0} and {users.Count - 1}");
                var userChoice = Console.ReadLine();
                if (userChoice == null || !int.TryParse(userChoice, out int userChoiceNumber) || userChoiceNumber < 0 || userChoiceNumber > users.Count -1)
                {
                    Log.Logger.Error("Invalid Number, Type any key to try again");
                    Console.ReadKey();
                    continue;
                }
                else
                {
                    Log.Logger.Information($"Great choice, you have chosen the player number {userChoiceNumber}");
                    selectedUser = users[userChoiceNumber];
                }

                Log.Logger.Information($"-------------------------------------------------------------");
                Log.Logger.Information($"-Spell selection phase---------------------------------------");
                Log.Logger.Information($"-------------------------------------------------------------");
                for (int i = 0; i < spells.Count; i++)
                {
                    Log.Logger.Information($"{i} - Spell {i}");
                    Log.Logger.Information($"Name : {spells[i].Name}");
                    Log.Logger.Information($"Level : {spells[i].Level}");
                    Log.Logger.Information($"CastingTime : {spells[i].CastingTime}");
                    Log.Logger.Information($"SavingThrow : {spells[i].SavingThrow}");
                    Log.Logger.Information($"Components : {spells[i].Components}");
                    Log.Logger.Information($"Duration : {spells[i].Duration}");
                    Log.Logger.Information($"-------------------------------------------------------------");
                }
                Log.Logger.Information($"Type a number between {0} and {spells.Count - 1}");
                var spellChoice = Console.ReadLine();
                if (spellChoice == null || !int.TryParse(spellChoice, out int spellChoiceNumber) || spellChoiceNumber < 0 || spellChoiceNumber > spells.Count -1)
                {
                    Log.Logger.Error("Invalid Number, Type any key to try again");
                    Console.ReadKey();
                    continue;
                }
                else
                {
                    Log.Logger.Information($"Great choice, you have chosen the spell number {spellChoiceNumber}");
                    selectedSpell = spells[spellChoiceNumber];
                }
                Log.Logger.Information($"-------------------------------------------------------------");
                Log.Logger.Information($"-Battle phase------------------------------------------------");
                Log.Logger.Information($"-------------------------------------------------------------");
                Log.Logger.Information($"Type any key to lunch spell {selectedSpell.Name} with the player {userChoice}");
                Console.ReadKey();

                // Use the spell checker to determine if the user can cast the spell.
                if(spellChecker.CanUserCastSpell(selectedUser, selectedSpell.Name))
                {
                    Log.Logger.Information($"Spell Succeeded !");
                }

                else
                {
                    Log.Logger.Information($"Spell Failed !");
                }

                Log.Logger.Information("Game ended, Type any key to try again or \"x\" to exit");
                
                exit = Console.ReadLine() == "x";
                Console.Clear();
            }
        }
    }
}