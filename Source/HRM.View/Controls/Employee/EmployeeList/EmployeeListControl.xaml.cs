using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using HRM.Model.Entities;
using HRM.Service;
using static HRM.View.DI;

namespace HRM.View
{
    /// <summary>
    /// Interaction logic for EmployeeListControl.xaml
    /// </summary>
    public partial class EmployeeListControl : UserControl
    {
        private BaseService<Employee> mEmployeeService;

        public List<EmployeeListItemViewModel> Employees { get; set; }

        public EmployeeListControl()
        {
            InitializeComponent();

            Employees = new List<EmployeeListItemViewModel>();
            AddEmployees();

            DataContext = this;
        }

        public void AddEmployees()
        {
            mEmployeeService = new BaseService<Employee>();
            Task.Run(async () =>
            {
                while (1 == 1)
                {
                    var response = mEmployeeService.Get();
                    if (response.Data != null)
                    {
                        Employees.Clear();
                        var employees = ((IEnumerable<Employee>)response.Data).ToList();
                        foreach (var employee in employees)
                        {
                            Employees.Add(new EmployeeListItemViewModel()
                            {
                                Employee = employee
                            });
                        }
                        Employees.Add(new EmployeeListItemViewModel()
                        {
                            Employee = null,
                            DisplayName = "Adicionar um novo empregado",
                            IsAddButton = true
                        });
                    }
                    await Task.Delay(10000);
                }
            });
        }
    }
}
