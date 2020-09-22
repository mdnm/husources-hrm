using HRM.Model.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRM.Model.Entities
{
    [Table("salary")]
    public class Salary : IEntity
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("net_amount")]
        public decimal NetAmount { get; set; }
        [Column("gross_amount")]
        public decimal GrossAmount { get; set; }
        [Column("employee_id")]
        public Guid EmployeeId { get; set; }
    }
}
