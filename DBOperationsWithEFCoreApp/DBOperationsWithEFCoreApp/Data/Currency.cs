namespace DBOperationsWithEFCoreApp.Data
{
    public class Currency
    {
        public int Id { get; set; }  //fk to Book
        public string CurrencyType{ get; set; } //pk  //this cant be same as the ClassName
        public string Description { get; set; }

        public ICollection<BookPrice> BooksPrices { get; set; }

    }
}
