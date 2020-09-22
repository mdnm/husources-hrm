using HRM.Model.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRM.Model.Entities
{
    [Table("extra")]
    public class Extra : IEntity
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("amount")]
        public decimal Amount { get; set; }
        [Column("salary_id")]
        public Guid SalaryId { get; set; }
    }
}
