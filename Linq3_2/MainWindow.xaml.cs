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

namespace Linq3_2
{
    public class OrderSummary : SalesOrderHeader
    {
        public override string ToString()
        {
            return $"{OrderDate.ToShortDateString()}, {SalesOrderID}, {TotalDue:c}";
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AdventureLiteEntities al = new AdventureLiteEntities();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var query = from a in al.SalesOrderHeaders
                        orderby a.Customer.CompanyName
                        select a.Customer.CompanyName;

            customersLbx.ItemsSource = query.ToList().Distinct();
        }

        private void customersLbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string customer = customersLbx.SelectedItem as string;

            if (customer != null)
            {
                var query = from a in al.SalesOrderHeaders
                            where a.Customer.CompanyName == customer
                            select new OrderSummary
                            {
                                SalesOrderID = a.SalesOrderID,
                                OrderDate = a.OrderDate,
                                TotalDue = a.TotalDue
                            };

                ordersLbx.ItemsSource = query.ToList();
            }
        }

        private void ordersLbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = Convert.ToInt32(ordersLbx.SelectedValue);

            if (id > 0)
            {
                var query = from a in al.SalesOrderDetails
                            where a.SalesOrderID == id
                            select new
                            {
                                ProductName = a.Product.Name,
                                a.UnitPrice,
                                a.UnitPriceDiscount,
                                a.OrderQty,
                                a.LineTotal
                            };

                detailsDg.ItemsSource = query.ToList();
            }
        }
    }
}
