using System;
using System.Transactions;

namespace FD.Simple.DB
{
    public class TransHelper : IDisposable
    {
        private TransHelper(ETransType transType)
        {
            TransactionScopeOption scopeOption;
            Enum.TryParse<TransactionScopeOption>(transType.ToString(), out scopeOption);
            var transOption = new TransactionOptions();
            transOption.Timeout = new TimeSpan(0, 0, 300);
            transOption.IsolationLevel = IsolationLevel.ReadCommitted;
            this.transactionScope = new TransactionScope(scopeOption, transOption);
        }

        private TransactionScope transactionScope;

        /// <summary>
        /// 使用事务访问数据库,语法 using(var trans = TransHelper){ //TODO}
        /// TODO中的所有数据库操作，将在同一个事务中进行。
        /// 如需回滚则抛出异常，如需提交调用trans.Commit()
        /// </summary>
        /// <param name="transType"></param>
        /// <returns></returns>
        public static TransHelper BeginTrans(ETransType transType = ETransType.Required)
        {
            return new TransHelper(transType);
        }

        public void Commit()
        {
            this.transactionScope.Complete();
        }

        public void Dispose()
        {
            this.transactionScope.Dispose();
        }
    }

    public enum ETransType
    {
        /// <summary>
        /// 使用已存在的事务，或者创建一个新事务
        /// </summary>
        Required = 0,

        /// <summary>
        /// 创建一个新事务
        /// </summary>
        //RequiresNew = 1,
        /// <summary>
        /// 不使用事务
        /// </summary>
        Suppress = 2,
    }
}