using HRM.Model.Entities;
using HRM.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HRM.View.DI;

namespace HRM.View
{
    /// <summary>
    /// A view model for the overview chat list
    /// </summary>
    public class EmployeeListViewModel : BaseViewModel
    {
        private BaseService<Employee> mEmployeeService;

        public List<EmployeeListItemViewModel> Employees { get; set; }

        public EmployeeListViewModel()
        {
            Employees = new List<EmployeeListItemViewModel>();
            AddEmployees();
        }

        public void AddEmployees()
        {
            mEmployeeService = new BaseService<Employee>();
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
        }
    }
}
