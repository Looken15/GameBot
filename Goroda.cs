using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Города
{
    class Program
    {
        static void Main_(string[] args)
        {
            int record = 0;
            string path = "input-files/cities.txt";
            string[] cities = File.ReadAllLines(path).Select(x => x.ToLower()).ToArray();
            bool[] used = new bool[cities.Length].Select(x => false).ToArray();
            

            Rules();
            Console.WriteLine("Итак, приступим. Напишите название любого реального города: ");

            bool flag = false;
            char c = '1';


            while (true)
            {

                string word = Console.ReadLine();
                word = word.ToLower();

                if (word == "завязывай")
                    break;

                while (!cities.Contains(word))
                {
                    Console.WriteLine("Я ещё ни разу не слышал про город с таким странным названием, назовите другой!");
                    word = Console.ReadLine();
                    word = word.ToLower();
                }

                if ((flag) && (word[0] != c))
                {
                    Console.WriteLine("Этот город не подходит! Вы проиграли...");
                    break;
                }

                flag = true;

                
                if ((cities.Contains(word) && (used[Arrind(cities, word)])))
                {
                    Console.WriteLine("Этот город уже был. Вы проиграли...");
                    break;
                }

                if (cities.Contains(word))
                    used[Arrind(cities, word)] = true;


                if ((word[word.Length - 1] != 'ь') && (word[word.Length - 1] != 'ъ'))
                    c = word[word.Length - 1];
                else if ((word[word.Length - 2] != 'ь') && (word[word.Length - 2] != 'ъ'))
                    c = word[word.Length - 2];
                else
                {
                    Console.WriteLine("Сдаётся, что вы меня обманываете. Я ещё ни разу не слышал про город с таким странным названием");
                    break;
                }

                record = record + 5;
                var s = SearchByLetter(cities, used, ref c);
                Console.WriteLine(TIName(s));

                


                if (s == "Похоже, у меня не осталось городов на эту букву. Вы победили")
                {
                    record = record + 100;
                    break;
                }
                    

            }


            Console.WriteLine($"Итак, ваш игровой счёт: {record}");

            var gg = Console.ReadLine(); //бессмысленный ввод. Уберите в релизе! 

        }

        

        static void Rules()
        {
            Console.WriteLine("Отлично! Вы выбрали игру 'Города'.");
            Console.WriteLine("Только давай договоримся, чур без жульничества! Условимся на следующих правилах: ");
            Console.WriteLine("1. Мы называем реально существующие города, хорошо?");
            Console.WriteLine("2. Каждый следующий город начинается на последнюю букву предыдущего");
            Console.WriteLine("3. Буквы Ъ Ь - исключения! Если они нам и попадутся, то следует назвать город, начинающийся с предыдущей буквы");
            Console.WriteLine("4. Города нельзя называть повторно");
            Console.WriteLine("Для выхода из игры напишите слово 'Завязывай' ");
        }

        static int Arrind (string[] arr, string word)
        {
            int res = 0;
            while (arr[res] != word)
                res++;

            return (res);
        }

        static string SearchByLetter(string[] arr,bool[] used,ref char c)
        {
            string res = "Похоже, у меня не осталось городов на эту букву. Вы победили";

            for (var i = 0; i < arr.Length; i++)
                if ((arr[i][0] == c) && (!used[i]))
                {
                    res = arr[i];
                    used[i] = true;
                    break;
                }

            if ((res[res.Length - 1] != 'ь') && (res[res.Length - 1] != 'ъ'))
                c = res[res.Length - 1];
            else c = res[res.Length - 2];
            return (res);
        }

        static string TIName(string word)
        {
            var sb = new StringBuilder(word);
            sb[0] = char.ToUpper(sb[0]);

            return (sb.ToString());
        }
    }
}
