using System;
//using System.Diagnostics.Metrics;
/*
3)Соревнования по футболу между командами проводятся в два этапа. Для  проведения первого этапа участники разбиваются на две группы по 12 команд.  
Для проведения второго этапа выбирается 6 лучших команд каждой группы по  результатам первого этапа. Составить список команд участников второго этапа.
2. Создать класс «Футбольная команда» с 2 классами-наследниками: «Женская футбольная команда» и «Мужская футбольная команда». 
Провести среди них отдельные соревнования, но вывести в общей таблице по 6 лучших женских и мужских команд, отсортированных по общему количеству баллов с указанием пола (т.е.: 1. ЦСКА женская команда 13 баллов; 2 Динамо мужская команда 12 баллов; 3 Спартак мужская команда 10 баллов…). 
Использовать динамическую связку: преобразование классов.
*/

namespace lab_7_task_3
{
    class Team
    {
        protected string name;
        protected int count = 0;
        public string Name { get { return name; } }
        public int Count { get { return count; } }
        public Team(string name)
        {
            this.name = name;
        }
        public void Win()
        {
            count++;
        }
        public void Print()
        {
            Console.WriteLine(name + "  count:" + count);
        }
    }
    class FemaleTeam : Team
    {
        public FemaleTeam(string name) : base("female " + name) { }
    }
    class MaleTeam : Team
    {
        public MaleTeam(string name) : base("male " + name) { }
    }
    class Program
    {
        static void Matches(Team[] teams)
        {
            Random rand = new Random();
            int a; int b;
            for (int i = 0; i < teams.Length - 1; i++)
            {
                for (int j = i + 1; j < teams.Length; j++)
                {
                    //Console.WriteLine(teams[i].Name + " - " + teams[j].Name);
                    a = rand.Next(0, 10); b = rand.Next(0, 10);
                    if (a > b)
                    {
                        teams[i].Win(); //Console.WriteLine("Winner - " + teams[i].Name); }
                        if (a < b)
                        {
                            teams[j].Win(); //Console.WriteLine("Winner - " + teams[j].Name); }
                                            //Console.WriteLine();
                        }
                    }
                }
            }
        }
        static void SortTeams(Team[] teams)
        {
            Team val;
            for (int i = 0; i < teams.Length; i++)
            {
                val = teams[i];
                for (int j = i - 1; j >= 0;)
                {
                    if (val.Count > teams[j].Count)
                    {
                        teams[j + 1] = teams[j];
                        j--;
                        teams[j + 1] = val;
                    }
                    else { break; }
                }
            }
        }
        static void Main(string[] args)
        {
            FemaleTeam[] femaleteams = new FemaleTeam[12];
            MaleTeam[] maleteams = new MaleTeam[12];
            for (int i = 0; i < femaleteams.Length; i++)
            {
                femaleteams[i] = new FemaleTeam("team_" + i);
                maleteams[i] = new MaleTeam("team_" + i);
            }
            Matches(maleteams);
            SortTeams(maleteams);
            Matches(femaleteams);
            SortTeams(femaleteams);
            Team[] generalteams = new Team[12];
            for (int i = 0; i < 6; i++)
            {
                generalteams[i] = maleteams[i];
            }
            for (int i = 0; i < 6; i++)
            {
                generalteams[i + 6] = femaleteams[i];
            }
            SortTeams(generalteams);
            for (int i = 0; i < generalteams.Length; i++)
            {
                generalteams[i].Print();
            }
            Console.ReadKey();
        }
    }
}
