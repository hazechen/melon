using System;
using System.Collections.Generic;
using AIMLbot;
using ConsoleUtils;
using AutoCompleteUtils;
using System.IO;
using System.Data;

namespace Melon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(80, 20);
            Console.WriteLine("waking up...");

            Brain brain = new Brain();
            ChatBot cb = new ChatBot();
            Console.Clear();
            brain.wakeUp();

            var commands = new List<string>{"exit","start ","open ","start service ","stop service ","restart service ","check service status ","search for ","search procedure for ","procedure ","google ","wiki ","learn ","remove ","help","memory"};
            var memories = new List<string> { };
            string memoryPath = @"C:\Temp\Memories.xml";
            DataTable dtMemory = new DataTable();
            dtMemory.Clear();
            dtMemory.TableName = "memories";
            dtMemory.Columns.Add("input");
            dtMemory.Columns.Add("type");
            dtMemory.Columns.Add("source");
            if (File.Exists(memoryPath))
            {
                dtMemory.ReadXml(memoryPath);
                foreach (DataRow row in dtMemory.Rows)
                {
                    memories.Add(row.ItemArray[0].ToString());
                }
            }
            
            bool running = true;
            var cyclingAutoComplete = new CyclingAutoComplete();
            var cylingCommands = new CyclingAutoComplete();
            while (running)
            {
                var result = ConsoleExt.ReadKey();
                switch (result.Key)
                {
                    case ConsoleKey.Enter:
                        var input = result.LineBeforeKeyPress.Line.ToLower();
                        bool found = false;
                        if (input.ToLower() == "exit")
                        {
                            break;
                        }
                        else if (input.ToLower().Contains("hey melon"))
                        {
                            Console.WriteLine("Melon: Hey :)");
                        }
                        else
                        {
                            foreach (var item in commands)
                            {
                                if (input.StartsWith(item))
                                {
                                    found = true;
                                    brain.getInput(input);
                                    break;
                                }
                            }
                            if (!found)
                            {
                                string output = cb.getOutput(input);
                                if (output.Length == 0)
                                {
                                    output = "Melon: I don't understand.";
                                }
                                else
                                {
                                    Console.WriteLine("Melon: " + output);
                                }
                            }
                        }

                        break;
                    case ConsoleKey.Tab:
                        var shiftPressed = (result.Modifiers & ConsoleModifiers.Shift) != 0;
                        var cyclingDirection = shiftPressed ? CyclingDirections.Backward : CyclingDirections.Forward;
                        var autoCompletedLine =
                            cyclingAutoComplete.AutoComplete(result.LineBeforeKeyPress.LineBeforeCursor,
                                commands, cyclingDirection);
                        if (autoCompletedLine == result.LineBeforeKeyPress.Line.ToString() && autoCompletedLine != "")
                        {
                            string memory = autoCompletedLine.Substring(autoCompletedLine.LastIndexOf(" ") + 1);
                            autoCompletedLine = autoCompletedLine.Substring(0, autoCompletedLine.LastIndexOf(" "));
                            autoCompletedLine += " ";
                            memory =
                            cylingCommands.AutoComplete(memory,
                                memories, cyclingDirection);
                            autoCompletedLine += memory;
                        }
                        ConsoleExt.SetLine(autoCompletedLine);
                        break;
                }

            }
        }
        public class ChatBot
        {
            const string UserId = "vdu";
            private Bot AimlBot;
            private User myUser;

            public ChatBot()
            {
                AimlBot = new Bot();
                myUser = new User(UserId, AimlBot);
                Initialize();
            }
            public void Initialize()
            {
                AimlBot.loadSettings();
                AimlBot.isAcceptingUserInput = false;
                AimlBot.loadAIMLFromFiles();
                AimlBot.isAcceptingUserInput = true;
            }
            public String getOutput(String input)
            {
                Request r = new Request(input, myUser, AimlBot);
                Result res = AimlBot.Chat(r);
                return (res.Output);
            }
        }
    }
}


