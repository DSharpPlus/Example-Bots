// THIS FILE IS A PART OF EMZI0767'S BOT EXAMPLES
//
// --------
// 
// Copyright 2019 Emzi0767
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// --------
//
// This is a WinForms example. It shows how to use WinForms without deadlocks.

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

namespace DSPlus.Examples
{
    // these delegates are used for invoking respective methods between threads
    public delegate void SetPropertyDelegate<TCtl, TProp>(TCtl control, Expression<Func<TCtl, TProp>> propexpr, TProp value) where TCtl : Control;
    public delegate TProp GetPropertyDelegate<TCtl, TProp>(TCtl control, Expression<Func<TProp>> propexpr) where TCtl : Control;
    public delegate void InvokeActionDelegate<TCtl>(TCtl control, Delegate dlg, params object[] args) where TCtl : Control;
    public delegate TResult InvokeFuncDelegate<TCtl, TResult>(TCtl control, Delegate dlg, params object[] args) where TCtl : Control;

    // these are various convenience extensions for thread-safe ui mutation
    public static class Extensions
    {
        // this method modifies specified property, assigning it the given value
        // usage is control.SetProperty(x => x.Property, value)
        public static void SetProperty<TCtl, TProp>(this TCtl control, Expression<Func<TCtl, TProp>> propexpr, TProp value) where TCtl : Control
        {
            // check for nulls
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (propexpr == null)
                throw new ArgumentNullException(nameof(propexpr));

            // check if cross-thread invocation is required
            if (control.InvokeRequired)
            {
                // if it is, perform it
                // this will invoke this method from (hopefully) the control's thread
                control.Invoke(new SetPropertyDelegate<TCtl, TProp>(SetProperty), control, propexpr, value);
                return;
            }

            // get the body of the expression, check if it's an expression that 
            // results in a class member being passed
            var propexprm = propexpr.Body as MemberExpression;
            if (propexprm == null)
                throw new ArgumentException("Invalid member expression.", nameof(propexpr));

            // get the member from the expression body, and check if it's a property
            var prop = propexprm.Member as PropertyInfo;
            if (prop == null)
                throw new ArgumentException("Invalid property supplied.", nameof(propexpr));

            // finally, set the value of the property to the supplied one
            prop.SetValue(control, value);
        }

        // this method reads the value of specified property, and returns it
        // usage is control.GetProperty(x => x.Property)
        public static TProp GetProperty<TCtl, TProp>(this TCtl control, Expression<Func<TProp>> propexpr) where TCtl : Control
        {
            // check for nulls
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (propexpr == null)
                throw new ArgumentNullException(nameof(propexpr));

            // check if cross-thread invocation is required
            // if it is, perform it
            // this will invoke this method from (hopefully) the control's thread
            if (control.InvokeRequired)
                return (TProp)control.Invoke(new GetPropertyDelegate<TCtl, TProp>(GetProperty), control, propexpr);

            // get the body of the expression, check if it's an expression that 
            // results in a class member being passed
            var propexprm = propexpr.Body as MemberExpression;
            if (propexprm == null)
                throw new ArgumentException("Invalid member expression.", nameof(propexpr));

            // get the member from the expression body, and check if it's a property
            var prop = propexprm.Member as PropertyInfo;
            if (prop == null)
                throw new ArgumentException("Invalid property supplied.", nameof(propexpr));

            // finally, set the value of the property to the supplied one
            return (TProp)prop.GetValue(control);
        }

        // this method invokes a return-less method for given control
        // usage is control.InvokeAction(new Action<T1, T2, ...>(method), arg1, arg2, ...)
        public static void InvokeAction<TCtl>(this TCtl control, Delegate dlg, params object[] args) where TCtl : Control
        {
            // check for nulls
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (dlg == null)
                throw new ArgumentNullException(nameof(dlg));

            // check if cross-thread invocation is required
            if (control.InvokeRequired)
            {
                // if it is, perform it
                // this will invoke this method from (hopefully) the control's thread
                control.Invoke(new InvokeActionDelegate<TCtl>(InvokeAction), control, dlg, args);
                return;
            }

            // finally, call the passed delegate, with supplied arguments
            dlg.DynamicInvoke(args);
        }
        
        // this method invokes a method which returns for given control, the returned value is returned to the caller
        // usage is control.InvokeAction<TReturn>(new Func<T1, T2, ..., TReturn>(method), arg1, arg2, ...)
        public static TResult InvokeFunc<TCtl, TResult>(this TCtl control, Delegate dlg, params object[] args) where TCtl : Control
        {
            // check for nulls
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (dlg == null)
                throw new ArgumentNullException(nameof(dlg));

            // check if cross-thread invocation is required
            if (control.InvokeRequired)
            {
                // if it is, perform it
                // this will invoke this method from (hopefully) the control's thread
                return (TResult)control.Invoke(new InvokeFuncDelegate<TCtl, TResult>(InvokeFunc<TCtl, TResult>), control, dlg, args);
            }

            // finally, call the passed delegate, with supplied arguments and return 
            // the result
            return (TResult)dlg.DynamicInvoke(args);
        }
    }
}
