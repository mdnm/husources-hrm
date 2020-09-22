using HRM.Model.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRM.Model.Entities
{
    [Table("work_shift")]
    public class WorkShift : IEntity
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("start_hour")]
        public TimeSpan StartHour { get; set; }
        [Column("end_hour")]
        public TimeSpan EndHour { get; set; }
    }
}
