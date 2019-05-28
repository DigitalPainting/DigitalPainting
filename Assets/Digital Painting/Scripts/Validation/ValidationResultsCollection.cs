using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace wizardscode.validation
{
    public class ValidationResultCollection
    {
        Dictionary<int, ValidationResult> collection = new Dictionary<int, ValidationResult>();


        /// <summary>
        /// Get or create a ValidationResult for a named validation test.
        /// </summary>
        /// <param name="name">The name of the validation test.</param>
        /// <returns>An existing ValidationResult if the test has already been run, or a new validation result with an untested state.</returns>
        public ValidationResult GetOrCreate(string name)
        {
            ValidationResult result;
            if (!collection.TryGetValue(name.GetHashCode(), out result))
            {
                result = new ValidationResult(name);
                AddOrUpdate(result);
            }
            return result;
        }

        public void AddOrUpdate(ValidationResult result)
        {
            Remove(result.name);
            collection[result.id] = result;
        }

        public void AddOrUpdateAll(ValidationResultCollection results)
        {
            foreach (ValidationResult result in results.collection.Values)
            {
                if (result.impact != ValidationResult.Level.OK)
                {
                    collection[result.id] = result;
                }
            }
        }

        public void Remove(string name)
        {
            collection.Remove(name.GetHashCode());
        }

        public int Count
        {
            get { return collection.Count(); }
        }

        public int CountWarning
        {
            get { return collection.Values.Count(x => x.impact == ValidationResult.Level.Warning); }
        }

        public int CountError
        {
            get { return collection.Values.Count(x => x.impact == ValidationResult.Level.Error); }
        }

        public List<ValidationResult> ErrorList
        {
            get { return collection.Values.Where(x => x.impact == ValidationResult.Level.Error).ToList(); }
        }

        public List<ValidationResult> WarningList
        {
            get { return collection.Values.Where(x => x.impact == ValidationResult.Level.Warning).ToList(); }
        }

        public List<ValidationResult> OKList
        {
            get { return collection.Values.Where(x => x.impact == ValidationResult.Level.OK).ToList(); }
        }
    }
}
