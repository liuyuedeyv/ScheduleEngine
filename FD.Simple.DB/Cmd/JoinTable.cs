using System.Collections.Generic;

namespace FD.Simple.DB.Cmd
{
    public class JoinTable
    {
        public static JoinTable New(string table, EJoinType joinType)
        {
            return new JoinTable(table, joinType);
        }

        public JoinTable(string table, EJoinType joinType)
        {
            this.Table = table;
            this.JoinType = joinType;
        }

        public EJoinType JoinType { get; set; }
        public string Table { get; set; }
        public Dictionary<string, string> Conditions { get; set; }



        public JoinTable On(string leftValue, string rightValue, bool rightIsCol = true)
        {
            if (Conditions == null)
            {
                Conditions = new Dictionary<string, string>();
            }
            Conditions.Add(leftValue, rightValue);
            return this;
        }
    }

    public enum EJoinType
    {
        Left,
        Right,
        Inner
    }
}
