using System;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
/*
1) Составить программу для обработки результатов кросса на 500 м для женщин.  
Для каждой участницы ввести фамилию, группу, фамилию преподавателя,
результат. Получить результирующую таблицу, упорядоченную по результатам, в  
которой содержится также информация о выполнении норматива. Определить  
суммарное количество участниц, выполнивших норматив.

2.	Сделать абстрактный класс, и от него создать 2-х наследников: бег на 100м и 500м. 
Провести соревнования по забегу на эти 2 дистанции и вывести 2 таблицы с результатами.
*/


namespace lab_7
{
    abstract class Race
    {
        public struct Participant
        {
            private string surname;
            private int group;
            private string surnameCoach;
            private int result;
            private int norm;

            public Participant(string surname, int group, string surnameCoach, int result, int norm)
            {
                this.surname = surname;
                this.group = group;
                this.surnameCoach = surnameCoach;
                this.result = result;
                this.norm = norm;
            }

            public int Result { get { return result; } }
            public bool Passed { get { return result <= norm; } }

            public void Print()
            {
                string status = Passed ? "Норматив выполнен" : "Норматив не выполнен";
                Console.WriteLine($"{surname} {group} {surnameCoach} {result} - {status}");
            }
        }

        public abstract Participant[] StartRace();

        public void RunRace()
        {
            Participant[] participants = StartRace();

            Array.Sort(participants, (a, b) => a.Result.CompareTo(b.Result));

            Console.WriteLine();
            foreach (var participant in participants)
            {
                participant.Print();
            }

            int count = 0;
            foreach (var participant in participants)
            {
                if (participant.Passed)
                {
                    count++;
                }
            }

            Console.WriteLine("Выполнило норматив: " + count);
        }
    }

    class Race100m : Race
    {
        public override Participant[] StartRace()
        {
            Participant[] participants = new Participant[4];

            participants[0] = new Participant("Иванов", 1, "Новиков", 10, 11);
            participants[1] = new Participant("Смирнов", 2, "Лебедев", 12, 11);
            participants[2] = new Participant("Петров", 1, "Новиков", 11, 11);
            participants[3] = new Participant("Соколов", 3, "Волков", 9, 11);
        
            return participants;
        }
    }

    class Race500m : Race
    {
        public override Participant[] StartRace()
        {
            Random random = new Random();
            Participant[] participants = new Participant[3];

            participants[0] = new Participant("Кузнецов", 4, "Иванов", random.Next(60, 100), 75);
            participants[1] = new Participant("Попов", 5, "Соколов", random.Next(60, 100), 75);
            participants[2] = new Participant("Морозов", 4, "Иванов", random.Next(60, 100), 75);

            return participants;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Race100m race100m = new Race100m();
            Race500m race500m = new Race500m();

            Console.WriteLine("Забег на 100м:");
            race100m.RunRace();

            Console.WriteLine();

            Console.WriteLine("Забег на 500м:");
            race500m.RunRace();

            Console.ReadLine();
        }
    }
}