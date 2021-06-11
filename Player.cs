using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GameBot
{
    public class Player
    {
        public string user_id;

        public int game_num;


        public bool viselica_play;
        public int viselica_life; //количество жизней
        public bool viselica_life_chosen; // выбрана ли сложность
        public int viselica_difficulty; //сложность виселицы
        public bool viselica_difficulty_chosen; //выбрана ли сложность 
        public int viselica_category;//категория виселицы
        public string viselica_category_name;// название категории
        public bool viselica_category_chosen;//выбрана ли категория 
        public bool viselica_word_chosen;// выбрано ли слово
        public string viselica_word;//выбранное слово
        public string myWord;//слово,которе будет заполняться буквами по ходу игры
        public int in_letter_num;//количество введённых букв
        public  char letter;
        public  char[] chr;
        public bool viselica_end_game;//закончилась ли игра
        public char[] in_letters;//массив уже введённых букв
        public char[] in_letters_v;//массив уже введённых букв всп
        public  int j = 0;
        public  int k = 0;

        public int goroda_record;//счёт
        public static string path = "input-files/cities.txt"; //путь к бд городов
        public static string[] cities = File.ReadAllLines(path).Select(x => x.ToLower()).ToArray(); //массив городов
        public bool[] used;//массив индексов использованных городов
        public bool flag;
        public char c;
        public bool game_over;


        public string offset1;
        public bool offset_p;


        public string bulls_difficulty;
        public bool bulls_difficulty_chosen;
        public bool print_start_mes1;
        public bool print_start_mes2;
        public string sbot;
        public int bulls;
        public int cows;
        public string mydigit;
        public bool b_gameover;
        public string b_offset;


        public int rps_n;
        public int rounds;
        public int rps_rand;
        public string rps_string;
        public string prevbot; //предыдущий ход бота
        public string prevplayer; //предыдущий ход игрока
        public bool rounds_chosen;
        public bool r1_chosen;
        public bool r2_chosen;
        public bool r3_chosen;
        public int rps_k;
        public bool rps_gameover;

        public string offset2;
        

        public Player(string id)
        {
            this.user_id = id;
        }
    }

    
}
