public class Transaction
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } // Add this property
    public int CategoryId { get; set; }
    public int AccountId { get; set; }
    public virtual void Validate() { }
    public virtual string GetTransactionDetails() => string.Empty;
}