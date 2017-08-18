using System;
using System.Windows.Forms;
using InfoFenix.IoC;
using WinFormsApplication = System.Windows.Forms.Application;

namespace InfoFenix.Application.Code {

    public sealed class FormManager : IFormManager {

        #region Private Read-Only Fields

        private readonly IResolver _resolver;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public FormManager(IResolver resolver) {
            Prevent.ParameterNull(resolver, nameof(resolver));

            _resolver = resolver;
        }

        #endregion Public Constructors

        #region Private Static Methods

        private static Form Retrieve(Type formType) {
            foreach (Form form in WinFormsApplication.OpenForms) {
                if (form.GetType() == formType) {
                    return form;
                }
            }
            return null;
        }

        #endregion Private Static Methods

        #region IFormManager Members

        public TForm Get<TForm>(Form mdi = null, bool multipleInstance = true, object state = null)
            where TForm : Form {
            var form = (!multipleInstance ? Retrieve(typeof(TForm)) : null) ?? _resolver.Resolve<TForm>();

            form.MdiParent = mdi;
            form.WindowState = FormWindowState.Normal;
            form.Tag = state;

            return (TForm)form;
        }

        #endregion IFormManager Members
    }
}