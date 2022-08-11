using System.ComponentModel;
using TestApp.Controls;

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
            if (addParents)
            {
                //Mom = new Person(false);
                Dad = new Person(false)
                {
                    Male = true
                };
            }
            //Address = new Address();

            AttributesProvider = new PersonAttributesProvider(this);
        }
        public Boolean Male { get; set; }
        public String Name { get; set; }
        [AutoControlModelExclude]
        public SByte Age { get; set; }

        [AutoControlAttributesProvider]
        public PersonAttributesProvider AttributesProvider { get; private set; }

        //public String LastName { get; set; }
        //public Address Address { get; set; }
        public Person Dad { get; set; }
        //public Person Mom { get; set; }

        public override String ToString()
        {
            return $@"{{Name = ""{Name}"", Male = {Male}, Age = {Age}, Dad = {Dad}}}";//, LastName = ""{LastName}"", Address = {Address}, Mom = {Mom},}}";
        }
    }
    internal sealed class PersonAttributesProvider
    {
        private readonly Person _person;

        public PersonAttributesProvider(Person person)
        {
            _person = person;
        }

        private static readonly Dictionary<String, Object> _emptyAttributes = new Dictionary<string, object>()
        {
            {"class", "my-1" }
        };

        private readonly Dictionary<String, Object> _longNameAttributes = new Dictionary<string, object>()
        {
            {"class","form-control my-1 text-danger"},
            {"placeholder", nameof(Person.Name) }
        };
        private readonly Dictionary<String, Object> _shortNameAttributes = new Dictionary<string, object>()
        {
            {"class","form-control my-1 text-success"},
            {"placeholder", nameof(Person.Name) }
        };
        public IDictionary<String, Object> GetNameAttributes()
        {
            return (_person.Name?.Length ?? 0) > 5 ?
                _longNameAttributes :
                _shortNameAttributes;
        }

        private static readonly Dictionary<String, Object> _maleAttributes = new()
        {
            {"class", "form-check-input my-1" }
        };
        public IDictionary<String, Object> GetMaleAttributes()
        {
            return _emptyAttributes;
        }

        private static Dictionary<String, Object> _ageAttributes = new()
        {
            {"class","form-control my-1" },
            {"min", 0 },
            {"max", SByte.MaxValue },
            {"placeholder", nameof(Person.Age) }
        };
        public IDictionary<String, Object> GetAgeAttributes()
        {
            return _ageAttributes;
        }

        private static readonly Dictionary<String, Object> _dadAttributes = new()
        {
            {"class", "form-group my-1"}
        };
        private static readonly Dictionary<String, Object> _noDadAttributes = new()
        {
            {"hidden", "" }
        };
        public IDictionary<String, Object> GetDadAttributes()
        {
            return _person.Male ? _noDadAttributes : _dadAttributes;
        }
    }
}
