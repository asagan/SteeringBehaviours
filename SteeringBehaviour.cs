﻿using System;
using System.Collections.Generic;
using System.Text;
using SimioAPI;
using SimioAPI.Extensions;

namespace UserDefinedStepAndElement1
{
    public class UserStepDefinition : IStepDefinition
    {
        #region IStepDefinition Members

        /// <summary>
        /// Property returning the full name for this type of step. The name should contain no spaces. 
        /// </summary>
        public string Name
        {
            get { return "UserStep"; }
        }

        /// <summary>
        /// Property returning a short description of what the step does.  
        /// </summary>
        public string Description
        {
            get { return "Description text for the UserStep step."; }
        }

        /// <summary>
        /// Property returning an icon to display for the step in the UI. 
        /// </summary>
        public System.Drawing.Image Icon
        {
            get { return null; }
        }

        /// <summary>
        /// Property returning a unique static GUID for the step.  
        /// </summary>
        public Guid UniqueID
        {
            get { return MY_ID; }
        }
        static readonly Guid MY_ID = new Guid("{761b81b4-068c-4b18-930a-8298324cb38e}");

        /// <summary>
        /// Property returning the number of exits out of the step. Can return either 1 or 2. 
        /// </summary>
        public int NumberOfExits
        {
            get { return 1; }
        }

        /// <summary>
        /// Method called that defines the property schema for the step.
        /// </summary>
        public void DefineSchema(IPropertyDefinitions schema)
        {
            // Example of how to add a property definition to the step.
            IPropertyDefinition pd;
            pd = schema.AddExpressionProperty("MyExpression", "0.0");
            pd.DisplayName = "My Expression";
            pd.Description = "An expression property for this step.";
            pd.Required = true;

            // Example of how to add an element property definition to the step.
            pd = schema.AddElementProperty("UserElementName", UserElementDefinition.MY_ID);
            pd.DisplayName = "UserElement Name";
            pd.Description = "The name of a UserElement element referenced by this step.";
            pd.Required = true;
        }

        /// <summary>
        /// Method called to create a new instance of this step type to place in a process. 
        /// Returns an instance of the class implementing the IStep interface.
        /// </summary>
        public IStep CreateStep(IPropertyReaders properties)
        {
            return new SteeringBehaviour(properties);
        }

        #endregion
    }

    class SteeringBehaviour : IStep
    {
        IPropertyReaders _properties;

        public SteeringBehaviour(IPropertyReaders properties)
        {
            _properties = properties;
        }

        #region IStep Members

        /// <summary>
        /// Method called when a process token executes the step.
        /// </summary>
        public ExitType Execute(IStepExecutionContext context)
        {
            // Example of how to get the value of a step property.
            IPropertyReader myExpressionProp = _properties.GetProperty("MyExpression") as IPropertyReader;
            string myExpressionPropStringValue = myExpressionProp.GetStringValue(context);
            double myExpressionPropDoubleValue = myExpressionProp.GetDoubleValue(context);

            // Example of how to get an element reference specified in an element property of the step.
            IElementProperty myElementProp = (IElementProperty)_properties.GetProperty("UserElementName");
            UserElement myElement = (UserElement)myElementProp.GetElement(context);

            // Example of how to display a trace line for the step.
            context.ExecutionInformation.TraceInformation(String.Format("The value of expression '{0}' is '{1}'.", myExpressionPropStringValue, myExpressionPropDoubleValue));

            return ExitType.FirstExit;
        }

        #endregion
    }
}
