using System;
using System.Xml.Linq;

/*
 2)Группа учащихся подготовительного отделения сдает выпускные экзамены по  
трем предметам (математика, физика, русский язык). Учащийся, получивший "2"  
сразу отчисляется. Вывести список учащихся, успешно сдавших экзамены, в  
порядке убывания полученного ими среднего балла по результатам трех экзаменов.

2.	Сделать класс «Человек». Класс «Учащийся» должен наследоваться от него, и иметь дополнительное поле «ид учащегося». 
При выводе в таблице должны быть ФИО, ид и средний балл.
 */


namespace lab7_task2
{
    public class Person
    {
        protected string name;

        public Person(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }
    }

    public class Student : Person
    {
        private int id;
        private int markMath;
        private int markRu;
        private int markPh;
        private bool pass;

        public Student(string name, int studentId, int markMath, int markRu, int markPh) : base(name)
        {
            this.id = studentId;
            this.markMath = markMath;
            this.markRu = markRu;
            this.markPh = markPh;
            pass = (markMath > 2 && markRu > 2 && markPh > 2);
        }

        public int Result { get { return (markMath + markRu + markPh) / 3; } }
        public bool Passed { get { return pass; } }

        public override string ToString()
        {
            if (pass && name != null)
            {
                return name + " (ID: " + id + "): " + Convert.ToInt32(Result); ;
            }
            else
            {
                return name + " (ID: " + id + "): отчислен(а)"; ;
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Student[] students = new Student[3];
            students[0] = new Student("Иванов Иван Иванович", 1, 4, 3, 5);
            students[1] = new Student("Петров Пётр Петрович", 2, 3, 4, 2);
            students[2] = new Student("Сидоров Илья Михайлович", 3, 5, 5, 5);

            for (int i = 1; i < students.Length; i++)
            {
                for (int j = i; j > 0 && students[j].Result < students[j - 1].Result; j--)
                {
                    Student temp = students[j];
                    students[j] = students[j - 1];
                    students[j - 1] = temp;

                }
            }
            Array.Reverse(students);

            Console.WriteLine("Таблица учеников:");
            foreach (var student in students)
            {
                Console.WriteLine(student);
            }
        }
    }
}