using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using System.Net;
using MihaZupan;


namespace GameBot
{
    class Program
    {
        //854951383:AAFW1_pt8fFphiqF2ssZsTBc6L4H-4znSIY
        //1038656017:AAHBhqYeyQmAUp6r2KHfEovpmuf5i5_3wvg - основной
        private static readonly TelegramBotClient Bot = new TelegramBotClient("1038656017:AAHBhqYeyQmAUp6r2KHfEovpmuf5i5_3wvg"/*, new WebProxy("217.23.69.146", 8080)*/);

        static void Main(string[] args)
        {
            Bot.OnMessage += Bot_OnMessage;
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }
        public static Random r = new Random();

        /*public static int game_num = -1;*/ // 0 - виселица 
                                             // 1 - города


        //=============================================================ВИСЕЛИЦА=============================================================
        static int[] GuessedLetter(string s, char letter)
        {
            var a = new int[s.Length];
            int j = 0;
            for (var i = 0; i < s.Length; i++)
            {
                if (letter == s[i])
                {
                    a[j] = i;
                    j++;
                }
            }
            for (var i = j; i < s.Length; i++)
            {
                a[i] = -1;
            }
            return a;
        }

        //public static bool viselica_play = false;
        //public static int viselica_life = -1; //количество жизней
        //public static bool viselica_life_chosen = false; // выбрана ли сложность
        //public static int viselica_difficulty = -1; //сложность виселицы
        //public static bool viselica_difficulty_chosen = false; //выбрана ли сложность 
        //public static int viselica_category = -1; //категория виселицы
        //public static string viselica_category_name = ""; // название категории
        //public static bool viselica_category_chosen = false; //выбрана ли категория 
        //public static bool viselica_word_chosen = false; // выбрано ли слово
        //public static string viselica_word = ""; //выбранное слово
        //public static string myWord = ""; //слово,которе будет заполняться буквами по ходу игры
        //public static int in_letter_num = 0; //количество введённых букв
        //public static char letter;
        //public static char[] chr;
        //public static bool viselica_end_game = false; //закончилась ли игра
        //public static char[] in_letters = new char[1000]; //массив уже введённых букв
        //public static char[] in_letters_v = new char[1000]; //массив уже введённых букв всп
        //public static int j = 0;
        //public static int k = 0;



        //=============================================================ГОРОДА=============================================================
        static int Arrind(string[] arr, string word)
        {
            int res = 0;
            while (arr[res] != word)
                res++;

            return (res);
        }

        static string SearchByLetter(string[] arr, bool[] used, ref char c)
        {
            string res = "Похоже, у меня не осталось городов на эту букву. Вы победили";
            var a = new string[arr.Length];
            int j = 0;
            for (var i = 0; i < arr.Length; i++)
            {
                if ((arr[i][0] == c) && (!used[i]))
                {
                    a[j] = arr[i];
                    j++;
                }
            }

            if (a.Length != 0)
            {
                res = a[r.Next(0, j)];
                used[Array.IndexOf(arr, res)] = true;
            }

            if ((res[res.Length - 1] != 'ь') && (res[res.Length - 1] != 'ъ') && (res[res.Length - 1] != 'ы'))
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

        //public static int goroda_record = 0; //счёт
        public static string path = "input-files/cities.txt"; //путь к бд городов
        public static string[] cities = File.ReadAllLines(path).Select(x => x.ToLower()).ToArray(); //массив городов
        //public static bool[] used = new bool[cities.Length].Select(x => false).ToArray(); //массив индексов использованных городов
        //public static bool flag = false;
        //public static char c = '1';
        //public static bool game_over = false;



        //функция,возвращающая произвольное двузначное число с неповторяющимися цифрами 
        static int RandomDigitBot2()
        {
            var r = new Random();
            int res = r.Next(1, 9);
            int last = res;//последняя цифра числа
            while (last == res)
                last = r.Next(0, 9);
            res = res * 10 + last;
            return res;
        }

        //функция,возвращающая произвольное трёхзначное число с неповторяющимися цифрами
        static int RandomDigitBot3()
        {
            var r = new Random();
            var res = RandomDigitBot2();//двузначное с неповторяющимися цифрами
            int last = res % 10;
            while (last == res % 10 | last == res / 10)
                last = r.Next(0, 9);
            res = res * 10 + last;
            return res;
        }

        //функция,возвращающая произвольное четырёхзначное число с неповторяющимися цифрами
        static int RandomDigitBot4()
        {
            var r = new Random();
            var res = RandomDigitBot3();//трёхзначное с неповторяющимися цифрами
            int last = res % 10;
            while (last == res % 10 | last == (res / 10) % 10 | last == res / 100)
                last = r.Next(0, 9);
            res = res * 10 + last;
            return res;
        }

        //функция,которая проверяет,все ли символы в строке различны
        static bool AreAllCharsDifferent(string s)
        {
            bool flag = true;
            for (int i = 0; i < s.Length - 1; i++)
                for (int j = i + 1; j < s.Length; j++)
                    if (s[i] == s[j])
                    {
                        flag = false;
                        break;
                    }
            return flag;
        }

        //функция,которая подсчитывает число коров и быков в введенном числе
        static void HowManyBullsAndCowsInDigit(string mydigit, string sbot, out int bulls, out int cows)
        {
            bulls = 0;
            cows = 0;
            for (int i = 0; i < mydigit.Length; i++)
            {
                if (sbot.Contains(mydigit[i]))
                    if (i == sbot.IndexOf(mydigit[i]))
                        bulls++;
                    else
                        cows++;
            }
        }



        public static Queue<string> q = new Queue<string>();
        public static Dictionary<string, int> dict = new Dictionary<string, int>();
        public static Dictionary<string, int> wordCount = new Dictionary<string, int>();


        static string IsBotWin(string s1, string s2, Dictionary<string, int> dict)
        {
            if (s1 == s2) //выборы одинаковы
            {
                //ничья
                dict["botvictories"]++;
                dict["playervictories"]++;
                return "НИЧЬЯ!";
            }
            else if (s1 == Contr(s2)) //если выбор бота - то, что побеждает выбор игрока
            {
                //бот победил
                dict["botvictories"]++;
                dict["winstreak"] = 0;
                dict["losestreak"]++;
                return "На этот раз бот победил.";
            }
            else //иначе (если выбор игрока - то, что побеждает выбор бота)
            {
                //игрок победил
                dict["playervictories"]++;
                dict["winstreak"]++;
                dict["losestreak"] = 0;
                return "Вы победили, поздравляю.";
            }
        }

        static string Contr(string s)
        {
            if (s == "камень")
                return "бумага";
            else if (s == "ножницы")
                return "камень";
            else //s == "бумага"
                return "ножницы";
        }



        public static LinkedList<string> players_id = new LinkedList<string>();
        public static LinkedList<Player> players = new LinkedList<Player>();

        static Player find_player(string id)
        {
            return players.Where(x => x.user_id == id).First();
        }




        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var text = e.Message.Text;

            var id = e.Message.From.Id.ToString();
            if (!players_id.Contains(id))
            {
                players_id.AddFirst(id);
                players.AddFirst(new Player(id));
            }
            var pl = find_player(id);
            Console.Write(pl.user_id + " " + e.Message.From.FirstName + " " + e.Message.From.LastName + " ");
            Console.WriteLine("ВВОД: " + text);
            if (text == "/start") //приветствие для start
            {
                pl.game_num = -1;
                Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Привет, {e.Message.From.Username}!" +
                    $"\nЯ Игровой Бот! Выбери игру в списке команд!");
                pl.viselica_play = false;
                pl.viselica_life = -1; //количество жизней
                pl.viselica_life_chosen = false; // выбрана ли сложность
                pl.viselica_difficulty = -1; //сложность виселицы
                pl.viselica_difficulty_chosen = false; //выбрана ли сложность 
                pl.viselica_category = -1; //категория виселицы
                pl.viselica_category_name = ""; // название категории
                pl.viselica_category_chosen = false; //выбрана ли категория 
                pl.viselica_word_chosen = false; // выбрано ли слово
                pl.viselica_word = ""; //выбранное слово
                pl.myWord = ""; //слово,которе будет заполняться буквами по ходу игры
                pl.letter = ' ';
                pl.chr = new char[50];
                pl.in_letters = new char[1000];
                pl.in_letters_v = new char[1000];
                pl.j = 0;
                pl.k = 0;
                pl.viselica_end_game = false; //закончилась ли игра
                pl.goroda_record = 0; //счёт
                path = "input-files/cities.txt"; //путь к бд городов
                cities = File.ReadAllLines(path).Select(x => x.ToLower()).ToArray(); //массив городов
                pl.used = new bool[cities.Length].Select(x => false).ToArray(); //массив индексов использованных городов
                pl.flag = false;
                pl.c = '1';
            }
            else if (text == "/viselica") //приветствие для виселицы
            {
                pl.game_num = 0;
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Добро пожаловать в игру 'Виселица'!" +
                    "\nВыберите уровень сложности, введя одно из чисел: " +
                    "\n1-легкий" +
                    "\n2-средний" +
                    "\n3-высокий");
                pl.viselica_play = false;
                pl.viselica_life = -1; //количество жизней
                pl.viselica_life_chosen = false; // выбрана ли сложность
                pl.viselica_difficulty = -1; //сложность виселицы
                pl.viselica_difficulty_chosen = false; //выбрана ли сложность 
                pl.viselica_category = -1; //категория виселицы
                pl.viselica_category_name = ""; // название категории
                pl.viselica_category_chosen = false; //выбрана ли категория 
                pl.viselica_word_chosen = false; // выбрано ли слово
                pl.viselica_word = ""; //выбранное слово
                pl.myWord = ""; //слово,которе будет заполняться буквами по ходу игры
                pl.letter = ' ';
                pl.chr = new char[50];
                pl.in_letters = new char[1000];
                pl.in_letters_v = new char[1000];
                pl.j = 0;
                pl.k = 0;
                pl.viselica_end_game = false; //закончилась ли игра

            }
            else if (text == "/goroda") //приветствие для городов
            {
                pl.game_num = 1;
                pl.game_over = false;
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Добро пожаловать в игру 'Города'!" +
                    "\nТолько давай договоримся, чур без жульничества! Условимся на следующих правилах: " +
                    "\n1. Мы называем реально существующие города, хорошо?" +
                    "\n2. Каждый следующий город начинается на последнюю букву предыдущего" +
                    "\n3. Буквы Ъ Ь - исключения! Если они нам и попадутся, то следует назвать город, начинающийся с предыдущей буквы" +
                    "\n. Города нельзя называть повторно" +
                    "\nДля выхода из игры напишите слово 'Завязывай' ");
                Thread.Sleep(2000);
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Итак, приступим. Напишите название любого реального города: ");
                pl.goroda_record = 0; //счёт
                path = "input-files/cities.txt"; //путь к бд городов
                cities = File.ReadAllLines(path).Select(x => x.ToLower()).ToArray(); //массив городов
                pl.used = new bool[cities.Length].Select(x => false).ToArray(); //массив индексов использованных городов
                pl.flag = false;
                pl.c = '1';
            }
            else if (text == "/quest1")
            {
                pl.game_num = 2;
                pl.offset1 = "0";
            }
            else if (text == "/bulls_cows")
            {
                pl.game_num = 3;
                pl.bulls_difficulty = "0";
                pl.bulls_difficulty_chosen = false;
                pl.print_start_mes1 = false;
                pl.print_start_mes2 = false;
                pl.mydigit = "";
                pl.sbot = "";
                pl.bulls = -1;
                pl.cows = -1;

            }
            else if (text == "/rps")
            {
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Добро пожаловать в игру \"камень-ножницы-бумага\"!");
                Thread.Sleep(500);
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Сколько раундов будем играть?");
                pl.game_num = 4;
                dict["botvictories"] = 0;
                dict["playervictories"] = 0;
                dict["winstreak"] = 0;
                dict["losestreak"] = 0;
                wordCount["ножницы"] = 0;
                wordCount["камень"] = 0;
                wordCount["бумага"] = 0;
                pl.rounds = 0;
                pl.rps_n = 0;
                pl.rps_rand = 0;
                pl.rps_string = "";
                pl.r1_chosen = false;
                pl.r2_chosen = false;
                pl.r3_chosen = false;
                pl.prevbot = "";
                pl.prevplayer = "";
                pl.rounds_chosen = false;
                pl.rps_k = 0;
            }
            else if (text == "/quest2")
            {
                pl.game_num = 5;
                pl.offset2 = "0";
            }




            //Console.WriteLine("Мы здесь " + pl.viselica_difficulty_chosen));
            switch (pl.game_num)
            {
                case 0:
                    {
                        //Console.WriteLine("Мы тут " + pl.game_num);
                        if (text != "/viselica")
                        {
                            if (!pl.viselica_difficulty_chosen)
                            {
                                var text_a = text;
                                pl.viselica_play = true;
                                if (text_a == "1")
                                {
                                    //Console.WriteLine("Мы тут " + pl.user_id);
                                    pl.viselica_difficulty = 1;
                                    pl.viselica_difficulty_chosen = true;
                                }
                                else if (text_a == "2")
                                {
                                    pl.viselica_difficulty = 2;
                                    pl.viselica_difficulty_chosen = true;
                                }
                                else if (text_a == "3")
                                {
                                    pl.viselica_difficulty = 3;
                                    pl.viselica_difficulty_chosen = true;
                                }
                                else
                                {
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Такой сложности нет!");
                                }
                                text = "";
                            } // Выбираем уровень сложности
                            if (pl.viselica_difficulty_chosen && !pl.viselica_category_chosen)
                            {
                                var text_b = text;
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите одну из следующих категорий,введя номер слева от нее:" +
                                                                "\n1-Растения\n2-Животные\n3-Страны мира\n4-Игры\n5-Певцы\n6-Актёры\n7-Футболисты\n8-Математические термины\n9-Национальности России\n10-Автомобили");
                                switch (text_b)
                                {
                                    case "1":
                                        {
                                            pl.viselica_category = 1;
                                            pl.viselica_category_chosen = true;
                                            pl.viselica_category_name = "Растения";
                                            break;
                                        }
                                    case "2":
                                        {
                                            pl.viselica_category = 2;
                                            pl.viselica_category_chosen = true;
                                            pl.viselica_category_name = "Животные";
                                            break;
                                        }
                                    case "3":
                                        {
                                            pl.viselica_category = 3;
                                            pl.viselica_category_chosen = true;
                                            pl.viselica_category_name = "Страны мира";
                                            break;
                                        }
                                    case "4":
                                        {
                                            pl.viselica_category = 4;
                                            pl.viselica_category_chosen = true;
                                            pl.viselica_category_name = "Игры";
                                            break;
                                        }
                                    case "5":
                                        {
                                            pl.viselica_category = 5;
                                            pl.viselica_category_chosen = true;
                                            pl.viselica_category_name = "Певцы";
                                            break;
                                        }
                                    case "6":
                                        {
                                            pl.viselica_category = 6;
                                            pl.viselica_category_chosen = true;
                                            pl.viselica_category_name = "Актёры";
                                            break;
                                        }
                                    case "7":
                                        {
                                            pl.viselica_category = 7;
                                            pl.viselica_category_chosen = true;
                                            pl.viselica_category_name = "Футболисты";
                                            break;
                                        }
                                    case "8":
                                        {
                                            pl.viselica_category = 8;
                                            pl.viselica_category_chosen = true;
                                            pl.viselica_category_name = "Математические термины";
                                            break;
                                        }
                                    case "9":
                                        {
                                            pl.viselica_category = 9;
                                            pl.viselica_category_chosen = true;
                                            pl.viselica_category_name = "Национальности России";
                                            break;
                                        }
                                    case "10":
                                        {
                                            pl.viselica_category = 10;
                                            pl.viselica_category_chosen = true;
                                            pl.viselica_category_name = "Автомобили";
                                            break;
                                        }

                                        //default:
                                        //    {
                                        //        Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Такой категории нет!");
                                        //        break;
                                        //    }
                                }
                                text = "";
                            } // Выбираем категорию
                            if (pl.viselica_difficulty_chosen && pl.viselica_category_chosen && !pl.viselica_word_chosen)
                            {
                                string path = $@"input-files/{pl.viselica_category}.txt";//шаблон файла со словами
                                try
                                {
                                    using (var fs = new FileStream(path, FileMode.Open))
                                    using (var sr = new StreamReader(fs, Encoding.UTF8))
                                    {
                                        while (!(sr.EndOfStream))
                                        {
                                            string str = sr.ReadLine();
                                            string[] arr = str.Split(' ');
                                            foreach (var x in arr)
                                                Console.WriteLine(x);
                                            Random r = new Random();
                                            int i = r.Next(0, arr.Length - 1);//рандомный номер слова в строке
                                            pl.viselica_word = arr[i];
                                            //Console.WriteLine(viselica_word);
                                            //GuessTheWord(arr[i], num, e, Bot);
                                        }
                                    }
                                }
                                catch (FileNotFoundException ex)
                                {
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, ex.Message);
                                }
                                catch (EndOfStreamException ex)
                                {
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, ex.Message);
                                }
                                //Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Выбрано слово:{viselica_word}");
                                pl.viselica_word_chosen = true;
                            } // Выбираем случайное слово из массива выбранной категории

                            if (pl.viselica_difficulty_chosen && pl.viselica_category_chosen && pl.viselica_word_chosen && !pl.viselica_life_chosen)
                            {

                                switch (pl.viselica_difficulty)//уровень сложности
                                {
                                    case 1:
                                        pl.viselica_life = 10; break; //кол-во жизней в соответствии с выбранным уровнем сложности
                                    case 2:
                                        pl.viselica_life = 7; break;
                                    case 3:
                                        pl.viselica_life = 5; break;
                                }
                                for (int i = 0; i < pl.viselica_word.Length; i++)
                                    pl.myWord = pl.myWord + '_';
                                //Bot.SendTextMessageAsync(e.Message.Chat.Id, myWord.Length.ToString());
                                Thread.Sleep(300);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Выбранная категория: {pl.viselica_category_name}" +
                                    $"\nВам нужно отгадать слово из {pl.viselica_word.Length} букв:" +
                                    $"\n{pl.myWord}");
                                Bot.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId + 1);
                                Thread.Sleep(300);
                                pl.chr = pl.myWord.ToCharArray();
                                pl.viselica_life_chosen = true;
                                text = "";
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, $"У вас есть {pl.viselica_life} попыток:" +
                                    $"\nВаша буква:");
                            }
                            if (pl.viselica_difficulty_chosen && pl.viselica_category_chosen && pl.viselica_word_chosen && pl.viselica_life_chosen && !pl.viselica_end_game)
                            {
                                var str = "";
                                var str_let = "";
                                if (pl.chr.Contains('_') && pl.viselica_life != 0)
                                {
                                    //Bot.SendTextMessageAsync(e.Message.Chat.Id, "заходит в цикл");
                                    //Console.WriteLine(text);
                                    if (Regex.IsMatch(text, @"[а-яА-Я]"))
                                    {
                                        if (text.Length == 1)
                                            pl.letter = Convert.ToChar(text.ToLower());//???????????????????????????????????????????????????????????????????????????????????
                                        else
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Введите один символ!");
                                        pl.in_letters_v[pl.k] = pl.letter;
                                        pl.k++;
                                        if (!pl.in_letters.Contains(pl.letter))
                                        {
                                            pl.in_letters[pl.j] = pl.letter;
                                            pl.j++;
                                        }
                                        for (var i = 0; i < pl.in_letters.Length; i++)
                                        {
                                            str_let += pl.in_letters[i] + " ";
                                        }
                                        //Console.WriteLine(in_letters);
                                        if (pl.viselica_word.Contains(pl.letter))
                                        {
                                            if (pl.in_letters_v.Count(x => x == pl.letter) == 1)
                                            {
                                                //Console.WriteLine("угадал");
                                                //Bot.SendTextMessageAsync(e.Message.Chat.Id, "заходит в условие");
                                                int[] a = GuessedLetter(pl.viselica_word, pl.letter);
                                                for (var i = 0; i < pl.myWord.Length; i++)
                                                {
                                                    if (a.Contains(i))
                                                    {
                                                        pl.chr[i] = pl.letter;
                                                    }
                                                }
                                                for (var i = 0; i < pl.chr.Length; i++)
                                                {
                                                    str += pl.chr[i];
                                                }
                                                //Console.WriteLine(str);
                                                if (pl.chr.Contains('_') & (pl.viselica_life > 0))
                                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Отлично! Следующая буква:" +
                                                        $"\nВаше слово: {str}" +
                                                        $"\nПопытки: {str_let}");
                                            }
                                            else
                                            {
                                                for (var i = 0; i < pl.chr.Length; i++)
                                                {
                                                    str += pl.chr[i];
                                                }
                                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вы уже вводили эту букву!" +
                                                        $"\nВаше слово: {str}" +
                                                        $"\nПопытки: {str_let}");
                                            }
                                        }
                                        else

                                        if (!(pl.in_letters_v.Count(x => x == pl.letter) == 1))
                                        {
                                            for (var i = 0; i < pl.chr.Length; i++)
                                            {
                                                str += pl.chr[i];
                                            }
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вы уже вводили эту букву!" +
                                                    $"\nВаше слово: {str}" +
                                                    $"\nПопытки: {str_let}");
                                        }

                                        else
                                        {
                                            //Console.WriteLine(viselica_life);
                                            pl.viselica_life--;
                                            if (pl.chr.Contains('_') & (pl.viselica_life > 0))
                                            {
                                                //Console.WriteLine(viselica_life);
                                                for (var i = 0; i < pl.chr.Length; i++)
                                                {
                                                    str += pl.chr[i];
                                                }
                                                Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Такой буквы нет!" +
                                                    $"\nУ вас осталось {pl.viselica_life} попыток. Итак, ваша буква?" +
                                                    $"\nВаше слово: {str}" +
                                                    $"\nПопытки: {str_let}");
                                            }
                                        }
                                        //break;
                                        //Console.WriteLine(letter);
                                        //Bot.SendTextMessageAsync(e.Message.Chat.Id, letter.ToString());
                                    }
                                    //break;
                                    //Bot.SendTextMessageAsync(e.Message.Chat.Id, letter.ToString());
                                    if (!(pl.chr.Contains('_') && pl.viselica_life != 0))
                                    {
                                        pl.viselica_end_game = true;
                                    }
                                }
                            }
                            if (pl.viselica_difficulty_chosen && pl.viselica_category_chosen && pl.viselica_word_chosen && pl.viselica_life_chosen && pl.viselica_end_game)
                            {
                                Console.WriteLine("арура");
                                if (!pl.chr.Contains('_'))
                                {
                                    Console.WriteLine("выиграл");
                                    //Bot.SendTextMessageAsync(e.Message.Chat.Id, (myWord);
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Поздравляем, вы выиграли!");//выигрыш
                                }
                                else
                                {
                                    Console.WriteLine("проиграл");
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over!" +
                                        $"\nБыло загадано слово: {pl.viselica_word}");//проигрыш
                                }
                                pl.game_num = -1; // 0 - виселица 
                                pl.viselica_play = false;
                                pl.viselica_life = -1; //количество жизней
                                pl.viselica_life_chosen = false; // выбрана ли сложность
                                pl.viselica_difficulty = -1; //сложность виселицы
                                pl.viselica_difficulty_chosen = false; //выбрана ли сложность 
                                pl.viselica_category = -1; //категория виселицы
                                pl.viselica_category_name = ""; // название категории
                                pl.viselica_category_chosen = false; //выбрана ли категория 
                                pl.viselica_word_chosen = false; // выбрано ли слово
                                pl.viselica_word = ""; //выбранное слово
                                pl.myWord = ""; //слово,которе будет заполняться буквами по ходу игры
                                pl.letter = ' ';
                                pl.chr = new char[50];
                                pl.in_letters = new char[1000];
                                pl.in_letters_v = new char[1000];
                                pl.j = 0;
                                pl.k = 0;
                                pl.viselica_end_game = false; //закончилась ли игра
                            }
                        }

                    }
                    break;

                case 1:
                    {
                        if (!pl.game_over)
                        {
                            if (text != "/goroda")
                            {
                                var word = text.ToLower();

                                if ((word == "завязывай" || word == "Завязывай") && !pl.game_over)
                                {
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вы завершили игру досрочно!" + $"\nВаш счёт: {pl.goroda_record}");
                                    pl.goroda_record = 0; //счёт
                                    path = "input-files/cities.txt"; //путь к бд городов
                                    cities = File.ReadAllLines(path).Select(x => x.ToLower()).ToArray(); //массив городов
                                    pl.used = new bool[cities.Length].Select(x => false).ToArray(); //массив индексов использованных городов
                                    pl.flag = false;
                                    pl.c = '1';
                                    pl.game_over = true;
                                }

                                if (!cities.Contains(word) && !pl.game_over)
                                {
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я ещё ни разу не слышал про город с таким странным названием, назовите другой!");
                                    Console.WriteLine("не подходит ");
                                    Console.WriteLine(pl.c);
                                    //flag = false;
                                    break;
                                }
                                else
                                if ((pl.flag) && (word[0] != pl.c) && !pl.game_over)
                                {
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Этот город не подходит! Вы проиграли..." +
                                        $"\nВаш счёт: {pl.goroda_record}");
                                    pl.goroda_record = 0; //счёт
                                    path = "input-files/cities.txt"; //путь к бд городов
                                    cities = File.ReadAllLines(path).Select(x => x.ToLower()).ToArray(); //массив городов
                                    pl.used = new bool[cities.Length].Select(x => false).ToArray(); //массив индексов использованных городов
                                    pl.flag = false;
                                    pl.c = '1';
                                    pl.game_over = true;
                                    break;
                                }
                                pl.flag = true;
                                if ((cities.Contains(word) && (pl.used[Arrind(cities, word)])) && !pl.game_over)
                                {
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Этот город уже был. Вы проиграли...");
                                    pl.game_over = true;
                                    break;
                                }
                                if (cities.Contains(word))
                                    pl.used[Arrind(cities, word)] = true;
                                if ((word[word.Length - 1] != 'ь') && (word[word.Length - 1] != 'ъ') && (word[word.Length - 1] != 'ы'))
                                    pl.c = word[word.Length - 1];
                                else if ((word[word.Length - 2] != 'ь') && (word[word.Length - 2] != 'ъ') && (word[word.Length - 2] != 'ы'))
                                    pl.c = word[word.Length - 2];
                                else
                                {
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Сдаётся, что вы меня обманываете. Я ещё ни разу не слышал про город с таким странным названием");
                                    break;
                                }
                                Console.WriteLine(pl.c);
                                if (cities.Contains(word) && !pl.game_over && (pl.c == word[word.Length - 1] || pl.c == '1'))
                                {
                                    pl.goroda_record += 5;
                                    var s = SearchByLetter(cities, pl.used, ref pl.c);
                                    Console.WriteLine("подходит");
                                    Console.WriteLine(pl.c);
                                    var z = TIName(s);
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, z);
                                    Console.WriteLine(z);
                                    break;
                                }

                                //Console.WriteLine(c);
                            }
                            else
                            {
                                pl.flag = false;
                                break;
                            }
                        }
                        else
                        {
                            Bot.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                            pl.flag = false;
                            break;
                        }
                    }
                    break;

                case 2:
                    {
                        if (text == "/quest1" && pl.offset1 == "0")
                        {
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, ("Привет?..")); //1
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, ("Это кто-нибудь читает?")); //2
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, ("1.– Да, я читаю!" +    //2.1
                                "\n2.– Что?.. Ты кто?"));                                        //2.2
                            break;
                        }
                        if (text == "1" && pl.offset1 == "0")
                        {
                            pl.offset1 = "2.1";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, ("У меня большие проблемы… Пожалуйста, выслушай меня!")); //3.1
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, ("1.– Что за проблемы?" +    //3.1.1
                                "\n2.– Звучит, как какая-то шутка..."));                             //3.1.2
                            break;
                        }
                        if (text == "2" && pl.offset1 == "2.1")
                        {
                            pl.offset1 = "3.1.1";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я не шучу, честно!"); //4
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Ладно, поверю тебе." +             //4.1
                                "\n2.– Опять школьники ерундой занимаются...");                                 //4.2
                            break;
                        }
                        if (text == "2" && pl.offset1 == "3.1.1")
                        {
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                            break;
                        }

                        if (text == "2" && pl.offset1 == "0")
                        {
                            pl.offset1 = "2.2";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я – человек в беде, помоги мне, прошу!"); //3.2
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Что у тебя случилось?" +    //3.2.1
                                "\n2.- Что за глупый розыгрыш...");                                      //3.2.2
                            break;
                        }
                        if (text == "2" && pl.offset1 == "2.2")
                        {
                            pl.offset1 = "3.2.1";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я не шучу, честно!"); //4
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Ладно, поверю тебе." +             //4.1
                                "\n2.– Опять школьники ерундой занимаются...");                                 //4.2
                            break;
                        }
                        if (text == "2" && pl.offset1 == "3.2.1")
                        {
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                            break;
                        }

                        if ((text == "1" && pl.offset1 == "2.1") || (text == "1" && pl.offset1 == "3.1.1") || (text == "1" && pl.offset1 == "2.2") || (text == "1" && pl.offset1 == "3.2.1"))
                        {
                            pl.offset1 = "5";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Произошло что-то непонятное… Я как будто попал в страшный сон."); //5
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Мы с другом собрались фотографировать природу."); //6
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Собрали рюкзаки, взяли фотоаппараты  и отправились в лес на окраине нашего города."); //7
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– В лес?! И ты ещё удивляешься, что произошло что-то плохое!" + //7.1
                            "\n2.– Продолжай.");                                                                                           //7.2
                            break;
                        }

                        if (pl.offset1 == "5")
                        {
                            if (text == "1")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Да мы же не собирались лезть в чащу! Просто хотели побродить по полянам, около проезжей части.");  //8
                                text = "2";
                            }
                            if (text == "2")
                            {
                                pl.offset1 = "11";
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "В общем, мы гуляли часа два и решили остановиться, чтобы передохнуть и поесть."); //9
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Последнее, что я помню, это то, что жевал бутерброд, и что мой друг вскрикнул..."); //10
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "А потом меня вырубили."); //11
                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Какой ужас, как ты сейчас?" + //11.1
                                "\n2.– Хочешь сказать, кому-то очень приглянулись ваши бутеры?");              //11.2
                                break;
                            }
                        }

                        if (pl.offset1 == "11")
                        {
                            if (text == "2")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Не стану это отрицать!"); //12
                                text = "1";
                            }
                            if (text == "1")
                            {
                                pl.offset1 = "16";
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Кроме головы ничего не болит, руки-ноги на месте. "); //13
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Крови нигде нет, как и половины вещей. Другая половина валяется по всей округе."); //14
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Скорее всего, это было банальное ограбление."); //15
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Моего друга тоже тут нет. А время-то близится к вечеру..."); //16
                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Подожди, а тогда откуда у тебя телефон?" +      //16.1
                                "\n2.– И ни одного бутерброда не осталось?");                                                    //16.2
                                break;
                            }
                        }

                        if (pl.offset1 == "16")
                        {
                            if (text == "2")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Да чего ты пристал с этими бутербродами? Сейчас не до этого!"); //17

                                text = "1";
                            }
                            if (text == "1")
                            {
                                pl.offset1 = "24";
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Этот телефон валялся в траве."); //18
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Не похож ни на мой, ни на телефон друга. Видимо его обронил тот, кто нас ограбил."); //19
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "А может, он и убить нас хотел, кто знает..."); //20
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ладно, сейчас это неважно."); //21
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я пробовал позвонить в службу спасения или хотя бы маме, но связь не ловит. Только сообщения отправляются, и то с задержкой."); //22
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "И кстати, ты первый, кто отозвался на мои крики о помощи."); //23
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Спасибо, что ты со мной!"); //24
                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Пока не за что. Как думаешь выбираться?" + //24.1
                                "\n2.– Не спеши радоваться, ты всё ещё застрял в какой-то дыре.");                          //24.2
                                break;
                            }
                        }

                        if (pl.offset1 == "24")
                        {
                            if (text == "2")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ты прав, пора действовать."); //25
                                text = "1";
                            }
                            if (text == "1")
                            {
                                pl.offset1 = "29";
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Сейчас вечереет, а я сижу на той же поляне с телефоном в руках."); //26
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "По мху я вижу, где юг, а где север, спасибо школьным урокам ОБЖ."); //27
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Однако на эту поляну меня привёл друг, и я не знаю, в какой стороне проезжая часть."); //28
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "А ещё я очень голоден."); //29
                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Предлагаю тебе поискать чего-нибудь съедобного." + //29.1
                                "\n2.– А до дома не дотерпишь? ");                                                                  //29.2
                                break;
                            }
                        }

                        if (pl.offset1 == "29")
                        {
                            if (text == "1")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Отличная идея! Так и сделаю."); //30.1
                                pl.offset1 = "31";
                            }
                            else if (text == "2")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ты смеёшься? Неизвестно, сколько мне ещё тут бродить!"); //30.2
                                pl.offset1 = "31";
                            }

                        }

                        if (pl.offset1 == "31")
                        {
                            pl.offset1 = "33";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Погоди пару минут, я осмотрю окрестности."); //31
                            Thread.Sleep(15000); //таймер  
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Есть небольшой улов."); //32
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Тут какие-то орехи и ягоды."); //33
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Что за орехи? " + //33.1
                            "\n2.– Ты, знаешь, что это за ягоды? ");                           //33.2
                            break;
                        }

                        if (pl.offset1 == "33" && text == "1")
                        {
                            pl.offset1 = "34.1.1";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Грецкие.Можно было бы расколоть скорлупу, если грабитель не забрал нож..."); //34.1
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Хм... Тогда расскажи про ягоды. "); //34.1.1
                            break;
                        }
                        if (pl.offset1 == "34.1.1" && text == "1")
                        {
                            pl.offset1 = "34.2";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Тёмно-синие. Похожи на чернику. Или на голубику."); //34.2

                        }

                        if (pl.offset1 == "33" && text == "2")
                        {
                            pl.offset1 = "34.2.1";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Тёмно-синие. Похожи на чернику. Или на голубику."); //34.2
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Смотрю, ты не эксперт в ягодах. А что по орехам?"); //34.2.1
                            break;
                        }
                        if (pl.offset1 == "34.2.1" && text == "1")
                        {
                            pl.offset1 = "34.2";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Грецкие.Можно было бы расколоть скорлупу, если грабитель не забрал нож..."); //34.2
                        }

                        if (pl.offset1 == "34.2")
                        {
                            pl.offset1 = "35";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Что думаешь?"); //35
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Неизвестно, что это за ягоды на самом деле. Не стоит рисковать." + //35.1
                            "\n2.– Ну, раз от скорлупы никак не избавиться, то выбора у тебя нет.");                                            //35.2
                            break;
                        }

                        if (pl.offset1 == "35" && text == "1")
                        {
                            pl.offset1 = "36.1";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "И что ты предлагаешь? Умереть с голоду?!"); //36.1
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Да, а как ты догадался? " + //36.1.1
                            "\n2.– Ха-ха. Нож ищи давай!");                                              //36.1.2
                            break;
                        }
                        if (pl.offset1 == "36.1" && text == "1")
                        {
                            pl.offset1 = "36.5";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вот такой я у мамы умный."); //37
                        }

                        if (pl.offset1 == "35" && text == "2")
                        {
                            pl.offset1 = "36.2";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ты уверен? Я тут подумал… А вдруг они ядовитые?"); //36.2
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Здравая мысль. Может, лучше поискать нож?" + //36.2.1
                            "\n2.– Ты же сказал, что это черника. Так какие проблемы? ");                                 //36.2.2
                            break;
                        }

                        if ((pl.offset1 == "36.2" && text == "1") || (pl.offset1 == "36.1" && text == "2"))
                        {
                            pl.offset1 = "36.3";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Действительно. Этим и займусь."); //36.3
                        }
                        if (pl.offset1 == "36.2")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Уговорил."); //36.4
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }

                        if (pl.offset1 == "36.3" || pl.offset1 == "36.5")
                        {
                            pl.offset1 = "42";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ладно, пойду, обыщу поляну."); //38
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Может, повезёт, и я найду свой рюкзак..."); //39
                            Thread.Sleep(10000); //таймер
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Нашел!"); //40
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вот он, мой родной. Как же я по тебе скучал."); //41
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Теперь я и с голоду не помру, и от волков если что отобьюсь!"); //42
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Как романтично, я сейчас заплачу. " +     //42.1
                            "\n2.– Кстати о волках. Ты говорил, что  в этом лесу есть чаща, а ещё скоро стемнеет..."); //42.2
                            break;
                        }

                        if (pl.offset1 == "42" && text == "1")
                        {
                            pl.offset1 = "43.2.1";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Как хорошо, что я этого всё равно не увижу. И вообще, дай поесть."); //43.1
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Приятного аппетита." + //43.2.1
                            "\n2.– Не подавись."); //43.2.2
                            break;
                        }
                        if (pl.offset1 == "43.2.1" && text == "1")
                        {
                            pl.offset1 = "44";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Спасибо!"); //44.1
                        }
                        if (pl.offset1 == "43.2.1" && text == "2")
                        {
                            pl.offset1 = "44";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Обязательно."); //44.1
                        }

                        if (pl.offset1 == "42" && text == "2")
                        {
                            pl.offset1 = "43.2.2";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ох, а ты прав. Сейчас, дай мне пару минут на ужин."); //43.1
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Приятного аппетита." +//43.2.1
                            "\n2.– Не подавись."); //43.2.2
                            break;
                        }
                        if (pl.offset1 == "43.2.2" && text == "1")
                        {
                            pl.offset1 = "44";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Спасибо!"); //44.1
                        }
                        if (pl.offset1 == "43.2.2" && text == "2")
                        {
                            pl.offset1 = "44";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Обязательно."); //44.1
                        }

                        if (pl.offset1 == "44")
                        {
                            pl.offset1 = "50";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Всё, теперь я готов выбираться отсюда."); //45
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Мой желудок успокоился.  Даже память понемногу возвращается."); //46
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я смутно помню, как мы долго бродили, а потом дошли до сюда."); //47
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "И ещё мы заходили в лес с юга, это точно."); //48
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я вспомнил закусочную, которая стоит около проезжей части."); //49
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Какие же там потрясающие бургеры..."); //50
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Так, сейчас не время мечтаний! Собери вещи, какие есть, и вперёд."); //50.1
                            break;
                        }
                        if (pl.offset1 == "50" && text == "1")
                        {
                            pl.offset1 = "54";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ты прав. Собирать-то особо и нечего, всего лишь рюкзак и остатки посуды."); //51
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "А, вот ещё бутылка воды. А я-то и пить не хочу."); //52
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ладно, я пока пойду по тропинке на юг. Как увижу что-нибудь интересное, напишу."); //53
                            Thread.Sleep(10000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Недолго моё путешествие длилось."); //54
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Что произошло? " + //54.1
                            "\n2.– Ты уже вышел из леса?! ");                                   //54.2
                            break;
                        }

                        if (pl.offset1 == "54")
                        {
                            pl.offset1 = "58";
                            if (text == "2")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Если бы!"); //55
                                text = "1";
                            }
                            if (text == "1")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Дорога раздваивается влево и вправо."); //56
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я совершенно не помню, в какую сторону надо идти."); //57
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Посоветуй, какой путь выбрать?"); //58
                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Иди направо." + //58.1
                                "\n2.– Давай налево."); //58.2
                                break;
                            }
                        }

                        if (pl.offset1 == "58" && text == "1")
                        {
                            pl.offset1 = "1.61";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хорошо!"); //59
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Эх."); //60
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Интересно, обо мне уже спохватились?.."); //61
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Скорее всего." + //61.1
                            "\n2.– Что за грустные мысли? Не думай об этом.  "); //61.2
                            break;
                        }

                        if (pl.offset1 == "1.61")
                        {
                            if (text == "1")
                            {
                                pl.offset1 = "1.63";
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вот бы уже вернуться домой..."); //62.1
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Кстати о домах."); //63.1
                            }
                            if (text == "2")
                            {
                                pl.offset1 = "1.63";
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Чёрт, ты прав."); //62.2
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Главное – не падать духом."); //62.3
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "О!"); //63.2
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я что-то вижу недалеко!"); //63.3
                            }
                        }

                        if (pl.offset1 == "1.63")
                        {
                            pl.offset1 = "1.67";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Тут небольшой домик."); //64
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Два этажа."); //65
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Неужели, в этом лесу кто-то живет."); //66
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Может, постучать, спросить дорогу?"); //67
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Дельная мысль." + //67.1
                            "\n2.– Серьезно? А если откроет тот, кто тебя ограбил?");          //67.2
                            break;
                        }

                        if (pl.offset1 == "1.67" && text == "1")
                        {
                            pl.offset1 = "1.68";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Бегу к дому!"); //68.1
                        }
                        if (pl.offset1 == "1.67" && text == "2")
                        {
                            pl.offset1 = "1.68";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хм... Возможно."); //68.2
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Но в любом случае, я не знаю, как он выглядит."); //68.3
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Плюс, у меня есть нож, если что."); //68.4
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Короче, я все равно попробую!"); //68.5
                            Thread.Sleep(5000);
                        }

                        if (pl.offset1 == "1.68")
                        {
                            pl.offset1 = "1.72";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я постучал пару раз, никто не открывает."); //69
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Видимо, никого нет дома."); //70
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "А может, это вообще заброшенный дом?"); //71
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хочу проверить это. Грабить не буду, но вдруг найду что-нибудь, что мне поможет."); //72
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Думаю, хуже не будет." + //72.1
                            "\n2.– Отговаривать тебя все равно бесполезно… ");                        //72.2
                            break;
                        }

                        if (pl.offset1 == "1.72")
                        {
                            if (text == "1")
                            {
                                pl.offset1 = "1.73";
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Согласен."); //73.1
                            }
                            if (text == "2")
                            {
                                pl.offset1 = "1.73";
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ха!"); //73.1
                            }
                        }

                        if (pl.offset1 == "1.73")
                        {
                            pl.offset1 = "1.74";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "На двери навесной замок."); //74
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Попробуй открыть его ножом." + //74.1
                            "\n2.– Поищи другой вход. ");                                                   //74.2
                            break;
                        }

                        if (pl.offset1 == "1.74" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Понял"); //75
                            Thread.Sleep(4000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Не открывается."); //76
                        }

                        if (pl.offset1 == "1.74")
                        {
                            pl.offset1 = "1.79";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Тут рядом мусор валяется."); //77
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Могу поискать там что-нибудь."); //78
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Правда руки марать не хочется..."); //79
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Глупая затея. Придумай другой способ пробраться." + //79.1
                            "\n2.– Ты застрял в лесу и при этом думаешь о чистоте рук? Ищи давай.");                             //79.2
                            break;
                        }

                        if (pl.offset1 == "1.79" && text == "1")
                        {
                            pl.offset1 = "1.80";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ты прав."); //80.1
                        }
                        if (pl.offset1 == "1.79" && text == "2")
                        {
                            pl.offset1 = "1.80";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ладно."); //80.2
                            Thread.Sleep(10000); //таймер
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Чёрт, видимо, дом всё-таки жилой."); //80.3
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Потому что тут только объедки и щепки."); //80.4
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Зря время потерял..."); //80.5
                        }

                        if (pl.offset1 == "1.80")
                        {
                            pl.offset1 = "1.85";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Пойду обойду дом кругом."); //81
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Тут есть высокое дерево."); //82
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "А его ветви как раз подходят к окну на втором этаже."); //83
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Оно открыто!"); //84
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Придётся лезть."); //85
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Будь аккуратен!" + //85.1
                            "\n2.– Не мог придумать что-нибудь адекватное...");                 //85.2
                            break;
                        }

                        if (pl.offset1 == "1.85")
                        {
                            pl.offset1 = "1.96";
                            if (text == "1")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хорошо!"); //86
                            }
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я внутри."); //87
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Видимо, это спальня."); //88
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Тут большая кровать, большой шкаф, полки с книгами..."); //89
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "А ещё огромная карта!"); //90
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Да тут от руки нарисованы все тропинки леса."); //91
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вот это находка!"); //92
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я вижу и ту поляну, и этот дом..."); //93
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "И проезжую часть."); //94
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Да тут совсем недалеко! Я был на правильном пути!"); //95
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Теперь я точно знаю, куда идти!"); //96
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Отлично! Продолжай путь." + //96.1
                            "\n2.– Здорово! А теперь выбирайся оттуда, пока хозяин не вернулся.");       //96.2
                            break;
                        }

                        if (pl.offset1 == "1.96")
                        {

                            if (text == "1")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Так точно!"); //97.1
                                Thread.Sleep(10000);
                                pl.offset1 = "1.97";
                            }
                            if (text == "2")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Чёрт, валю отсюда."); //97.2
                                Thread.Sleep(10000);
                                pl.offset1 = "1.97";
                            }
                        }

                        if (pl.offset1 == "1.97")
                        {
                            pl.offset1 = "1.100";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Иду дальше по тропинке."); //98
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Пока всё спокойно."); //99
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Или не очень..."); //100
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Что случилось?" + //100.1
                            "\n2.– Что опять?");                                               //100.2
                            break;
                        }

                        if (pl.offset1 == "1.100")
                        {
                            pl.offset1 = "1.101";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я слышу, как тут кто-то ходит."); //101
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Ну вот, хозяин дома пришел по твою душу." + //101.1
                            "\n2.– Может, тебе показалось? ");                                                           //101.2
                            break;
                        }

                        if (pl.offset1 == "1.101")
                        {

                            if (text == "1")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Уж лучше он, чем какой-нибудь..."); //102.1
                                pl.offset1 = "1.102";
                            }
                            if (text == "2")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Не думаю. Я кого-то вижу..."); //102.2
                                pl.offset1 = "1.102";
                            }
                        }

                        if (pl.offset1 == "1.102")
                        {
                            pl.offset1 = "1.107";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "МЕДВЕДЬ?!"); //103
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Черт, он заметил меня."); //104
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он выглядит голодным."); //105
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "И подходит все ближе."); //106
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Что делать?!"); //107
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Лезь на дерево, там он тебя не достанет." + //107.1 >>108
                            "\n2.– Спокойно, медленно отходи." +                                                         //107.2 >>112
                            "\n3.– Беги быстрее! ");                                                                     //107.3 >>121
                            break;
                        }

                        if (pl.offset1 == "1.107" && text == "1")
                        {
                            pl.offset1 = "1.1.108";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ты уверен, что он до меня не доберется?"); //108
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Определенно. " + //108.1 >>109
                            "\n2.– А ты прав. Медленно отходи." +                             //108.2 >> 112
                            "\n3.– Может, и доберется. Лучше беги. ");                        //108.3 >> 121
                            break;
                        }

                        if (pl.offset1 == "1.1.108" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Лезу!"); //109
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "О нет, он полез следом!"); //110
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он сейчас меня достанет!.."); //111
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }

                        if (pl.offset1 == "1.1.108" && text == "2")
                        {
                            pl.offset1 = "1.1.115";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Понял."); //112
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он подходит ближе."); //113
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я стараюсь не паниковать."); //114
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Стараюсь..."); //115
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Притворись мертвым, так он потеряет к тебе интерес" + //115.1
                            "\n2.– Черт с этим, беги");                                                                            //115.2
                            break;
                        }

                        if (pl.offset1 == "1.1.115" && text == "1")
                        {
                            pl.offset1 = "1.120";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ох,  это будет непросто..."); //116
                            Thread.Sleep(15000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я живой!"); //117
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Получилось!!!"); //118
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он долго меня обнюхивал, но ничего не заподозрил и ушел."); //119
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вроде, его не видно, двигаюсь дальше."); //120
                        }
                        if ((pl.offset1 == "1.1.115" && text == "2"))
                        {
                            pl.offset1 = "1.1.121";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ты уверен в этом? А если он меня догонит?"); //121
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Да не догонит, беги!" + //121.1
                            "\n2.– Ты прав. Сохраняй спокойствие и отходи потихоньку.");             //121.2
                            break;
                        }

                        if (pl.offset1 == "1.1.121" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Бегу!"); //122
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }

                        if (pl.offset1 == "1.1.121" && text == "2")
                        {
                            pl.offset1 = "1.1.1.115";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Понял"); //112
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он подходит ближе."); //113
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я стараюсь не паниковать."); //114
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Стараюсь..."); //115
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Притворись мертвым, так он потеряет к тебе интерес"); //115.1
                            break;
                        }
                        if (pl.offset1 == "1.1.1.115" && text == "1")
                        {
                            pl.offset1 = "1.120";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ох,  это будет непросто..."); //116
                            Thread.Sleep(15000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я живой!"); //117
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Получилось!!!"); //118
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он долго меня обнюхивал, но ничего не заподозрил и ушел."); //119
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вроде, его не видно, двигаюсь дальше."); //120
                        }

                        if (pl.offset1 == "1.1.108" && text == "3")
                        {
                            pl.offset1 = "1.3.121";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ты уверен в этом? А если он меня догонит?"); //121
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Да не догонит, беги!" + //121.1
                            "\n2.– Ты прав. Сохраняй спокойствие и отходи потихоньку.");             //121.2
                            break;
                        }
                        if (pl.offset1 == "1.1.121" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Бегу!"); //122
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }

                        if (pl.offset1 == "1.3.121" && text == "2")
                        {
                            pl.offset1 = "1.3.1.115";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Понял"); //112
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он подходит ближе."); //113
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я стараюсь не паниковать."); //114
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Стараюсь..."); //115
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Притворись мертвым, так он потеряет к тебе интерес"); //115.1
                            break;
                        }
                        if (pl.offset1 == "1.3.1.115" && text == "1")
                        {
                            pl.offset1 = "1.120";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ох,  это будет непросто..."); //116
                            Thread.Sleep(15000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я живой!"); //117
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Получилось!!!"); //118
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он долго меня обнюхивал, но ничего не заподозрил и ушел."); //119
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вроде, его не видно, двигаюсь дальше."); //120
                        }

                        if (pl.offset1 == "1.107" && text == "2")
                        {
                            pl.offset1 = "1.2.115";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Понял."); //112
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он подходит ближе."); //113
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я стараюсь не паниковать."); //114
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Стараюсь..."); //115
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Притворись мертвым, так он потеряет к тебе интерес" + //115.1
                            "\n2.– Черт с этим, беги");                                                                            //115.2
                            break;
                        }

                        if (pl.offset1 == "1.2.115" && text == "1")
                        {
                            pl.offset1 = "1.120";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ох,  это будет непросто..."); //116
                            Thread.Sleep(15000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я живой!"); //117
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Получилось!!!"); //118
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он долго меня обнюхивал, но ничего не заподозрил и ушел."); //119
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вроде, его не видно, двигаюсь дальше."); //120
                        }

                        if (pl.offset1 == "1.2.115" && text == "2")
                        {
                            pl.offset1 = "1.2.121";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ты уверен в этом? А если он меня догонит?"); //121
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Да не догонит, беги!" + //121.1
                            "\n2.– Ты прав. Сохраняй спокойствие и отходи потихоньку.");             //121.2
                            break;
                        }

                        if (pl.offset1 == "1.2.121" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Бегу!"); //122
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }

                        if (pl.offset1 == "1.2.121" && text == "2")
                        {
                            pl.offset1 = "1.2.1.115";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Понял"); //112
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он подходит ближе."); //113
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я стараюсь не паниковать."); //114
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Стараюсь..."); //115
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Притворись мертвым, так он потеряет к тебе интерес"); //115.1
                            break;
                        }
                        if (pl.offset1 == "1.2.1.115" && text == "1")
                        {
                            pl.offset1 = "1.120";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ох,  это будет непросто..."); //116
                            Thread.Sleep(15000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я живой!"); //117
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Получилось!!!"); //118
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он долго меня обнюхивал, но ничего не заподозрил и ушел."); //119
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вроде, его не видно, двигаюсь дальше."); //120
                        }

                        if (pl.offset1 == "1.107" && text == "3")
                        {
                            pl.offset1 = "1.3.121";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ты уверен в этом? А если он меня догонит?"); //121
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Да не догонит, беги!" + //121.1
                            "\n2.– Ты прав. Сохраняй спокойствие и отходи потихоньку.");             //121.2
                            break;
                        }

                        if (pl.offset1 == "1.3.121" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Бегу!"); //122
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }

                        if (pl.offset1 == "1.3.121" && text == "2")
                        {
                            pl.offset1 = "1.3.1.115";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Понял"); //112
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он подходит ближе."); //113
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я стараюсь не паниковать."); //114
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Стараюсь..."); //115
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Притворись мертвым, так он потеряет к тебе интерес"); //115.1
                            break;
                        }
                        if (pl.offset1 == "1.3.1.115" && text == "1")
                        {
                            pl.offset1 = "1.120";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ох,  это будет непросто..."); //116
                            Thread.Sleep(15000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я живой!"); //117
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Получилось!!!"); //118
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он долго меня обнюхивал, но ничего не заподозрил и ушел."); //119
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вроде, его не видно, двигаюсь дальше."); //120
                        }

                        if (pl.offset1 == "1.120")
                        {
                            pl.offset1 = "1.125";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вот это был экшн… Никогда такого не забуду."); //123
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вроде как я уже около дороги, почему медведь забрёл сюда"); //124
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Может, я где-то сбился с пути?.."); //125
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Глупости, я думаю, ты идёшь правильно." + //125.1
                            "\n2.– В любом случае, иди дальше. Куда-нибудь да придёшь.");                              //125.2
                            break;
                        }
                        if (pl.offset1 == "1.125")
                        {
                            pl.offset1 = "1.127";
                            if (text == "1")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "А как я на это надеюсь."); //126.1
                            }
                            if (text == "2")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Скорее бы это закончилось..."); //126.2
                            }
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я вижу!!!"); //127
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Что?" + //127.1
                            "\n2.– Кого?");                                          //127.2
                        }

                        if (pl.offset1 == "1.127")
                        {
                            pl.offset1 = "-1";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Дорогу!"); //128
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "А вон и закусочная вдалеке!"); //129
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ура, я спасён!"); //130
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Спасибо тебе за помощь, друг!"); //131
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Если когда-нибудь встретимся, то с меня бутерброд."); //132
                            Thread.Sleep(2000);
                        }

                        if (pl.offset1 == "58" && text == "2")
                        {
                            pl.offset1 = "2.62";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хорошо!"); //59
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Как же кругом красиво. Сейчас бы сделать парочку снимков..."); //60
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Кстати, фотоаппарата не было."); //61
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Интересно, его забрал мой друг или грабитель?"); //62
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Чего гадать, сейчас мы это вряд ли узнаем." + //62.1
                            "\n2.– В любом случае, друзья у тебя так себе."); //62.2
                            break;
                        }

                        if (pl.offset1 == "2.62")
                        {
                            pl.offset1 = "2.67";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Эх, получается так, да."); //63
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Стой."); //64
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я слышу какой-то вой."); //65
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Неужели это..."); //66
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "ВОЛКИ?!"); //67
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– У тебя большие проблемы." + //67.1
                            "\n2.– Ты уверен, что это не ветер?"); //67.2
                            break;
                        }

                        if (pl.offset1 == "2.67")
                        {
                            if (text == "1")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "И сам знаю!"); //68.1
                            }
                            if (text == "2")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Да! Я слышу, как они приближаются."); //68.2
                            }
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Чёрт, да там большая стая."); //68.3
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я же сейчас в обморок упаду! Что мне делать?"); //69
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Лезь на ближайшее дерево. " + //69.1 >>80
                            "\n2.– Спрячься в кусты." +                                                      //69.2 >> 70
                            "\n3.– Убегай подальше!");                                                      //69.3 >> 75
                            pl.offset1 = "2.69";
                            break;
                        }

                        if (pl.offset1 == "2.69" && text == "1")
                        {
                            pl.offset1 = "2.80";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хорошо!"); //80
                            Thread.Sleep(20000);
                        }

                        if (pl.offset1 == "2.69" && text == "2")
                        {
                            pl.offset1 = "2.70";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Странная идея... Они меня не учуют?"); //70
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Доверься мне." + //70.1 >>71
                            "\n2.– Хм... Могут и учуять. Залезай на дерево." + //70.2 >> 80
                            "\n3.– Ты прав. Лучше делай ноги оттуда. "); //70.3 >>75
                            break;
                        }

                        if (pl.offset1 == "2.70" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Прячусь!"); //71
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Они здесь."); //72
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Один из них подходит всё ближе..."); //73
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "О нет, он нашел ме..."); //74
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }
                        if (pl.offset1 == "2.70" && text == "2")
                        {
                            pl.offset1 = "2.80";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хорошо!"); //80
                            Thread.Sleep(20000);
                        }
                        if (pl.offset1 == "2.70" && text == "3")
                        {
                            pl.offset1 = "2.3.75";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Убежать? Ты уверен?"); //75
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Если не будешь стоять на месте, то они не догонят тебя." + //75.1 >> 76
                            "\n2.– Мда, так себе затея. Пережди на дереве." +                                                           //75.2 >> 80
                            "\n3.– Соглашусь, не лучший план. Спрячься в ближайших кустах.");                                           //75.3 >> 70
                        }

                        if (pl.offset1 == "2.3.75" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Убегаю!"); //76
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ноги отваливаются... Надо передохнуть."); //77
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "О нет, они бегут за мной!"); //78
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Помо..."); //79
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }

                        if (pl.offset1 == "2.3.75" && text == "2")
                        {
                            pl.offset1 = "2.80";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хорошо!"); //80
                            Thread.Sleep(20000);
                        }

                        if (pl.offset1 == "2.3.75" && text == "3")
                        {
                            pl.offset1 = "2.3.70";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Странная идея... Они меня не учуют?"); //70
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Доверься мне." + //70.1 >>71
                            "\n2.– Хм... Могут и учуять. Залезай на дерево."); //70.2 >> 80
                            break;
                        }

                        if (pl.offset1 == "2.3.70" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Прячусь!"); //71
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Они здесь."); //72
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Один из них подходит всё ближе..."); //73
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "О нет, он нашел ме..."); //74
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }
                        if (pl.offset1 == "2.3.70" && text == "2")
                        {
                            pl.offset1 = "2.80";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хорошо!"); //80
                            Thread.Sleep(20000);
                        }


                        if (pl.offset1 == "2.69" && text == "3")
                        {
                            pl.offset1 = "3.3.75";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Убежать? Ты уверен?"); //75
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Если не будешь стоять на месте, то они не догонят тебя." + //75.1 >> 76
                            "\n2.– Мда, так себе затея. Пережди на дереве." +                                                           //75.2 >> 80
                            "\n3.– Соглашусь, не лучший план. Спрячься в ближайших кустах.");                                           //75.3 >> 70
                        }

                        if (pl.offset1 == "3.3.75" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Убегаю!"); //76
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ноги отваливаются... Надо передохнуть."); //77
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "О нет, они бегут за мной!"); //78
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Помо..."); //79
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }

                        if (pl.offset1 == "3.3.75" && text == "2")
                        {
                            pl.offset1 = "2.80";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хорошо!"); //80
                            Thread.Sleep(20000);
                        }

                        if (pl.offset1 == "3.3.75" && text == "3")
                        {
                            pl.offset1 = "3.3.70";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Странная идея... Они меня не учуют?"); //70
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Доверься мне." + //70.1 >>71
                            "\n2.– Хм... Могут и учуять. Залезай на дерево."); //70.2 >> 80
                            break;
                        }

                        if (pl.offset1 == "3.3.70" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Прячусь!"); //71
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Они здесь."); //72
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Один из них подходит всё ближе..."); //73
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "О нет, он нашел ме..."); //74
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }
                        if (pl.offset1 == "3.3.70" && text == "2")
                        {
                            pl.offset1 = "2.80";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хорошо!"); //80
                            Thread.Sleep(20000);
                        }

                        if (pl.offset1 == "2.80")
                        {
                            pl.offset1 = "2.85";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Фух, пронесло."); //81
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Они просто пробежали мимо. Слезаю."); //82
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Чуть не свалился, но живой. Иду дальше."); //83
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Главное, снова не нарваться на хищников."); //84
                            Thread.Sleep(10000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "У меня маленькая проблема."); //85
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Маленькая? " + //85.1
                            "\n2.– Что такое? "); //85.2
                            break;
                        }

                        if (pl.offset1 == "2.85")
                        {
                            pl.offset1 = "2.87";
                            if (text == "1")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ну, ладно, не маленькая."); //86
                            }
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я упёрся в реку."); //87
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Что предлагаешь делать?" + //87.1
                            "\n2.– Ты не перепрыгнешь её? "); //87.2
                            break;
                        }

                        if (pl.offset1 == "2.87")
                        {
                            pl.offset1 = "2.91";
                            if (text == "2")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Нет, она слишком широкая."); //88
                            }
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Насколько я помню, через неё есть несколько мостов."); //89
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Но я не знаю, как долго мне придётся идти до ближайшего из них."); //90
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я мог бы попробовать переплыть её…"); //91
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Это слишком опасно. Лучше поищи мост. " + //91.1 >>97
                            "\n2.– Отличная идея. Так ты сэкономишь кучу времени. "); //91.2
                            break;
                        }

                        if (pl.offset1 == "2.91" && text == "2")
                        {
                            pl.offset1 = "2.92";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Уверен, что течение не унесёт меня? Оно кажется сильным..."); //92
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Не сомневайся в своих силах." + //92.1
                            "\n2.– Ты не говорил про течение. Тогда лучше перейти по мосту."); //92.2
                            break;
                        }

                        if (pl.offset1 == "2.92" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Захожу в воду."); //93
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Это сложнее, чем я думал!"); //94
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "У меня заканчиваются силы стоять, не то, что плыть."); //95
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ну нет, я возвращаюсь обра..."); //96
                            Thread.Sleep(1000);
                            Console.WriteLine("Game over.");
                            pl.offset1 = "-1";
                        }

                        if (pl.offset1 == "2.92" || pl.offset1 == "2.91")
                        {
                            pl.offset1 = "2.98";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хорошо. Снова поворачиваю налево. "); //97
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Как же мне повезло!"); //98
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Ты нашел цивилизацию? " + //98.1
                            "\n2.– Что случилось? "); //98.2
                            break;
                        }

                        if (pl.offset1 == "2.98")
                        {
                            pl.offset1 = "2.104";
                            if (text == "1")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Почти."); //99
                            }
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Мост оказался не очень далеко!"); //100
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хорошо, что не полез в воду."); //101
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Иду в предыдущем направлении."); //102
                            Thread.Sleep(10000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ух, хоть я шел и недолго, но чувствую, что устал."); //103
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Мне надо передохнуть."); //104
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Только не долго. " + //104.1
                            "\n2.– Устраиваешь перерыв на ужин при свечах?"); //104.2
                            break;
                        }

                        if (pl.offset1 == "2.104")
                        {
                            pl.offset1 = "2.108";
                            if (text == "2")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ха-ха."); //105
                            }
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я попью воды и на пару минут упрусь спиной в дерево."); //106
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Чёрт."); //107
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Нет нет нет!"); //108
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Уже отдохнул?" + //108.1
                            "\n2.– Что происходит?"); //108.2
                            break;
                        }

                        if (pl.offset1 == "2.108")
                        {
                            pl.offset1 = "2.110";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "ОКОЛО МОЕЙ НОГИ ЗМЕЯ!"); //109
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Как ее отогнать?! Тут около меня длинная палка..."); //110
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Отпугни ее палкой! " + //110.1 >> 111
                            "\n2.– Не трогай палку! Аккуратно отодвинь ногу, медленно встань и уходи." + //110.2 >> 117
                            "\n3.– Кинь в нее бутылку, она испугается! "); //110.3 >> 112
                            break;
                        }

                        if (pl.offset1 == "2.110" && text == "1")
                        {
                            pl.offset1 = "2.1.111";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Думаешь, сработает?"); //111
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Конечно! " + //111.1 >> 113
                            "\n2.– Нет, лучше действуй наверняка и кидай бутылку." + //111.2 >> 112
                            "\n3.– Хм, лучше не рисковать. Спокойно отдаляйся от нее. "); //111.3 >> 117
                            break;
                        }
                        if (pl.offset1 == "2.1.111" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Понял. Беру палку."); //113
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }

                        if (pl.offset1 == "2.1.111" & text == "2")
                        {
                            pl.offset1 = "2.1.112";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "А она не разозлится?"); //112
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Конечно нет! Она сразу же уползет." + //112.1 >> 114
                            "\n2.– Только этого не хватало. Аккуратно уходи оттуда."); //112.3 >> 117
                        }

                        if (pl.offset1 == "2.1.112" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Кидаю бутылку."); //114
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Черт, ей это не очень понравилось."); //115
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Она зашипела!.."); //116
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }

                        if (pl.offset1 == "2.1.112" && text == "2")
                        {
                            pl.offset1 = "2.117";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Постараюсь не делать резких движений."); //117
                        }

                        if (pl.offset1 == "2.1.111" & text == "3")
                        {
                            pl.offset1 = "2.117";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Постараюсь не делать резких движений."); //117
                        }

                        if (pl.offset1 == "2.110" && text == "2")
                        {
                            pl.offset1 = "2.117";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Постараюсь не делать резких движений."); //117
                        }

                        if (pl.offset1 == "2.110" && text == "3")
                        {
                            pl.offset1 = "2.3.112";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "А она не разозлится?"); //112
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Конечно нет! Она сразу же уползет." + //112.1 >> 114
                            "\n2.– Только этого не хватало. Аккуратно уходи оттуда."); //112.3 >> 117
                        }

                        if (pl.offset1 == "2.3.112" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Кидаю бутылку."); //114
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Черт, ей это не очень понравилось."); //115
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Она зашипела!.."); //116
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }

                        if (pl.offset1 == "2.3.112" && text == "2")
                        {
                            pl.offset1 = "2.117";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Постараюсь не делать резких движений."); //117
                        }

                        if (pl.offset1 == "2.117")
                        {
                            pl.offset1 = "2.120";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я в безопасности."); //118
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Она не стала проявлять агрессию."); //119
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Какой ужас..."); //120
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Ты молодец, что справился!" + //120.1
                            "\n2.– Да кошмар просто. Теперь иди дальше."); //120.2
                            break;
                        }

                        if (pl.offset1 == "2.120")
                        {
                            pl.offset1 = "2.126";
                            if (text == "1")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Спасибо!"); //121
                            }
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Продолжаю путь."); //122
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Черт, снова есть хочется."); //123
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Поиск моста не прошел бесследно."); //124
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "О!"); //125
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я вижу улей лесных пчел."); //126
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– И что? " + //126.1 >>127.1
                            "\n2.– Нет."); //126.2 >> 127.2
                            break;
                        }

                        if (pl.offset1 == "2.126" && text == "1")
                        {
                            pl.offset1 = "2.127.1";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Их там немного... Может позаимствовать у них немного меда?.."); //127.1
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Ты сдурел? Они на тебе живого места не оставят!" + //127.1.1 >> 133
                            "\n2.– Попробуй."); //127.1.2 >> 128
                            break;
                        }

                        if (pl.offset1 == "2.127.1" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ладно, потерплю. Иду дальше."); //133
                            pl.offset1 = "2.133";
                        }

                        if (pl.offset1 == "2.127.1" && text == "2")
                        {
                            Thread.Sleep(2000);
                            pl.offset1 = "2.128";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "А, может, подождать, когда их будет поменьше?"); //128
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Они тебе ничего не сделают. " + //128.1
                            "\n2.– А, может, потерпишь до своей закусочной и не будешь ввязываться в неприятности?"); //128.2
                            break;
                        }

                        if (pl.offset1 == "2.128" && text == "1")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ура, еда!"); //129
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Они, видимо, против моего вмешательства."); //130
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "О нет, они летят ко мне! Всем роем!.."); //131
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over.");
                            pl.offset1 = "-1";
                        }

                        if (pl.offset1 == "2.128" && text == "2")
                        {
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ладно, потерплю. Иду дальше."); //133
                            pl.offset1 = "2.133";
                        }

                        if (pl.offset1 == "2.126" && text == "2")
                        {
                            pl.offset1 = "2.127.2";
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Что «нет»?"); //127.2
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.– Даже не думай." + //127.2.1
                            "\n2.– Хочешь быть съеденным пчелами?"); //127.2.2
                            break;
                        }

                        if (pl.offset1 == "2.127.2")
                        {
                            if (text == "1")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ладно, потерплю. Иду дальше."); //133
                            }
                            if (text == "2")
                            {
                                Thread.Sleep(2000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Нет, не хочу!"); //132
                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ладно, потерплю. Иду дальше."); //133
                            }
                            pl.offset1 = "2.133";
                        }

                        if (pl.offset1 == "2.133")
                        {
                            Thread.Sleep(10000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Это случилось!"); //134
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я вижу проезжую часть!"); //135
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Пойду искать закусочную, попрошу там помощи."); //136
                            Thread.Sleep(2000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Спасибо за советы! Без тебя я бы не справился."); //137
                            pl.offset1 = "-1";
                        }

                    }
                    break;

                case 3:
                    {

                        if (!pl.b_gameover)
                        {


                            if (!pl.print_start_mes2)
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Добро пожаловать в игру \"Быки и коровы\" !");


                                //RulesOfTheGame("rules.txt");
                                //link2:
                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите уровень сложности, введя одно из чисел 1-3 слева от него:");
                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "1-легкий(двузначное число)" +
                                    "\n2-средний(трёхзначное число)" + "\n3-высокий(четырёхзначное число)");
                                pl.print_start_mes2 = true;
                            }
                            if (!pl.bulls_difficulty_chosen)
                            {
                                if (text == "1")
                                {
                                    pl.bulls_difficulty = "1";
                                    pl.bulls_difficulty_chosen = true;
                                    pl.print_start_mes1 = true;
                                    int botnum = RandomDigitBot2();
                                    pl.sbot = botnum.ToString();
                                }
                                if (text == "2")
                                {
                                    pl.bulls_difficulty = "2";
                                    pl.bulls_difficulty_chosen = true;
                                    pl.print_start_mes1 = true;
                                    int botnum = RandomDigitBot3();
                                    pl.sbot = botnum.ToString();
                                }
                                if (text == "3")
                                {
                                    pl.bulls_difficulty = "3";
                                    pl.bulls_difficulty_chosen = true;
                                    pl.print_start_mes1 = true;
                                    int botnum = RandomDigitBot4();
                                    pl.sbot = botnum.ToString();
                                    Console.WriteLine(pl.sbot);
                                }

                            }

                            if (pl.print_start_mes1 || pl.b_offset == "1")
                            {

                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Напоминание: если у Вас не получается отгадать число,введите \"выйти\", чтобы завершить игру досрочно");
                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Поехали!");
                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Итак,Ваша попытка ?");
                                pl.print_start_mes1 = false;
                            }

                            if (pl.bulls_difficulty_chosen)
                            {
                                if (pl.bulls_difficulty == "1")
                                {
                                    //link1:
                                    if (text.Length != 1)
                                    {

                                        pl.mydigit = text;
                                        //Console.WriteLine("pop" + pl.mydigit);
                                        if (pl.mydigit == pl.sbot)
                                        {
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Поздравляем,вы выиграли!");
                                            //pl.b_gameover = true;
                                            //Bot.SendTextMessageAsync(e.Message.Chat.Id, "1-начать заново" +
                                            //    "\n2-сменить уровень сложности" + "\n3-выйти");
                                            Thread.Sleep(500);
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Отправьте любой символ (кроме цифры), чтобы начать сначала");
                                            pl.game_num = 3;
                                            pl.bulls_difficulty = "0";
                                            pl.bulls_difficulty_chosen = false;
                                            pl.print_start_mes1 = false;
                                            pl.print_start_mes2 = false;
                                            pl.mydigit = "";
                                            pl.sbot = "";
                                            pl.bulls = -1;
                                            pl.cows = -1;
                                            break;

                                        }
                                        if (pl.mydigit.ToLower() == "выйти")
                                        {
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Game over! Было загадано число {pl.sbot}");
                                            break;
                                        }

                                        if (!pl.mydigit.All(x => Char.IsDigit(x)) | pl.mydigit.Length != 2 | !AreAllCharsDifferent(pl.mydigit))
                                        {
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Неверный ввод");
                                            Thread.Sleep(1000);
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вводимая строка должна быть двузначным числом с неповторяющимися числами");
                                            Thread.Sleep(1000);
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Повторите ввод:");
                                            break;
                                        }
                                        else
                                        {
                                            HowManyBullsAndCowsInDigit(pl.mydigit, pl.sbot, out pl.bulls, out pl.cows);
                                            Thread.Sleep(1000);
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Быков - {pl.bulls} , коров - {pl.cows}");
                                            if (pl.bulls != 2)
                                            {
                                                Thread.Sleep(1000);
                                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Следующая попытка : ");
                                            }
                                        }
                                        Thread.Sleep(1000);
                                        //Bot.SendTextMessageAsync(e.Message.Chat.Id, "Итак,Ваша попытка ?");
                                    }

                                }
                                if (pl.bulls_difficulty == "2")
                                {
                                    //link1:
                                    if (text.Length != 1)
                                    {

                                        pl.mydigit = text;
                                        //Console.WriteLine("pop" + pl.mydigit);
                                        if (pl.mydigit == pl.sbot)
                                        {
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Поздравляем,вы выиграли!");
                                            //pl.b_gameover = true;
                                            //Bot.SendTextMessageAsync(e.Message.Chat.Id, "1-начать заново" +
                                            //    "\n2-сменить уровень сложности" + "\n3-выйти");
                                            Thread.Sleep(500);
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Отправьте любой символ (кроме цифры), чтобы начать сначала");
                                            pl.game_num = 3;
                                            pl.bulls_difficulty = "0";
                                            pl.bulls_difficulty_chosen = false;
                                            pl.print_start_mes1 = false;
                                            pl.print_start_mes2 = false;
                                            pl.mydigit = "";
                                            pl.sbot = "";
                                            pl.bulls = -1;
                                            pl.cows = -1;
                                            break;

                                        }
                                        if (pl.mydigit.ToLower() == "выйти")
                                        {
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Game over! Было загадано число {pl.sbot}");
                                            break;
                                        }

                                        if (!pl.mydigit.All(x => Char.IsDigit(x)) | pl.mydigit.Length != 3 | !AreAllCharsDifferent(pl.mydigit))
                                        {
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Неверный ввод");
                                            Thread.Sleep(1000);
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вводимая строка должна быть трёхзначным числом с неповторяющимися числами");
                                            Thread.Sleep(1000);
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Повторите ввод:");
                                            break;
                                        }
                                        else
                                        {
                                            HowManyBullsAndCowsInDigit(pl.mydigit, pl.sbot, out pl.bulls, out pl.cows);
                                            Thread.Sleep(1000);
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Быков - {pl.bulls} , коров - {pl.cows}");
                                            if (pl.bulls != 2)
                                            {
                                                Thread.Sleep(1000);
                                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Следующая попытка : ");
                                            }
                                        }
                                        Thread.Sleep(1000);
                                        //Bot.SendTextMessageAsync(e.Message.Chat.Id, "Итак,Ваша попытка ?");
                                    }

                                }
                                if (pl.bulls_difficulty == "3")
                                {
                                    //link4:
                                    if (text.Length != 1)
                                    {

                                        pl.mydigit = text;
                                        //Console.WriteLine("pop" + pl.mydigit);
                                        if (pl.mydigit == pl.sbot)
                                        {
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Поздравляем,вы выиграли!");
                                            //pl.b_gameover = true;
                                            //Bot.SendTextMessageAsync(e.Message.Chat.Id, "1-начать заново" +
                                            //    "\n2-сменить уровень сложности" + "\n3-выйти");
                                            Thread.Sleep(500);
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Отправьте любой символ (кроме цифры), чтобы начать сначала");
                                            pl.game_num = 3;
                                            pl.bulls_difficulty = "0";
                                            pl.bulls_difficulty_chosen = false;
                                            pl.print_start_mes1 = false;
                                            pl.print_start_mes2 = false;
                                            pl.mydigit = "";
                                            pl.sbot = "";
                                            pl.bulls = -1;
                                            pl.cows = -1;
                                            break;

                                        }
                                        if (pl.mydigit.ToLower() == "выйти")
                                        {
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Game over! Было загадано число {pl.sbot}");
                                            break;
                                        }

                                        if (!pl.mydigit.All(x => Char.IsDigit(x)) | pl.mydigit.Length != 4 | !AreAllCharsDifferent(pl.mydigit))
                                        {
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Неверный ввод");
                                            Thread.Sleep(1000);
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вводимая строка должна быть четырёхзначным числом с неповторяющимися числами");
                                            Thread.Sleep(1000);
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Повторите ввод:");
                                            break;
                                        }
                                        else
                                        {
                                            HowManyBullsAndCowsInDigit(pl.mydigit, pl.sbot, out pl.bulls, out pl.cows);
                                            Thread.Sleep(1000);
                                            Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Быков - {pl.bulls} , коров - {pl.cows}");
                                            if (pl.bulls != 2)
                                            {
                                                Thread.Sleep(1000);
                                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Следующая попытка : ");
                                            }
                                        }
                                        Thread.Sleep(1000);
                                        //Bot.SendTextMessageAsync(e.Message.Chat.Id, "Итак,Ваша попытка ?");
                                    }

                                }
                            }
                        }
                        else
                        {
                            pl.b_offset = "1";
                        }

                    }
                    break;

                case 4:
                    {
                        if (!pl.rounds_chosen)
                        {
                            int.TryParse(text, out int n);
                            if (n == 0 && text == "0")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ну, как хотите!");
                                break;
                            }
                            else if (n < 0)
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Количество раундов не может быть отрицательным! Повторите ввод.");
                            }
                            else if (n > 0)
                            {
                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Хорошо! Играем {n} раундов!");
                                pl.rps_n = n;
                                pl.rounds_chosen = true;
                                pl.rounds++;
                                text = "";
                                //break;
                            }
                            else if (text != "/rps")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Извините, я не понял. Попробуйте еще раз.");
                            }
                        }
                        if (pl.rounds_chosen && pl.rounds == 1)
                        {
                            if (!pl.r1_chosen)
                            {
                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Раунд 1 из " + pl.rps_n + ".");
                                Thread.Sleep(500);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Что выберете? Ответьте цифрой или словом. Обещаю не жульничать!" +
                                "\n1 - камень" +
                                "\n2 - ножницы" +
                                "\n3 - бумага");
                                pl.r1_chosen = true;
                            }
                            if (!pl.r2_chosen)
                            {
                                if (text == "1" || text == "камень")
                                {
                                    pl.rps_string = "камень";
                                    pl.r2_chosen = true;
                                    //break;
                                }
                                else if (text == "2" || text == "ножницы")
                                {
                                    pl.rps_string = "ножницы";
                                    pl.r2_chosen = true;
                                    //break;
                                }
                                else if (text == "3" || text == "бумага")
                                {
                                    pl.rps_string = "бумага";
                                    pl.r2_chosen = true;
                                    //break;
                                }
                                else
                                {
                                    //Console.WriteLine("abc");
                                    break;
                                }
                            }
                            if (!pl.r3_chosen)
                            {
                                //Console.WriteLine("ziz");
                                pl.rps_rand = r.Next(1, 7);
                                if (pl.rps_rand == 6) //1/6 что у игрока бумага
                                {
                                    pl.prevbot = "ножницы";
                                    Thread.Sleep(500);
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "У вас: " + pl.rps_string + ";  у бота: " + pl.prevbot + "." + '\n');
                                    Thread.Sleep(500);
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, IsBotWin(pl.prevbot, pl.rps_string, dict));
                                }
                                else if (pl.rps_rand > 3) //2/6 что у игрока ножницы
                                {
                                    pl.prevbot = "камень";
                                    Thread.Sleep(500);
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "У вас: " + pl.rps_string + ";  у бота: " + pl.prevbot + "." + '\n');
                                    Thread.Sleep(500);
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, IsBotWin(pl.prevbot, pl.rps_string, dict));
                                }
                                else //3/6 что у игрока камень 
                                {

                                    pl.prevbot = "бумага";
                                    Thread.Sleep(500);
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "У вас: " + pl.rps_string + ";  у бота: " + pl.prevbot + "." + '\n');
                                    Thread.Sleep(500);
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, IsBotWin(pl.prevbot, pl.rps_string, dict));
                                }
                                Console.WriteLine("резы 1");
                                pl.prevplayer = pl.rps_string;
                                pl.rounds++;
                                pl.r1_chosen = false;
                                pl.r2_chosen = false;
                                pl.r3_chosen = false;
                                pl.rps_string = "";
                                text = "";
                            }
                        }
                    label:
                        if (pl.rounds_chosen && pl.rounds != pl.rps_n + 1)
                        {
                            if (!pl.r1_chosen)
                            {
                                Thread.Sleep(1000);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Раунд {pl.rounds} из " + pl.rps_n + ".");
                                Thread.Sleep(500);
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Что выберете? Ответьте цифрой или словом. Обещаю не жульничать!" +
                                "\n1 - камень" +
                                "\n2 - ножницы" +
                                "\n3 - бумага");
                                q.Enqueue(pl.prevplayer);
                                wordCount[pl.prevplayer]++;
                                pl.r1_chosen = true;
                            }
                            if (!pl.r2_chosen)
                            {
                                if (text == "1" || text == "камень")
                                {
                                    pl.rps_string = "камень";
                                    pl.r2_chosen = true;
                                    //break;
                                }
                                else if (text == "2" || text == "ножницы")
                                {
                                    pl.rps_string = "ножницы";
                                    pl.r2_chosen = true;
                                    //break;
                                }
                                else if (text == "3" || text == "бумага")
                                {
                                    pl.rps_string = "бумага";
                                    pl.r2_chosen = true;
                                    //break;
                                }
                                else
                                {
                                    //Console.WriteLine("abc");
                                    break;
                                }
                            }
                            if (!pl.r3_chosen)
                            {
                                if (r.Next(1, 11) < 8)
                                {
                                    if (q.Count >= 3)
                                    {
                                        while (q.Count != 3)
                                            wordCount[q.Dequeue()] -= 1; //выгружаем до тех пор, пока в очереди не будет три последних элемента
                                        pl.rps_k = Math.Max(wordCount["камень"], Math.Max(wordCount["ножницы"], wordCount["бумага"]));
                                    }

                                    if (wordCount["камень"] == pl.rps_k && wordCount["ножницы"] == pl.rps_k && wordCount["бумага"] == pl.rps_k)
                                    {
                                        pl.rps_rand = r.Next(1, 4);
                                        if (pl.rps_rand == 1)
                                            pl.prevbot = "камень";
                                        else if (pl.rps_rand == 2)
                                            pl.prevbot = "ножницы";
                                        else
                                            pl.prevbot = "бумага";
                                    }
                                    else if (wordCount["камень"] == pl.rps_k && wordCount["ножницы"] == pl.rps_k)
                                    {
                                        pl.rps_rand = r.Next(1, 3);
                                        if (pl.rps_rand == 1)
                                            pl.prevbot = "бумага";
                                        else
                                            pl.prevbot = "камень";
                                    }
                                    else if (wordCount["камень"] == pl.rps_k && wordCount["бумага"] == pl.rps_k)
                                    {
                                        pl.rps_rand = r.Next(1, 3);
                                        if (pl.rps_rand == 1)
                                            pl.prevbot = "бумага";
                                        else
                                            pl.prevbot = "ножницы";
                                    }
                                    else if (wordCount["ножницы"] == pl.rps_k && wordCount["бумага"] == pl.rps_k)
                                    {
                                        pl.rps_rand = r.Next(1, 3);
                                        if (pl.rps_rand == 1)
                                            pl.prevbot = "камень";
                                        else
                                            pl.prevbot = "ножницы";
                                    }
                                    else
                                    {
                                        if (wordCount["камень"] == pl.rps_k)
                                            pl.prevbot = "бумага";
                                        else if (wordCount["ножницы"] == pl.rps_k)
                                            pl.prevbot = "камень";
                                        else //бумага
                                            pl.prevbot = "ножницы";
                                    }
                                    Console.WriteLine("резы после 1");
                                    Thread.Sleep(1000);
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "У вас: " + pl.rps_string + ";  у бота: " + pl.prevbot + "." + '\n');
                                    Thread.Sleep(500);
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, IsBotWin(pl.prevbot, pl.rps_string, dict));
                                    pl.prevplayer = pl.rps_string;
                                    pl.rounds++;
                                    pl.r1_chosen = false;
                                    pl.r2_chosen = false;
                                    pl.r3_chosen = false;
                                    pl.rps_string = "";
                                    text = "";
                                    if (pl.rounds == pl.rps_n + 1)
                                    {
                                        pl.rps_gameover = true;
                                    }
                                    goto label;
                                }
                                else
                                {
                                    if (dict["losestreak"] > 1)
                                    {
                                        pl.rps_rand = r.Next(1, 7);

                                        if (pl.rps_rand == 6) //1/6 что игрок выберет предыдущий вариант
                                            pl.prevbot = Contr(pl.prevplayer);

                                        else if (pl.rps_rand > 3)//2/6 что игрок выберет то, что выбирал до этого бот
                                            pl.prevbot = Contr(pl.prevbot);

                                        else //3/6 что игрок выберет то, что в прошлом раунде помогло бы победить бота
                                            pl.prevbot = Contr(Contr(pl.prevbot)); //то, что поможет победить то, что может победить прошлый выбор бота
                                    }
                                    else if (dict["winstreak"] > 1)
                                    {
                                        pl.rps_rand = r.Next(1, 6);
                                        if (pl.rps_rand == 5) //1/5 что игрок выберет то что выбрал бот
                                            pl.prevbot = Contr(pl.prevbot);

                                        //else if(r == 4) //1/5 что игрок выберет то что проиграет прошлому выбору бота
                                        //тогда ничего не меняем

                                        else if (pl.rps_rand < 4)//3/5 что игрок сохранит стратегию
                                            pl.prevbot = Contr(pl.prevplayer);
                                    }
                                    else //иначе полный рандом
                                    {
                                        pl.rps_rand = r.Next(1, 4);
                                        if (pl.rps_rand == 1)
                                            pl.prevbot = "камень";
                                        else if (pl.rps_rand == 2)
                                            pl.prevbot = "ножницы";
                                        else
                                            pl.prevbot = "бумага";
                                    }
                                    Thread.Sleep(1000);
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "У вас: " + pl.rps_string + ";  у бота: " + pl.prevbot + "." + '\n');
                                    Thread.Sleep(500);
                                    Bot.SendTextMessageAsync(e.Message.Chat.Id, IsBotWin(pl.prevbot, pl.rps_string, dict));
                                    pl.prevplayer = pl.rps_string;
                                    pl.rounds++;
                                    pl.r1_chosen = false;
                                    pl.r2_chosen = false;
                                    pl.r3_chosen = false;
                                    pl.rps_string = "";
                                }
                            }
                        }
                        else if (pl.rounds == pl.rps_n + 1)
                        {
                            Thread.Sleep(1000);
                            if (dict["botvictories"] > dict["playervictories"])
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "По итогу бот победил, но не отчаивайтесь! В следующий раз повезет.");
                            else if (dict["botvictories"] < dict["playervictories"])
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "По итогу вы победили, поздравляю!");
                            else
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "По итогу вышла ничья! Вот это да!");
                            Thread.Sleep(1000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Счет. Вы выиграли раундов: " + dict["playervictories"] + "; бот: " + dict["botvictories"]);
                            break;
                        }
                    }
                    break;

                case 5:
                    {
                        if (text == "/quest2" && pl.offset2 == "0")
                        {
                            pl.offset2 = "3";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "*помехи*..Мен..*помехи*..ит?.."); //1
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "*помехи*..Меня кто-нибудь слышит?.."); //2
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Кто-нибудь, прошу, ответьте."); //3
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1.-Кто это говорит?" + //3.1
                            "\n2.-Я вас слышу.");                                               //3.2
                            break;
                        }
                        if (pl.offset2 == "3")
                        {
                            if (text == "1")
                            {
                                pl.offset2 = "6";
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Неужели я смог с кем-то связаться. Я капитан экипажа пилотируемого космического корабля №415. Нам нужна ваша помощь."); //4
                            }
                            if (text == "2")
                            {
                                pl.offset2 = "5";
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Оох, наконец кто-то вышел на связь. Мне нужна ваша помощь!"); //5
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -С кем я говорю?"); //5.1
                                break;
                            }
                        }
                        if (pl.offset2 == "5" && text == "1")
                        {
                            pl.offset2 = "6";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я капитан космического корабля №415."); //6
                        }
                        if (pl.offset2 == "6")
                        {
                            pl.offset2 = "8";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Наш экипаж направлялся на Спутник-311 и строго держался курса, однако случилось столкновение с астероидом."); //7
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я сейчас нахожусь в кабине пилота."); //8
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Вы один в кабине ?" + //8.1
                            "\n2. -А что с другими членами экипажа?");                             //8.2
                            break;
                        }
                        if (pl.offset2 == "8")
                        {
                            if (text == "1")
                            {
                                pl.offset2 = "11";
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Да. Кроме меня на корабле еще есть помощник капитана и бортмеханник."); //9
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Но я понятия не имею что с ними."); //10
                            }
                            if (text == "2")
                            {
                                pl.offset2 = "11";
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я не знаю. Мой помощник должен был быть в своей каюте, а бортмеханник в двигательном отсеке."); //11
                            }
                        }
                        if (pl.offset2 == "11")
                        {
                            pl.offset2 = "13";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Надеюсь, что с ними все в порядке."); //12
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Если я смог с вами связаться, значит, я вблизи вашего объекта."); //13
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Да, я диспетчер космической станции Планеты-14." + //13.1
                            "\n2. -Верно. Я говорю с космической станции Планеты-14."); //13.2
                            break;
                        }
                        if (pl.offset2 == "13")
                        {
                            pl.offset2 = "14";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Значит, вы можете нам помочь. Можете выслать нам спасательный отряд?"); //14
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Я не могу отследить ваше местоположение." + //14.1
                            "\n2. -Да, но на это понадобится время. Вас нет на радарах."); //14.2
                            break;
                        }
                        if (pl.offset2 == "14" && text == "1")
                        {
                            pl.offset2 = "15.1";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Судя по всему, навигационная система вышла из строя."); //15.1
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Я направлю спасательный отряд, но поиски могут быть долгими." + //15.1.1
                            "2. -Вы можете попытаться отладить систему навигации?"); //15.1.2
                            break;
                        }
                        if (pl.offset2 == "15.1")
                        {
                            if (text == "1")
                            {
                                pl.offset2 = "18";
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я понимаю. Нужно попытаться восстановить навигацию."); //16
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "К сожалению, это может сделать только бортмеханник."); //17
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Но я даже не знаю что с ним."); //18
                            }
                            if (text == "2")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "К сожалению, это может сделать только бортмеханник."); //17
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Но я даже не знаю что с ним."); //18
                            }
                        }
                        if (pl.offset2 == "14" && text == "2")
                        {
                            pl.offset2 = "15.2";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Система навигации вышла из строя. Искать нас будут долго."); //15.2
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Если починить систему, вас спасут гораздо быстрее." + //15.2.1
                            "\n2. -Да, вам необходимо продержатся."); //15.2.2
                            break;
                        }
                        if (pl.offset2 == "15.2" && text == "2")
                        {
                            pl.offset2 = "18";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Да, но это может сделать только бортмеханник."); //19
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "А я понятия не имею, что с ним."); //20
                        }
                        if (pl.offset2 == "18")
                        {
                            pl.offset2 = "23";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Что ж, так или иначе мне надо что-то предпринимать."); //22
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Нужно найти остальных членов экипажа. Я надеваю скафандр и выдвигаюсь."); //23
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. - Будьте осторожны, я буду держать с вами связь."); //23.1
                            break;
                        }
                        if (pl.offset2 == "23" && text == "1")
                        {
                            pl.offset2 = "28";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хорошо."); //25
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Так, я спускаюсь на нижнии этаж."); //26
                            Thread.Sleep(15000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я внизу."); //27
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Тут два прохода, один ведет в отсек с каютами, другой в двигательный."); //28
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Я думаю, вам надо как можно скорее найти механника." + //28.1
                            "\n2. -Проверьте каюты. Вы говорили, что там сейчас ваш помощник."); //28.2
                            break;
                        }

                        if (pl.offset2 == "28" && text == "1")
                        {
                            pl.offset2 = "1.33";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Да, нужно найти его как можно скорее."); //29
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я прохожу к двигательному отсеку."); //30
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Черт!"); //31
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Похоже этой части корабля досталось больше всего."); //32
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Проход завалило обломками."); //33
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Неужели нельзя попасть туда как-то по-другому?" + //33.1
                            "\n2. -Может, попробуете их разобрать?"); //33.2
                            break;
                        }
                        if (pl.offset2 == "1.33" && text == "1")
                        {
                            pl.offset2 = "1.34";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Нет, это единственный проход."); //34
                        }
                        if (pl.offset2 == "1.33" && text == "2")
                        {
                            pl.offset2 = "1.34";
                        }
                        if (pl.offset2 == "1.34")
                        {
                            pl.offset2 = "1.37";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ну, у меня нет выбора."); //35
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Придется разбирать."); //36
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Правда, без инструмента тут не обойтись."); //37
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -На корабле есть что-то подходящее?"); //37.1
                            break;
                        }
                        if (pl.offset2 == "1.37" && text == "1")
                        {
                            pl.offset2 = "1.42";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Да, в комнате снаряжения должен быть лом."); //38
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Комната находится рядом с каютами, так что мне придется вернуться. "); //39
                            Thread.Sleep(15000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я нашел лом."); //40
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "И еще..."); //41
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я подумал, может стоит пойти в каюты и найти своего помощника? А то у меня плохое предчуствие."); //42
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Нет, лучше ищите механника. Он может быть в большей беде." + //42.1
                            "\n2. -Хм, ну он сможет помочь разобрать обломки. Найдите его."); //42.2
                            break;
                        }

                        if (pl.offset2 == "1.42" && text == "1")
                        {
                            pl.offset2 = "1.1.46";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Тогда я возвращаюсь."); //43
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Как же я надеюсь, что они оба целы."); //44
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Так, сейчас попытаюсь разобрать обломки."); //45
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Черт."); //46
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -В чем дело?" + //46.1
                            "\n2. -Что случилось?"); //46.2
                            break;
                        }
                        if (pl.offset2 == "1.1.46" && (text == "1" || text == "2"))
                        {
                            pl.offset2 = "1.1.48";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ничего не выходит."); //47
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Одному мне здесь не справится."); //48
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Надо было отправить вас за помощником..." + //48.1
                            "\n2. -Быстрее неправляйтесь в каюты."); //48.2
                            break;
                        }
                        if (pl.offset2 == "1.1.48")
                        {
                            if (text == "1")
                            {
                                pl.offset2 = "1.1.50";
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я переоценил свои возможности."); //49
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Нужно быстрее бежать в каюту"); //50
                                Thread.Sleep(15000);
                            }
                            if (text == "2")
                            {
                                pl.offset2 = "1.1.50";
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Бегу."); //50 
                                Thread.Sleep(15000);
                            }
                        }
                        if (pl.offset2 == "1.1.50")
                        {
                            pl.offset2 = "1.1.51";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Черт, дверь в его каюту не открывается!"); //51
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Воспользуйтесь ломом!"); //51.1
                            break;
                        }
                        if (pl.offset2 == "1.1.51" && text == "1")
                        {
                            pl.offset2 = "1.1.54";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Получилось!"); //52
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "..."); //53
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я не успел."); //54
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -В чем дело?!"); //54.1
                            break;
                        }
                        if (pl.offset2 == "1.1.54" && text == "1")
                        {
                            pl.offset2 = "1.1.56";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "У него совсем не осталось кислорода..."); //55
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Кажется, я обречен."); //56
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Подмога в пути, они ищут тебя, не обрывай связь!"); //56.1
                            break;
                        }
                        if (pl.offset2 == "1.1.56" && text == "1")
                        {
                            pl.offset2 = "1.1.57";
                            Thread.Sleep(10000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Эй!" + //57.1
                            "\n2. -ЭЙ! Что с тобой!?"); //57.2
                            break;
                        }
                        if (pl.offset2 == "1.1.57" && (text == "1" || text == "2"))
                        {
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over!");
                            pl.offset2 = "-1";
                        }

                        if (pl.offset2 == "1.42" && text == "2")
                        {
                            pl.offset2 = "1.2.44";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Тогда я возвращаюсь."); //43
                            Thread.Sleep(15000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Черт, дверь заблокирована."); //44
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Попробуйте открыть её ломом."); //44.1
                            break;
                        }
                        if (pl.offset2 == "1.2.44" && text == "1")
                        {
                            pl.offset2 = "1.2.49";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Получилось!"); //45
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "..."); //46
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Фух, еще бы чуть-чуть..."); //47
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Он в порядке, но у него почти не осталось кислорода. Я окажу ему помощь."); //48
                            Thread.Sleep(10000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Отлично, теперь мы идем за механником."); //49
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Может, дадите отдохнуть помощнику?" + //49.1
                            "\n2. -Поторопитесь!"); //49.2
                            break;
                        }
                        if (pl.offset2 == "1.2.49")
                        {
                            pl.offset2 = "1.2.53";
                            if (text == "1")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "У нас нет на это времени. Надо спешить."); //50
                            }
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Мы идем в двигательный отсек."); //51
                            Thread.Sleep(20000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Теперь нам надо разобрать эти обломки."); //52
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Что там со спасательным экипажем. Они еще не обнаружили нас?"); //53
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Нет. Но они продолжают поиски." + //53.1
                            "\n2. -С неисправной системой навигации они обнаружат вас нескоро."); //53.2
                            break;
                        }
                        if (pl.offset2 == "1.2.53")
                        {
                            pl.offset2 = "1.2.59";
                            if (text == "2")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Только бы механник был цел..."); //54
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Если он починит навигацию, у нас будет больше шансов"); //55
                            }
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Остается надеяться на лучшее."); //56
                            Thread.Sleep(15000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Мы разобрали обломки. Заходим."); //57
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "..."); //58
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Черт! Он ранен!"); //59
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Скорее окажите ему помощь!"); //59.1
                            break;
                        }
                        if (pl.offset2 == "1.2.59" && text == "1")
                        {
                            pl.offset2 = "1.2.61";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "..."); //60
                            Thread.Sleep(15000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Все хорошо, ранение оказалось несерьезным."); //61
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Осталось починить навигацию."); //61.1
                            break;
                        }
                        if (pl.offset2 == "1.2.61" && text == "1")
                        {
                            pl.offset2 = "1.2.63";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Да."); //62
                            Thread.Sleep(10000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Мы наладили работу системы, посылаем сигнал SOS."); //63
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Отлично! Они обнаружили вас. " + //63.1
                            "\n2. -Отряд получил сигнал. Они направляются к вам."); //63.2
                            break;
                        }
                        if (pl.offset2 == "1.2.63" && (text == "1" || text == "2"))
                        {
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Какое счастье, мы спасены!"); //64
                            pl.offset2 = "-1";
                        }

                        if (pl.offset2 == "28" && text == "2")
                        {
                            pl.offset2 = "2.30";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хорошо. Я иду к каютам."); //29
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я перед дверью."); //30
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Так открывайте скорее!" + //30.1
                            "\n2. -Ну же, откройте её, он может быть в беде."); //30.2
                            break;
                        }
                        if (pl.offset2 == "2.30" && (text == "1" || text == "2"))
                        {
                            pl.offset2 = "2.32";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я пытаюсь! Она не поддается.");
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Похоже, заклинило");
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -И что думаете делать?" + //32.1
                            "\n2. -Попробуйте найти какой-нибудь рычаг!"); //32.2
                            break;
                        }
                        if (pl.offset2 == "2.32")
                        {
                            pl.offset2 = "2.42";
                            if (text == "1")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Есть одна идея."); //33
                            }
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "В комнате со снаряжением есть лом."); //34
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Я схожу туда."); //35
                            Thread.Sleep(20000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Так, лом у меня, осталось открыть дверь."); //36
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ух! Тяжело..."); //37
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Но, кажется, у меня получается..."); //38
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Есть!"); //39
                            //Bot.SendTextMessageAsync(e.Message.Chat.Id, "..."); //40
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Фух! Мой помощник цел."); //41
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хотя, задержись я, у него бы закончился кислород."); //42
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Хорошо, что он цел!" + //42.1
                            "\n2. -Не время об этом думать. Отправляйтесь за механником."); //42.2
                            break;
                        }
                        if (pl.offset2 == "2.42")
                        {
                            pl.offset2 = "2.47";
                            if (text == "1")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Да, надеюсь с механником тоже все хорошо!");
                            }
                            if (text == "2")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Да, нужно торопиться!");
                            }
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Мы идем в двигательный отсек.");
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Спасательный отряд еще не нашел нас?");
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Нет. Но они не прекращают поиски." +
                            "\n2. -Без навигационных систем это может занять очень много времени.");
                            break;
                        }
                        if (pl.offset2 == "2.47")
                        {
                            pl.offset2 = "2.50";
                            if (text == "1")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Надеюсь, они найдут нас.");
                            }
                            if (text == "2")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Думаю, мы скоро починим их.");
                            }
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "..."); //48
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "У нас тут проблема."); //49
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Проход завалило обломками."); //50
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -А по-другому в двигательный отсек не попасть?" + //50.1
                            "\n2. -И что теперь делать?"); //50.2
                            break;
                        }
                        if (pl.offset2 == "2.50")
                        {
                            pl.offset2 = "2.53";
                            if (text == "1")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Нет, только через этот проход."); //51
                            }
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Попробуем разобрать эти обломки."); //52
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Не выходит. Слишком тяжелые."); //53
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Даже вдвоем не выходит их поднять?"); //53.1
                            break;
                        }
                        if (pl.offset2 == "2.53" && text == "1")
                        {
                            pl.offset2 = "2.57";
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Поднять получается, но сдвинуть никак."); //54 
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Хм... А что если...");
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Есть идея. Подождите минуту.");
                            Thread.Sleep(20000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Отлично! Я у двигательного отсека.");
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Я думаю, не время разъяснений, поторопитесь!" +
                            "\n2. -Что? Как вы это сделали?");
                            break;
                        }
                        if (pl.offset2 == "2.57")
                        {
                            pl.offset2 = "2.65";
                            if (text == "1")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Да!");
                            }
                            if (text == "2")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Там была небольшая щель, в которую я немного не помещался.");
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Помощник приподнял обломок и я смог пролезть.");
                            }
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Так, я захожу в двигательный отсек."); //61
                            //Console.WriteLine("..."); //62
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Успели."); //63
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "С механником все в порядке. Осталось отладить систему навигации."); //64
                            Thread.Sleep(20000);
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Готово! Отправлям сигнал."); //65
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1. -Поздравляю! Спасатели обнаружили вас." + //65.1
                            "\n2. -Сигнал поступил, спасательный отряд направляется к вам."); //65.2
                            break;
                        }
                        if (pl.offset2 == "2.65" && (text == "1" || text == "2"))
                        {
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Мы спасены! Спасибо вам за помощь."); //66
                            pl.offset2 = "-1";
                        }

                    }
                    break;

            }
        }
    }
}




