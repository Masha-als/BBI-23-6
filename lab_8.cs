using System;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
/* 
 * 1. В заданном тексте определить частоту, с которой встречаются в тексте различные буквы русского 
 * алфавита (в долях от общего количества букв).
 * 
 * 3. Разбить исходный текст на строки длиной не более 50 символов. Перенос на новую 
 * строку осуществлять на месте пробела (слова не переносить).
 * 
 * 6. Определить, сколько слов в тексте содержит один слог, два слога, три слога и т.д.
 * 
 * 12. Считая, что в памяти компьютера хранится таблица кодов часто встречающихся 
 * слов, ввести текст в массив, заменяя слова кодами после ввода. Распечатать текст в 
 * исходном виде, т.е. заменяя коды словами.
 * 
 * 13. Определить долю в процентах слов, начинающихся на различные буквы. 
 * Выписать эти буквы и доли начинающихся на них слов
 * 
 * 15. Текст содержит слова и целые числа произвольного порядка. Найти сумму включенных в текст чисел
 * 
*/
abstract class Task
{
    public Task(string text) { }
    public abstract void ParseText(string text); // все разные
}

class Task_1 : Task
{
    private Dictionary<char, int> wordCounts;
    public Task_1(string text) : base(text)
    {
        wordCounts = new Dictionary<char, int>();
    }
    public override void ParseText(string text)
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
                            else { wordCounts[firstLetter] = 1; }
                        }
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
    public void Print()
    {
        Console.WriteLine("Статистика частоты повторения буквы:");
        foreach (var entry in wordCounts.OrderBy(x => x.Key))
        {
            char letter = entry.Key;
            int count = entry.Value;
            double percentage = (double)count / wordCounts.Values.Sum() * 100;
            Console.WriteLine($"Буква '{letter}': {percentage:F2}%");
        }
    }
}

class Task_3 : Task
{
    private List<string> splittedText;
    public Task_3(string text) : base(text)
    {
        splittedText = new List<string>();
    }

    public override void ParseText(string text)
    {
        int startIndex = 0;
        int endIndex = 50;

        while (startIndex < text.Length)
        {
            int spaceIndex = text.LastIndexOf(' ', endIndex - 1, endIndex - startIndex);

            if (spaceIndex == -1 || spaceIndex == startIndex)
            {
                splittedText.Add(text.Substring(startIndex, endIndex - startIndex));
                startIndex = endIndex; 
            }
            else
            {
                splittedText.Add(text.Substring(startIndex, spaceIndex - startIndex));
                startIndex = spaceIndex + 1;
            }
            endIndex = Math.Min(startIndex + 50, text.Length);
        }
        Console.WriteLine("Разбитый текст:");
        foreach (string line in splittedText)
        {
            Console.WriteLine(line);
        }
    }
}

class Task_6 : Task
{
    public Task_6(string text) : base(text) { }

    public override void ParseText(string text)
    {
        string[] words = text.Split(new char[] { ' ', ',', '.', '!', '?', ')', '(', '"', '-' }, StringSplitOptions.RemoveEmptyEntries);
        int[] syllablesCount = new int[words.Length];
        int i = 0;
        foreach (string word in words)
        {
            int syllables = 0;
            if (!string.IsNullOrEmpty(word))
            {
                foreach (char letter in word.ToLower())
                {
                    if ("aeiouаеёиоуыэюя".Contains(letter))
                    {
                        syllables++;
                    }
                }
                syllablesCount[i] = syllables;
                i++;
            }
        }
        Array.Sort(syllablesCount);
        int count = 0;
        for (int j = 0; j < syllablesCount.Length - 1; j++)
        {
            if (syllablesCount[j] != syllablesCount[j + 1]) 
            {
                Console.WriteLine(syllablesCount[j] + " слог(а): " + count);
                count = 0; 
            }
            else { count++; }
        }
    }
}

class Task_12 : Task
{
    private Dictionary<string, int> table;
    private List<string> words;
    private List<string> mfwords;
    public Task_12(string text) : base(text) 
    {
        table = new Dictionary<string, int>();
        words = new List<string>();
        mfwords = new List<string>();

        mfwords = MostFrequency(text);
       
        for (int i = 0; i < mfwords.Count; i++)
        {
            table.Add(mfwords[i], i);
        }
        string[] splitText = text.Split(' ');
        foreach (string word in splitText)
        {
            words.Add(word);
        }
    }
    public string ReplaceWordsWithCodes()
    {
        for (int i = 0; i < words.Count; i++)
        {
            if (table.ContainsKey(words[i]))
            {
                words[i] = table[words[i]].ToString();
            }
        }
        return string.Join(" ", words);
    }

    public string GetTextWithWords()
    {
        List<string> decodedWords = new List<string>();
        foreach (string word in words)
        {
            if (int.TryParse(word, out int code))
            {
                foreach (var item in table)
                {
                    if (item.Value == code)
                    {
                        decodedWords.Add(item.Key);
                        break;
                    }
                }
            }
            else
            {
                decodedWords.Add(word); // если слово не является кодом, остается как есть
            }
        }
        return string.Join(" ", decodedWords);
    }

    public List<string> MostFrequency(string text)
    {
        Dictionary<string, int> wordFrequency = new Dictionary<string, int>();
        string[] words = text.Split(new char[] { ' ', ',', '.', '!', '?', ')', '(', '"', '-' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in words)
        {
            if (wordFrequency.ContainsKey(word))
            {
                wordFrequency[word]++;
            }
            else
            {
                wordFrequency[word] = 1;
            }
        }

        var sortedWords = wordFrequency.OrderByDescending(pair => pair.Value).Take(5).Select(pair => pair.Key).ToList();
        Console.WriteLine(string.Join(" ", sortedWords));
        return sortedWords;
    }
    public override void ParseText(string text) { }
}

class Task_13 : Task
{
    private Dictionary<char, int> wordCounts;
    public Task_13(string text) : base(text) 
    {
        wordCounts = new Dictionary<char, int>();
    }
    public override void ParseText(string text)
    {
        string[] words = text.Split(new char[] { ' ', ',', '.', '!', '?', ')', '(', '"', '-' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in words) 
        { 
            if (!string.IsNullOrEmpty(word))
            {
                char firstLetter = char.ToLowerInvariant(word[0]);
                if (char.IsLetter(firstLetter)) 
                {
                    if (wordCounts.ContainsKey(firstLetter)) { wordCounts[firstLetter]++; }
                    else { wordCounts[firstLetter] = 1; }
                }
            }
        }
    }
    public void Print()
    {
        Console.WriteLine("Статистика слов, начинающихся на различные буквы:");
        foreach (var entry in wordCounts.OrderBy(x => x.Key))
        {
            char letter = entry.Key;
            int count = entry.Value;
            double percentage = (double)count / wordCounts.Values.Sum() * 100;
            Console.WriteLine($"Буква '{letter}': {percentage:F2}%");
        }
    }
}

class Task_15 : Task
{
    private List<int> digits;
    public Task_15(string text) : base(text) 
    {
        digits = new List<int>();
    }

    public override void ParseText(string text)
    {
        string[] words = text.Split(new char[] { ' ', ',', '.', '!', '?', ')', '(', '"', '-' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in words) 
        {
            string digit = "";
            if (!string.IsNullOrEmpty(word))
            {
                foreach (char letter in word) 
                { 
                    if (char.IsDigit(letter)) { digit += letter; }
                }
                if (digit.Length > 0) { digits.Add(int.Parse(digit)); }
            }
        }
        Console.WriteLine("Сумма всех чисел в тексте:" + digits.Sum(x => x));
    }
}
class Program
{
    public static void Main()
    {
        string inputText = Console.ReadLine();
        Task_1 task1 = new Task_1(inputText);
        task1.ParseText(inputText);
        task1.Print();
        Console.WriteLine();

        inputText = Console.ReadLine();
        Task_3 task3 = new Task_3(inputText);
        task3.ParseText(inputText);
        Console.WriteLine();

        inputText = Console.ReadLine();
        Task_6 task6 = new Task_6(inputText);
        task6.ParseText(inputText);
        Console.WriteLine();

        inputText = Console.ReadLine();
        Task_12 task12 = new Task_12(inputText);
        Console.WriteLine("Текст с замененными кодами:");
        Console.WriteLine(task12.ReplaceWordsWithCodes());
        Console.WriteLine("\nТекст с восстановленными словами:");
        Console.WriteLine(task12.GetTextWithWords());
        Console.WriteLine();

        inputText = Console.ReadLine();
        Task_13 task13 = new Task_13(inputText);
        task13.ParseText(inputText);
        task13.Print();
        Console.WriteLine();

        inputText = Console.ReadLine();
        Task_15 task15 = new Task_15(inputText);
        task15.ParseText(inputText);
        Console.WriteLine();
    }
}