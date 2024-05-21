using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Globalization;
using System.Data.Common;
using System.Windows.Media.Imaging;

namespace ThreeInRow_SmirnovAndrey
{
    internal class Field
    {
        private int moves = 0;//сколько осталось ходов
        Label Moves;
        private int colorsValue;
        Random Rnd = new Random();
        private int[,] field;//двумерный массив для чисел, обозначающих цвета плиток
        private int SizeX;
        private int SizeY;
        UniformGrid GridForField;
        Label Record;
        private int check;//переменная для счёта
        Button[,] tiles;
        BitmapImage blueGem = new BitmapImage(new Uri(@"pack://application:,,,/Gems/blue.png", UriKind.Absolute));
        BitmapImage brownGem = new BitmapImage(new Uri(@"pack://application:,,,/Gems/brown.png", UriKind.Absolute));
        BitmapImage greenGem = new BitmapImage(new Uri(@"pack://application:,,,/Gems/green.png", UriKind.Absolute));
        BitmapImage orangeGem = new BitmapImage(new Uri(@"pack://application:,,,/Gems/orange.png", UriKind.Absolute));
        BitmapImage pinkGem = new BitmapImage(new Uri(@"pack://application:,,,/Gems/pink.png", UriKind.Absolute));
        BitmapImage redGem = new BitmapImage(new Uri(@"pack://application:,,,/Gems/red.png", UriKind.Absolute));
        BitmapImage yellowGem = new BitmapImage(new Uri(@"pack://application:,,,/Gems/yellow.png", UriKind.Absolute));
        public void GenerateField(int sizeY, int sizeX, int colorsValue, Label Moves)//метод, получающий размеры поля по X и Y и количетсво цветов плиток
        {
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            this.colorsValue = colorsValue;
            int[,] field = new int[sizeY, sizeX];//поле для чисел, обозначающих цвет
            for (int i = 0; i < sizeY; i++)//пробегает по горизонтали
            {
                for (int j = 0; j < sizeX; j++)//пробегает по вертикали
                {
                    field[i, j] = Rnd.Next(0, colorsValue);//генерирует случайные числа для поля
                }    
            }
            //return field;//возвращает массив с числами для поля
            this.field = field;//присваивает полю двумерный массив с числами
            this.Moves = Moves;
            moves = 20;
            Moves.Content = moves;
        }
        private int[,] getField()//возвращает поле
        {
            return field;
        }
        public void GenerateGrid(UniformGrid GridForField)
        {
            //Button[,]
            tiles = new Button[SizeY, SizeX];
            this.GridForField = GridForField;
            GridForField.Rows = SizeX;//количество строк в поле
            GridForField.Columns = SizeY;//количество столбцов в поле
            GridForField.Width = SizeX * (50 + 4);//размеры поля по высоте (число ячеек * (размер кнопки в ячейки + толщина её границ))
            GridForField.Height = SizeY * (50 + 4);//размеры поля по ширине (число ячеек * (размер кнопки в ячейки + толщина её границ))
            GridForField.Margin = new Thickness(5, 5, 5, 5);//толщина границ поля
            for (int i = 0; i < SizeY * SizeX; i++)//цикл, создающий кнопки
            {
                int n = i / SizeX;
                int m = i % SizeY;
                //Button btn = new Button();//создание кнопки
                tiles[n, m] = new Button();//создание кнопки в массиве tiles
                tiles[n, m].Tag = i;//запись номера кнопки
                tiles[n, m].Width = 50;//установка высоты кнопки
                tiles[n, m].Height = 50;//установка ширины кнопки
                tiles[n, m].Content = field[n, m];//текст на кнопке
                tiles[n, m].Margin = new Thickness(2);//толщина границ кнопки
                tiles[n, m].Click += Btn_Click;//при нажатии кнопки, будет вызываться метод Btn_Click
                GridForField.Children.Add(tiles[n, m]);//добавление кнопки в поле
                int sc = 1;
                while (sc != 0)//выполняет метод SearchCombo до тех пор, пока комбо не останеться. Таким образом очищает стартовое поле от комбо
                {
                    sc = SearchCombo();
                }
                Colorize();
            }
        }
        int selectButtI = 9999;//координ
        int selectButtJ = 9999;//переменная для ширины кнопки
        private void Btn_Click(object sender, RoutedEventArgs e)//метод нажатияя кнопки на поле
        {
            //throw new NotImplementedException();
            if (selectButtI > 9998 || selectButtJ > 9998)
            {
                int n = (int)((Button)sender).Tag;
                selectButtI = n / SizeX;
                selectButtJ = n % SizeY;
            }
            else
            {
                int n = (int)((Button)sender).Tag;
                int i = n / SizeX;
                int j = n % SizeY;
                Swap(selectButtI, selectButtJ, i, j);
                selectButtI = 9999;
                selectButtJ = 9999;
            }
        }
        private void Swap(int i, int j, int m, int n)//метод, меняющий местами две плитки
        {
            if (moves > 0)
            {
                if (((i == m) && ((j == n - 1) || (j == n + 1))) || (((i == m - 1) || (i == m + 1)) && (j == n)))
                {
                    int temp = field[i, j];
                    field[i, j] = field[m, n];
                    field[m, n] = temp;
                    int sc = 1;
                    while (sc != 0)//выполняет метод SearchCombo до тех пор, пока комбо не останеться. Таким образом очищает стартовое поле от комбо
                    {
                        sc = SearchCombo();
                        check = check + sc;
                    }
                    Record.Content = check;
                    Colorize();
                    moves--;
                    Moves.Content = moves;
                    if (moves == 0)
                    {
                        MessageBox.Show("Игра окончена, ваш счёт: " + Record.Content + ". Поздравляем!");
                        Record.Content = 0;
                    }
                }
                else
                {
                    MessageBox.Show("Выбранные плитки расположены слишком далеко друг от друга");
                }
            }
            else
            {
                MessageBox.Show("У вас не осталось ходов");
            }
        }
        public void SetRecord(Label Record)
        {
            this.Record = Record;
        }
        public int SearchCombo()//метод для поиска комбо из плиток
        {
            int score = 0;
            //int combo = 0;
            //int maxCombo = 0;
            int[] iBut = new int[5];//массив для плиток в комбо
            int[] jBut = new int[5];//массив для плиток в комбо
            for (int i = 0; i < SizeY; i++)//пробегает по вертикали
            {
                for (int j = 4; j < SizeX; j++)
                {
                    if ((field[i, j] == field[i, j - 1]) && (field[i, j] == field[i, j - 2]) && (field[i, j] == field[i, j - 3]) && (field[i, j] == field[i, j - 4]))
                    {
                        score = score + 5;
                        iBut[0] = i; iBut[1] = i; iBut[2] = i; iBut[3] = i; iBut[4] = i;
                        jBut[0] = j - 4; jBut[1] = j - 3; jBut[2] = j - 2; jBut[3] = j - 1; jBut[4] = j;
                        RandomGenerate(iBut, jBut, 5);
                    }
                }
                for (int j = 3; j < SizeX; j++)
                {
                    if ((field[i, j] == field[i, j - 1]) && (field[i, j] == field[i, j - 2]) && (field[i, j] == field[i, j - 3]))
                    {
                        score = score + 4;
                        iBut[0] = i; iBut[1] = i; iBut[2] = i; iBut[3] = i;
                        jBut[0] = j - 3; jBut[1] = j - 2; jBut[2] = j - 1; jBut[3] = j;
                        RandomGenerate(iBut, jBut, 4);
                    }
                }
                for (int j = 2; j < SizeX; j++)
                {
                    if ((field[i, j] == field[i, j - 1]) && (field[i, j] == field[i, j - 2]))
                    {
                        score = score + 3;
                        iBut[0] = i; iBut[1] = i; iBut[2] = i;
                        jBut[0] = j - 2; jBut[1] = j - 1; jBut[2] = j;
                        RandomGenerate(iBut, jBut, 3);
                    }
                }
            }
            for (int j = 0; j < SizeX; j++)//пробегает по горизонтали
            {
                for (int i = 4; i < SizeY; i++)
                {
                    if ((field[i, j] == field[i - 1, j]) && (field[i, j] == field[i - 2, j]) && (field[i, j] == field[i - 3, j]) && (field[i, j] == field[i - 4, j]))
                    {
                        score = score + 5;
                        iBut[0] = i - 4; iBut[1] = i - 3; iBut[2] = i - 2; iBut[3] = i - 1; iBut[4] = i;
                        jBut[0] = j; jBut[1] = j; jBut[2] = j; jBut[3] = j; jBut[4] = j;
                        RandomGenerate(iBut, jBut, 5);
                    }
                }
                for (int i = 3; i < SizeY; i++)
                {
                    if ((field[i, j] == field[i - 1, j]) && (field[i, j] == field[i - 2, j]) && (field[i, j] == field[i - 3, j]))
                    {
                        score = score + 4;
                        iBut[0] = i - 3; iBut[1] = i - 2; iBut[2] = i - 1; iBut[3] = i;
                        jBut[0] = j; jBut[1] = j; jBut[2] = j; jBut[3] = j;
                        RandomGenerate(iBut, jBut, 4);
                    }
                }
                for (int i = 2; i < SizeY; i++)
                {
                    if ((field[i, j] == field[i - 1, j]) && (field[i, j] == field[i - 2, j]))
                    {
                        score = score + 3;
                        iBut[0] = i - 2; iBut[1] = i - 1; iBut[2] = i;
                        jBut[0] = j; jBut[1] = j; jBut[2] = j;
                        RandomGenerate(iBut, jBut, 3);
                    }
                }
            }
            return score;
        }
        private void RandomGenerate(int[] n, int[] m, int k)//получает массив с координатами по i, массив с координатами по j, и количество элементов, которые нужно сделать случайными
        {
            for (int i = 0; i < k; i++)
            {
                field[n[i], m[i]] = Rnd.Next(0, colorsValue);
            }
        }
        private void Colorize()//метод, раскрашивающий плитки в цвета
        {
            for (int i = 0; i < SizeY; i++)
            {
                for (int j = 0; j < SizeX; j++)
                {
                    if (tiles[i, j] != null)
                    {
                        switch (field[i, j])
                        {
                            case 0:
                                {
                                    tiles[i, j].Content = setStackPanel(blueGem);
                                    //tiles[i, j].Content = field[i, j];
                                    break;
                                }
                            case 1:
                                {
                                    tiles[i, j].Content = setStackPanel(brownGem);
                                    //tiles[i, j].Content = field[i, j];
                                    break;
                                }
                            case 2:
                                {
                                    tiles[i, j].Content = setStackPanel(greenGem);
                                    //tiles[i, j].Content = field[i, j];
                                    break;
                                }
                            case 3:
                                {
                                    tiles[i, j].Content = setStackPanel(orangeGem);
                                    //tiles[i, j].Content = field[i, j];
                                    break;
                                }
                            case 4:
                                {
                                    tiles[i, j].Content = setStackPanel(pinkGem);
                                    //tiles[i, j].Content = field[i, j];
                                    break;
                                }
                            case 5:
                                {
                                    tiles[i, j].Content = setStackPanel(redGem);
                                    //tiles[i, j].Content = field[i, j];
                                    break;
                                }
                            case 6:
                                {
                                    tiles[i, j].Content = setStackPanel(yellowGem);
                                    //tiles[i, j].Content = field[i, j];
                                    break;
                                }
                        }
                    }
                }
            }
        }
        private StackPanel setStackPanel(BitmapImage gem)//загружкает картинку в StackPanel
        {
            Image img = new Image();//создание контейнера для хранения изображения
            img.Source = gem;//запись картинки в контейнер
            StackPanel stackP = new StackPanel();//создание компонента для отображения изображения
            stackP.Margin = new Thickness(1);//установка толщины границ компонента
            stackP.Children.Add(img);//добавление контейнера с картинкой в компонент
            return stackP;
        }
    }
}
