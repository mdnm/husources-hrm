using HRM.Model.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRM.Model.Entities
{
    [Table("employee")]
    public class Employee : IEntity
    {
        [Column("id")]
        public Guid Id { get; set; }        
        [Column("name")]
        public string Name { get; set; }
        [Column("department_id")]
        public Guid DepartmentId { get; set; }
        [Column("work_shift_id")]
        public Guid WorkShiftId { get; set; }
    }
}
