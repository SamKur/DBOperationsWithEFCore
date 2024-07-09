namespace DBOperationsWithEFCoreApp.Data
{
    public class BookPrice
    {
        public int Id { get; set; }  //pk
        public int BookId { get; set; } //fk
        /// <summary>
        /// By default, EF Core looks for properties following the ClassNameId pattern 
        /// to automatically configure foreign keys. For instance, BookId in BookPrice 
        /// is expected to be a foreign key for Book.
        /// </summary>
        public int Amount { get; set; }
        public int CurrencyId { get; set; }

        //public ICollection<Book> Books { get; set; }
        public Book Book { get; set; }
        public Currency Currency { get; set; }

    }
}
