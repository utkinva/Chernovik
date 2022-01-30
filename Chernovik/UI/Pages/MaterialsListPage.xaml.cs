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
using Chernovik.Entities;
using Chernovik.Utilities;

namespace Chernovik.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для MaterialsListPage.xaml
    /// </summary>
    public partial class MaterialsListPage : Page
    {
        public MaterialsListPage()
        {
            InitializeComponent();
            materialsListV.ItemsSource = Transition.Context.Material.ToList();
            var filterItems = Transition.Context.MaterialType.ToList();
            filterItems.Insert(0, new MaterialType { Title = "Все типы"});
            filterComboBox.ItemsSource = filterItems;
            sortComboBox.SelectedIndex = 0;
            filterComboBox.SelectedIndex = 0;
        }
        #region Метод сортировки, фильтрации и поиска
        void Sorting()
        {
            var updatedList = Transition.Context.Material.ToList();
            if (filterComboBox.SelectedIndex > 0)
            {
                updatedList = updatedList.Where(x => x.MaterialType.Title == (filterComboBox.SelectedItem as MaterialType).Title).ToList();
            }
            if (sortComboBox.SelectedIndex > 0)
            {
                switch (sortComboBox.SelectedIndex)
                {
                    case 1:
                        {
                            if ((bool)ascDescCheckBox.IsChecked)
                            {
                                updatedList = updatedList.OrderByDescending(x => x.Title).ToList();
                            }
                            else
                            {
                                updatedList = updatedList.OrderBy(x => x.Title).ToList();
                            }
                            break;
                        }
                    case 2:
                        {
                            if ((bool)ascDescCheckBox.IsChecked)
                            {
                                updatedList = updatedList.OrderByDescending(x => x.CountInStock).ToList();
                            }
                            else
                            {
                                updatedList = updatedList.OrderBy(x => x.CountInStock).ToList();
                            }
                            break;
                        }
                    case 3:
                        {
                            if ((bool)ascDescCheckBox.IsChecked)
                            {
                                updatedList = updatedList.OrderByDescending(x => x.Cost).ToList();
                            }
                            else
                            {
                                updatedList = updatedList.OrderBy(x => x.Cost).ToList();
                            }
                            break;
                        }
                }
            }
            if (string.IsNullOrWhiteSpace(searchTextBox.Text) != true)
            {
                updatedList = updatedList.Where(x => x.Title.ToLower().Contains(searchTextBox.Text.ToLower()) 
                                                || x.Description.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();
            }
            materialsListV.ItemsSource = updatedList;
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sorting();
        }

        private void ascDescCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Sorting();
        }

        private void ascDescCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Sorting();
        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sorting();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sorting();
        }
        #endregion

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var itemToDelete = materialsListV.SelectedItem as Material;
            if (itemToDelete != null)
            {
                if (MessageBox.Show($"Вы хотите удалить запись №{itemToDelete.ID}?","Удаление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Transition.Context.Material.Remove(itemToDelete);
                        Transition.Context.SaveChanges();
                        MessageBox.Show("Данные удалены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            Sorting();
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            Transition.mainFrame.Navigate(new AddEditPage(materialsListV.SelectedItem as Material));
            Sorting();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            Transition.mainFrame.Navigate(new AddEditPage(null));
            Sorting();
        }

        private void materialsListV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            editBtn.Visibility = Visibility.Visible;
            deleteBtn.Visibility = Visibility.Visible;
        }
    }
}
