using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Madan
{
    class Good_match
    {
        public bool validInputs(string name_1, string name_2)
        {
            // Function Checks if inputs are valid and contains only characters using a Regex String
            string checkString = @"^[a-zA-Z]+$";
            if (name_1.Length > 0 && name_2.Length > 0 && Regex.IsMatch(name_1, checkString) && Regex.IsMatch(name_2, checkString))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string get_char_count(String name_1, String name_2)
        {
            String matchString = $"{name_1}matches{name_2}";
            String charCountString = "";

            while(matchString.Length > 0)
            {
                int charCount = 1;

                if (matchString.Length > 1)
                {
                    for(int i=1; i<matchString.Length; i++)
                    {
                        if(matchString.Substring(i,1) == matchString.Substring(0,1))
                        {
                            charCount++;
                        }
                    }
                }
                charCountString += charCount;
                matchString = matchString.Replace(matchString.Substring(0,1), "");
            }
            return charCountString;
        }

        public String get_match_percentage(String charCount)
        {
            String newCharCount = "";
            int length;
            
            if (charCount.ToString().Length == 2)
            {
                return charCount;
            }

            if (charCount.ToString().Length % 2 == 0)
            {
                length = charCount.ToString().Length;
                for (int i=0; i < charCount.ToString().Length/2; i++)
                {
                    newCharCount += Int32.Parse(charCount.Substring(i, 1)) + Int32.Parse(charCount.Substring(length - 1 - i, 1));
                }
            }
            else if(charCount.ToString().Length % 2 == 1)
            {
                length = charCount.ToString().Length;
                for (int i = 0; i < (charCount.ToString().Length - 1) / 2; i++)
                {
                    newCharCount += Int32.Parse(charCount.Substring(i, 1)) + Int32.Parse(charCount.Substring(length - 1 - i, 1));
                }

                int remIdx = (charCount.ToString().Length - 1) / 2;
                newCharCount += Int32.Parse(charCount.Substring(remIdx, 1));
            }
            return get_match_percentage(newCharCount);
        }

        public String displayResult(String name_1, String name_2, String percentage)
        {
            String additional_info = "";
            if(Int32.Parse(percentage) > 80)
            {
                additional_info = ", Good Match";
            }

            return $"{name_1} Matches {name_2} {percentage}%{additional_info}";
        }

        public void match_csv()
        {
            if(File.Exists("people.csv"))
            {
                List<String> males = new List<String>();
                List<String> females = new List<String>();
                List<String> results = new List<String>();

                var reader = new StreamReader(File.OpenRead("people.csv"));
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim() ;
                    var values = line.Split(',');

                    if(values[1].Trim().Equals("m") || values[1].Trim().Equals("M"))
                    {
                        bool in_list = false;
                        foreach(String male in males)
                        {
                            if(male.Equals(values[0]))
                            {
                                in_list = true;
                            }
                        }

                        if (!in_list)
                        {
                            males.Add(values[0]);
                        }

                        // Checks if the person was already in the list, if not, only then will they be added.
                    }
                    else if(values[1].Trim().Equals("f") || values[1].Trim().Equals("F"))
                    {
                        bool in_list = false;
                        foreach (String female in females)
                        {
                            if (female.Equals(values[0]))
                            {
                                in_list = true;
                            }
                        }

                        if (!in_list)
                        {
                            females.Add(values[0]);
                        }
                        // Checks if the person was already in the list, if not, only then will they be added.
                    }
                    else
                    {
                        this.log($"Unknown Gender for {values[0]}");
                    }
                }

                foreach(String male in males)
                {
                    foreach(String female in females)
                    {
                        String char_count = this.get_char_count(male, female);
                        String percentage = this.get_match_percentage(char_count);
                        results.Add(this.displayResult(male, female, percentage));
                        Console.WriteLine(this.displayResult(male, female, percentage));
                    }
                }
                this.print_results(results);
            }
            else
            {
                Console.WriteLine("File Does Not Exist");
                this.log("File Not Found");
            }
            
        }

        public void print_results(List<String> results)
        {
            if (File.Exists("output.txt"))
            {
                TextWriter tw = new StreamWriter("output.txt");

                foreach(String line in results)
                {
                    tw.WriteLine(line);
                }
                tw.Close();
                Console.WriteLine("Results Printed in 'output.txt'");
                this.log("Results Printed in 'output.txt'");
            }
            else
            {
                File.Create("output.txt");
                Console.WriteLine("\n===============\nOutput.txt File Created\n===============");
                this.log("output.txt File Created");
                print_results(results);
            }
        }

        public void log(string error)
        {
            DateTime dt = DateTime.Now;
            String line = $"{dt.ToString()} - {error}";

            if (File.Exists("logs.txt"))
            {
                using (StreamWriter sw = File.AppendText("logs.txt"))
                {
                    sw.WriteLine(line);
                    sw.Close();
                }
            }
            else
            {
                File.Create("logs.txt");
            }
        }

        static void Main(string[] args)
        {
            Good_match gm = new Good_match();
            String n1 = "", n2 = "";
            bool valid = false;

            while (!valid)
            {
                Console.Write("Enter Name #1: ");
                n1 = Console.ReadLine();
                Console.Write("\nEnter Name #2: ");
                n2 = Console.ReadLine();

                if (gm.validInputs(n1, n2))
                {
                    valid = true;
                }
                else
                {

                    gm.log($"Incorrect Inputs - {n1} - {n2}");
                    Console.WriteLine("Incorrect Inputs");
                    // Writes an Error to a log file
                }
                Console.WriteLine("\n");
            }

            String char_count = gm.get_char_count(n1, n2);
            String percentage = gm.get_match_percentage(char_count);
            Console.WriteLine(gm.displayResult(n1, n2, percentage));

            Console.WriteLine("\n\n");
            gm.match_csv();

            Console.ReadLine();
        }
    }


    class Match
    {
        public string Name_1 { get; set; }
    }
}
