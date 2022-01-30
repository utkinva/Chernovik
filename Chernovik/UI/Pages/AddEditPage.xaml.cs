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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        Material currentMaterial;
        public AddEditPage(Material material)
        {
            InitializeComponent();
            currentMaterial = material ?? new Material();
            materialTypeComboBox.ItemsSource = Transition.Context.MaterialType.ToList();
            DataContext = currentMaterial;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Transition.mainFrame.GoBack();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentMaterial.Title))
                errors.AppendLine("Укажите наименование");
            if (currentMaterial.MaterialType == null)
                errors.AppendLine("Выберите тип материала");
            if (!int.TryParse(currentMaterial.CountInStock.ToString(), out _))
                errors.AppendLine("Укажите количество материалов на складе");
            if (string.IsNullOrWhiteSpace(currentMaterial.Unit))
                errors.AppendLine("Укажите единицу измерения");
            if (!int.TryParse(currentMaterial.CountInPack.ToString(), out _))
                errors.AppendLine("Укажите количество материалов в упаковке");
            if (!int.TryParse(currentMaterial.MinCount.ToString(), out _))
                errors.AppendLine("Укажите мин. кол-во материалов");
            else if (currentMaterial.MinCount.ToString().StartsWith("-"))
                errors.AppendLine("Мин. кол-во должно быть не отрицательным");
            if (!decimal.TryParse(currentMaterial.Cost.ToString(), out _))
                errors.AppendLine("Укажите стоимость за единицу");
            else if (currentMaterial.Cost.ToString().StartsWith("-"))
                errors.AppendLine("Стоимость должна быть не отрицательной");

            if (string.IsNullOrWhiteSpace(currentMaterial.Description))
                currentMaterial.Description = "";

            if (errors.Length > 0)
            {
                MessageBox.Show($"Ошибка при сохранении данных:\n{errors.ToString()}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (currentMaterial.ID == 0)
                {
                    Transition.Context.Material.Add(currentMaterial);
                }
                try
                {
                    Transition.Context.SaveChanges();
                    MessageBox.Show("Данные сохранены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    Transition.mainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
