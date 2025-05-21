using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Model
{
    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [MaxLength(24)]
        [Required]
        public string Name { get; set; }

        
        public string Description { get; set; }

        [Required]
        public DateTime BuyDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int ToDoListId { get; set; }
        [ForeignKey("ToDoListId")]
        public virtual ExpenseCategory ExpenseCategory { get; set; }
    }
}