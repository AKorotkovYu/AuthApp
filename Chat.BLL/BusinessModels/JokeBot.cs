﻿using System;
using System.Collections.Generic;
using System.Linq;
using OneChat.BLL.Interfaces;

namespace OneChat.BLL.BusinessModel
{
    public class JokeBot: IBot
    {
        private readonly List<string> phrases = new();
        private readonly List<string> jokes = new();
        private readonly Random randomizer = new();

        public JokeBot()
        {
            phrases.Add("скучно");
            phrases.Add("грустно");

            jokes.Add("Шутка 1");
            jokes.Add("Шутка 2");
            jokes.Add("Шутка 3");
            jokes.Add("Шутка 4");
            jokes.Add("Шутка 5");
            jokes.Add("Шутка 6");
            jokes.Add("Шутка 7");
            jokes.Add("Шутка 8");
            jokes.Add("Шутка 9");
        }

        public string Name => "Joker";
       
        public string Execute(string message)
        {
            if (message != null)
            {
                var splittedMessage = message.Split();
                foreach (var spltMsg in splittedMessage)
                    foreach (var phrase in phrases)
                        if (spltMsg == phrase)
                        {
                            string answer = TakeRandAnswer(jokes);
                            return answer;
                        }
            }
            return null;
        }

        string TakeRandAnswer(List<string> lAnswer)
        {

            if (lAnswer.Count == 0)
                return "ERROR";
            int r = randomizer.Next(0, lAnswer.Count);
            var an1 = lAnswer.Skip(r).Take(1).ToArray();

            return an1.FirstOrDefault();
        }
    }
}
