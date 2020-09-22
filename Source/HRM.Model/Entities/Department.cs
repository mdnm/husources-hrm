using HRM.Model.Entities.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Model.Entities
{
    [Table("department")]
    public class Department : IEntity
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
    }
}
