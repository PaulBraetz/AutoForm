using System.ComponentModel;

namespace TestApp.Models
{
    [AutoControlModel]
    internal sealed class Address
    {
        public String Street { get; set; }
        public String City { get; set; }

        public override String ToString()
        {
            return $@"{{Street = ""{Street}"", City = ""{City}""}}";
        }
    }
    [AutoControlModel]
    internal sealed class Person
    {
        public Person() : this(true)
        {
        }
        private Person(Boolean addParents)
        {
            //if (addParents)
            //{
            //    Mom = new Person(false);
            //    Dad = new Person(false);
            //}
            //Address = new Address();

            AttributesProvider = new PersonAttributesProvider(this);
        }
        public String Name { get; set; }

        [AutoControlAttributesProvider]
        public PersonAttributesProvider AttributesProvider { get; private set; }

        //public String LastName { get; set; }
        //public Address Address { get; set; }
        //public Person Dad { get; set; }
        //public Person Mom { get; set; }

        public override String ToString()
        {
            return $@"{{Name = ""{Name}""}}";//, LastName = ""{LastName}"", Address = {Address}, Mom = {Mom}, Dad = {Dad}}}";
        }
    }
    internal sealed class PersonAttributesProvider
    {
        private readonly Person _person;

        public PersonAttributesProvider(Person person)
        {
            _person = person;
        }

        public IDictionary<String, Object> GetNameAttributes()
        {
            return new Dictionary<String, Object>()
            {
                { "class", _person.Name.Length > 5?"long-name":"short-name" },
            };
        }
    }
}
