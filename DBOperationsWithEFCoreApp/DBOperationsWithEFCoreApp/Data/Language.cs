namespace DBOperationsWithEFCoreApp.Data
{
    public class Language
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        //public string Books { get; set; }  //this will be normal colm then

        //for 1 to many relation we need to the following changes
        //Books -> LanguageId (foreign key exists)
        public ICollection<Book> Books { get; set; }  //fk to Book
    }
}
