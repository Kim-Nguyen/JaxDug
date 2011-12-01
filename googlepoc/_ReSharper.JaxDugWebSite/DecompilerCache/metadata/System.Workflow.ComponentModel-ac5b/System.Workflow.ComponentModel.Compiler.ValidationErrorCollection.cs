// Type: System.Workflow.ComponentModel.Compiler.ValidationErrorCollection
// Assembly: System.Workflow.ComponentModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// Assembly location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Workflow.ComponentModel.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Workflow.ComponentModel.Compiler
{
    [Serializable]
    public sealed class ValidationErrorCollection : Collection<ValidationError>
    {
        public ValidationErrorCollection();
        public ValidationErrorCollection(ValidationErrorCollection value);
        public ValidationErrorCollection(IEnumerable<ValidationError> value);
        public bool HasErrors { get; }
        public bool HasWarnings { get; }
        protected override void InsertItem(int index, ValidationError item);
        protected override void SetItem(int index, ValidationError item);
        public void AddRange(IEnumerable<ValidationError> value);
        public ValidationError[] ToArray();
    }
}
