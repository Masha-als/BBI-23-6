using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;
using System.Drawing;

#region общие элементы для 2-х заданий по строкам должны быть в базовом классе
abstract class Task
{
    protected string text = "eeeeeeeee, hello, funny students!";
    public string Text
    {
        get => text;
        protected set => text = value;
    }
    public Task(string text)
    {
        this.text = text;
    }
    public virtual void ParseText() { }

}
#endregion

class Task1 : Task
{
    [JsonConstructor]
    public Task1(string text) : base(text) { }
    public override string ToString()
    {
        return text;
    }
    public override void ParseText()
    {
        string[] words = text.Split(new char[] { ' ', '.', ',', ':', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        Console.WriteLine(string.Join(" ", words));
    }

}

class Task2 : Task
{

    private int amount = 1;
    public int Amount
    {
        get => amount;
        private set => amount = value;
    }
    [JsonConstructor]
    public Task2(string text, int amount = 0) : base(text)
    {
        this.amount = amount;
        wordCounts = new Dictionary<char, int>();

    }
    public override string ToString()
    {
        return amount.ToString();
    }
    private Dictionary<char, int> wordCounts;
    public override void ParseText()
    {

        char firstLetter;
        string[] words = text.Split(new char[] { ' ', ',', '.', '!', '?', ')', '(', '"', '-' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in words)
        {
            if (!string.IsNullOrEmpty(word))
            {
                foreach (char letter in word)
                {
                    if (char.IsLetter(letter))
                    {
                        if (letter == word[0])
                        {
                            firstLetter = char.ToLowerInvariant(word[0]);
                            if (wordCounts.ContainsKey(firstLetter)) { wordCounts[firstLetter]++; }
                            else
                            {
                                if (wordCounts.ContainsKey(letter)) { wordCounts[letter]++; }
                                else { wordCounts[letter] = 1; }
                            }
                        }
                    }
                }
            }
        }
        char maxLetter = wordCounts.MaxBy(entry => entry.Value).Key;
        for (int i = 0; i < words.Length; i++)
        {
            if (!string.IsNullOrEmpty(words[i]))
            {
                foreach (char letter in words[i])
                {
                    if (maxLetter == letter) { words[i] = ""; break; }
                }
            }
        }
        Console.WriteLine(string.Join(" ", words));

    }
}

 


class JsonIO
    {
        public static void Write<T>(T obj, string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize(fs, obj);
            }
        }
        public static T Read<T>(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                return JsonSerializer.Deserialize<T>(fs);
            }
            return default(T);
        }
    }
class Program
    {
        static void Main()
        {

            string text = "eeeeeeeee, hello, funny students!";
            Task[] tasks = { new Task1(text), new Task2(text) };

            tasks[0].ParseText();
            tasks[1].ParseText();

 

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string folderName = "Control work";
            path = Path.Combine(path, folderName);
            if (!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path);
            }
            string fileName1 = "cw_2_task_1.json";
            string fileName2 = "cw_2_task_2.json";

            fileName1 = Path.Combine(path, fileName1);
            fileName2 = Path.Combine(path, fileName2);


            if (!File.Exists(fileName1))
            {
                var filec = File.Create(fileName1);
                filec.Close();
            }



            if (!File.Exists(fileName2)) 
            {
                JsonIO.Write<Task1>(tasks[0] as Task1, fileName1); 
            }
            else 
            {
                var t1 = JsonIO.Read<Task1>(fileName1);
                var t2 = JsonIO.Read<Task2>(fileName2);
                Console.WriteLine(t1);
                Console.WriteLine(t2);
            }
        }
    }
