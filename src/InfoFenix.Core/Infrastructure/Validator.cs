using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InfoFenix.Core.Infrastructure {
    public static class Validator {

        #region Public Static Methods

        public static ErrorCollection Validate(object obj) {
            var errors = new List<ValidationResult>();
            var result = new ErrorCollection();
            var success = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(obj, new ValidationContext(obj, null, null), errors);
            if (!success) {
                foreach (var item in errors) {
                    result.Add(new Error(item.ErrorMessage, item.MemberNames.ToArray()));
                }
            }
            return result;
        }

        #endregion Public Static Methods
    }

    public class Error {

        #region Public Properties

        public string Message { get; }
        public IEnumerable<string> Fields { get; }

        #endregion Public Properties

        #region Public Constructors

        public Error(string message, params string[] fields) {
            Message = message;
            Fields = fields ?? Enumerable.Empty<string>();
        }

        #endregion Public Constructors
    }

    public class ErrorCollection : Collection<Error> {

        #region Public Static Read-Only Fields

        public static readonly ErrorCollection Empty = new ErrorCollection();

        #endregion Public Static Read-Only Fields

        #region Public Properties

        public bool IsValid {
            get { return Count == 0; }
        }

        #endregion Public Properties
    }
}