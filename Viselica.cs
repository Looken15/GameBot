using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Telegram.Bot;



namespace GameBot
{
    class Viselica
    {
        static public void VIS(Telegram.Bot.Args.MessageEventArgs e, TelegramBotClient Bot)
        {
            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Добро пожаловать в игру 'Виселица' !");
            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите уровень сложности,введя одно из чисел 1-3:");
            Bot.SendTextMessageAsync(e.Message.Chat.Id, "1-легкий");
            Bot.SendTextMessageAsync(e.Message.Chat.Id, "2-средний");
            Bot.SendTextMessageAsync(e.Message.Chat.Id, "3-высокий");
            int num = Convert.ToInt32(e.Message.Text);//уровень сложности
            switch (num)
            {
                case 1:
                    OfferCategories(e,Bot);
                    int category = Convert.ToInt32(Console.ReadLine());//ввод категории
                    Debug.Assert((category > 1) & (category <= 10));
                    LoadingOfFileWithWords(category, num,e,Bot);
                    break;
                case 2:
                    OfferCategories(e,Bot);
                    category = Convert.ToInt32(Console.ReadLine());
                    Debug.Assert((category > 1) & (category <= 10));
                    LoadingOfFileWithWords(category, num,e,Bot);
                    break;
                case 3:
                    OfferCategories(e,Bot);
                    category = Convert.ToInt32(Console.ReadLine());
                    Debug.Assert((category > 1) & (category <= 10));
                    LoadingOfFileWithWords(category, num,e,Bot);
                    break;
                default:
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ошибка ввода");
                    break;
            }

        }
        
        static void OfferCategories(Telegram.Bot.Args.MessageEventArgs e, TelegramBotClient Bot)//функция,выводящая на экран числа от 1 до 10 и соответствующие им категории
        {
            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите одну из следующих категорий,введя номер слева от нее:");
            Console.Write("1-Растения"); Console.Write("2-Животные");
            Console.Write("3-Страны мира"); Console.Write("4-Игры");
            Bot.SendTextMessageAsync(e.Message.Chat.Id, "5-Певцы(по фамилиям)"); Console.Write("6-Актеры (по фамилиям)");
            Console.Write("7-Футболисты(по фамилиям)"); Console.Write("8-Математические термины");
            Console.Write("9-Национальности России"); Console.Write("10-Автомобили");
        }
        //функция,открывающая файл со словами в соответствии с выбранной категорией и рандомно выбирающее слово из него для отгадки
        static void LoadingOfFileWithWords(int category, int num, Telegram.Bot.Args.MessageEventArgs e, TelegramBotClient Bot)
        {
            string path = $@"input-files\\{category}.txt";//шаблон файла со словами
            try
            {
                using (var fs = new FileStream(path, FileMode.Open))
                using (var sr = new StreamReader(fs, Encoding.ASCII))
                {
                    while (!(sr.EndOfStream))
                    {
                        string str = sr.ReadLine();
                        string[] arr = str.Split(' ');
                        Random r = new Random();
                        int i = r.Next(0, arr.Length - 1);//рандомный номер слова в строке
                        GuessTheWord(arr[i], num,e,Bot);
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
        }

        static int[] GuessedLetter(string s,char letter)
        {
            var a = new int[s.Length];
            int j = 0;
            for (var i = 0;i<s.Length;i++)
            {
                if (letter == s[i])
                {
                    a[j] = i;
                    j++;
                }
            }
            for (var i = j;i<s.Length;i++)
            {
                a[i] = -1;
            }
            return a;
        }

        //функция,реализующая непосредственно геймплей
        static void GuessTheWord(string s, int num, Telegram.Bot.Args.MessageEventArgs e, TelegramBotClient Bot)
        {
            
            Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Вам нужно отгадать слово из {s.Length} букв:");
            int life = 0;
            switch (num)//уровень сложности
            {
                case 1:
                    life = 10; break; //кол-во жизней в соответствии с выбранным уровнем сложности
                case 2:
                    life = 7; break;
                case 3:
                    life = 5; break;
            }
            Bot.SendTextMessageAsync(e.Message.Chat.Id, $"У вас есть {life} попыток:");
            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Попробуйте отгадать слово! Итак,Ваша буква ?");
            string myWord = "";//слово,которе будет заполняться буквами по ходу игры
            for (int i = 0; i < s.Length; i++)
                myWord = myWord + ' ';
            char[] chr = myWord.ToCharArray();
            while (chr.Contains(' ') & (life != 0))
            {
                char letter = Convert.ToChar(Console.ReadLine());
                if (s.Contains(letter))
                {
                    
                    //myWord.Where(x => x == letter).Select(elem => letter);
                    int[] a = GuessedLetter(s, letter);
                    for (var i = 0; i<myWord.Length;i++)
                    {
                        if (a.Contains(i))
                        {
                            chr[i] = letter;
                        }
                    }
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, chr.ToString());
                    if (chr.Contains(' ') & (life > 0))
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "Отлично!Следующая буква");
                }
                else
                {
                    life = life - 1;
                    if (chr.Contains(' ') & (life > 0))
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, $"У вас осталось {life} попыток.Итак,Ваша буква ?");
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Слово");
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, chr.ToString());
                }
            }
            myWord = chr.ToString();
            if (!(chr.Contains(' ')))
            {
                //Bot.SendTextMessageAsync(e.Message.Chat.Id, (myWord);
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Поздравляем,вы выиграли!");//выигрыш
            }
            else
            {
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Game over!");//проигрыш
                Console.Write("Было загадано слово "); Bot.SendTextMessageAsync(e.Message.Chat.Id, s);
            }
        }

    }
}
