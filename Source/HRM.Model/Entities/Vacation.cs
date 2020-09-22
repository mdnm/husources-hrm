using HRM.Model.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRM.Model.Entities
{
    [Table("vacation")]
    public class Vacation : IEntity
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("date_start")]
        public DateTime DateStart { get; set; }
        [Column("date_end")]
        public DateTime DateEnd { get; set; }
        [Column("sold_days")]
        public bool SoldDays { get; set; }
        [Column("employee_id")]
        public Guid EmployeeId { get; set; }
    }
}
