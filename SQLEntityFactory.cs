using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenArmsSeries.Repositories.RDBMS
{

    public class SQLEntityFactory
    {
        public static Dictionary<string, SQLEntity> Items = new Dictionary<string, SQLEntity>();

        public class SQLEntity
        {
            public string dbname { get; set; }

            private string _sqlByQuerySingle = string.Empty;
            public string sqlByQuerySingle
            {
                get { return _sqlByQuerySingle; }
                set
                {
                    _sqlByQuerySingle = value;
                    this.Add("sqlByQuerySingle", value);
                }
            }

            private string _sqlByRepositoryCreate = string.Empty;
            public string sqlByRepositoryCreate
            {
                get { return _sqlByRepositoryCreate; }
                set
                {
                    _sqlByRepositoryCreate = value;
                    this.Add("sqlByRepositoryCreate", value);
                }
            }

            private string _sqlByRepositoryUpdate = string.Empty;
            public string sqlByRepositoryUpdate
            {
                get { return _sqlByRepositoryUpdate; }
                set
                {
                    _sqlByRepositoryUpdate = value;
                    this.Add("sqlByRepositoryUpdate", value);
                }
            }

            private string _sqlByRepositoryRemove = string.Empty;
            public string sqlByRepositoryRemove
            {
                get { return _sqlByRepositoryRemove; }
                set
                {
                    _sqlByRepositoryRemove = value;
                    this.Add("sqlByRepositoryRemove", value);
                }
            }

            private string _sqlByQueryMore = string.Empty;
            public string sqlByQueryMore
            {
                get { return _sqlByQueryMore; }
                set
                {
                    _sqlByQueryMore = value;
                    this.Add("sqlByQueryMore", value);
                }
            }

            private string _sqlByQueryMoreCount = string.Empty;
            public string sqlByQueryMoreCount
            {
                get { return _sqlByQueryMoreCount; }
                set
                {
                    _sqlByQueryMoreCount = value;
                    this.Add("sqlByQueryMoreCount", value);
                }
            }

            private IDictionary<string, string> _Items = new Dictionary<string, string>();
            public IDictionary<string, string> Items { get { return _Items; } private set { } }
            public void Add(string key, string value)
            {
                if (this.Items.ContainsKey(key))
                {
                    this._Items[key] = value;
                }
                else
                {
                    this._Items.Add(key, value);
                }
            }
        }
    }
}
