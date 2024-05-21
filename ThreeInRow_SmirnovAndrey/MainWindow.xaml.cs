using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThreeInRow_SmirnovAndrey
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int colors = 6;//количество цветов плиток
        Field field = new Field();//двумерный массив с числами для цветов плиток 
        public MainWindow()
        {
            InitializeComponent();



            ////указыается количество строк и столбцов в сетке
            //GridForField.Rows = 5;
            //GridForField.Columns = 5;
            ////указываются размеры сетки (число ячеек * (размер кнопки в ячейки + толщина её границ))
            //GridForField.Width = 5 * (50 + 4);
            //GridForField.Height = 5 * (50 + 4);
            ////толщина границ сетки
            //GridForField.Margin = new Thickness(5, 5, 5, 5);


            //    for (int i = 0; i < 5 * 5; i++)
            //    {
            //        //создание кнопки
            //        Button btn = new Button();
            //        //запись номера кнопки
            //        btn.Tag = i;
            //        //установка размеров кнопки
            //        btn.Width = 50;
            //        btn.Height = 50;
            //        //текст на кнопке
            //        btn.Content = " ";
            //        //толщина границ кнопки
            //        btn.Margin = new Thickness(2);
            //        //при нажатии кнопки, будет вызываться метод Btn_Click
            //        btn.Click += Btn_Click;
            //        //добавление кнопки в сетку
            //        GridForField.Children.Add(btn);
            //    }



        }

        //private void Btn_Click(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void CreateButton_Click(object sender, RoutedEventArgs e)//Нажатие кнопки, создающей поле
        {
            if (RB1.IsChecked == true)
            {
                field.GenerateField(5, 5, colors, MovesLeft);//генерация массива 5x5
                field.GenerateGrid(GridForField);
            }
            if (RB2.IsChecked == true)
            {
                field.GenerateField(10, 10, colors, MovesLeft);//генерация массива 5x5
                field.GenerateGrid(GridForField);
            }
            if (RB3.IsChecked == true)
            {
                field.GenerateField(15, 15, colors, MovesLeft);//генерация массива 5x5
                field.GenerateGrid(GridForField);
            }
            if (RB1.IsChecked != true && RB2.IsChecked != true && RB3.IsChecked != true)
            {
                MessageBox.Show("Не выбран размер поля");
            }
            field.SetRecord(Record);
            field.SearchCombo();
        }
    }
}
