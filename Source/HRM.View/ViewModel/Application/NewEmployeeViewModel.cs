using HRM.Model.Entities;
using HRM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static HRM.View.DI;

namespace HRM.View
{
    public class NewEmployeeViewModel : BaseViewModel
    {
        public TextEntryViewModel Name { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public NewEmployeeViewModel()
        {
            Name = new TextEntryViewModel
            {
                Label = "Nome",
                OriginalText = ""
            };

            AddCommand = new RelayCommand(SalvarUsuarioAsync);
            CloseCommand = new RelayCommand(Close);
        }

        public void Close()
        {
            // Close settings menu
            ViewModelApplication.NewEmployeeMenuVisible = false;
        }


        public async void SalvarUsuarioAsync()
        {
            var employee = new Employee()
            {
                Name = Name.EditedText,
                DepartmentId = new Guid("820f4629-100a-4bd7-b189-785eff883cf9"),
                WorkShiftId = new Guid("1aa669da-a187-473d-b9de-e8d4a7cf9b29")
            };
            var employeeService = new BaseService<Employee>();

            employeeService.Post(employee);

            await UI.ShowMessage(new MessageBoxDialogViewModel
            {
                Title = "Sucesso",
                Message = "Usuário cadastrado com sucesso!",
                YesText = "OK"
            });

            Name.OriginalText = string.Empty;
            Close();
        }
    }
}
