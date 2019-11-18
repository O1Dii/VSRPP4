using SQLite;
using System;
using System.Collections.Generic;
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

namespace VSRPP4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteConnection con;
        private int currentId;
        private int lastId;

        public MainWindow()
        {
            con = new SQLiteConnection(App.DatabasePath);
            InitializeComponent();

            con.CreateTable<Call>();
            List<Call> items = getData();

            currentId = 0;
            CurrentSelectedId.Content = currentId;

            if (items.Count > 0)
            {
                currentId = items.OrderBy(c => c.Id).ToList().Last().Id;
            }

            lastId = currentId;
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            Call call = new Call()
            {
                Id = lastId + 1,
                Num1 = Number_Input_1.Text,
                Num2 = Number_Input_2.Text,
                Start = Start_DateTimePicker.Text,
                End = End_DateTimePicker.Text
            };

            bool exists = getData().Exists(x => x.Num1 == call.Num1 && x.Num2 == call.Num2 && x.Start == call.Start && x.End == call.End);

            if (!exists && AreParamsOK(call))
            {
                con.Insert(call);
                currentId = call.Id;
                CurrentSelectedId.Content = currentId;

                if (currentId > lastId)
                {
                    lastId = currentId;
                }
            }

            getData();
        }

        private bool AreParamsOK(Call call)
        {
            if (call.Num1 == call.Num2)
            {
                return false;
            }

            try
            {
                DateTime start = DateTime.Parse(call.Start);
                DateTime end = DateTime.Parse(call.End);

                if (start > end)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentId != 0)
            {
                con.Delete<Call>(currentId);

                currentId = 0;
                CurrentSelectedId.Content = currentId;
            }

            getData();
        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentId != 0)
            {
                Call call = new Call()
                {
                    Id = currentId,
                    Num1 = Number_Input_1.Text,
                    Num2 = Number_Input_2.Text,
                    Start = Start_DateTimePicker.Text,
                    End = End_DateTimePicker.Text
                };

                if (AreParamsOK(call))
                {
                    con.Delete<Call>(currentId);
                    con.Insert(call);
                }
            }

            getData();
        }

        private List<Call> getData(bool render = true)
        {
            Output_List.Items.Clear();

            List<Call> items = (con.Table<Call>().ToList())?.OrderBy(c => c.Start).ToList();

            if (render)
            {
                foreach (Call call in items)
                {
                    Button btn = new Button() { Background = null, BorderThickness = new Thickness(0, 0, 0, 0) };
                    Grid grid = new Grid();
                    Border gridBorder = new Border() { BorderBrush = Brushes.Gray, BorderThickness = new Thickness(0, 0, 0, 1) };
                    gridBorder.Child = grid;
                    ColumnDefinition columnDefinition1 = new ColumnDefinition();
                    ColumnDefinition columnDefinition2 = new ColumnDefinition();
                    ColumnDefinition columnDefinition3 = new ColumnDefinition();
                    RowDefinition rowDefinition1 = new RowDefinition();
                    RowDefinition rowDefinition2 = new RowDefinition();
                    grid.ColumnDefinitions.Add(columnDefinition1);
                    grid.ColumnDefinitions.Add(columnDefinition2);
                    grid.ColumnDefinitions.Add(columnDefinition3);
                    grid.RowDefinitions.Add(rowDefinition1);
                    grid.RowDefinitions.Add(rowDefinition2);

                    Label id = new Label() { Content = call.Id };
                    Label num1 = new Label() { Content = call.Num1 };
                    Label num2 = new Label() { Content = call.Num2 };
                    Label timeStart = new Label() { Content = call.Start };
                    Label timeEnd = new Label() { Content = call.End };

                    grid.Children.Add(id);
                    grid.Children.Add(num1);
                    grid.Children.Add(num2);
                    grid.Children.Add(timeStart);
                    grid.Children.Add(timeEnd);

                    Grid.SetColumn(id, 0);
                    Grid.SetRowSpan(id, 2);
                    Grid.SetColumn(num1, 1);
                    Grid.SetColumn(num2, 1);
                    Grid.SetRow(num1, 0);
                    Grid.SetRow(num2, 1);
                    Grid.SetColumn(timeStart, 2);
                    Grid.SetColumn(timeEnd, 2);
                    Grid.SetRow(timeStart, 0);
                    Grid.SetRow(timeEnd, 1);

                    btn.Content = gridBorder;
                    Output_List.Items.Add(btn);

                    btn.Click += new RoutedEventHandler(this.Element_Clicked);
                }
            }

            return items;
        }

        private void Show_Button_Click(object sender, RoutedEventArgs e)
        {
            getData();
        }

        private void Element_Clicked(object sender, RoutedEventArgs e)
        {
            Grid grid = (((sender as Button).Content as Border).Child as Grid);

            currentId = Int32.Parse((grid.Children[0] as Label).Content.ToString());
            CurrentSelectedId.Content = currentId;

            Number_Input_1.Text = (grid.Children[1] as Label).Content.ToString();
            Number_Input_2.Text = (grid.Children[2] as Label).Content.ToString();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            con.Close();
        }
    }
}
