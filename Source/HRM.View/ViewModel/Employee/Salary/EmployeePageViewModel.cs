using HRM.Model.Entities;
using HRM.Model.Enum.Salary;
using HRM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using static HRM.View.DI;

namespace HRM.View
{
    /// <summary>
    /// A view model for a chat message thread list
    /// </summary>
    public class EmployeePageViewModel : BaseViewModel
    {
        #region Protected Members

        protected Employee mEmployee;

        protected DateTime mReferenceDate;

        protected string mGrossMonthSalary;

        protected string mExtraName;

        protected string mExtraAmount;

        protected bool mIsUpdating = false;

        protected bool mFirst13Portion = false;
        #endregion

        #region Public Properties

        public bool IsAddExtraRunning { get; set; }

        public bool IsSaveRunning { get; set; }

        public bool IsCalculateRunning { get; set; }

        public List<SalaryItemViewModel> Salaries { get; set; }

        public List<SalaryItemViewModel> RemovedSalaries { get; set; }

        public Employee Employee
        {
            get => mEmployee;
            set
            {
                mEmployee = value;

                if (value == null)
                {
                    return;
                }

                Id = mEmployee.Id;
                Name = mEmployee.Name;
                SearchEmployeeData();
            }
        }

        /// <summary>
        /// The employee id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The employee name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The employee department
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// The employee work shift name
        /// </summary>
        public string WorkShiftName { get; set; }

        /// <summary>
        /// The employee work shift hours
        /// </summary>
        public string WorkShiftHours { get; set; }

        public DateTime ReferenceDate
        {
            get => mReferenceDate;
            set
            {
                mReferenceDate = value;
                SearchEmployeeSalaryAsync(false, value);
            }
        }

        public string GrossMonthSalary
        {
            get => mGrossMonthSalary;
            set => mGrossMonthSalary = value;
        }

        public string ExtraName
        {
            get => mExtraName;
            set => mExtraName = value;
        }

        public string ExtraAmount
        {
            get => mExtraAmount;
            set => mExtraAmount = value;
        }

        public int VacationDays { get; set; }

        public bool SellVacation { get; set; }
        #endregion

        #region Public Commands
        public ICommand AddExtraCommand { get; set; }

        public ICommand CalculateCommand { get; set; }

        public ICommand DeleteExtraCommand { get; set; }

        public ICommand SaveCommand { get; set; }
        #endregion

        #region Private Properties
        private BaseService<Department> mDepartmentService;
        private BaseService<WorkShift> mWorkShiftService;
        private SalaryService mSalaryService;
        private BaseService<Extra> mExtraService;
        private BaseService<Vacation> mVacationService;
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public EmployeePageViewModel()
        {
            // Create commands
            AddExtraCommand = new RelayCommand(async () => await AddExtraAsync());
            CalculateCommand = new RelayCommand(async () => await CalculateAsync());
            DeleteExtraCommand = new RelayParameterizedCommand(async (parameter) => await DeleteExtraAsync(parameter));
            SaveCommand = new RelayCommand(async () => await SaveAsync());

            Salaries = new List<SalaryItemViewModel>();
            RemovedSalaries = new List<SalaryItemViewModel>();
            ReferenceDate = DateTime.Now;
            SellVacation = false;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Clears the search text
        /// </summary>
        public void ClearSearch()
        {
            GrossMonthSalary = string.Empty;
            Salaries = new List<SalaryItemViewModel>();
        }

        public async Task AddExtraAsync()
        {
            await RunCommandAsync(() => IsAddExtraRunning, async () =>
            {
                if (string.IsNullOrEmpty(ExtraName) || string.IsNullOrEmpty(ExtraAmount))
                {
                    await UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Erro",
                        Message = "Preencha todos os campos!",
                        YesText = "OK"
                    });
                    return;
                }

                if (!decimal.TryParse(ExtraAmount, out var amount))
                {
                    await UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Erro",
                        Message = "Digite um valor válido!",
                        YesText = "OK"
                    });
                    return;
                }

                if (Salaries.Where(x => x.Name.ToUpper() == ExtraName.ToUpper()).Count() > 0)
                {
                    await UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Erro",
                        Message = $"Já existe um salário deste tipo! ({ExtraName})",
                        YesText = "OK"
                    });
                    return;
                }

                AddSalary(new SalaryItemViewModel()
                {
                    Name = ExtraName,
                    Amount = amount,
                    SalaryType = amount > 0 ? SalaryType.Increase : SalaryType.Decrease,
                    DeleteCommand = DeleteExtraCommand,
                });
            });
        }

        public async Task DeleteExtraAsync(object parameter)
        {
            var salaryItem = (SalaryItemViewModel)parameter;
            await UI.ShowMessage(new MessageBoxDialogViewModel
            {
                Title = "Atenção",
                Message = $"Deseja realmente deletar o salário {salaryItem.Name}?",
                YesCommand = new RelayParameterizedCommand(async (window) => await RunCommandAsync(() => salaryItem.DeleteIsRunning, async () =>
                {
                    DeleteItem(salaryItem);

                    await Task.Delay(1);
                    ((DialogWindow)window).Close();
                })),
                NoCommand = new RelayParameterizedCommand((window) => { ((DialogWindow)window).Close(); }),
                HaveNoButton = true,
            });
        }

        private void DeleteItem(SalaryItemViewModel salaryItem)
        {
            var deletedExtra = Salaries.Where(x => x.Name == salaryItem.Name.ToString()).FirstOrDefault();
            RemovedSalaries.Add(deletedExtra);
            Salaries = Salaries.Where(x => x.Name != salaryItem.Name.ToString()).ToList();

            var netSalary = Salaries.Where(x => x.SalaryType == SalaryType.Net).FirstOrDefault();
            if (netSalary != null)
            {
                Salaries.Remove(netSalary);
                netSalary.Amount += (salaryItem.Amount) * -1;
                Salaries.Add(netSalary);
            }

            if (deletedExtra.Name == "13º Parcela 1")
            {
                mFirst13Portion = false;
            }
        }

        public async Task CalculateAsync()
        {
            await RunCommandAsync(() => IsCalculateRunning, async () => 
            {
                if (ReferenceDate.Month == 12)
                {
                    CalculateDecemberSalary();
                    return;
                }

                if (mFirst13Portion)
                {
                    BaseCalculusAsync();
                    return;
                }

                await AskForThirteenthSalaryAsync();
            });
        }

        private void CalculateDecemberSalary()
        {
            BaseCalculusAsync();

            var grossMonthSalary = decimal.Parse(GrossMonthSalary);
            mSalaryService = new SalaryService();

            var INSS = mSalaryService.CalculateINSS(grossMonthSalary);
            var IRRF = mSalaryService.CalculateIRRF(grossMonthSalary, INSS);

            if (mFirst13Portion)
            {
                AddSalary(new SalaryItemViewModel()
                {
                    Name = "13º Parcela 2",
                    Amount = (grossMonthSalary / 2) - IRRF - INSS,
                    SalaryType = SalaryType.Increase,
                    DeleteCommand = DeleteExtraCommand,
                });
            }
            else
            {
                AddSalary(new SalaryItemViewModel()
                {
                    Name = "13º Parcela Única",
                    Amount = grossMonthSalary - IRRF - INSS,
                    SalaryType = SalaryType.Increase,
                    DeleteCommand = DeleteExtraCommand,
                });
            }
        }

        private async void BaseCalculusAsync(decimal? parsedValue = null)
        {
            var grossMonthSalary = 0m;
            if (parsedValue != null)
            {
                grossMonthSalary = (decimal)parsedValue;
            }
            else
            {
                if (!decimal.TryParse(GrossMonthSalary, out grossMonthSalary))
                {
                    await UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Erro",
                        Message = "Digite um valor válido!",
                        YesText = "OK"
                    });
                    return;
                }
            }

            mSalaryService = new SalaryService();

            AddSalary(new SalaryItemViewModel()
            {
                Name = "Salário Liquído",
                Amount = grossMonthSalary,
                SalaryType = SalaryType.Net,
            });

            AddSalary(new SalaryItemViewModel()
            {
                Name = "FGTS",
                Amount = grossMonthSalary * 0.08m * -1,
                SalaryType = SalaryType.Decrease,
                DeleteCommand = DeleteExtraCommand,
            });

            var INSS = mSalaryService.CalculateINSS(grossMonthSalary);
            AddSalary(new SalaryItemViewModel()
            {
                Name = "INSS",
                Amount = (INSS * -1),
                SalaryType = SalaryType.Decrease,
                DeleteCommand = DeleteExtraCommand,
            });

            var IRRF = mSalaryService.CalculateIRRF(grossMonthSalary, INSS);
            if (IRRF > 0)
            {
                AddSalary(new SalaryItemViewModel()
                {
                    Name = "IRRF",
                    Amount = (IRRF * -1),
                    SalaryType = SalaryType.Decrease,
                    DeleteCommand = DeleteExtraCommand,
                });
            }
            else
            {
                var IRRFSalary = Salaries.Where(x => x.Name == "IRRF").FirstOrDefault();
                if (IRRFSalary != null)
                {
                    RemovedSalaries.Add(IRRFSalary);
                    Salaries = Salaries.Where(x => x.Name != "IRRF").ToList();
                }
            }

            CalculateVacation(grossMonthSalary);
        }

        private void CalculateVacation(decimal grossMonthsalary)
        {
            if (VacationDays <= 0)
            {
                if (Salaries.Where(x => x.Name == "Férias").Count() > 0)
                {
                    DeleteItem(Salaries.Where(x => x.Name == "Férias").FirstOrDefault());
                }
                return;
            }

            var vacation = new Vacation
            {
                EmployeeId = Id,
                DateStart = ReferenceDate
            };

            mSalaryService = new SalaryService();
            mSalaryService.CalculateVacation(grossMonthsalary, vacation, SellVacation, ReferenceDate, VacationDays, out var soldDaysValue, out var totalValue);
            var INSS = mSalaryService.CalculateINSS(totalValue);

            AddSalary(new SalaryItemViewModel()
            {
                Name = "Férias",
                Amount = totalValue + soldDaysValue - INSS,
                SalaryType = SalaryType.Increase,
                DeleteCommand = DeleteExtraCommand,
            });

            mVacationService = new BaseService<Vacation>();
            var response = mVacationService.Get();
            if (response.Data != null)
            {
                var allVacations = ((IEnumerable<Vacation>)response.Data);
                var monthVacation = allVacations.Where(x => x.DateStart.Date == ReferenceDate.Date).FirstOrDefault();
                if (monthVacation != null)
                {
                    vacation.Id = monthVacation.Id;
                    mVacationService.Put(vacation);
                }
                else
                {
                    mVacationService.Post(vacation);
                }
            }
        }

        private async Task AskForThirteenthSalaryAsync()
        {
            var dialogBox = new MessageBoxDialogViewModel
            {
                Title = "Atenção",
                Message = $"Deseja adiantar o 13º salário?",
                HaveNoButton = true,
            };
            dialogBox.YesCommand = new RelayParameterizedCommand(async (window) =>
            {
                await RunCommandAsync(() => dialogBox.IsYesCommandRunning, async () =>
                {
                    if (!decimal.TryParse(GrossMonthSalary, out var grossMonthSalary))
                    {
                        await UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Erro",
                            Message = "Digite um valor válido!",
                            YesText = "OK"
                        });
                        return;
                    }

                    BaseCalculusAsync(grossMonthSalary);
                    AddSalary(new SalaryItemViewModel()
                    {
                        Name = "13º Parcela 1",
                        Amount = grossMonthSalary / 2,
                        SalaryType = SalaryType.Increase,
                        DeleteCommand = DeleteExtraCommand,
                    });
                    mFirst13Portion = true;
                    await Task.Delay(1);
                    ((DialogWindow)window).Close();
                });
            });
            dialogBox.NoCommand = new RelayParameterizedCommand(async (window) =>
            {
                await RunCommandAsync(() => dialogBox.IsNoCommandRunning, async () =>
                {
                    BaseCalculusAsync();
                    await Task.Delay(1);
                    ((DialogWindow)window).Close();
                });
            });

            await UI.ShowMessage(dialogBox);
        }

        private void SearchEmployeeData()
        {
            SearchEmployeeDepartment();
            SearchEmployeeWorkShift();
            SearchEmployeeSalaryAsync(true, null);
        }

        private async void SearchEmployeeSalaryAsync(bool isFirstSearch, DateTime? monthReference = null)
        {
            if (monthReference == null)
                monthReference = DateTime.Now;

            mSalaryService = new SalaryService();
            mExtraService = new BaseService<Extra>();

            var employeeSalaries = mSalaryService.GetEmployeeSalaries(Id);
            var monthSalary = employeeSalaries.Where(x => x.Date.Month == monthReference?.Month
                                                       && x.Date.Year == monthReference?.Year).ToList();
            if (monthSalary.Count == 0)
            {
                if (!isFirstSearch && !Id.Equals(Guid.Empty))
                {
                    await UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Erro",
                        Message = "Nenhum salário cadastrado para este mês!",
                        YesText = "OK"
                    });
                }

                mIsUpdating = false;
                ClearSearch();
                return;
            }

            var searchedSalary = monthSalary.Where(x => x.Date.Date == monthReference?.Date).FirstOrDefault();
            if (searchedSalary == null)
            {
                mIsUpdating = false;
                searchedSalary = monthSalary.Where(x => x.Date.Month == monthReference?.Month).OrderByDescending(x => x.Date).FirstOrDefault();
                await UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Aviso",
                    Message = $"Nenhum salário cadastrado para o dia {monthReference?.ToString("dd/MM/yyyy")}, \n o salário do dia {searchedSalary.Date:dd/MM/yyyy} foi buscado no lugar!",
                    YesText = "OK"
                });
            }
            else
            {
                mIsUpdating = true;
            }

            GrossMonthSalary = searchedSalary.GrossAmount.ToString();

            var response = mExtraService.Get();
            if (response.Data != null)
            {
                var extras = ((IEnumerable<Extra>)response.Data).ToList();
                var salaryExtras = extras.Where(x => x.SalaryId == searchedSalary.Id).ToList();
                var items = new List<SalaryItemViewModel>();
                foreach (var salaryExtra in salaryExtras)
                {
                    items.Add(new SalaryItemViewModel()
                    {
                        Name = salaryExtra.Name,
                        Amount = salaryExtra.Amount,
                        SalaryType = salaryExtra.Amount < 0 ? SalaryType.Decrease : SalaryType.Increase,
                        DeleteCommand = DeleteExtraCommand
                    });
                }
                items.Add(new SalaryItemViewModel()
                {
                    Name = "Salário Liquído",
                    Amount = searchedSalary.NetAmount,
                    SalaryType = SalaryType.Net
                });
                Salaries = items.OrderBy(x => x.SalaryType).ToList();

                var salariesIds = employeeSalaries.Select(x => x.Id);
                var employeeExtras = extras.Where(x => salariesIds.Contains(x.SalaryId)).ToList();
                mFirst13Portion = employeeExtras.Where(x => x.Name == "13º Parcela 1").Count() > 0;
            }
        }

        private void SearchEmployeeDepartment()
        {
            mDepartmentService = new BaseService<Department>();
            var departmentResponse = mDepartmentService.Get(mEmployee.DepartmentId);
            if (departmentResponse.Data != null)
            {
                Department = ((Department)departmentResponse.Data).Name;
            }
        }

        private void SearchEmployeeWorkShift()
        {
            mWorkShiftService = new BaseService<WorkShift>();
            var workShiftResponse = mWorkShiftService.Get(mEmployee.WorkShiftId);
            if (workShiftResponse.Data != null)
            {
                var workShift = ((WorkShift)workShiftResponse.Data);
                WorkShiftName = workShift.Name;
                WorkShiftHours = workShift.StartHour.ToString("hh\\:mm") + " até " + workShift.EndHour.ToString("hh\\:mm");
            }
        }

        private void AddSalary(SalaryItemViewModel salary)
        {
            if (Salaries.Where(x => x.Name.ToUpper() == salary.Name.ToUpper()).Count() == 0)
            {
                Salaries.ForEach(x => x.NewSalary = false);
                salary.NewSalary = true;
                Salaries.Add(salary);
            }
            else
            {
                Salaries.ForEach(x =>
                {
                    if (x.Name.ToUpper() == salary.Name.ToUpper())
                    {
                        x.Amount = salary.Amount;
                    }
                });
            }

            var netSalary = Salaries.Where(x => x.SalaryType == SalaryType.Net).FirstOrDefault();
            if (netSalary != null && salary.SalaryType != SalaryType.Net)
            {
                Salaries.ForEach(x =>
                {
                    if (x.SalaryType == SalaryType.Net)
                    {
                        x.Amount += salary.Amount;
                    }
                });
            }

            Salaries = Salaries.OrderBy(x => x.SalaryType).ToList();
        }

        public async Task SaveAsync()
        {
            await RunCommandAsync(() => IsSaveRunning, async () =>
            {
                var baseSalaryService = new BaseService<Salary>();

                var salaryId = CreateAndUpdateSalary(baseSalaryService);

                mExtraService = new BaseService<Extra>();
                var allExtras = ((IEnumerable<Extra>)mExtraService.Get().Data).ToList();
                var salaryExtras = allExtras.Where(x => x.SalaryId == salaryId).ToList();

                CreateAndUpdateExtras(salaryId, salaryExtras);
                DeleteSalaries(salaryExtras);
                await Task.Delay(1);
            });
        }

        private Guid CreateAndUpdateSalary(BaseService<Salary> baseSalaryService)
        {
            mSalaryService = new SalaryService();
            var netSalary = Salaries.Where(x => x.SalaryType == SalaryType.Net).FirstOrDefault();

            if (mIsUpdating)
            {
                var salary = mSalaryService.GetEmployeeSalaries(Id).Where(x => x.Date.Date == ReferenceDate.Date).FirstOrDefault();
                salary.GrossAmount = decimal.Parse(GrossMonthSalary);
                salary.NetAmount = netSalary.Amount;
                return (Guid)baseSalaryService.Put(salary).Data;
            }
            else
            {
                return (Guid)baseSalaryService.Post(new Salary()
                {
                    EmployeeId = Id,
                    GrossAmount = decimal.Parse(GrossMonthSalary),
                    NetAmount = netSalary.Amount,
                    Date = ReferenceDate,
                }).Data;
            }
        }

        private void CreateAndUpdateExtras(Guid salaryId, List<Extra> salaryExtras)
        {
            var extras = Salaries.Where(x => x.SalaryType != SalaryType.Net).ToList();
            if (extras.Count > 0)
            {
                foreach (var extra in extras)
                {
                    if (salaryExtras.Where(x => x.Name == extra.Name).Count() > 0)
                    {
                        var currentExtra = salaryExtras.Where(x => x.Name == extra.Name).FirstOrDefault();
                        currentExtra.Amount = extra.Amount;
                        mExtraService.Put(currentExtra);
                    }
                    else
                    {
                        mExtraService.Post(new Extra()
                        {
                            Name = extra.Name,
                            Amount = extra.Amount,
                            SalaryId = salaryId
                        });
                    }
                }
            }
        }

        private void DeleteSalaries(List<Extra> salaryExtras)
        {
            if (RemovedSalaries.Count > 0)
            {
                foreach (var extra in RemovedSalaries)
                {
                    if (salaryExtras.Where(x => x.Name == extra.Name).Count() > 0)
                    {
                        var currentExtra = salaryExtras.Where(x => x.Name == extra.Name).FirstOrDefault();
                        mExtraService.Delete(currentExtra);
                    }
                }
            }
        }

        #endregion
    }
}
