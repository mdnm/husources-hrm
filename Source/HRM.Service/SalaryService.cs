using HRM.Model;
using HRM.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Service
{
    public class SalaryService
    {
        public BaseService<Salary> mBaseService;

        public SalaryService()
        {
            mBaseService = new BaseService<Salary>();
        }

        public List<Salary> GetEmployeeSalaries(Guid employeeId)
        {
            var employeeSalaries = new List<Salary>();
            var response = mBaseService.Get();
            if (response.Data != null)
            {
                var allSalaries = (IEnumerable<Salary>)response.Data;
                employeeSalaries.AddRange(allSalaries.Where(x => x.EmployeeId == employeeId));
            }
            return employeeSalaries;
        }

        public decimal CalculateINSS(decimal grossMonthSalary)
        {
            if (grossMonthSalary < 1045m)
            {
                grossMonthSalary *= 0.075m;
            }
            else if (grossMonthSalary > 1045m && grossMonthSalary < 2089.60m)
            {
                grossMonthSalary *= 0.09m;
            }
            else if (grossMonthSalary > 2089.60m && grossMonthSalary < 3134.40m)
            {
                grossMonthSalary *= 0.12m;
            }
            else if (grossMonthSalary > 3134.40m && grossMonthSalary < 6101.06m)
            {
                grossMonthSalary *= 0.14m;
            }
            else
            {
                grossMonthSalary = 713.10m;
            }

            return grossMonthSalary;
        }

        public decimal CalculateIRRF(decimal grossMonthSalary, decimal INSS)
        {
            var IRRF = 0m;
            var IRRFCalculationBase = grossMonthSalary - INSS;

            if (IRRFCalculationBase >= 1903.99m && IRRFCalculationBase <= 2826.65m)
            {
                IRRF = IRRFCalculationBase * 0.075m;
                IRRF -= 142.80m;
            }
            else if (IRRFCalculationBase >= 2826.66m && IRRFCalculationBase <= 3751.05m)
            {
                IRRF = IRRFCalculationBase * 0.15m;
                IRRF -= 354.80m;
            }
            else if (IRRFCalculationBase >= 3751.06m && IRRFCalculationBase <= 4664.68m)
            {
                IRRF = IRRFCalculationBase * 0.225m;
                IRRF -= 636.13m;
            }
            else if (IRRFCalculationBase >= 4664.69m)
            {
                IRRF = IRRFCalculationBase * 0.275m;
                IRRF -= 869.36m;
            }

            return IRRF;
        }

        public void CalculateVacation(decimal grossMonthSalary, Vacation vacation, bool sellVacation, DateTime referenceDate, int vacationDays, out decimal soldDaysValue, out decimal totalValue)
        {
            var valuePerDay = grossMonthSalary / 30;
            soldDaysValue = 0m;
            if (sellVacation)
            {
                var tenDaysValue = valuePerDay * 10;
                soldDaysValue = tenDaysValue + (tenDaysValue / 3);
                vacationDays -= 10;
                vacation.SoldDays = true;
            }
            vacation.DateEnd = referenceDate.AddDays(vacationDays);

            var valuePerVacationDays = valuePerDay * vacationDays;
            totalValue = valuePerVacationDays + (valuePerVacationDays / 3);
        }
    }
}
