using System;
using System.Web.Mvc;

namespace MvcBootEx.Form
{
    public class FormGroup : IDisposable
    {
        private readonly ViewContext _viewContext;
        private bool _disposed;

        public FormGroup(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            _viewContext = viewContext;
        }

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;
            FormGroupEx.EndFormGroup(_viewContext);
        }

        public void EndForm()
        {
            Dispose(true);
        }
    }
}