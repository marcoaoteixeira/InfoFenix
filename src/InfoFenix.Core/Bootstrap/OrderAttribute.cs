using System;

namespace InfoFenix.Core.Bootstrap {

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class OrderAttribute : Attribute {

        #region Public Properties

        public int Order { get; }

        #endregion Public Properties

        #region Public Constructors

        public OrderAttribute(int order) {
            Order = order;
        }

        #endregion Public Constructors

        #region Public Override Explicit

        public static implicit operator int(OrderAttribute order) {
            return order != null ? order.Order : 0;
        }

        public static implicit operator OrderAttribute(int order) {
            return new OrderAttribute(order);
        }

        #endregion Public Override Explicit
    }
}