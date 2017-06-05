using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace InfoFenix.Core {
    /// <summary>
    /// Class that provides extensible properties and methods. This
    /// dynamic object stores 'extra' properties in a dictionary or
    /// checks the actual properties of the instance.
    ///
    /// This means you can subclass this expando and retrieve either
    /// native properties or properties from values in the dictionary.
    ///
    /// This type allows you three ways to access its properties:
    ///
    /// Directly: any explicitly declared properties are accessible
    /// Dynamic: dynamic cast allows access to dictionary and native properties/methods
    /// Dictionary: Any of the extended properties are accessible via IDictionary interface
    ///
    /// Author: Rick Strahl
    /// Website: https://weblog.west-wind.com/posts/2012/feb/08/creating-a-dynamic-extensible-c-expando-object
    /// </summary>

    public class Expando : DynamicObject, IDynamicMetaObjectProvider {

        #region Private Fields

        private object _instance;
        private Type _instanceType;

        #endregion Private Fields

        #region Private Properties

        private PropertyInfo[] _instancePropertyInfo;

        private PropertyInfo[] InstancePropertyInfo {
            get {
                if (_instancePropertyInfo == null && _instance != null) {
                    _instancePropertyInfo = _instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                }
                return _instancePropertyInfo;
            }
        }

        #endregion Private Properties

        #region Public Properties

        /// <summary>
        /// Convenience method that provides a string Indexer
        /// to the Properties collection AND the strongly typed
        /// properties of the object by name.
        ///
        /// // dynamic
        /// exp["Address"] = "112 nowhere lane";
        /// // strong
        /// var name = exp["StronglyTypedProperty"] as string;
        /// </summary>
        /// <remarks>
        /// The getter checks the Properties dictionary first
        /// then looks in PropertyInfo for properties.
        /// The setter checks the instance properties before
        /// checking the Properties dictionary.
        /// </remarks>
        /// <param name="key"></param>
        ///
        /// <returns></returns>
        public object this[string key] {
            get {
                try { return Properties[key]; /* try to get from properties collection first */ } catch (KeyNotFoundException) {
                    // try reflection on instanceType
                    if (GetProperty(_instance, key, out object result)) { return result; }

                    // nope doesn't exist
                    throw;
                }
            }
            set {
                if (Properties.ContainsKey(key)) {
                    Properties[key] = value;
                    return;
                }

                // check instance for existance of type first
                var array = _instanceType.GetMember(key, BindingFlags.Public | BindingFlags.GetProperty);
                if (!array.IsNullOrEmpty()) { SetProperty(_instance, key, value); } else { Properties[key] = value; }
            }
        }

        /// <summary>
        /// String Dictionary that contains the extra dynamic values
        /// stored on this object/instance
        /// </summary>
        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// This constructor just works off the internal dictionary and any
        /// public properties of this object.
        ///
        /// Note you can subclass Expando.
        /// </summary>
        public Expando() {
            Initialize(this);
        }

        /// <summary>
        /// Allows passing in an existing instance variable to 'extend'.
        /// </summary>
        /// <remarks>
        /// You can pass in null here if you don't want to
        /// check native properties and only check the Dictionary!
        /// </remarks>
        /// <param name="instance"></param>
        public Expando(object instance) {
            Initialize(instance);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Returns and the properties of
        /// </summary>
        /// <param name="includeInstanceProperties"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, object>> GetProperties(bool includeInstanceProperties = false) {
            if (includeInstanceProperties && _instance != null) {
                foreach (var prop in InstancePropertyInfo)
                    yield return new KeyValuePair<string, object>(prop.Name, prop.GetValue(_instance, null));
            }

            foreach (var key in Properties.Keys) {
                yield return new KeyValuePair<string, object>(key, Properties[key]);
            }
        }

        /// <summary>
        /// Checks whether a property exists in the Property collection
        /// or as a property on the instance
        /// </summary>
        /// <param name="item"></param>
        /// <param name="includeInstanceProperties"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, object> item, bool includeInstanceProperties = false) {
            var contains = Properties.ContainsKey(item.Key);
            if (contains) { return true; }

            if (includeInstanceProperties && _instance != null) {
                foreach (var prop in InstancePropertyInfo) {
                    if (prop.Name == item.Key) {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion Public Methods

        #region Public Override Methods

        /// <summary>
        /// Try to retrieve a member by name first from instance properties
        /// followed by the collection entries.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            result = null;

            // first check the Properties collection for member
            if (Properties.Keys.Contains(binder.Name)) {
                result = Properties[binder.Name];
                return true;
            }

            // Next check for Public properties via Reflection
            if (_instance != null) {
                try { return GetProperty(_instance, binder.Name, out result); } catch { }
            }

            // failed to retrieve a property
            result = null;
            return false;
        }

        /// <summary>
        /// Property setter implementation tries to retrieve value from instance
        /// first then into this object
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value) {
            // first check to see if there's a native property to set
            if (_instance != null) {
                try {
                    var result = SetProperty(_instance, binder.Name, value);
                    if (result) { return true; }
                } catch { }
            }

            // no match - set or add to dictionary
            Properties[binder.Name] = value;
            return true;
        }

        /// <summary>
        /// Dynamic invocation method. Currently allows only for Reflection based
        /// operation (no ability to add methods dynamically).
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) {
            if (_instance != null) {
                try {
                    // check instance passed in for methods to invoke
                    if (InvokeMethod(_instance, binder.Name, args, out result)) { return true; }
                } catch { }
            }

            result = null;
            return false;
        }

        #endregion Public Override Methods

        #region Protected Virtual Methods

        /// <summary>
        /// Initializes the instance.
        /// </summary>
        /// <param name="instance">The instance to extend.</param>
        protected virtual void Initialize(object instance) {
            _instance = instance;
            _instanceType = instance?.GetType();
        }

        #endregion Protected Virtual Methods

        #region Protected Methods

        /// <summary>
        /// Reflection Helper method to retrieve a property
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected bool GetProperty(object instance, string name, out object result) {
            if (instance == null) { instance = this; }

            var array = _instanceType.GetMember(name, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            if (!array.IsNullOrEmpty()) {
                var item = array[0];
                if (item.MemberType == MemberTypes.Property) {
                    result = ((PropertyInfo)item).GetValue(instance, null);
                    return true;
                }
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Reflection helper method to set a property value
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool SetProperty(object instance, string name, object value) {
            if (instance == null) { instance = this; }

            var array = _instanceType.GetMember(name, BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance);
            if (!array.IsNullOrEmpty()) {
                var item = array[0];
                if (item.MemberType == MemberTypes.Property) {
                    ((PropertyInfo)item).SetValue(_instance, value, null);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Reflection helper method to invoke a method
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected bool InvokeMethod(object instance, string name, object[] args, out object result) {
            if (instance == null) { instance = this; }

            // Look at the instanceType
            var array = _instanceType.GetMember(name, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance);

            if (!array.IsNullOrEmpty()) {
                var item = array[0] as MethodInfo;
                result = item.Invoke(_instance, args);
                return true;
            }

            result = null;
            return false;
        }

        #endregion Protected Methods
    }
}